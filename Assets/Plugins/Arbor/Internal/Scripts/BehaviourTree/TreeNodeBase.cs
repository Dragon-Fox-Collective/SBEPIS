//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeのノードの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class of Behavior Tree's node.
	/// </summary>
#endif
	public abstract class TreeNodeBase : Node
	{
		#region Serialize fields

		[SerializeField]
		private bool _EnablePriority = false;

		[SerializeField]
		private int _Priority = 0;

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートノードから辿って接続されていればtrueを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns true if it is traced from the root node and connected.
		/// </summary>
#endif
		public bool enablePriority
		{
			get
			{
				return _EnablePriority;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの優先順位。
		/// </summary>
#else
		/// <summary>
		/// The priority of the node.
		/// </summary>
#endif
		public int priority
		{
			get
			{
				return _Priority;
			}
		}

		private NodeStatus _Status;
		private bool _IsActive = false;

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
		/// 親ノードを取得。
		/// </summary>
#else
		/// <summary>
		/// Get parent node.
		/// </summary>
#endif
		public TreeNodeBase parentNode
		{
			get
			{
				var parentLinkHolder = this as IParentLinkSlotHolder;
				if (parentLinkHolder == null)
				{
					return null;
				}

				NodeBranch branch = parentLinkHolder.GetParentBranch();
				if (branch == null)
				{
					return null;
				}

				return behaviourTree.GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの状態。
		/// </summary>
#else
		/// <summary>
		/// The state of the node.
		/// </summary>
#endif
		public NodeStatus status
		{
			get
			{
				return _Status;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブならtrueを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns true if it is active.
		/// </summary>
#endif
		public bool isActive
		{
			get
			{
				return _IsActive;
			}
		}

		internal TreeNodeBase(NodeGraph nodeGraph, int nodeID)
			: base(nodeGraph, nodeID)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 親へのNodeLinkSlotを持っているかどうか
		/// </summary>
		/// <returns>持っている場合はtrue、なければfalse。</returns>
#else
		/// <summary>
		/// Whether this node has a NodeLinkSlot to parent.
		/// </summary>
		/// <returns>True if it has a NodeLinkSlot to parent, false otherwise.</returns>
#endif
		[System.Obsolete("use IParentLinkSlotHolder", true)]
		public bool HasParentLinkSlot()
		{
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 子へのNodeLinkSlotを持っているかどうか
		/// </summary>
		/// <returns>持っている場合はtrue、なければfalse。</returns>
#else
		/// <summary>
		/// Whether this node has a NodeLinkSlot to child.
		/// </summary>
		/// <returns>True if it has a NodeLinkSlot to child, false otherwise.</returns>
#endif
		[System.Obsolete("use IChildLinkSlotHolder", true)]
		public bool HasChildLinkSlot()
		{
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// priorityが変更されたときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when priority changes.
		/// </summary>
#endif
		public event System.Action onChangedPriority;

		void CallChangedPriority()
		{
			onChangedPriority?.Invoke();
		}

		internal void ClearPriority()
		{
			_EnablePriority = false;
			_Priority = 0;

			CallChangedPriority();
		}

		internal int CalculatePriority(int priority)
		{
			_EnablePriority = true;
			_Priority = priority;

			CallChangedPriority();

			priority++;

			var childLinkSlotHolder = this as IChildLinkSlotHolder;
			if (childLinkSlotHolder != null)
			{
				priority = childLinkSlotHolder.OnCalculateChildPriority(priority);
			}

			return priority;
		}

		internal bool Activate(bool active, bool interrupt, bool isRevaluator)
		{
			if (_IsActive == active)
			{
				return true;
			}

			_IsActive = active;

			if (active)
			{
				_Status = NodeStatus.Running;
			}

			return OnActivate(active, interrupt, isRevaluator);
		}

		internal virtual bool OnActivate(bool active, bool interrupt, bool isRevaluator)
		{
			return true;
		}

		internal void Pause()
		{
			OnPause();
		}

		internal virtual void OnPause()
		{
		}

		internal void Resume()
		{
			OnResume();
		}

		internal virtual void OnResume()
		{
		}

		internal void Stop()
		{
			OnStop();
			_IsActive = false;
		}

		internal virtual void OnStop()
		{
		}

		internal virtual bool HasConditionCheck()
		{
			return false;
		}

		internal virtual bool OnConditionCheck(AbortFlags abortFlags)
		{
			return true;
		}

		internal virtual bool HasAbortFlags(AbortFlags abortFlags)
		{
			return false;
		}

		internal bool ConditionCheck(AbortFlags abortFlags)
		{
			return OnConditionCheck(abortFlags);
		}

		private bool _IsExecuting = false;

		internal void FinishExecute(bool result)
		{
			if (!_IsExecuting)
			{
				Debug.LogError("FinishExecute can only be used from OnExecute.");
				return;
			}

			FinishExecuteInternal(result);
		}

		internal void FinishExecuteInternal(bool result)
		{
			result = OnFinishExecute(result);

			_Status = result ? NodeStatus.Success : NodeStatus.Failure;
		}

		internal virtual bool OnFinishExecute(bool result)
		{
			return result;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when executing.
		/// </summary>
#endif
		protected abstract void OnExecute();

		internal virtual bool RepeatCheck(bool nodeResult)
		{
			return false;
		}

		internal virtual void OnRestart()
		{
		}

		internal virtual void OnUpdate()
		{
		}

		internal virtual void OnLateUpdate()
		{
		}

		internal virtual void OnFixedUpdate()
		{
		}

		internal NodeStatus Execute()
		{
			_IsExecuting = true;

			OnExecute();

			_IsExecuting = false;

			if (_Status != NodeStatus.Running)
			{
				if (RepeatCheck(_Status == NodeStatus.Success))
				{
					_Status = NodeStatus.Running;
					OnRestart();
				}
			}

			return _Status;
		}

		internal virtual void OnChildExecuted(NodeStatus status)
		{
		}

		internal virtual void OnAbort()
		{
		}

		internal void Abort()
		{
			OnAbort();

			FinishExecuteInternal(false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 削除できるかどうかを返す。
		/// </summary>
		/// <returns>削除できる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it can be deleted.
		/// </summary>
		/// <returns>Returns true if it can be deleted.</returns>
#endif
		public override bool IsDeletable()
		{
			return !_IsActive;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードを文字列に変換（デバッグ用）。
		/// </summary>
		/// <returns>変換された文字列</returns>
#else
		/// <summary>
		/// Convert node to string (for debugging).
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			if (_EnablePriority)
			{
				return GetName() + "(" + priority + ")";
			}
			else
			{
				return GetName() + "(disable)";
			}
		}
	}
}