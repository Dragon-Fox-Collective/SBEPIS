//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// なんらかのキーが押されたときにステートを遷移します。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when any key is pressed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/AnyKeyDownTransition")]
	[BuiltInBehaviour]
	public sealed class AnyKeyDownTransition : InputBehaviourBase, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 押している状態を判定するかどうか。<br/>
		/// チェックを外すと、入力がない場合に遷移する。
		/// </summary>
#else
		/// <summary>
		/// Whether to judge the pressed state or not.<br/>
		/// When unchecked, it transits when there is no input.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckDown = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : Update
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : Update
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_CheckDown")]
		[HideInInspector]
		private bool _OldCheckDown = true;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Update is called once per frame
		protected override void OnUpdate()
		{
			if (Input.anyKeyDown == _CheckDown.value)
			{
				Transition(_NextState);
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_CheckDown = (FlexibleBool)_OldCheckDown;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
#endif