//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree
{
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// TreeBehaviourNodeの挙動を定義する基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class that defines the behavior of TreeBehaviourNode.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class TreeNodeBehaviour : PlayableBehaviour, IPlayableBehaviourCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ビヘイビアツリーを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the behaviour tree.
		/// </summary>
#endif
		public BehaviourTreeInternal behaviourTree
		{
			get
			{
				return nodeGraph as BehaviourTreeInternal;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TreeNodeBaseを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the TreeNodeBase.
		/// </summary>
#endif
		public TreeNodeBase treeNode
		{
			get
			{
				return node as TreeNodeBase;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enabledの初期化を行うために呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called to perform enabled initialization.
		/// </summary>
#endif
		protected sealed override void OnInitializeEnabled()
		{
			enabled = false;
			if (treeNode.isActive)
			{
				this.Activate(true, true);
			}
		}

		void CallAbortEvent()
		{
			if (!this.IsActive())
			{
				return;
			}

			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnAbort()")))
#endif
				{
					OnAbort();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

		internal void AbortInternal()
		{
			CallAbortEvent();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードが初めてアクティブになったときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node becomes active for the first time.
		/// </summary>
#endif
		protected virtual void OnAwake()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブになったときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node becomes active.
		/// </summary>
#endif
		protected virtual void OnStart()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブの間、毎フレーム更新する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when updating every frame while the own node is active.
		/// </summary>
#endif
		protected virtual void OnUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブの間、LateUpdateで呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called in LateUpdate while the local node is active.
		/// </summary>
#endif
		protected virtual void OnLateUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードがアクティブの間、FixedUpdateで呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called in FixedUpdate while the local node is active.
		/// </summary>
#endif
		protected virtual void OnFixedUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードが終了したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node ends.
		/// </summary>
#endif
		protected virtual void OnEnd()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数は自ノードが中止されるときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the own node is aborted.
		/// </summary>
#endif
		protected virtual void OnAbort()
		{
		}

		void IPlayableBehaviourCallbackReceiver.OnAwake()
		{
			OnAwake();
		}

		void IPlayableBehaviourCallbackReceiver.OnStart()
		{
			OnStart();
		}

		void IPlayableBehaviourCallbackReceiver.OnEnd()
		{
			OnEnd();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallUpdate()
		{
			return true;
		}

		void IPlayableBehaviourCallbackReceiver.OnUpdate()
		{
			OnUpdate();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallLateUpdate()
		{
			return true;
		}

		void IPlayableBehaviourCallbackReceiver.OnLateUpdate()
		{
			OnLateUpdate();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallFixedUpdate()
		{
			return true;
		}

		void IPlayableBehaviourCallbackReceiver.OnFixedUpdate()
		{
			OnFixedUpdate();
		}
	}
}