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
	/// Rendererの色を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change color of Renderer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenColor")]
	[BuiltInBehaviour]
	public sealed class TweenColor : TweenBase
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
		/// Colorのプロパティ名。
		/// </summary>
#else
		/// <summary>
		/// Property name of Color.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _PropertyName = new FlexibleString("_Color");

#if ARBOR_DOC_JA
		/// <summary>
		/// 色の変化の指定。
		/// </summary>
#else
		/// <summary>
		/// Specifying the color change.
		/// </summary>
#endif
		[SerializeField] private FlexibleGradient _Gradient = new FlexibleGradient(new Gradient());

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Renderer _OldTarget = null;

		[SerializeField]
		[FormerlySerializedAs("_Gradient")]
		[HideInInspector]
		private Gradient _OldGradient = new Gradient();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		private Gradient _CurrentGradient;

		private Renderer _CachedTarget;

		private int _CachedPropertyID;

		private RendererPropertyBlock _Block = null;

		protected override void OnTweenBegin()
		{
			base.OnTweenBegin();

			_CachedTarget = _Target.value as Renderer;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedPropertyID = Shader.PropertyToID(_PropertyName.value);

			_Block = RendererPropertyBlock.Get(_CachedTarget, _MaterialIndex.value);

			_Block.Update();

			_CurrentGradient = _Gradient.value;
			if (_CurrentGradient == null)
			{
				_CurrentGradient = new Gradient();
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget == null)
			{
				return;
			}

			_Block.Update();

			_Block.SetColor(_CachedPropertyID, _CurrentGradient.Evaluate(factor));

			_Block.Apply();
		}

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleComponent)_OldTarget;
		}

		void SerializeVer2()
		{
			_Gradient = (FlexibleGradient)_OldGradient;
		}

		void SerializeVer3()
		{
			_Target.SetHierarchyIfConstantNull();
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
