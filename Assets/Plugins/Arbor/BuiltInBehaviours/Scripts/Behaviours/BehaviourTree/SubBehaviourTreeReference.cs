//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;


namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.BehaviourTree;
	using Arbor.ObjectPooling;
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// 子グラフとして外部BehaviourTreeを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play external BehaviourTree as a child graph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BehaviourTree/SubBehaviourTreeReference")]
	[BuiltInBehaviour]
	public sealed class SubBehaviourTreeReference : StateBehaviour, INodeGraphContainer, ISubGraphBehaviour, ISubGraphReference, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize Fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照する外部BT
		/// </summary>
#else
		/// <summary>
		/// Reference external BT
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(BehaviourTree))]
		private FlexibleComponent _ExternalBT = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内のBehaviourTreeを直接利用するフラグ。<br/>
		/// このフラグをtrueにした場合、ExternalBTで指定したBehaviourTreeがシーン内にあり再生中でなければ子BehaviourTreeとして利用する。<br/>
		/// falseの場合は子オブジェクトとしてInstantiateして利用する。
		/// </summary>
#else
		/// <summary>
		/// A flag that directly uses BehaviourTree in the scene. <br/>
		/// When this flag is set to true, the BehaviourTree specified by ExternalBT is used as a child BehaviourTree unless it is in the scene and playing.<br/>
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
		[FormerlySerializedAs("_ExternalBT")]
		private BehaviourTree _OldExternalBT = null;

		#endregion // old

		#endregion // Serialize Fields

		private const int kCurrentSerializeVersion = 1;

		private BehaviourTree _CacheExternalBT;
		private BehaviourTree _RuntimeBT;
		private bool _CacheUseDirectryInScene;

		private bool _IsRuntimeGraphInScene = false;

		public BehaviourTree runtimeBT
		{
			get
			{
				return _RuntimeBT;
			}
		}

		void OnEnable()
		{
			if (_RuntimeBT != null && _RuntimeBT.playState != PlayState.Stopping)
			{
				if (_IsRuntimeGraphInScene)
				{
					_RuntimeBT.Resume();
				}
				else
				{
					SetActiveRuntimeGraph(true);
				}
			}
		}

		void OnDisable()
		{
			if (_RuntimeBT != null && _RuntimeBT.playState != PlayState.Stopping)
			{
				if (_IsRuntimeGraphInScene)
				{
					_RuntimeBT.Pause();
				}
				else
				{
					SetActiveRuntimeGraph(false);
				}
			}
		}

		void CreateBT()
		{
			BehaviourTree externalBT = _ExternalBT.value as BehaviourTree;
			bool useDirectoryInScene = _UseDirectlyInScene.value;

			if (_CacheExternalBT == externalBT && _CacheUseDirectryInScene == useDirectoryInScene)
			{
				return;
			}

			_CacheExternalBT = externalBT;
			_CacheUseDirectryInScene = useDirectoryInScene;

			if (_RuntimeBT != null)
			{
				if (_IsRuntimeGraphInScene)
				{
					StopRuntimeGraph();
					_RuntimeBT.SetExternal(null);
				}
				else
				{
					ObjectPool.Destroy(_RuntimeBT.gameObject);
				}
				_RuntimeBT = null;
				_IsRuntimeGraphInScene = false;
			}

			if (_CacheExternalBT == null)
			{
				return;
			}

			if (_CacheUseDirectryInScene)
			{
				if (!_CacheExternalBT.gameObject.scene.IsValid())
				{
					Debug.LogWarning("SubBehaviourTreeReference : ExternalBT specifies an object outside the scene. Cannot run as a child Graph.", this);
					return;
				}

				Object ownerBehaviourObject = _CacheExternalBT.ownerBehaviourObject;
				ISubGraphReference ownerSubGraphReference = ownerBehaviourObject as ISubGraphReference;
				bool isRuntimeGraphInScene = (ownerSubGraphReference != null && ownerSubGraphReference.IsRuntimeGraphInScene());
				if (ownerBehaviourObject != null && !isRuntimeGraphInScene)
				{
					Debug.LogWarning("SubBehaviourTreeReference : ExternalBT is already in use as a child FSM. Cannot run as a child Graph.", this);
					return;
				}

				if (!_CacheExternalBT.isActiveAndEnabled)
				{
					Debug.LogWarning("SubBehaviourTreeReference : ExternalBT is not active. Cannot run as a child Graph.", this);
					return;
				}

				if (_CacheExternalBT.playState != PlayState.Stopping)
				{
					Debug.LogWarning("SubBehaviourTreeReference : ExternalBT is already playing. Cannot run as a child FSM.", this);
					return;
				}

				if (isRuntimeGraphInScene)
				{
					ownerSubGraphReference.ReleaseRuntimeGraphInScene();
				}

				_RuntimeBT = _CacheExternalBT;
				_RuntimeBT.SetExternal(this);
				_IsRuntimeGraphInScene = true;
			}
			else
			{
				_RuntimeBT = NodeGraph.Instantiate<BehaviourTree>(_CacheExternalBT, _UsePool.value);
				_RuntimeBT.SetExternal(this, true);
				_RuntimeBT.playOnStart = false;
			}
			_RuntimeBT.updateSettings.type = UpdateType.Manual;

			SetActiveRuntimeGraph(false);
		}

		bool ISubGraphReference.IsRuntimeGraphInScene()
		{
			return _IsRuntimeGraphInScene;
		}

		void ISubGraphReference.ReleaseRuntimeGraphInScene()
		{
			if (!_IsRuntimeGraphInScene || _RuntimeBT == null)
			{
				return;
			}

			StopRuntimeGraph();
			_RuntimeBT.SetExternal(null);
			_RuntimeBT = null;
			_IsRuntimeGraphInScene = false;

			_CacheExternalBT = null;
		}

		private void SetActiveRuntimeGraph(bool value)
		{
			if (_IsRuntimeGraphInScene || _RuntimeBT == null)
			{
				return;
			}

			_RuntimeBT.gameObject.SetActive(value);
		}

		void StopRuntimeGraph()
		{
			if (_RuntimeBT == null)
			{
				return;
			}

			_RuntimeBT.Stop();
			SetActiveRuntimeGraph(false);
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CreateBT();

			if (_RuntimeBT != null)
			{
				_ArgumentList.UpdateInput(_RuntimeBT, GraphArgumentUpdateTiming.Enter);

				SetActiveRuntimeGraph(true);
				_RuntimeBT.Play();
			}
		}

		public override void OnStateUpdate()
		{
			if (_RuntimeBT != null)
			{
				_ArgumentList.UpdateInput(_RuntimeBT, GraphArgumentUpdateTiming.Execute);

				_RuntimeBT.ExecuteUpdate();
			}
		}

		public override void OnStateLateUpdate()
		{
			if (_RuntimeBT != null)
			{
				_ArgumentList.UpdateInput(_RuntimeBT, GraphArgumentUpdateTiming.Execute);

				_RuntimeBT.ExecuteLateUpdate();
			}
		}

		public override void OnStateEnd()
		{
			if (_RuntimeBT != null)
			{
				StopRuntimeGraph();

				_ArgumentList.UpdateOutput(_RuntimeBT);
			}
		}

		protected override void OnGraphPause()
		{
			if (_RuntimeBT != null)
			{
				_RuntimeBT.Pause();
			}
		}

		protected override void OnGraphResume()
		{
			if (_RuntimeBT != null)
			{
				_RuntimeBT.Resume();
			}
		}

		protected override void OnGraphStop()
		{
			StopRuntimeGraph();
		}

		int INodeGraphContainer.GetNodeGraphCount()
		{
			return _RuntimeBT != null ? 1 : 0;
		}

		T INodeGraphContainer.GetNodeGraph<T>(int index)
		{
			return _RuntimeBT as T;
		}

		void INodeGraphContainer.SetNodeGraph(int index, NodeGraph graph)
		{
			_RuntimeBT = graph as BehaviourTree;
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
				if (_RuntimeBT != null)
				{
					return false;
				}
				return true;
			}
		}

		NodeGraph ISubGraphBehaviour.GetSubGraph()
		{
			if (_RuntimeBT != null)
			{
				return _RuntimeBT;
			}

			if (_ExternalBT.type != FlexibleSceneObjectType.Constant)
			{
				return null;
			}

			return _ExternalBT.GetConstantObject() as NodeGraph;
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_ExternalBT = (FlexibleComponent)_OldExternalBT;
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