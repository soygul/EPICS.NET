// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.VirtualAccelerator
{
	using System;
	using System.Windows.Forms;

	using DevExpress.Skins;

	using Microsoft.VisualBasic.ApplicationServices;

	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
			Application.ThreadException += NBug.Handler.ThreadException;
			System.Threading.Tasks.TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;

			SkinManager.EnableFormSkins();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			#if !DEBUG // If this is a release build, display the splash screen
				new MyApp().Run(args);
			#else
				Application.Run(new MainForm());
			#endif
		}

		#if !DEBUG // If this is a release build, display the splash screen
		class MyApp : WindowsFormsApplicationBase
		{
			protected override void OnCreateSplashScreen()
			{
				this.SplashScreen = new Forms.SplashForm();
			}
			protected override void OnCreateMainForm()
			{
				// Do your time consuming stuff here...
				// ...
				System.Threading.Thread.Sleep(3000); // Display the splash screen during startup + 3 seconds
				// Then create the main form, the splash screen will close automatically
				this.MainForm = new MainForm();
			}
		}
		#endif
	}
}
