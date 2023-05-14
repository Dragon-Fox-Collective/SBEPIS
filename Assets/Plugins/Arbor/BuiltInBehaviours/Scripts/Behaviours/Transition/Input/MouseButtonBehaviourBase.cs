//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

#if ARBOR_SUPPORT_UGUI
using UnityEngine.EventSystems;
#endif

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// マウスボタンを判定する基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for determining mouse buttons
	/// </summary>
#endif
	public abstract class MouseButtonBehaviourBase : InputBehaviourBase, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// マウスボタンの指定。
		/// </summary>
#else
		/// <summary>
		/// Specified mouse button.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Button = new FlexibleInt(0);

#if ARBOR_DOC_JA
		/// <summary>
		/// マウスポインターがUI上にある場合の入力を無視する。
		/// </summary>
#else
		/// <summary>
		/// Ignore input when the mouse pointer is over the UI.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IgnoreUI = new FlexibleBool(false);

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
		[HideInInspector]
		[FormerlySerializedAs("_Button")]
		private int _OldButton = 0;

		#endregion // old

		#endregion // Serialize fields

		protected abstract bool IsInput(int button);

		protected sealed override void OnUpdate()
		{
			int button = _Button.value;
			if (IsInput(button))
			{
#if ARBOR_SUPPORT_UGUI
				if (_IgnoreUI.value)
				{
					EventSystem current = EventSystem.current;
					if (EventSystem.current.IsPointerOverGameObject())
					{
						return;
					}
					if (button == 0 && Input.touchCount > 0)
					{
						var touch = Input.GetTouch(0);
						if (current.IsPointerOverGameObject(touch.fingerId))
						{
							return;
						}
					}
				}
#endif
				Transition(_NextState);
			}
		}

		private const int kCurrentSerializeVersion = 1;

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Button = (FlexibleInt)_OldButton;
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

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
