//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	public abstract class KeyBehaviourBase : InputBehaviourBase, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// キーの指定。
		/// </summary>
#else
		/// <summary>
		/// Specified key.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleKeyCode _KeyCode = new FlexibleKeyCode(KeyCode.None);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersionBase = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_KeyCode")]
		[HideInInspector]
		private KeyCode _OldKeyCode = KeyCode.None;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected KeyCode keyCode
		{
			get
			{
				return _KeyCode.value;
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersionBase = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_KeyCode = (FlexibleKeyCode)_OldKeyCode;
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
