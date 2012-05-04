// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsArrayRecord.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	using System;

	using Epics.Base;

	public class EpicsArrayRecord<TDataType> : EpicsRecord
	{
		internal EpicsArrayRecord(string recordName, EpicsServer server, int size)
			: base(recordName, server)
		{
			try
			{
				this.TYPE = EpicsCodec.CTypeTranslator[typeof(TDataType)];
				var data = new EpicsArray<TDataType>(size);
				data.DataManipulated += this.DataManipulated;
				this.dataCount = size;
				this.value = data;
			}
			catch (Exception e)
			{
				throw new Exception("Type not Supported by Epics Server");
			}
		}

		public EpicsArray<TDataType> VAL
		{
			get
			{
				return (EpicsArray<TDataType>)this.value;
			}

			private set
			{
				throw new Exception("WRONG CALL");
			}
		}

		internal void DataManipulated()
		{
			this.handleChange(RecordProperty.VAL, this.value);
		}

		internal override Type GetValueType()
		{
			return typeof(TDataType);
		}
	}
}