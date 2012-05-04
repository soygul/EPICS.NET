// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsArray.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System;

	#endregion

	/// <summary>
	/// The handle change delegate.
	/// </summary>
	internal delegate void handleChangeDelegate();

	/// <summary>
	/// EpicsArray allows to observe changes on single fields.
	/// </summary>
	/// <typeparam name="DataType">
	/// type of array to manipulate
	/// </typeparam>
	public class EpicsArray<DataType>
	{
		/// <summary>
		///   The values.
		/// </summary>
		private DataType[] values;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsArray{DataType}"/> class. 
		///   Constructor
		/// </summary>
		/// <param name="init">
		/// init Array
		/// </param>
		public EpicsArray(DataType[] init)
		{
			this.values = init;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsArray{DataType}"/> class. 
		///   Constructor
		/// </summary>
		/// <param name="count">
		/// dimension of the Array
		/// </param>
		public EpicsArray(int count)
		{
			this.values = new DataType[count];
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsArray{DataType}" /> class. 
		///   Constructor
		/// </summary>
		public EpicsArray()
		{
		}

		/// <summary>
		///   The data manipulated.
		/// </summary>
		internal event handleChangeDelegate DataManipulated;

		/// <summary>
		///   retrieve or manipulate the length of the array
		/// </summary>
		public int Count
		{
			get
			{
				return this.values.Length;
			}

			set
			{
				var tmpArray = new DataType[value];

				for (var i = 0; i < value; i++)
				{
					tmpArray[i] = this.values[i];
				}

				this.values = tmpArray;
			}
		}

		/// <summary>
		///   Direct Access to the Data
		/// </summary>
		/// <param name = "key">numeric position starting by 0</param>
		/// <returns>value at requested position</returns>
		public DataType this[int key]
		{
			get
			{
				return this.values[key];
			}

			set
			{
				this.values[key] = value;
				this.DataManipulated();
			}
		}

		/// <summary>
		/// Returns an Array of Values of the given size
		/// </summary>
		/// <param name="size">
		/// size of the array
		/// </param>
		/// <returns>
		/// array of values
		/// </returns>
		public DataType[] Get(int size)
		{
			if (size > this.values.Length)
			{
				throw new IndexOutOfRangeException();
			}

			var tmpArray = new DataType[size];

			for (var i = 0; i < size; i++)
			{
				tmpArray[i] = this.values[i];
			}

			return tmpArray;
		}

		/// <summary>
		/// returns the full value array
		/// </summary>
		/// <returns>
		/// array of values
		/// </returns>
		public DataType[] Get()
		{
			return this.values;
		}

		/// <summary>
		/// Sets an array of new values, but will not change the size of the RecordArray-Size.
		/// </summary>
		/// <param name="newValues">
		/// array of new data
		/// </param>
		public void Set(DataType[] newValues)
		{
			if (newValues.Length > this.values.Length)
			{
				throw new IndexOutOfRangeException();
			}

			for (var i = 0; i < newValues.Length; i++)
			{
				this.values[i] = newValues[i];
			}

			this.DataManipulated();
		}

		/// <summary>
		/// Replaces the array with a new one, will take the size of the new array.
		/// </summary>
		/// <param name="newValues">
		/// array of new values
		/// </param>
		internal void Replace(DataType[] newValues)
		{
			this.values = newValues;

			this.DataManipulated();
		}

		/// <summary>
		/// The set.
		/// </summary>
		/// <param name="newValues">
		/// The new values.
		/// </param>
		internal void Set(object newValues)
		{
			var type = newValues.GetType().GetElementType();

			// check if same-type, if yes we can prevent stupid converts.
			if (type == typeof(DataType))
			{
				this.Set((DataType[])newValues);
			}

			if (type == typeof(string))
			{
				var arr = (string[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}
			else if (type == typeof(int))
			{
				var arr = (int[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}
			else if (type == typeof(short))
			{
				var arr = (short[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}
			else if (type == typeof(double))
			{
				var arr = (double[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}
			else if (type == typeof(float))
			{
				var arr = (float[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}
			else if (type == typeof(SByte))
			{
				var arr = (SByte[])newValues;
				for (var i = 0; i < arr.Length; i++)
				{
					this.values[i] = (DataType)Convert.ChangeType(arr[i], typeof(DataType));
				}
			}

			this.DataManipulated();
		}
	}
}