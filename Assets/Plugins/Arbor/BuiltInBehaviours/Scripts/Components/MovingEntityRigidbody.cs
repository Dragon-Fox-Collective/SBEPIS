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
	/// Rigidbodyによって移動するMovingEntity
	/// </summary>
#else
	/// <summary>
	/// MovingEntity moved by Rigidbody
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/MovingEntityRigidbody")]
	[BuiltInComponent]
	[RequireComponent(typeof(Rigidbody))]
	[Internal.DocumentManual("/manual/builtin/movingentity/movingentityrigidbody.md")]
	public sealed class MovingEntityRigidbody : MovingEntity
	{
		private Rigidbody _Rigidbody;

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
				return _Rigidbody.velocity;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// position
		/// </summary>
#endif
		public override Vector3 position
		{
			get
			{
				return _Rigidbody.position;
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
			_Rigidbody = GetComponent<Rigidbody>();
		}
	}
}