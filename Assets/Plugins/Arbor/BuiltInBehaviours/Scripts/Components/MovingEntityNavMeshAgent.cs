//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NavMeshAgentによって移動するMovingEntity
	/// </summary>
#else
	/// <summary>
	/// MovingEntity moved by NavMeshAgent
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/MovingEntityNavMeshAgent")]
	[BuiltInComponent]
	[RequireComponent(typeof(NavMeshAgent))]
	[Internal.DocumentManual("/manual/builtin/movingentity/movingentitynavmeshagent.md")]
	public sealed class MovingEntityNavMeshAgent : MovingEntity
	{
		private NavMeshAgent _NavMeshAgent;

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動速度
		/// </summary>
#else
		/// <summary>
		/// Movement velocity
		/// </summary>
#endif
		public override Vector3 velocity
		{
			get
			{
				return _NavMeshAgent.velocity;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトのインスタンスがロードされたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script instance is being loaded.
		/// </summary>
#endif
		protected override void Awake()
		{
			base.Awake();
			_NavMeshAgent = GetComponent<NavMeshAgent>();
		}
	}
}