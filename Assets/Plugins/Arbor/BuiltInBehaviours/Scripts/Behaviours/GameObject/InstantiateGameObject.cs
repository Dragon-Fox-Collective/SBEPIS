//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using Arbor.ObjectPooling;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectをインスタンス化する。
	/// </summary>
#else
	/// <summary>
	/// Instantiate a GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/InstantiateGameObject")]
	[BuiltInBehaviour]
	public sealed class InstantiateGameObject : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンス化する元のGameObject。
		/// </summary>
#else
		/// <summary>
		/// The original GameObject to instantiate.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _Prefab = new FlexibleGameObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// 親に指定するTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform that specified in the parent.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Parent = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 姿勢タイプ
		/// </summary>
#else
		/// <summary>
		/// Posture type
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(PostureType))]
		private FlexiblePostureType _PostureType = new FlexiblePostureType(PostureType.Transform);

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期時に指定するTransform。Posture TypeがTransformの時に使用する。
		/// </summary>
#else
		/// <summary>
		/// Transform that you specify for the initial time. Used when Posture Type is Transform
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _InitTransform = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期時に指定する位置。Posture TypeがDirectlyの時に使用する。
		/// </summary>
#else
		/// <summary>
		/// The position specified at the beginning. Used when Posture Type is Directly.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _InitPosition = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期時に指定する回転。Posture TypeがDirectlyの時に使用する。
		/// </summary>
#else
		/// <summary>
		/// The rotation specified at the initial stage. Used when Posture Type is Directly.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleQuaternion _InitRotation = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期時に指定する空間。Posture TypeがDirectlyの時に使用する。
		/// </summary>
#else
		/// <summary>
		/// The space specified at the beginning. Used when Posture Type is Directly.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _InitSpace = new FlexibleSpace(Space.World);

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectPoolを使用してインスタンス化するフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag to instantiate using ObjectPool.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _UsePool = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectPoolの生存時間フラグ
		/// </summary>
#else
		/// <summary>
		/// ObjectPool lifetime flag
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(LifeTimeFlags))]
		private FlexibleLifeTimeFlags _LifeTimeFlags = new FlexibleLifeTimeFlags(LifeTimeFlags.SceneUnloaded);

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectPoolの生存時間
		/// </summary>
#else
		/// <summary>
		/// ObjectPool lifetime
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _LifeDuration = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンス化したGameObjectを格納するパラメータ
		/// </summary>
#else
		/// <summary>
		/// Parameter to store the instantiated GameObject
		/// </summary>
#endif
		[SerializeField] private GameObjectParameterReference _Parameter = new GameObjectParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンス化したGameObjectを出力するスロット。
		/// </summary>
#else
		/// <summary>
		/// A slot that outputs an instantiated GameObject.
		/// </summary>
#endif
		[SerializeField] private OutputSlotGameObject _Output = new OutputSlotGameObject();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Prefab")]
		private GameObject _OldPrefab = null;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_PostureType")]
		private PostureType _OldPostureType = PostureType.Transform;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_InitSpace")]
		private Space _OldInitSpace = Space.World;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public override void OnStateBegin()
		{
			GameObject prefab = _Prefab?.value;
			if (prefab == null)
			{
				return;
			}

			PostureType postureType = _PostureType.value;
			Space space = Space.World;

			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;

			switch (postureType)
			{
				case PostureType.Transform:
					{
						var initTransform = _InitTransform.value;
						if (initTransform == null)
						{
							var prefabTransform = prefab.transform;
							position = prefabTransform.position;
							rotation = prefabTransform.rotation;

							var parentTransform = _Parent.value;
							if (parentTransform != null)
							{
								position = parentTransform.TransformPoint(position);
								rotation = parentTransform.rotation * rotation;
							}
						}
						else
						{
							position = initTransform.position;
							rotation = initTransform.rotation;
						}
						space = Space.World;
					}
					break;
				case PostureType.Directly:
					{
						position = _InitPosition.value;
						rotation = _InitRotation.value;
						space = _InitSpace.value;
					}
					break;
			}

			GameObject obj = null;
			if (_UsePool.value)
			{
				obj = ObjectPool.Instantiate(prefab, position, rotation, _Parent.value, space, _LifeTimeFlags.value, _LifeDuration.value) as GameObject;
			}
			else
			{
				bool activeSelf = prefab.activeSelf;
				prefab.SetActive(false);

				obj = Object.Instantiate(prefab, _Parent.value, false);

				var objTransform = obj.transform;
				switch (space)
				{
					case Space.World:
						objTransform.position = position;
						objTransform.rotation = rotation;
						break;
					case Space.Self:
						objTransform.localPosition = position;
						objTransform.localRotation = rotation;
						break;
				}

				obj.SetActive(activeSelf);

				prefab.SetActive(activeSelf);
			}

			_Parameter.value = obj;

			_Output.SetValue(obj);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Prefab = (FlexibleGameObject)_OldPrefab;
		}

		void SerializeVer2()
		{
			_PostureType = (FlexiblePostureType)_OldPostureType;
			_InitSpace = (FlexibleSpace)_OldInitSpace;
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

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}
