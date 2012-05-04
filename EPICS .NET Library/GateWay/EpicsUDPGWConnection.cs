// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsUDPGWConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Diagnostics;
	using System.Net;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// The epics udpgw connection.
	/// </summary>
	internal class EpicsUDPGWConnection : EpicsUDPConnection
	{
		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		///   The codec.
		/// </summary>
		private readonly EpicsCodec codec;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsUDPGWConnection"/> class.
		/// </summary>
		/// <param name="gateWay">
		/// The gate way.
		/// </param>
		/// <param name="Codec">
		/// The codec.
		/// </param>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		public EpicsUDPGWConnection(EpicsGateWay gateWay, EpicsCodec Codec, IPEndPoint ipe)
			: base(ipe)
		{
			this.GateWay = gateWay;

			this.UDPSocket.SendBufferSize = this.GateWay.Config.UDPBufferSize;
			this.UDPSocket.ReceiveBufferSize = this.GateWay.Config.UDPBufferSize;

			if (this.GateWay.Config.ServerList.Count > 0)
			{
				this.wrapTargetList(this.GateWay.Config.ServerList.getStringList());

				gateWay.Config.ConfigChanged += this.Config_ConfigChanged;
			}

			this.codec = Codec;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsUDPGWConnection"/> class.
		/// </summary>
		/// <param name="gateWay">
		/// The gate way.
		/// </param>
		/// <param name="Codec">
		/// The codec.
		/// </param>
		public EpicsUDPGWConnection(EpicsGateWay gateWay, EpicsCodec Codec)
		{
			this.GateWay = gateWay;

			this.UDPSocket.SendBufferSize = this.GateWay.Config.UDPBufferSize;
			this.UDPSocket.ReceiveBufferSize = this.GateWay.Config.UDPBufferSize;

			this.wrapTargetList(this.GateWay.Config.ServerList.getStringList());

			gateWay.Config.ConfigChanged += this.Config_ConfigChanged;

			this.codec = Codec;
		}

		/// <summary>
		/// The send.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		public override void Send(byte[] data)
		{
			this.GateWay.Statistic.BytesSend += data.Length;
			base.Send(data);
		}

		/// <summary>
		/// The send.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <param name="receiver">
		/// The receiver.
		/// </param>
		public override void Send(byte[] data, IPEndPoint receiver)
		{
			this.GateWay.Statistic.BytesSend += data.Length;
			base.Send(data, receiver);
		}

		/// <summary>
		/// The process received data.
		/// </summary>
		/// <param name="dataPipe">
		/// The data pipe.
		/// </param>
		/// <param name="remoteEndPoint">
		/// The remote end point.
		/// </param>
		/// <param name="size">
		/// The size.
		/// </param>
		protected override void ProcessReceivedData(Pipe dataPipe, EndPoint remoteEndPoint, int size)
		{
			try
			{
				this.codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (NullReferenceException exc)
			{
				while (this.codec == null)
				{
					Thread.Sleep(0);
				}

				this.codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (Exception e)
			{
				Trace.Write("WGateWay Parsing Problem! (Reason: " + e + ")");
			}
		}

		/// <summary>
		/// The config_ config changed.
		/// </summary>
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		private void Config_ConfigChanged(string propertyName)
		{
			if (propertyName == "ServerList")
			{
				this.wrapTargetList(this.GateWay.Config.ServerList.getStringList());
			}
		}
	}
}