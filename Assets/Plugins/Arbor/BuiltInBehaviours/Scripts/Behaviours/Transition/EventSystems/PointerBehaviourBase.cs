//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Pointerのボタンを判定する基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for judging Pointer buttons
	/// </summary>
#endif
	public abstract class PointerBehaviourBase : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ボタンをチェックするかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to check the button.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckButton = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// チェックするボタン。
		/// </summary>
#else
		/// <summary>
		/// Button to check.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInputButton _Button = new FlexibleInputButton(PointerEventData.InputButton.Left);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersionBase = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_CheckButton")]
		[HideInInspector]
		private bool _OldCheckButton = false;

		[SerializeField]
		[FormerlySerializedAs("_Button")]
		[HideInInspector]
		private PointerEventData.InputButton _OldButton = PointerEventData.InputButton.Left;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public bool CheckButton(PointerEventData.InputButton button)
		{
			return !_CheckButton.value || button == _Button.value;
		}

		protected virtual void Reset()
		{
			_SerializeVersionBase = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_CheckButton = (FlexibleBool)_OldCheckButton;
			_Button = new FlexibleInputButton(_OldButton);
		}

		void Serialize()
		{
			while (_SerializeVersionBase != kCurrentSerializeVersion)
			{
				switch (_SerializeVersionBase)
				{
					case 0:
						SerializeVer1();
						_SerializeVersionBase++;
						break;
					default:
						_SerializeVersionBase = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}
	}
}
#endif
