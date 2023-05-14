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
	/// Spriteを設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the Sprite.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/SetSprite")]
	[BuiltInBehaviour]
	public sealed class SetSprite : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるSpriteRenderer。
		/// </summary>
#else
		/// <summary>
		/// SpriteRenderer of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(SpriteRenderer))]
		private FlexibleComponent _Target = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定するSprite。
		/// </summary>
#else
		/// <summary>
		/// Setting Sprite that.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Sprite))]
		private FlexibleAssetObject _Sprite = new FlexibleAssetObject((Object)null);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Target")]
		[SerializeField]
		[HideInInspector]
		private SpriteRenderer _OldTarget = null;

		[SerializeField]
		[FormerlySerializedAs("_Sprite")]
		[HideInInspector]
		private Sprite _OldSprite = null;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public SpriteRenderer cachedSpriteRenderer
		{
			get
			{
				return _Target.value as SpriteRenderer;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			SpriteRenderer spriteRenderer = cachedSpriteRenderer;
			if (spriteRenderer != null)
			{
				spriteRenderer.sprite = _Sprite.value as Sprite;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleComponent)_OldTarget;
		}

		void SerializeVer2()
		{
			_Target.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
			_Sprite = (FlexibleAssetObject)_OldSprite;
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
					case 2:
						SerializeVer3();
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
