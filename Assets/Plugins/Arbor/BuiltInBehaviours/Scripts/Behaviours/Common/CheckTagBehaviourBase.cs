//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// タグ判定の基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for tag check
	/// </summary>
#endif
	public abstract class CheckTagBehaviourBase : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// タグチェック
		/// </summary>
#else
		/// <summary>
		/// tag check
		/// </summary>
#endif
		[SerializeField]
		private TagChecker _TagChecker = new TagChecker();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersionBase = 0;

		#region old

		[FormerlySerializedAs("_IsCheckTag")]
		[SerializeField]
		[HideInInspector]
		private bool _OldIsCheckTag = false;

		[FormerlySerializedAs("_Tag")]
		[SerializeField]
		[HideInInspector]
		private string _OldTag = "Untagged";

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected bool CheckTag(GameObject gameObject)
		{
			return _TagChecker.CheckTag(gameObject);
		}

		protected bool CheckTag(Component component)
		{
			return _TagChecker.CheckTag(component);
		}

		protected virtual void Reset()
		{
			_SerializeVersionBase = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_TagChecker = new TagChecker(_OldIsCheckTag, _OldTag);
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
