//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ボタンをクリックしたら遷移する。
	/// </summary>
#else
	/// <summary>
	/// Click the button to make a transition.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UIButtonTransition")]
	[BuiltInBehaviour]
	public sealed class UIButtonTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// クリック判定をするボタン
		/// </summary>
#else
		/// <summary>
		/// Button to click judgment
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Button))]
		private FlexibleComponent _Button = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : Button.onClick
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : Button.onClick
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Button")]
		[SerializeField]
		[HideInInspector]
		private Button _OldButton = null;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public Button cachedButton
		{
			get
			{
				return _Button.value as Button;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Button button = cachedButton;
			if (button != null)
			{
				button.onClick.AddListener(OnClick);
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			Button button = cachedButton;
			if (button != null)
			{
				button.onClick.RemoveListener(OnClick);
			}
		}

		public void OnClick()
		{
			Transition(_NextState);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Button = (FlexibleComponent)_OldButton;
		}

		void SerializeVer2()
		{
			_Button.SetHierarchyIfConstantNull();
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
					case 1:
						SerializeVer2();
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
