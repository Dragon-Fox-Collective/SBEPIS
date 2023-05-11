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
	/// TextureのUV座標を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change UV cordinates of Texture.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenTextureOffset")]
	[BuiltInBehaviour]
	public sealed class TweenTextureOffset : TweenBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるRenderer。
		/// </summary>
#else
		/// <summary>
		/// Renderer of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Renderer))]
		private FlexibleComponent _Target = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のマテリアルのインデックス<br/>
		/// インデックスがどのマテリアルであるかは各種RendererのMaterials(例えば<a href="https://docs.unity3d.com/ja/current/Manual/class-MeshRenderer.html#materials">MeshRendererのMaterials</a>)を参照してください。
		/// </summary>
#else
		/// <summary>
		/// Index of target material<br/>
		/// See the various Renderer Materials (eg <a href="https://docs.unity3d.com/Manual/class-MeshRenderer.html#materials">Mesh Renderer Materials</a>) to see which material the index is.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _MaterialIndex = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Textureのプロパティ名。
		/// </summary>
#else
		/// <summary>
		/// Property name of Texture.
		/// </summary>
#endif
		[SerializeField] private FlexibleString _PropertyName = new FlexibleString("_MainTex");

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始した状態からの相対的な変化かどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether the relative change from the start state.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TweenMoveType))]
		private FlexibleTweenMoveType _TweenMoveType = new FlexibleTweenMoveType(TweenMoveType.Absolute);

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始UV座標。
		/// </summary>
#else
		/// <summary>
		/// Start UV coordinates.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _From = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標UV座標。
		/// </summary>
#else
		/// <summary>
		/// The goal UV coordinates.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _To = new FlexibleVector2();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

#region old

		[SerializeField]
		[FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Renderer _OldTarget = null;

		[SerializeField]
		[FormerlySerializedAs("_PropertyName")]
		[HideInInspector]
		private string _OldPropertyName = "_MainTex";

		[SerializeField, FormerlySerializedAs("_From")]
		[HideInInspector]
		private Vector2 _OldFrom = Vector2.zero;

		[SerializeField, FormerlySerializedAs("_To")]
		[HideInInspector]
		private Vector2 _OldTo = Vector2.zero;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Relative")]
		[FormerlySerializedAs("_TweenMoveType")]
		private TweenMoveType _OldTweenMoveType = TweenMoveType.Absolute;

#endregion // old

#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 5;

		private Renderer _CachedTarget;

		private int _CachedPropertyID;

		private Vector2 _CachedFromValue;
		private Vector2 _CachedToValue;

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_From = (FlexibleVector2)_OldFrom;
			_To = (FlexibleVector2)_OldTo;
		}

		void SerializeVer2()
		{
			_Target = (FlexibleComponent)_OldTarget;
		}

		void SerializeVer3()
		{
			_PropertyName = (FlexibleString)_OldPropertyName;
		}

		void SerializeVer4()
		{
			_Target.SetHierarchyIfConstantNull();
		}

		void SerializeVer5()
		{
			_TweenMoveType = (FlexibleTweenMoveType)_OldTweenMoveType;
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
					case 3:
						SerializeVer4();
						_SerializeVersion++;
						break;
					case 4:
						SerializeVer5();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}

		RendererPropertyBlock _Block = null;

		protected override void OnTweenBegin()
		{
			_CachedTarget = _Target.value as Renderer;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedPropertyID = Shader.PropertyToID(_PropertyName.value + "_ST");

			_CachedFromValue = _From.value;
			_CachedToValue = _To.value;

			_Block = RendererPropertyBlock.Get(_CachedTarget, _MaterialIndex.value);

			_Block.Update();

			Vector2 startOffset = _Block.GetTextureOffset(_CachedPropertyID);

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startOffset;
					_CachedToValue += startOffset;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startOffset;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget == null)
			{
				return;
			}

			Vector2 textureOffset = Vector2.Lerp(_CachedFromValue, _CachedToValue, factor);

			_Block.Update();

			_Block.SetTextureOffset(_CachedPropertyID, textureOffset);

			_Block.Apply();
		}
	}
}
