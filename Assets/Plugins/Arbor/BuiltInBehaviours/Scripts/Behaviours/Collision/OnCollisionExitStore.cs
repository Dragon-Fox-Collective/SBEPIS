﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnCollisionExitが呼び出された際、相手のGameObjectをパラメータに格納する。
	/// </summary>
#else
	/// <summary>
	/// When OnCollisionExit is called, it will store an opponent GameObject to the parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnCollisionExitStore")]
	[BuiltInBehaviour]
	public sealed class OnCollisionExitStore : CheckTagBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 格納先のパラメータ
		/// </summary>
#else
		/// <summary>
		/// Storage destination parameters
		/// </summary>
#endif
		[SerializeField]
		private GameObjectParameterReference _Parameter = new GameObjectParameterReference();

		#endregion // Serialize fields

		void OnCollisionExit(Collision collision)
		{
			if (!enabled)
			{
				return;
			}

			GameObject target = collision.gameObject;

			if (CheckTag(target))
			{
				_Parameter.value = target;
			}
		}
	}
}
