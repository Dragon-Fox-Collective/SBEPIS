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
	/// Vector3Intの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// Vector3Int extension class
	/// </summary>
#endif
	public static class Vector3IntExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Intを整数で除算する。
		/// </summary>
		/// <param name="v">被除数のVector3Int</param>
		/// <param name="i">除数の整数</param>
		/// <returns>除算したVector3Intを返す。</returns>
#else
		/// <summary>
		/// Divide Vector3Int by an integer.
		/// </summary>
		/// <param name="v">Vector3Int of the divisor</param>
		/// <param name="i">Integer for the divisor</param>
		/// <returns>Returns the divided Vector3Int.</returns>
#endif
		public static Vector3Int Div(this Vector3Int v, int i)
		{
			return v / i;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Intの各成分の符号を反転する
		/// </summary>
		/// <param name="v">各成分の符号を反転するVector3Int</param>
		/// <returns>各成分の符号を反転した結果を返す。</returns>
#else
		/// <summary>
		/// Invert the sign of each component of Vector3Int.
		/// </summary>
		/// <param name="v">Vector3Int to reverse the sign of each component</param>
		/// <returns>Returns the result of reversing the sign of each component.</returns>
#endif
		public static Vector3Int Negative(this Vector3Int v)
		{
			return new Vector3Int(-v.x, -v.y, -v.z);
		}
	}
}