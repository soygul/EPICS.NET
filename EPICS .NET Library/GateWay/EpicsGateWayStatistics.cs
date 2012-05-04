// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayStatistics.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	/// <summary>
	/// The epicsgate way stat names.
	/// </summary>
	public enum EpicsgateWayStatNames
	{
		/// <summary>
		///   The puts.
		/// </summary>
		Puts, 

		/// <summary>
		///   The gets.
		/// </summary>
		Gets, 

		/// <summary>
		///   The searches.
		/// </summary>
		Searches, 

		/// <summary>
		///   The new connections.
		/// </summary>
		NewConnections, 

		/// <summary>
		///   The closed connections.
		/// </summary>
		ClosedConnections, 

		/// <summary>
		///   The bytes received.
		/// </summary>
		BytesReceived, 

		/// <summary>
		///   The bytes send.
		/// </summary>
		BytesSend, 

		/// <summary>
		///   The client connected.
		/// </summary>
		ClientConnected, 

		/// <summary>
		///   The server connected.
		/// </summary>
		ServerConnected, 

		/// <summary>
		///   The monitors running.
		/// </summary>
		MonitorsRunning, 

		/// <summary>
		///   The open server channels.
		/// </summary>
		OpenServerChannels, 

		/// <summary>
		///   The open client channels.
		/// </summary>
		OpenClientChannels
	}

	/// <summary>
	/// The epics gate way statistics.
	/// </summary>
	public class EpicsGateWayStatistics
	{
		/// <summary>
		///   The client connections.
		/// </summary>
		private int clientConnections;

		/// <summary>
		///   The server connections.
		/// </summary>
		private int serverConnections;

		/// <summary>
		///   Gets or sets BytesReceived.
		/// </summary>
		public int BytesReceived { get; set; }

		/// <summary>
		///   Gets or sets BytesSend.
		/// </summary>
		public int BytesSend { get; set; }

		/// <summary>
		///   Gets or sets ClientConnected.
		/// </summary>
		public int ClientConnected
		{
			get
			{
				return this.clientConnections;
			}

			internal set
			{
				if (value > this.clientConnections)
				{
					this.NewConnections++;
				}
				else
				{
					this.ClosedConnections++;
				}

				this.clientConnections = value;
			}
		}

		/// <summary>
		///   Gets or sets ClosedConnections.
		/// </summary>
		public int ClosedConnections { get; set; }

		/// <summary>
		///   Gets or sets Gets.
		/// </summary>
		public int Gets { get; set; }

		/// <summary>
		///   Gets or sets MonitorsRunning.
		/// </summary>
		public int MonitorsRunning { get; internal set; }

		/// <summary>
		///   Gets or sets NewConnections.
		/// </summary>
		public int NewConnections { get; set; }

		/// <summary>
		///   Gets or sets OpenClientChannels.
		/// </summary>
		public int OpenClientChannels { get; internal set; }

		/// <summary>
		///   Gets or sets OpenServerChannels.
		/// </summary>
		public int OpenServerChannels { get; internal set; }

		/// <summary>
		///   Gets or sets Puts.
		/// </summary>
		public int Puts { get; set; }

		/// <summary>
		///   Gets or sets Searches.
		/// </summary>
		public int Searches { get; set; }

		/// <summary>
		///   Gets or sets ServerConnected.
		/// </summary>
		public int ServerConnected
		{
			get
			{
				return this.serverConnections;
			}

			internal set
			{
				if (value > this.serverConnections)
				{
					this.NewConnections++;
				}
				else
				{
					this.ClosedConnections++;
				}

				this.serverConnections = value;
			}
		}
	}
}