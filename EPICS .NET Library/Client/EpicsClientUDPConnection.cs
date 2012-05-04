// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsClientUDPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	#region Using Directives

	using System;
	using System.Net;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// The epics client udp connection.
	/// </summary>
	internal class EpicsClientUDPConnection : EpicsUDPConnection
	{
		/// <summary>
		///   The client.
		/// </summary>
		private readonly EpicsClient Client;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsClientUDPConnection"/> class.
		/// </summary>
		/// <param name="client">
		/// The client.
		/// </param>
		public EpicsClientUDPConnection(EpicsClient client)
		{
			this.Client = client;
			this.Client.Config.ConfigChanged += this.config_ConfigChanged;

			this.UDPSocket.SendBufferSize = client.Config.UDPBufferSize;
			this.UDPSocket.ReceiveBufferSize = client.Config.UDPBufferSize;

			// just be sure it's set
			this.wrapTargetList(this.Client.Config.ServerList.getStringList());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsClientUDPConnection"/> class.
		/// </summary>
		/// <param name="client">
		/// The client.
		/// </param>
		/// <param name="port">
		/// The port.
		/// </param>
		public EpicsClientUDPConnection(EpicsClient client, int port)
			: base(port)
		{
			this.Client = client;

			this.UDPSocket.SendBufferSize = client.Config.UDPBufferSize;
			this.UDPSocket.ReceiveBufferSize = client.Config.UDPBufferSize;
		}

		/// <summary>
		/// The answer to.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <param name="iep">
		/// The iep.
		/// </param>
		internal void AnswerTo(byte[] data, EndPoint iep)
		{
			this.UDPSocket.SendTo(data, iep);
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
				this.Client.Codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (NullReferenceException exc)
			{
				while (this.Client == null)
				{
					Thread.Sleep(0);
				}

				this.Client.Codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (Exception e)
			{
				this.Client.ExceptionContainer.Add(e);
			}
		}

		/// <summary>
		/// The config_ config changed.
		/// </summary>
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		private void config_ConfigChanged(string propertyName)
		{
			if (propertyName == "ServerList")
			{
				this.wrapTargetList(this.Client.Config.ServerList.getStringList());
			}
		}
	}
}