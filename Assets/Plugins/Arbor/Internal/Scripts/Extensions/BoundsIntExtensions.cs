//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Extensions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// BoundsInt extension class
	/// </summary>
#endif
	public static class BoundsIntExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt(0, 0, 0, 0, 0, 0)を返す。
		/// </summary>
#else
		/// <summary>
		/// Returns BoundsInt (0, 0, 0, 0, 0, 0).
		/// </summary>
#endif
		public static BoundsInt zero
		{
			get
			{
				return new BoundsInt(0, 0, 0, 0, 0, 0);
			}
		}

#if !UNITY_2020_1_OR_NEWER
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntを文字列に変換する。
		/// </summary>
		/// <param name="boundsInt">文字列に変換するBoundsInt</param>
		/// <param name="format">フォーマット</param>
		/// <returns>変換した文字列。</returns>
#else
		/// <summary>
		/// Convert BoundsInt to a string.
		/// </summary>
		/// <param name="boundsInt">BoundsInt to convert to a string</param>
		/// <param name="format">Format</param>
		/// <returns>Converted string.</returns>
#endif
		public static string ToString(this BoundsInt boundsInt, string format)
		{
			return string.Format("Position: {0}, Size: {1}", boundsInt.position.ToString(format), boundsInt.size.ToString(format));
		}
#endif
	}
}