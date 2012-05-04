// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValue.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	public interface IValue<T>
	{
		T Value { get; set; }
	}
}
