//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.ObjectPooling;
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// 子グラフとして外部ArborFSMを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play external ArborFSM as a child graph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("StateMachine/SubStateMachineReference")]
	[BuiltInBehaviour]
	public sealed class SubStateMachineReference : StateBehaviour, INodeGraphContainer, ISubGraphBehaviour, ISubGraphReference, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize Fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照する外部FSM
		/// </summary>
#else
		/// <summary>
		/// Reference external FSM
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(ArborFSM))]
		private FlexibleComponent _ExternalFSM = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内のFSMを直接利用するフラグ。<br/>
		/// このフラグをtrueにした場合、ExternalFSMで指定したFSMがシーン内にあり再生中でなければ子FSMとして利用する。<br/>
		/// falseの場合は子オブジェクトとしてInstantiateして利用する。
		/// </summary>
#else
		/// <summary>
		/// A flag that directly uses FSM in the scene. <br/>
		/// When this flag is set to true, the FSM specified by ExternalFSM is used as a child FSM unless it is in the scene and playing.<br/>
		/// If false, use Instantiate as a child object.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _UseDirectlyInScene = new FlexibleBool(false);

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
		/// トリガーを子FSMに受け渡すフラグ
		/// </summary>
#else
		/// <summary>
		/// Flag to pass trigger to child FSM
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _PassThroughTrigger = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの引数<br/>
		/// <list type="bullet">
		/// <item><description>+ボタンから、パラメータを選択するかパラメータ名を指定して作成。</description></item>
		/// <item><description>パラメータを選択し、-ボタンをクリックで削除。</description></item>
		/// </list>
		/// </summary>
#else
		/// <summary>
		/// Arguments of the graph<br/>
		/// <list type="bullet">
		/// <item><description>Create from the + button by selecting a parameter or specifying a parameter name.</description></item>
		/// <item><description>Select the parameter and delete it by clicking the - button.</description></item>
		/// </list>
		/// </summary>
#endif
		[SerializeField]
		private GraphArgumentList _ArgumentList = new GraphArgumentList();

#if ARBOR_DOC_JA
		/// <summary>
		/// 成功時の遷移<br />
		/// 遷移メソッド : OnFinishNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Transition on success<br />
		/// Transition Method : OnFinishNodeGraph
		/// </summary>
#endif
		[SerializeField]
		private StateLink _SuccessLink = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// 失敗時の遷移<br />
		/// 遷移メソッド : OnFinishNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Transition on failure<br />
		/// Transition Method : OnFinishNodeGraph
		/// </summary>
