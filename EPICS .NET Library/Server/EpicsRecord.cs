// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsRecord.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Threading;

	using Epics.Base.Constants;

	public delegate void PropertyChangedDelegate(EpicsRecord sender, RecordProperty prop, object newValue, Setter setter);

	public delegate void ValueCallBackFunction(EpicsRecord sender, RecordProperty prop, out object Value);

	public enum Setter
	{
		local,
		external
	}

	/// <summary>
	/// C# Simulation of a EpicsRecord where properties and alerts will be behaving as normal
	///   epicschannels. So it's easily possible to publish it to other EpicsServers or IOCs.
	/// </summary>
	public abstract class EpicsRecord : IDisposable
	{
		internal int dataCount;

		internal EpicsType type = 0;

		internal object value;

		protected EpicsServer Server;

		protected string accessRightGroup = "DEFAULT";

		protected string description = string.Empty;

		protected string egu = string.Empty;

		protected double filter;

		protected double highAlarmLimit;

		protected Severity highAlarmSeverity = Severity.MINOR;

		/// <summary>
		///   The high alert limit.
		/// </summary>
		protected double highAlertLimit;

		/// <summary>
		///   The high alert severity.
		/// </summary>
		protected Severity highAlertSeverity = Severity.MAJOR;

		/// <summary>
		///   The high control limit.
		/// </summary>
		protected double highControlLimit;

		/// <summary>
		///   The high display limit.
		/// </summary>
		protected double highDisplayLimit;

		/// <summary>
		///   The last change.
		/// </summary>
		protected DateTime lastChange = DateTime.Now;

		/// <summary>
		///   The last monitored value.
		/// </summary>
		protected double lastMonitoredValue;

		/// <summary>
		///   The low alarm limit.
		/// </summary>
		protected double lowAlarmLimit;

		/// <summary>
		///   The low alarm severity.
		/// </summary>
		protected Severity lowAlarmSeverity = Severity.MINOR;

		/// <summary>
		///   The low alert limit.
		/// </summary>
		protected double lowAlertLimit;

		/// <summary>
		///   The low alert severity.
		/// </summary>
		protected Severity lowAlertSeverity = Severity.MAJOR;

		/// <summary>
		///   The low control limit.
		/// </summary>
		protected double lowControlLimit;

		/// <summary>
		///   The low display limit.
		/// </summary>
		protected double lowDisplayLimit;

		/// <summary>
		///   The name.
		/// </summary>
		protected string name;

		/// <summary>
		///   The precision.
		/// </summary>
		protected short precision;

		/// <summary>
		///   The property access.
		/// </summary>
		protected Dictionary<string, MethodInfo> propertyAccess = new Dictionary<string, MethodInfo>();

		/// <summary>
		///   The scan interval.
		/// </summary>
		protected int scanInterval;

		/// <summary>
		///   The severity.
		/// </summary>
		protected Severity severity = Severity.INVALID;

		/// <summary>
		///   The status.
		/// </summary>
		protected Status status = Status.UDF;

		/// <summary>
		///   The wait for value set complete.
		/// </summary>
		protected AutoResetEvent waitForValueSetComplete = new AutoResetEvent(false);

		/// <summary>
		///   The change list.
		/// </summary>
		private readonly Dictionary<RecordProperty, KeyValuePair<object, Setter>> changeList =
			new Dictionary<RecordProperty, KeyValuePair<object, Setter>>();

		/// <summary>
		///   The set mode.
		/// </summary>
		private Setter SetMode = Setter.local;

		/// <summary>
		///   The automated.
		/// </summary>
		private bool automated;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsRecord"/> class.
		/// </summary>
		/// <param name="Name">
		/// The name.
		/// </param>
		/// <param name="server">
		/// The server.
		/// </param>
		internal EpicsRecord(string Name, EpicsServer server)
		{
			this.name = Name;
			this.Server = server;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsRecord" /> class.
		/// </summary>
		protected EpicsRecord()
		{
		}

		/// <summary>
		///   The call back function.
		/// </summary>
		public event ValueCallBackFunction CallBackFunction;

		/// <summary>
		///   Event which announces changes on a Property. Will call back the function with a Property-identifier
		///   and the new value. If SCAN is set, it will only be calling on those intervals.
		/// </summary>
		public event PropertyChangedDelegate PropertyChanged
		{
			add
			{
				if (this.PrivPropertyChanged == null && this.automated == false)
				{
					this.Server.SetMonitor(this, this.scanInterval);
				}

				this.PrivPropertyChanged += value;
			}

			remove
			{
				this.PrivPropertyChanged -= value;
				if (this.PrivPropertyChanged == null && this.automated == false)
				{
					this.Server.SetMonitor(this, 0);
				}
			}
		}

		/// <summary>
		///   The priv property changed.
		/// </summary>
		private event PropertyChangedDelegate PrivPropertyChanged;

		/// <summary>
		///   Gets ASG.
		/// </summary>
		public string ASG
		{
			get
			{
				return this.accessRightGroup;
			}

			private set
			{
				if (this.accessRightGroup == value)
				{
					return;
				}

				this.accessRightGroup = value;
				this.handleChange(RecordProperty.ASG, value);
			}
		}

		/// <summary>
		///   Changes the Behavior of the Internal EpicsMonitors. 
		///   If not automated it will only actualize if somebody is listening, if it is automated
		///   it will always refresh itself at the scaninterval rate. If no scaninterval or no valuecallback
		///   is set, nothing will change.
		/// </summary>
		public bool AUTOMATED
		{
			get
			{
				return this.automated;
			}

			set
			{
				if (this.automated == value)
				{
					return;
				}

				this.automated = value;

				// switched to optimized mode which will only run if somebody is listening
				if (this.automated == false)
				{
					if (this.PrivPropertyChanged != null)
					{
						return;
					}
					else
					{
						this.Server.SetMonitor(this, 0);
					}
				}
				else
				{
					// switch to always running
					if (this.PrivPropertyChanged != null)
					{
						return;
					}
					else
					{
						this.Server.SetMonitor(this, this.scanInterval);
					}
				}
			}
		}

		/// <summary>
		///   Gets or sets DESC.
		/// </summary>
		public string DESC
		{
			get
			{
				return this.description;
			}

			set
			{
				if (this.description == value)
				{
					return;
				}

				this.description = value;
				this.handleChange(RecordProperty.DESC, value);
			}
		}

		// Properties

		/// <summary>
		///   EnGineer Unit of the Value (shouldn't be longer then 10 characters)
		/// </summary>
		public string EGU
		{
			get
			{
				return this.egu;
			}

			set
			{
				if (this.egu == value)
				{
					return;
				}

				this.egu = value;
				this.handleChange(RecordProperty.EGU, value);
			}
		}

		/// <summary>
		///   Gets or sets HHSV.
		/// </summary>
		public Severity HHSV
		{
			get
			{
				return this.highAlertSeverity;
			}

			set
			{
				if (this.highAlertSeverity == value)
				{
					return;
				}

				this.highAlertSeverity = value;
				this.handleChange(RecordProperty.HHSV, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets HIGH.
		/// </summary>
		public double HIGH
		{
			get
			{
				return this.highAlarmLimit;
			}

			set
			{
				if (this.highAlarmLimit == value)
				{
					return;
				}

				this.highAlarmLimit = value;
				this.handleChange(RecordProperty.HIGH, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets HIHI.
		/// </summary>
		public double HIHI
		{
			get
			{
				return this.highAlertLimit;
			}

			set
			{
				if (this.highAlertLimit == value)
				{
					return;
				}

				this.highAlertLimit = value;
				this.handleChange(RecordProperty.HIHI, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets HOPR.
		/// </summary>
		public double HOPR
		{
			get
			{
				return this.highControlLimit;
			}

			set
			{
				if (this.highControlLimit == value)
				{
					return;
				}

				this.highControlLimit = value;
				this.handleChange(RecordProperty.HOPR, value);
			}
		}

		/// <summary>
		///   Gets or sets HSV.
		/// </summary>
		public Severity HSV
		{
			get
			{
				return this.highAlarmSeverity;
			}

			set
			{
				if (this.highAlarmSeverity == value)
				{
					return;
				}

				this.highAlarmSeverity = value;
				this.handleChange(RecordProperty.HSV, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets HighDisplayLimit.
		/// </summary>
		public double HighDisplayLimit
		{
			get
			{
				return this.highDisplayLimit;
			}

			set
			{
				if (this.highDisplayLimit == value)
				{
					return;
				}

				this.highDisplayLimit = value;
				this.handleChange(RecordProperty.HIGHDISP, value);
			}
		}

		/// <summary>
		///   Gets or sets LLSV.
		/// </summary>
		public Severity LLSV
		{
			get
			{
				return this.lowAlertSeverity;
			}

			set
			{
				if (this.lowAlertSeverity == value)
				{
					return;
				}

				this.lowAlertSeverity = value;
				this.handleChange(RecordProperty.LLSV, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Low Alert Limit
		/// </summary>
		public double LOLO
		{
			get
			{
				return this.lowAlertLimit;
			}

			set
			{
				if (this.lowAlertLimit == value)
				{
					return;
				}

				this.lowAlertLimit = value;
				this.handleChange(RecordProperty.LOLO, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets LOPR.
		/// </summary>
		public double LOPR
		{
			get
			{
				return this.lowControlLimit;
			}

			set
			{
				if (this.lowControlLimit == value)
				{
					return;
				}

				this.lowControlLimit = value;
				this.handleChange(RecordProperty.LOPR, value);
			}
		}

		/// <summary>
		///   Low Alarm Limit
		/// </summary>
		public double LOW
		{
			get
			{
				return this.lowAlarmLimit;
			}

			set
			{
				if (this.lowAlarmLimit == value)
				{
					return;
				}

				this.lowAlarmLimit = value;
				this.handleChange(RecordProperty.LOW, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets LSV.
		/// </summary>
		public Severity LSV
		{
			get
			{
				return this.lowAlarmSeverity;
			}

			set
			{
				if (this.lowAlarmSeverity == value)
				{
					return;
				}

				this.lowAlarmSeverity = value;
				this.handleChange(RecordProperty.LSV, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   Gets or sets LowDisplayLimit.
		/// </summary>
		public double LowDisplayLimit
		{
			get
			{
				return this.lowDisplayLimit;
			}

			set
			{
				if (this.lowDisplayLimit == value)
				{
					return;
				}

				this.lowDisplayLimit = value;
				this.handleChange(RecordProperty.LOWDISP, value);
			}
		}

		/// <summary>
		///   Gets or sets MDEL.
		/// </summary>
		public double MDEL
		{
			get
			{
				return this.filter;
			}

			set
			{
				if (this.filter == value)
				{
					return;
				}

				this.filter = value;
				this.handleChange(RecordProperty.MDEL, value);
			}
		}

		/// <summary>
		///   Gets NAME.
		/// </summary>
		public string NAME
		{
			get
			{
				return this.name;
			}

			private set
			{
				if (this.name == value)
				{
					return;
				}

				this.name = value;
				this.handleChange(RecordProperty.NAME, value);
			}
		}

		/// <summary>
		///   Gets or sets PREC.
		/// </summary>
		public short PREC
		{
			get
			{
				return this.precision;
			}

			set
			{
				if (this.precision == value)
				{
					return;
				}

				this.precision = value;
				this.handleChange(RecordProperty.PREC, value);
			}
		}

		/// <summary>
		///   Gets or sets SCANINTERVAL.
		/// </summary>
		public int SCANINTERVAL
		{
			get
			{
				return this.scanInterval;
			}

			set
			{
				if (this.scanInterval == value)
				{
					return;
				}

				this.scanInterval = value;
				this.handleChange(RecordProperty.SCANINTERVAL, value);
			}
		}

		/// <summary>
		///   Gets or sets SEVR.
		/// </summary>
		public Severity SEVR
		{
			get
			{
				return this.severity;
			}

			protected set
			{
				if (this.severity == value)
				{
					return;
				}

				this.severity = value;
				this.handleChange(RecordProperty.SEVR, value);
			}
		}

		/// <summary>
		///   Gets or sets STAT.
		/// </summary>
		public Status STAT
		{
			get
			{
				return this.status;
			}

			protected set
			{
				if (this.status == value)
				{
					return;
				}

				this.status = value;
				this.handleChange(RecordProperty.STAT, value);
			}
		}

		/// <summary>
		///   Gets TIME.
		/// </summary>
		public DateTime TIME
		{
			get
			{
				return this.lastChange;
			}

			private set
			{
				this.lastChange = value;
			}
		}

		/// <summary>
		///   Gets or sets TYPE.
		/// </summary>
		internal EpicsType TYPE
		{
			get
			{
				return this.type;
			}

			set
			{
				this.type = value;
			}
		}

		/// <summary>
		///   Value of the Record
		/// </summary>
		internal object VAL
		{
			get
			{
				return this.value;
			}

			set
			{
				if (this.value == value)
				{
					return;
				}

				this.value = value;
				this.handleChange(RecordProperty.VAL, value);
				this.reEvaluateSeverity();
			}
		}

		/// <summary>
		///   The this.
		/// </summary>
		/// <param name = "key">
		///   The key.
		/// </param>
		internal object this[string key]
		{
			get
			{
				try
				{
					return this.GetPropertyInvoker(key, true).Invoke(this, new object[] { });
				}
				catch (Exception e)
				{
					return null;
				}
			}

			set
			{
				this.SetMode = Setter.external;
				this.GetPropertyInvoker(key, false).Invoke(this, new[] { value });
				this.SetMode = Setter.local;
			}
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.automated || this.PrivPropertyChanged != null)
			{
				this.Server.SetMonitor(this, 0);
			}
		}

		/// <summary>
		/// The get value type.
		/// </summary>
		/// <returns>
		/// </returns>
		internal abstract Type GetValueType();

		/// <summary>
		/// The scan trigger.
		/// </summary>
		internal void scanTrigger()
		{
			if (this.PrivPropertyChanged != null || this.automated)
			{
				if (this.CallBackFunction != null)
				{
					object val;
					this.CallBackFunction(this, RecordProperty.VAL, out val);

					this.VAL = val;

					if (!this.waitForValueSetComplete.WaitOne(this.scanInterval))
					{
						return;
					}
				}
				else if (this.automated)
				{
					return;
				}

				lock (this.changeList)
				{
					foreach (var change in this.changeList)
					{
						this.PrivPropertyChanged(this, change.Key, change.Value.Key, change.Value.Value);
					}

					this.changeList.Clear();
				}
			}
		}

		/// <summary>
		/// The handle change.
		/// </summary>
		/// <param name="prop">
		/// The prop.
		/// </param>
		/// <param name="newVal">
		/// The new val.
		/// </param>
		protected void handleChange(RecordProperty prop, object newVal)
		{
			this.TIME = DateTime.Now;

			// no listeners no handling
			if (this.PrivPropertyChanged != null)
			{
				if (prop == RecordProperty.VAL)
				{
					// filter defined
					if (this.filter > 0)
					{
						var tmpVal = Convert.ToDouble(newVal);
						if (Math.Abs(tmpVal - this.lastMonitoredValue) > this.filter)
						{
							this.lastMonitoredValue = tmpVal;
						}
						else
						{
							return;
						}
					}
				}

				if (this.scanInterval == 0)
				{
					this.PrivPropertyChanged(this, prop, newVal, this.SetMode);
				}
				else
				{
					// scan triggered
					lock (this.changeList)
					{
						if (this.changeList.ContainsKey(prop))
						{
							this.changeList[prop] = new KeyValuePair<object, Setter>(newVal, this.SetMode);
						}
						else
						{
							this.changeList.Add(prop, new KeyValuePair<object, Setter>(newVal, this.SetMode));
						}
					}
				}
			}
		}

		/// <summary>
		/// The re evaluate severity.
		/// </summary>
		protected virtual void reEvaluateSeverity()
		{
			this.waitForValueSetComplete.Set();
			return;
		}

		/// <summary>
		/// The update value.
		/// </summary>
		protected void updateValue()
		{
			if (this.PrivPropertyChanged == null && this.CallBackFunction != null)
			{
				object val;
				this.CallBackFunction(this, RecordProperty.VAL, out val);

				this.waitForValueSetComplete.Reset();
				this.VAL = val;

				if (!this.waitForValueSetComplete.WaitOne(this.scanInterval))
				{
					return;
				}
			}
		}

		/// <summary>
		/// The get property invoker.
		/// </summary>
		/// <param name="property">
		/// The property.
		/// </param>
		/// <param name="getter">
		/// The getter.
		/// </param>
		/// <returns>
		/// </returns>
		private MethodInfo GetPropertyInvoker(string property, bool getter)
		{
			var key = (getter ? "GET_" : "SET_") + property;
			lock (this.propertyAccess)
			{
				if (this.propertyAccess.ContainsKey(key))
				{
					return this.propertyAccess[key];
				}

				PropertyInfo prop;
				if (property == "VAL")
				{
					prop = this.GetType().GetProperties()[0];
				}
				else
				{
					prop = this.GetType().GetProperty(property);
				}

				MethodInfo method;
				if (getter)
				{
					method = prop.GetGetMethod();
				}
				else
				{
					method = prop.GetSetMethod();
				}

				this.propertyAccess.Add(key, method);
				return method;
			}
		}
	}
}