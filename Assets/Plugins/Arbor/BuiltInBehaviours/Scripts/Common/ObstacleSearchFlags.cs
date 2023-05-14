//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 障害物の検索フラグ
	/// </summary>
#else
	/// <summary>
	/// Obstacle search flag
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum ObstacleSearchFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 移動経路を計算する(高負荷)
		/// </summary>
#else
		/// <summary>
		/// Calculate the movement route (high load)
		/// </summary>
#endif
		Path = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// すべての障害物を走査する。
		/// </summary>
#else
		/// <summary>
		/// Scan all obstacles.
		/// </summary>
#endif
		ScanAll = 0x02,
	}
}