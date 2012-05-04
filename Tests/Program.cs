// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Tests
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	using Epics.Client;
	using Epics.Server;

	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			/*Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TestApp());*/

			var client = new EpicsClient();
			var monitor = client.CreateChannel("TestChannel01:Counter");
			monitor.MonitorChanged += (sender, value) => Console.WriteLine(value);
			
			Task.Factory.StartNew(() =>
				{
					var server = new EpicsServer();
					var record = server.GetEpicsRecord<string>("TestChannel01:Counter");
					
					for (int i = 0; i < 500; i++)
					{
						record.VAL = i.ToString();
						Thread.Sleep(1000);
					}
				});



			/*var server = new EpicsServer();
			var record = server.GetEpicsRecord<double>("TestChannel01:Counter");
			record.HIGH = 50;
			record.HIHI = 75;
			record.VAL = 19;

			for (int i = 0; i < 10; i++)
			{
				record.VAL = i;
				Thread.Sleep(1000);
			}*/

			Console.ReadKey();
		}
	}
}
