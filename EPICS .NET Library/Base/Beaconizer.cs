// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beaconizer.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	using System;
	using System.Net;
	using System.Threading;

	/// <summary>
	/// The beaconizer.
	/// </summary>
	internal class Beaconizer : IDisposable
	{
		/// <summary>
		///   The anomaly cooldown.
		/// </summary>
		private readonly TimeSpan AnomalyCooldown = new TimeSpan(0, 5, 0);

		/// <summary>
		///   The beacon port.
		/// </summary>
		private readonly int BeaconPort;

		/// <summary>
		///   The beacon pusher.
		/// </summary>
		private readonly Thread BeaconPusher;

		/// <summary>
		///   The conn.
		/// </summary>
		private readonly EpicsUDPConnection Conn;

		/// <summary>
		///   The last anomaly.
		/// </summary>
		private readonly DateTime lastAnomaly = DateTime.Now;

		/// <summary>
		///   The is not disposing.
		/// </summary>
		private bool isNotDisposing = true;

		/// <summary>
		///   The loop interval.
		/// </summary>
		private int loopInterval = 30;

		/// <summary>
		///   The max loop interval.
		/// </summary>
		private int maxLoopInterval = 15000;

		/// <summary>
		/// Initializes a new instance of the <see cref="Beaconizer"/> class.
		/// </summary>
		/// <param name="conn">
		/// The conn.
		/// </param>
		/// <param name="beaconPort">
		/// The beacon port.
		/// </param>
		public Beaconizer(EpicsUDPConnection conn, int beaconPort)
		{
			this.Conn = conn;
			this.BeaconPort = beaconPort;

			this.BeaconPusher = new Thread(this.PushBeacon);
			this.BeaconPusher.IsBackground = true;
			this.BeaconPusher.Start();
		}

		/// <summary>
		/// The produce anomaly.
		/// </summary>
		public void ProduceAnomaly()
		{
			if (DateTime.Now - this.lastAnomaly > this.AnomalyCooldown)
			{
				this.loopInterval = 30;
			}
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.isNotDisposing)
			{
				this.isNotDisposing = false;
			}
			else
			{
				return;
			}
		}

		/// <summary>
		/// The push beacon.
		/// </summary>
		private void PushBeacon()
		{
			var loopTime = new TimeSpan(0, 0, 0, 0, 10);
			var loops = 0;
			var counter = 0;

			while (this.isNotDisposing)
			{
				if (10 * loops >= this.loopInterval)
				{
					loops = 0;
					if (this.loopInterval < this.maxLoopInterval)
					{
						this.loopInterval *= 2;
					}

					this.Conn.Send(
						EpicsCodec.beaconMessage(this.Conn.Port, counter++, this.Conn.IP), 
						new IPEndPoint(this.Conn.BroadcastIPAddress, this.BeaconPort));
				}
				else
				{
					loops++;
				}

				Thread.Sleep(loopTime);
			}
		}
	}
}