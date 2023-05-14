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
	/// 対象となる障害物タイプのフラグ
	/// </summary>
#else
	/// <summary>
	/// Target obstacle type flag
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum ObstacleTargetFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="UnityEngine.AI.NavMeshObstacle"/>を障害物として扱う
		/// </summary>
#else
		/// <summary>
		/// Treat <see cref="UnityEngine.AI.NavMeshObstacle"/> as an obstacle
		/// </summary>
#endif
		NavMeshObstacle = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="UnityEngine.AI.NavMeshAgent"/>を障害物として扱う
		/// </summary>
#else
		/// <summary>
		/// Treat <see cref="UnityEngine.AI.NavMeshAgent"/> as an obstacle
		/// </summary>
#endif
		NavMeshAgent = 0x02,
	}
}