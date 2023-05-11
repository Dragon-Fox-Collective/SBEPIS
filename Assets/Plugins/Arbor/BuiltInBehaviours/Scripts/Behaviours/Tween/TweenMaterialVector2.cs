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
	/// MaterialのVector2を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Tween Vector2 of Material.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenMaterialVector2")]
	[BuiltInBehaviour]
	public sealed class TweenMaterialVector2 : TweenBase
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
		[SerializeField] private FlexibleString _PropertyName = new FlexibleString("");

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2のテクスチャ座標タイプ。
		/// </summary>
#else
		/// <summary>
		/// Texcoord Vector2 Type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TexcoordVector2Type))]
		private FlexibleTexcoordVector2Type _TexcoordVector2Type = new FlexibleTexcoordVector2Type(TexcoordVector2Type.XY);

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
		[SerializeField] private FlexibleVector2 _From = new FlexibleVector2(Vector2.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標値。
		/// </summary>
#else
		/// <summary>
		/// The goal value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _To = new FlexibleVector2(Vector2.zero);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TexcoordVector2Type")]
		private TexcoordVector2Type _OldTexcoordVector2Type = TexcoordVector2Type.XY;

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

			_CachedPropertyID = Shader.PropertyToID(_PropertyName.value);

			_CachedFromValue = _From.value;
			_CachedToValue = _To.value;

			_Block = RendererPropertyBlock.Get(_CachedTarget, _MaterialIndex.value);

			_Block.Update();

			Vector4 vector = _Block.GetVector(_CachedPropertyID);

			Vector2 startVector = Vector2.zero;
			switch (_TexcoordVector2Type.value)
			{
				case TexcoordVector2Type.XY:
					startVector.x = vector.x;
					startVector.y = vector.y;
					break;
				case TexcoordVector2Type.ZW:
					startVector.x = vector.z;
					startVector.y = vector.w;
					break;
			}

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startVector;
					_CachedToValue += startVector;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startVector;
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

			Vector4 vector = _Block.GetVector(_CachedPropertyID);

			switch (_TexcoordVector2Type.value)
			{
				case TexcoordVector2Type.XY:
					vector.x = textureScale.x;
					vector.y = textureScale.y;
					break;
				case TexcoordVector2Type.ZW:
					vector.z = textureScale.x;
					vector.w = textureScale.y;
					break;
			}

			_Block.SetVector(_CachedPropertyID, vector);

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
			_TexcoordVector2Type = (FlexibleTexcoordVector2Type)_OldTexcoordVector2Type;
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
