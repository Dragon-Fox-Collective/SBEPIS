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
	/// ImageにSpriteを設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the Sprite to Image.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetImage")]
	[BuiltInBehaviour]
	public sealed class UISetImage : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるImage。
		/// </summary>
#else
		/// <summary>
		/// Image of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Image))]
		private FlexibleComponent _Image = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定するSprite
		/// </summary>
#else
		/// <summary>
		/// Sprite to be set
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Sprite))]
		private FlexibleAssetObject _Sprite = new FlexibleAssetObject((Object)null);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Image")]
		[HideInInspector]
		private Image _OldImage = null;

		[SerializeField]
		[FormerlySerializedAs("_Sprite")]
		[HideInInspector]
		private Sprite _OldSprite = null;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public Image cachedImage
		{
			get
			{
				return _Image.value as Image;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Image image = cachedImage;
			if (image != null)
			{
				image.sprite = _Sprite.value as Sprite;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Image = (FlexibleComponent)_OldImage;
		}

		void SerializeVer2()
		{
			_Image.SetHierarchyIfConstantNull();
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
#endif