#endif
		[SerializeField]
		private StateLink _FailureLink = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_ExternalFSM")]
		private ArborFSM _OldExternalFSM = null;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_PassThroughTrigger")]
		private bool _OldPassThroughTrigger = true;

		#endregion // old

		#endregion // Serialize Fields

		private const int kCurrentSerializeVersion = 2;

		private ArborFSM _CacheExternalFSM;
		private ArborFSM _RuntimeFSM;
		private bool _CacheUseDirectryInScene;

		private bool _IsRuntimeGraphInScene = false;

		public ArborFSM runtimeFSM
		{
			get
			{
				return _RuntimeFSM;
			}
		}

		void OnEnable()
		{
			if (_RuntimeFSM != null && _RuntimeFSM.playState != PlayState.Stopping)
			{
				if (_IsRuntimeGraphInScene)
				{
					_RuntimeFSM.Resume();
				}
				else
				{
					SetActiveRuntimeGraph(true);
				}
			}
		}

		void OnDisable()
		{
			if (_RuntimeFSM != null && _RuntimeFSM.playState != PlayState.Stopping)
			{
				if (_IsRuntimeGraphInScene)
				{
					_RuntimeFSM.Pause();
				}
				else
				{
					SetActiveRuntimeGraph(false);
				}
			}
		}

		void CreateFSM()
		{
			ArborFSM externalFSM = _ExternalFSM.value as ArborFSM;
			bool useDirectoryInScene = _UseDirectlyInScene.value;

			if (_CacheExternalFSM == externalFSM && _CacheUseDirectryInScene == useDirectoryInScene)
			{
				return;
			}

			_CacheExternalFSM = externalFSM;
			_CacheUseDirectryInScene = useDirectoryInScene;

			if (_RuntimeFSM != null)
			{
				if (_IsRuntimeGraphInScene)
				{
					StopRuntimeGraph();
					_RuntimeFSM.SetExternal(null);
				}
				else
				{
					ObjectPool.Destroy(_RuntimeFSM.gameObject);
				}
				_RuntimeFSM = null;
				_IsRuntimeGraphInScene = false;
			}

			if (_CacheExternalFSM == null)
			{
				return;
			}

			if (_CacheUseDirectryInScene)
			{
				if (!_CacheExternalFSM.gameObject.scene.IsValid())
				{
					Debug.LogWarning("SubStateMachineReference : ExternalFSM specifies an object outside the scene. Cannot run as a child FSM.", this);
					return;
				}

				Object ownerBehaviourObject = _CacheExternalFSM.ownerBehaviourObject;
				ISubGraphReference ownerSubGraphReference = ownerBehaviourObject as ISubGraphReference;
				bool isRuntimeGraphInScene = (ownerSubGraphReference != null && ownerSubGraphReference.IsRuntimeGraphInScene());
				if (ownerBehaviourObject != null && !isRuntimeGraphInScene)
				{
					Debug.LogWarning("SubStateMachineReference : ExternalFSM is already in use as a child FSM. Cannot run as a child FSM.", this);
					return;
				}

				if (!_CacheExternalFSM.isActiveAndEnabled)
				{
					Debug.LogWarning("SubStateMachineReference : ExternalFSM is not active. Cannot run as a child FSM.", this);
					return;
				}

				if (_CacheExternalFSM.playState != PlayState.Stopping)
				{
					Debug.LogWarning("SubStateMachineReference : ExternalFSM is already playing. Cannot run as a child FSM.", this);
					return;
				}

				if (isRuntimeGraphInScene)
				{
					ownerSubGraphReference.ReleaseRuntimeGraphInScene();
				}

				_RuntimeFSM = _CacheExternalFSM;
				_RuntimeFSM.SetExternal(this);
				_IsRuntimeGraphInScene = true;
			}
			else
			{
				_RuntimeFSM = NodeGraph.Instantiate<ArborFSM>(_CacheExternalFSM, _UsePool.value);
				_RuntimeFSM.SetExternal(this, true);
				_IsRuntimeGraphInScene = false;
			}

			_RuntimeFSM.playOnStart = false;
			_RuntimeFSM.updateSettings.type = UpdateType.Manual;

			SetActiveRuntimeGraph(false);
		}

		bool ISubGraphReference.IsRuntimeGraphInScene()
		{
			return _IsRuntimeGraphInScene;
		}

		void ISubGraphReference.ReleaseRuntimeGraphInScene()
		{
			if (!_IsRuntimeGraphInScene || _RuntimeFSM == null)
			{
				return;
			}

			StopRuntimeGraph();
			_RuntimeFSM.SetExternal(null);
			_RuntimeFSM = null;
			_IsRuntimeGraphInScene = false;

			_CacheExternalFSM = null;
		}

		private void SetActiveRuntimeGraph(bool value)
		{
			if (_IsRuntimeGraphInScene || _RuntimeFSM == null)
			{
				return;
			}

			_RuntimeFSM.gameObject.SetActive(value);
		}

		void StopRuntimeGraph()
		{
			if (_RuntimeFSM == null)
			{
				return;
			}

			_RuntimeFSM.Stop();
			SetActiveRuntimeGraph(false);
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CreateFSM();

			if (_RuntimeFSM != null)
			{
				_ArgumentList.UpdateInput(_RuntimeFSM, GraphArgumentUpdateTiming.Enter);

				SetActiveRuntimeGraph(true);
				_RuntimeFSM.Play();
			}
		}

		public override void OnStateUpdate()
		{
			if (_RuntimeFSM != null)
			{
				_ArgumentList.UpdateInput(_RuntimeFSM, GraphArgumentUpdateTiming.Execute);

				_RuntimeFSM.ExecuteUpdate();
			}
		}

		public override void OnStateLateUpdate()
		{
			if (_RuntimeFSM != null)
			{
				_ArgumentList.UpdateInput(_RuntimeFSM, GraphArgumentUpdateTiming.Execute);

				_RuntimeFSM.ExecuteLateUpdate();
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_RuntimeFSM != null)
			{
				StopRuntimeGraph();

				_ArgumentList.UpdateOutput(_RuntimeFSM);
			}
		}

		protected override void OnGraphPause()
		{
			if (_RuntimeFSM != null)
			{
				_RuntimeFSM.Pause();
			}
		}

		protected override void OnGraphResume()
		{
			if (_RuntimeFSM != null)
			{
				_RuntimeFSM.Resume();
			}
		}

		protected override void OnGraphStop()
		{
			StopRuntimeGraph();
		}

		public override void OnStateTrigger(string message)
		{
			if (_PassThroughTrigger.value && _RuntimeFSM != null)
			{
				_RuntimeFSM.SendTrigger(message);
			}
		}

		int INodeGraphContainer.GetNodeGraphCount()
		{
			return _RuntimeFSM != null ? 1 : 0;
		}

		T INodeGraphContainer.GetNodeGraph<T>(int index)
		{
			return _RuntimeFSM as T;
		}

		void INodeGraphContainer.SetNodeGraph(int index, NodeGraph graph)
		{
			_RuntimeFSM = graph as ArborFSM;
		}

		void INodeGraphContainer.OnFinishNodeGraph(NodeGraph graph, bool success)
		{
			StopRuntimeGraph();

			if (success)
			{
				Transition(_SuccessLink);
			}
			else
			{
				Transition(_FailureLink);
			}
		}

		bool ISubGraphBehaviour.isExternal
		{
			get
			{
				if (_RuntimeFSM != null)
				{
					return false;
				}
				return true;
			}
		}

		NodeGraph ISubGraphBehaviour.GetSubGraph()
		{
			if (_RuntimeFSM != null)
			{
				return _RuntimeFSM;
			}

			if (_ExternalFSM.type != FlexibleSceneObjectType.Constant)
			{
				return null;
			}

			return _ExternalFSM.GetConstantObject() as NodeGraph;
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_ExternalFSM = (FlexibleComponent)_OldExternalFSM;
		}

		void SerializeVer2()
		{
			_PassThroughTrigger = (FlexibleBool)_OldPassThroughTrigger;
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