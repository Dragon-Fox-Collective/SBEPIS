//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Waypointでの移動タイプ
	/// </summary>
#else
	/// <summary>
	/// Movement type in Waypoint
	/// </summary>
#endif
	[Internal.Documentable]
	public enum MoveWaypointType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// １回のみ
		/// </summary>
#else
		/// <summary>
		/// Only once
		/// </summary>
#endif
		Once,

#if ARBOR_DOC_JA
		/// <summary>
		/// 終点から始点へ移動してループ
		/// </summary>
#else
		/// <summary>
		/// Move from the end point to the start point and loop
		/// </summary>
#endif
		Cycle,

#if ARBOR_DOC_JA
		/// <summary>
		/// 終端で折り返し
		/// </summary>
#else
		/// <summary>
		/// Turn back at the end
		/// </summary>
#endif
		PingPong,
	}
}