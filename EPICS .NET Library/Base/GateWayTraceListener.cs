// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateWayTraceListener.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System.Diagnostics;

	#endregion

	/// <summary>
	/// The gate way trace listener.
	/// </summary>
	public class GateWayTraceListener : TextWriterTraceListener
	{
		/// <summary>
		///   The id.
		/// </summary>
		private int id;

		/// <summary>
		/// The write.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public override sealed void Write(string message)
		{
			var type = message[0];
			string category;
			this.id++;

			switch (type)
			{
				case 'I':
					category = "Information";
					this.TraceEvent(new TraceEventCache(), string.Empty, TraceEventType.Information, this.id, message.Substring(1));
					break;
				case 'E':
					category = "Error";
					this.TraceEvent(new TraceEventCache(), string.Empty, TraceEventType.Error, this.id, message.Substring(1));
					break;
				case 'W':
					category = "Warning";
					this.TraceEvent(new TraceEventCache(), string.Empty, TraceEventType.Warning, this.id, message.Substring(1));
					break;
				case 'D':
					category = "Debug";
					break;
				default:
					category = "Unkown";
					break;
			}

			this.WriteLine(message.Substring(1));
			this.WriteLine(message.Substring(1), category);

			base.Write(message);
		}
	}
}