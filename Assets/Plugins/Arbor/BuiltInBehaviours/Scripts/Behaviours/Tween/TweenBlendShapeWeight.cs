//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BlendShapeのWeight値を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Tween Weight value of BlendShape.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenBlendShapeWeight")]
	[BuiltInBehaviour]
	public sealed class TweenBlendShapeWeight : TweenBase
	{
		[Arbor.Internal.Documentable]
		[System.Serializable]
		private sealed class BlendShape : ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
		{
			#region Serialize fields

#if ARBOR_DOC_JA
			/// <summary>
			/// 対象となるSkinnedMeshRenderer。
			/// </summary>
#else
			/// <summary>
			/// SkinnedMeshRenderer of interest.
			/// </summary>
#endif
			[SerializeField]
			[SlotType(typeof(SkinnedMeshRenderer))]
			private FlexibleComponent _Target = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
			/// <summary>
			/// BlendShapeの名前。
			/// </summary>
#else
			/// <summary>
			/// Name of blend shape.
			/// </summary>
#endif
			[SerializeField] private FlexibleString _Name = new FlexibleString();

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
			[SerializeField] private FlexibleFloat _From = new FlexibleFloat(0.0f);

#if ARBOR_DOC_JA
			/// <summary>
			/// 目標値。
			/// </summary>
#else
			/// <summary>
			/// The goal value.
			/// </summary>
#endif
			[SerializeField] private FlexibleFloat _To = new FlexibleFloat(0.0f);

			[SerializeField]
			[HideInInspector]
			private SerializeVersion _SerializeVersion = new SerializeVersion();

			#region old

			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("_SerializeVersion")]
			private int _SerializeVersionOld = 0;

			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("_IsInitialized")]
			private bool _IsInitializedOld = true;

			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("_TweenMoveType")]
			private TweenMoveType _OldTweenMoveType = TweenMoveType.Absolute;

			#endregion // old

			#endregion // Serialize fields

			private const int kCurrentSerializeVersion = 2;

			private SkinnedMeshRenderer _CachedTarget;
			private int _CachedIndex;
			private float _CachedFromValue;
			private float _CachedToValue;

			public BlendShape()
			{
				// Initialize when calling from script.
				_SerializeVersion.Initialize(this);
			}

			public void OnBegin()
			{
				_CachedTarget = _Target.value as SkinnedMeshRenderer;

				if (_CachedTarget == null)
				{
					return;
				}

				Mesh sharedMesh = _CachedTarget.sharedMesh;
				if (sharedMesh == null)
				{
					_CachedIndex = -1;
					return;
				}

				_CachedIndex = sharedMesh.GetBlendShapeIndex(_Name.value);

				if (_CachedIndex < 0)
				{
					return;
				}

				_CachedFromValue = _From.value;
				_CachedToValue = _To.value;

				float startWeight = _CachedTarget.GetBlendShapeWeight(_CachedIndex);

				switch (_TweenMoveType.value)
				{
					case TweenMoveType.Absolute:
						break;
					case TweenMoveType.Relative:
						_CachedFromValue += startWeight;
						_CachedToValue += startWeight;
						break;
					case TweenMoveType.ToAbsolute:
						_CachedFromValue = startWeight;
						break;
				}
			}

			public void OnUpdate(float factor)
			{
				if (_CachedTarget == null || _CachedIndex < 0)
				{
					return;
				}

				float weight = Mathf.Lerp(_CachedFromValue, _CachedToValue, factor);
				_CachedTarget.SetBlendShapeWeight(_CachedIndex, weight);
			}

			#region ISerializeVersionCallbackReceiver

			int ISerializeVersionCallbackReceiver.newestVersion
			{
				get
				{
					return kCurrentSerializeVersion;
				}
			}

			void ISerializeVersionCallbackReceiver.OnInitialize()
			{
				_SerializeVersion.version = kCurrentSerializeVersion;
				_Target = new FlexibleComponent(FlexibleHierarchyType.Self);
			}

			void SerializeVer1()
			{
				_Target.SetHierarchyIfConstantNull();
			}

			void SerializeVer2()
			{
				_TweenMoveType = (FlexibleTweenMoveType)_OldTweenMoveType;
			}

			void ISerializeVersionCallbackReceiver.OnSerialize(int version)
			{
				switch (version)
				{
					case 0:
						SerializeVer1();
						break;
					case 1:
						SerializeVer2();
						break;
				}
			}

			void ISerializeVersionCallbackReceiver.OnVersioning()
			{
				if (_IsInitializedOld)
				{
					_SerializeVersion.version = _SerializeVersionOld;
				}
			}

			#endregion // ISerializeVersionCallbackReceiver

			#region ISerializationCallbackReceiver

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				_SerializeVersion.AfterDeserialize();
			}

			void ISerializationCallbackReceiver.OnBeforeSerialize()
			{
				_SerializeVersion.BeforeDeserialize();
			}

			#endregion // ISerializationCallbackReceiver
		}

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のBlendShape
		/// </summary>
#else
		/// <summary>
		/// Target BlendShapes
		/// </summary>
#endif
		[SerializeField]
		private List<BlendShape> _BlendShapes = new List<BlendShape>();

		#endregion // Serialize fields

		protected override void OnTweenBegin()
		{
			for (int i = 0, count = _BlendShapes.Count; i < count; i++)
			{
				_BlendShapes[i].OnBegin();
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			for (int i = 0, count = _BlendShapes.Count; i < count; i++)
			{
				_BlendShapes[i].OnUpdate(factor);
			}
		}
	}
}
