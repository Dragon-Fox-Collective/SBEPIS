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
	/// <see cref="UnityEngine.AI.OffMeshLink"/>を通過する方法の設定
	/// </summary>
#else
	/// <summary>
	/// Setting how to traverse <see cref="UnityEngine.AI.OffMeshLink"/>
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public sealed class OffMeshLinkTraverseData
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 通過中にTrueにするBool型パラメータ
		/// </summary>
#else
		/// <summary>
		/// Bool type parameter to be True during traverse
		/// </summary>
#endif
		[AnimatorParameterName(AnimatorControllerParameterType.Bool)]
		public AnimatorName parameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// 通過方向へ回転する速さ
		/// </summary>
#else
		/// <summary>
		/// The speed of rotation in the traverse direction
		/// </summary>
#endif
		public float angularSpeed = 120f;

#if ARBOR_DOC_JA
		/// <summary>
		/// ジャンプする高さ<br/>0を指定した場合は直線移動する。
		/// </summary>
#else
		/// <summary>
		/// Height to jump<br/>If 0 is specified, it moves linearly.
		/// </summary>
#endif
		public float jumpHeight = 0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// 最小の移動速度。<see cref="UnityEngine.AI.NavMeshAgent.speed"/>と比較して速い方を使用する。
		/// </summary>
#else
		/// <summary>
		/// Minimum movement speed. Use the faster one compared to <see cref="UnityEngine.AI.NavMeshAgent.speed"/>.
		/// </summary>
#endif
		public float minSpeed = 3.5f;

#if ARBOR_DOC_JA
		/// <summary>
		/// 向き直ってから通過し始めるまでの待機設定。
		/// </summary>
#else
		/// <summary>
		/// Waiting setting from turning around to starting traversing.
		/// </summary>
#endif
		public OffMeshLinkTraverseWaitData startWait;

#if ARBOR_DOC_JA
		/// <summary>
		/// 通過し終えてからNavMesh上への移動に切り替わるまでの待機設定。
		/// </summary>
#else
		/// <summary>
		/// Waiting setting from the end of traversal to the switch to move to the NavMesh.
		/// </summary>
#endif
		public OffMeshLinkTraverseWaitData endWait;
	}
}