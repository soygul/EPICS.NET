// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigList.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// String List for Epics Configuration
	/// </summary>
	public class ConfigList
	{
		/// <summary>
		///   The item list.
		/// </summary>
		private readonly List<string> itemList = new List<string>();

		/// <summary>
		///   Initializes a new instance of the <see cref = "ConfigList" /> class. 
		///   Constructor
		/// </summary>
		public ConfigList()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigList"/> class. 
		///   Constructor
		/// </summary>
		/// <param name="item">
		/// single Item
		/// </param>
		public ConfigList(string item)
		{
			this.itemList.Add(item);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigList"/> class. 
		///   Constructor
		/// </summary>
		/// <param name="items">
		/// string list of items
		/// </param>
		public ConfigList(List<string> items)
		{
			this.itemList = items;
		}

		/// <summary>
		/// The list changed.
		/// </summary>
		internal delegate void ListChanged();

		/// <summary>
		///   The config changed.
		/// </summary>
		internal event ListChanged ConfigChanged;

		/// <summary>
		///   Count of items contained by the list
		/// </summary>
		public int Count
		{
			get
			{
				return this.itemList.Count;
			}

			private set
			{
			}
		}

		/// <summary>
		/// Adds an Item to the ConfigList
		/// </summary>
		/// <param name="item">
		/// new Item
		/// </param>
		public void Add(string item)
		{
			this.itemList.Add(item);
			if (this.ConfigChanged != null)
			{
				this.ConfigChanged();
			}
		}

		/// <summary>
		/// Drops the whole ConfigList
		/// </summary>
		public void Clear()
		{
			this.itemList.Clear();
			if (this.ConfigChanged != null)
			{
				this.ConfigChanged();
			}
		}

		/// <summary>
		/// Removes an item from the ConfigList
		/// </summary>
		/// <param name="item">
		/// </param>
		public void Remove(string item)
		{
			this.itemList.Remove(item);
			if (this.ConfigChanged != null)
			{
				this.ConfigChanged();
			}
		}

		/// <summary>
		/// returns all configured items in a string list
		/// </summary>
		/// <returns>
		/// string list of all values
		/// </returns>
		public List<string> getStringList()
		{
			return this.itemList;
		}
	}
}