// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientServerModelTests.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Tests.Integration.EpicsLibraryTests
{
	using Epics.Client;
	using Epics.Server;

	using Xunit;

	public class ClientServerModelTests
	{
		[Fact]
		public void ClientServer()
		{
			using (var server = new EpicsServer())
			using (var client = new EpicsClient())
			{
				server.Config.BeaconPort = 95;
				server.Config.TCPPort = 115;
				server.Config.UDPPort = 115;
				server.Config.UDPDestPort = 115;

				var record = server.GetEpicsRecord<double>("OMA:Counter");
				record.HIGH = 50;
				record.HIHI = 75;
				record.VAL = 19;

				client.Config.UDPBeaconPort = 95;
				client.Config.UDPIocPort = 115;

				var monitor = client.CreateChannel("OMA:Counter");
				Assert.Equal(19, monitor.Get());
			}
		}
	}
}
