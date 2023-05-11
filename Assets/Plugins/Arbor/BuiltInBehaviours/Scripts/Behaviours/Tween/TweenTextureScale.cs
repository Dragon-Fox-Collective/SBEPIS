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
	/// TextureのTilingを徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change Tiling of Texture.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenTextureScale")]
	[BuiltInBehaviour]
	public sealed class TweenTextureScale : TweenBase
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
		/// 開始値。
		/// </summary>
#else
		/// <summary>
		/// Start value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _From = new FlexibleVector2(Vector2.one);

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標値。
		/// </summary>
#else
		/// <summary>
		/// The goal value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _To = new FlexibleVector2(Vector2.one);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TweenMoveType")]
		private TweenMoveType _OldTweenMoveType = TweenMoveType.Absolute;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		private Renderer _CachedTarget;

		private int _CachedPropertyID;

		private Vector2 _CachedFromValue;
		private Vector2 _CachedToValue;

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

			Vector2 startScale = _Block.GetTextureScale(_CachedPropertyID);

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startScale;
					_CachedToValue += startScale;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startScale;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget == null)
			{
				return;
			}

			Vector2 textureScale = Vector2.Lerp(_CachedFromValue, _CachedToValue, factor);

			_Block.Update();

			_Block.SetTextureScale(_CachedPropertyID, textureScale);

			_Block.Apply();
		}

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target.SetHierarchyIfConstantNull();
		}

		void SerializeVer2()
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
	}
}
