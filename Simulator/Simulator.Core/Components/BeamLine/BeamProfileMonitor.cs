// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamProfileMonitor.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;

	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	public class BeamProfileMonitor : BeamLineComponent
	{
		/// <summary>
		/// Determines if the beam profile monitor camera is on or off.
		/// </summary>
		public readonly IServerChannel<int> CameraOn;

		/// <summary>
		/// Determines if the view screen is inside the beam pipe or out.
		/// </summary>
		public readonly IServerChannel<int> ViewScreenIn;

		/// <summary>
		/// Determines if the laser marker is on or off.
		/// </summary>
		public readonly IServerChannel<int> LaserOn;

		/// <summary>
		/// Simple frame of image acquired via a frame grabber from the profile monitor camera.
		/// </summary>
		public readonly IServerChannel<string> Image;

		private readonly Stopwatch stopwatch = new Stopwatch();
		private readonly Random random = new Random();
		private const int ImageSide = 153; // Default image size is 155-2, 155-2 (-2 for the focus rectangle)
		private Bitmap bitmap = new Bitmap(153, 153, PixelFormat.Format24bppRgb);
		private int beamImageRadius = 70;
		private int beamImageDensity = 10000;
		private int imageXCoordinate;
		private int imageYCoordinate;
		private int imageCenterXOffset = 70;
		private int imageCenterYOffset = 70;
		private int beamImageCenterXCoordinate;
		private int beamImageCenterYCoordinate;
		private int imageOverflowScatter;
		private int randomRadius;

		public BeamProfileMonitor(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.CameraOn = this.CreateChannel<int>(PredefinedChannels.BeamProfileMonitor.CameraOn);
			this.ViewScreenIn = this.CreateChannel<int>(PredefinedChannels.BeamProfileMonitor.ViewScreenIn);
			this.LaserOn = this.CreateChannel<int>(PredefinedChannels.BeamProfileMonitor.LaserOn);
			this.Image = this.CreateChannel<string>(PredefinedChannels.BeamProfileMonitor.Image);
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.CameraOn.Value == 1)
			{
				if (!this.stopwatch.IsRunning)
				{
					this.stopwatch.Start();
				}
				else if (this.ViewScreenIn.Value == 0)
				{
					// Serialize then publish the new beam profile image
					using (var stream = new MemoryStream())
					{
						this.bitmap = new Bitmap(ImageSide, ImageSide, PixelFormat.Format24bppRgb);
						this.bitmap.Save(stream, ImageFormat.Gif);
						this.Image.Value = Convert.ToBase64String(stream.ToArray());
						this.stopwatch.Restart();
					}
				}
				else if (this.stopwatch.ElapsedMilliseconds > 1000)
				{
					// Take a snapshot of the beam profile once every second
					this.bitmap = new Bitmap(ImageSide, ImageSide, PixelFormat.Format24bppRgb);
					this.beamImageRadius = (int)((electronBeam.Radius * ImageSide) / (Parameters.DriftTubeRadius * 2));
					this.beamImageDensity = (int)(10000 * electronBeam.ElectronDensity / 2E15);
					this.imageCenterXOffset = (int)(((electronBeam.Location.X - this.Location.X) * ImageSide) / (Parameters.DriftTubeRadius * 2));
					this.imageCenterYOffset = (int)(((electronBeam.Location.Y - this.Location.Y) * ImageSide) / (Parameters.DriftTubeRadius * 2));
					this.beamImageCenterXCoordinate = (ImageSide / 2) + this.imageCenterXOffset;
					this.beamImageCenterYCoordinate = (ImageSide / 2) + this.imageCenterYOffset;

					for (var i = 0; i < this.beamImageDensity; i++)
					{
						this.imageXCoordinate = this.random.Next(0, ImageSide);
						this.imageYCoordinate = this.random.Next(0, ImageSide);
						this.randomRadius = (int)
							Math.Sqrt(
								((this.imageXCoordinate - this.beamImageCenterXCoordinate) * (this.imageXCoordinate - this.beamImageCenterXCoordinate))
								+ ((this.imageYCoordinate - this.beamImageCenterYCoordinate) * (this.imageYCoordinate - this.beamImageCenterYCoordinate)));

						if (this.LaserOn.Value == 1 && this.randomRadius < 5)
						{
							this.bitmap.SetPixel(this.imageXCoordinate, this.imageYCoordinate, Color.Red);
						}
						else if (this.randomRadius < this.beamImageRadius)
						{
							// ToDo: Performance can be improved via LockBits method (http://www.bobpowell.net/lockingbits.htm)
							this.bitmap.SetPixel(this.imageXCoordinate, this.imageYCoordinate, Color.Blue);
						}
						else
						{
							if (++this.imageOverflowScatter > 15)
							{
								this.imageOverflowScatter = 0;
								this.bitmap.SetPixel(this.imageXCoordinate, this.imageYCoordinate, Color.Blue);
							}
						}
					}

					// Serialize then publish the new beam profile image
					using (var stream = new MemoryStream())
					{
						this.bitmap.Save(stream, ImageFormat.Gif);
						this.Image.Value = Convert.ToBase64String(stream.ToArray());
						this.stopwatch.Restart();
					}
				}
			}
			else
			{
				if (this.stopwatch.IsRunning)
				{
					this.stopwatch.Stop();
				}
			}
		}
	}
}
