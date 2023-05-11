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
	/// CharacterControllerによって移動するMovingEntity
	/// </summary>
#else
	/// <summary>
	/// MovingEntity moved by CharacterController
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/MovingEntityCharacterController")]
	[BuiltInComponent]
	[RequireComponent(typeof(CharacterController))]
	[Internal.DocumentManual("/manual/builtin/movingentity/movingentitycharactercontroller.md")]
	public sealed class MovingEntityCharacterController : MovingEntity
	{
		private CharacterController _CharacterController;

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
				return _CharacterController.velocity;
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

			_CharacterController = GetComponent<CharacterController>();
		}
	}
}