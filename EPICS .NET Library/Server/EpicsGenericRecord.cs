// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGenericRecord.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	using System;

	using Epics.Base;
	using Epics.Base.Constants;

	public class EpicsRecord<TDataType> : EpicsRecord
	{
		// Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsRecord{TDataType}"/> class.
		/// </summary>
		internal EpicsRecord(string recordName, EpicsServer server)
			: base(recordName, server)
		{
			try
			{
				if (typeof(TDataType).IsArray)
				{
					throw new Exception("Use EpicsArrayRecord for Arrays!");
				}
				else
				{
					this.TYPE = EpicsCodec.CTypeTranslator[typeof(TDataType)];
				}

				this.dataCount = 1;
			}
			catch (Exception e)
			{
				throw new Exception("Type not Supported by Epics Server");
			}
		}

		/// <summary>
		/// Value of the Record.
		/// </summary>
		public TDataType VAL
		{
			get
			{
				this.updateValue();

				if (this.value == null)
				{
					return (TDataType)Convert.ChangeType(0, typeof(TDataType));
				}
				else
				{
					return (TDataType)this.value;
				}
			}

			set
			{
				this.value = value;
				this.handleChange(RecordProperty.VAL, value);
				this.reEvaluateSeverity();
			}
		}

		internal override Type GetValueType()
		{
			return typeof(TDataType);
		}

		protected override void reEvaluateSeverity()
		{
			var valType = typeof(TDataType);

			if (valType == typeof(int) || valType == typeof(double) || valType == typeof(short) || valType == typeof(float))
			{
				// it's numeric so there could be some kind of severity
				var numVal = Convert.ToDouble(this.value);
				if (numVal > this.highAlertLimit && this.highAlertLimit != 0)
				{
					if (this.status != Status.HIHI)
					{
						base.STAT = Status.HIHI;
					}

					if (this.severity != this.highAlertSeverity)
					{
						base.SEVR = this.highAlertSeverity;
					}
				}
				else if (numVal > this.highAlarmLimit && this.highAlarmLimit != 0)
				{
					if (this.status != Status.HIGH)
					{
						this.STAT = Status.HIGH;
					}

					if (this.severity != this.highAlarmSeverity)
					{
						this.SEVR = this.highAlarmSeverity;
					}
				}
				else if (numVal < this.lowAlertLimit && this.lowAlertLimit != 0)
				{
					if (this.status != Status.LOLO)
					{
						this.STAT = Status.LOLO;
					}

					if (this.severity != this.lowAlertSeverity)
					{
						this.SEVR = this.lowAlertSeverity;
					}
				}
				else if (numVal < this.lowAlarmLimit && this.lowAlarmLimit != 0)
				{
					if (this.status != Status.LOW)
					{
						this.STAT = Status.LOW;
					}

					if (this.severity != this.lowAlarmSeverity)
					{
						this.SEVR = this.lowAlarmSeverity;
					}
				}
				else
				{
					if (this.status != Status.NO_ALARM)
					{
						this.STAT = Status.NO_ALARM;
					}

					if (this.severity != Severity.NO_ALARM)
					{
						this.SEVR = Severity.NO_ALARM;
					}
				}

				this.waitForValueSetComplete.Set();
			}
		}
	}
}