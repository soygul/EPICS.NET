// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsTransitionStatus.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// The epics transition status.
	/// </summary>
	internal enum EpicsTransitionStatus
	{
		/// <summary>
		///   The ec a_ normal.
		/// </summary>
		ECA_NORMAL = 0, 

		/// <summary>
		///   The ec a_ maxioc.
		/// </summary>
		ECA_MAXIOC = 1, 

		/// <summary>
		///   The ec a_ uknhost.
		/// </summary>
		ECA_UKNHOST = 2, 

		/// <summary>
		///   The ec a_ uknserv.
		/// </summary>
		ECA_UKNSERV = 3, 

		/// <summary>
		///   The ec a_ sock.
		/// </summary>
		ECA_SOCK = 4, 

		/// <summary>
		///   The ec a_ conn.
		/// </summary>
		ECA_CONN = 5, 

		/// <summary>
		///   The ec a_ allocmem.
		/// </summary>
		ECA_ALLOCMEM = 6, 

		/// <summary>
		///   The ec a_ uknchan.
		/// </summary>
		ECA_UKNCHAN = 7, 

		/// <summary>
		///   The ec a_ uknfield.
		/// </summary>
		ECA_UKNFIELD = 8, 

		/// <summary>
		///   The ec a_ tolarge.
		/// </summary>
		ECA_TOLARGE = 9, 

		/// <summary>
		///   The ec a_ timeout.
		/// </summary>
		ECA_TIMEOUT = 10, 

		/// <summary>
		///   The ec a_ nosupport.
		/// </summary>
		ECA_NOSUPPORT = 11, 

		/// <summary>
		///   The ec a_ strtobig.
		/// </summary>
		ECA_STRTOBIG = 12, 

		/// <summary>
		///   The ec a_ disconnchid.
		/// </summary>
		ECA_DISCONNCHID = 13, 

		/// <summary>
		///   The ec a_ badtype.
		/// </summary>
		ECA_BADTYPE = 14, 

		/// <summary>
		///   The ec a_ chidnotfnd.
		/// </summary>
		ECA_CHIDNOTFND = 15, 

		/// <summary>
		///   The ec a_ chidretry.
		/// </summary>
		ECA_CHIDRETRY = 16, 

		/// <summary>
		///   The ec a_ internal.
		/// </summary>
		ECA_INTERNAL = 17, 

		/// <summary>
		///   The ec a_ dblclfail.
		/// </summary>
		ECA_DBLCLFAIL = 18, 

		/// <summary>
		///   The ec a_ getfail.
		/// </summary>
		ECA_GETFAIL = 19, 

		/// <summary>
		///   The ec a_ putfail.
		/// </summary>
		ECA_PUTFAIL = 20, 

		/// <summary>
		///   The ec a_ addfail.
		/// </summary>
		ECA_ADDFAIL = 21, 

		/// <summary>
		///   The ec a_ badcount.
		/// </summary>
		ECA_BADCOUNT = 22, 

		/// <summary>
		///   The ec a_ badstr.
		/// </summary>
		ECA_BADSTR = 23, 

		/// <summary>
		///   The ec a_ disconn.
		/// </summary>
		ECA_DISCONN = 24, 

		/// <summary>
		///   The ec a_ dblchnl.
		/// </summary>
		ECA_DBLCHNL = 25, 

		/// <summary>
		///   The ec a_ evdisallow.
		/// </summary>
		ECA_EVDISALLOW = 26, 

		/// <summary>
		///   The ec a_ buildget.
		/// </summary>
		ECA_BUILDGET = 27, 

		/// <summary>
		///   The ec a_ needsfp.
		/// </summary>
		ECA_NEEDSFP = 28, 

		/// <summary>
		///   The ec a_ ovevfail.
		/// </summary>
		ECA_OVEVFAIL = 29, 

		/// <summary>
		///   The ec a_ badmonid.
		/// </summary>
		ECA_BADMONID = 30, 

		/// <summary>
		///   The ec a_ newaddr.
		/// </summary>
		ECA_NEWADDR = 31, 

		/// <summary>
		///   The ec a_ newconn.
		/// </summary>
		ECA_NEWCONN = 32, 

		/// <summary>
		///   The ec a_ nocactx.
		/// </summary>
		ECA_NOCACTX = 33, 

		/// <summary>
		///   The ec a_ defunct.
		/// </summary>
		ECA_DEFUNCT = 34, 

		/// <summary>
		///   The ec a_ emptystr.
		/// </summary>
		ECA_EMPTYSTR = 35, 

		/// <summary>
		///   The ec a_ norepeater.
		/// </summary>
		ECA_NOREPEATER = 36, 

		/// <summary>
		///   The ec a_ nochanmsg.
		/// </summary>
		ECA_NOCHANMSG = 37, 

		/// <summary>
		///   The ec a_ dlckrest.
		/// </summary>
		ECA_DLCKREST = 38, 

		/// <summary>
		///   The ec a_ servbehind.
		/// </summary>
		ECA_SERVBEHIND = 39, 

		/// <summary>
		///   The ec a_ nocast.
		/// </summary>
		ECA_NOCAST = 40, 

		/// <summary>
		///   The ec a_ badmask.
		/// </summary>
		ECA_BADMASK = 41, 

		/// <summary>
		///   The ec a_ iodone.
		/// </summary>
		ECA_IODONE = 42, 

		/// <summary>
		///   The ec a_ ioinprogress.
		/// </summary>
		ECA_IOINPROGRESS = 43, 

		/// <summary>
		///   The ec a_ badsyncgrp.
		/// </summary>
		ECA_BADSYNCGRP = 44, 

		/// <summary>
		///   The ec a_ putcbinprog.
		/// </summary>
		ECA_PUTCBINPROG = 45, 

		/// <summary>
		///   The ec a_ nordaccess.
		/// </summary>
		ECA_NORDACCESS = 46, 

		/// <summary>
		///   The ec a_ nowtaccess.
		/// </summary>
		ECA_NOWTACCESS = 47, 

		/// <summary>
		///   The ec a_ anachronism.
		/// </summary>
		ECA_ANACHRONISM = 48, 

		/// <summary>
		///   The ec a_ nosearchaddr.
		/// </summary>
		ECA_NOSEARCHADDR = 49, 

		/// <summary>
		///   The ec a_ noconvert.
		/// </summary>
		ECA_NOCONVERT = 50, 

		/// <summary>
		///   The ec a_ badchid.
		/// </summary>
		ECA_BADCHID = 51, 

		/// <summary>
		///   The ec a_ badfuncptr.
		/// </summary>
		ECA_BADFUNCPTR = 52, 

		/// <summary>
		///   The ec a_ isattached.
		/// </summary>
		ECA_ISATTACHED = 53, 

		/// <summary>
		///   The ec a_ unavailinserv.
		/// </summary>
		ECA_UNAVAILINSERV = 54, 

		/// <summary>
		///   The ec a_ chandestroy.
		/// </summary>
		ECA_CHANDESTROY = 55, 

		/// <summary>
		///   The ec a_ badpriority.
		/// </summary>
		ECA_BADPRIORITY = 56, 

		/// <summary>
		///   The ec a_ notthreaded.
		/// </summary>
		ECA_NOTTHREADED = 57, 

		/// <summary>
		///   The ec a_16 karrayclient.
		/// </summary>
		ECA_16KARRAYCLIENT = 58, 

		/// <summary>
		///   The ec a_ connseqtmo.
		/// </summary>
		ECA_CONNSEQTMO = 59
	}
}