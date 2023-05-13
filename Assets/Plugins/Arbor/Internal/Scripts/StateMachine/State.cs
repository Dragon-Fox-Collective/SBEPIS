//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// ステートを表すクラス
	/// </summary>
#else
	/// <summary>
	/// Class that represents the state
	/// </summary>
#endif
	[System.Serializable]
	public sealed class State : Node, INodeBehaviourContainer
	{
		[SerializeField]
		private bool _Resident = false;

		[SerializeField]
		private List<Object> _Behaviours = new List<Object>();

		[SerializeField]
		private bool _BreakPoint;

		private class CallbackEntry
		{
			public bool isDirty = true;
			public StateBehaviour[] enableBehaviours = null;

			public void Update(List<Object> behaviours)
			{
				if (!isDirty)
				{
					return;
				}

				this.enableBehaviours = null;

				int behaviourCount = behaviours.Count;
				if (behaviourCount > 0)
				{
					List<StateBehaviour> enableBehaviours = new List<StateBehaviour>();

					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						StateBehaviour behaviour = behaviours[behaviourIndex] as StateBehaviour;
						if (behaviour == null || !behaviour.behaviourEnabled)
						{
							continue;
						}

						enableBehaviours.Add(behaviour);
					}

					if (enableBehaviours.Count > 0)
					{
						this.enableBehaviours = enableBehaviours.ToArray();
					}
				}
				
				isDirty = false;
			}
		}

		[System.NonSerialized]
		private CallbackEntry _CallbackEntry = new CallbackEntry();

		private bool _IsActive = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// FSMを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the state machine.
		/// </summary>
#endif
		public ArborFSMInternal stateMachine
		{
			get
			{
				return nodeGraph as ArborFSMInternal;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 非推奨。behaviourCountとGetBehaviourFromIndexを使用して下さい。
		/// </summary>
#else
		/// <summary>
		/// Deprecated. Use behaviourCount and GetBehaviourFromIndex.
		/// </summary>
#endif
		[System.Obsolete("use behaviourCount and GetBehaviourFromIndex()")]
		public StateBehaviour[] behaviours
		{
			get
			{
				ArrayList array = new ArrayList(_Behaviours);
				return (StateBehaviour[])array.ToArray(typeof(StateBehaviour));
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Behaviourの数を取得。
		/// </summary>
#else
		/// <summary>
		/// Get a count of Behavior.
		/// </summary>
#endif
		public int behaviourCount
		{
			get
			{
				return _Behaviours.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the state identifier.
		/// </summary>
#endif
		[System.Obsolete("use Node.nodeID")]
		public int stateID
		{
			get
			{
				return nodeID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 常駐する<see cref="State"/>かどうかを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets a value indicating whether this <see cref="State"/> is resident.
		/// </summary>
#endif
		public bool resident
		{
			get
			{
				return _Resident;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ブレークポイント。
		/// このプロパティがtrueのとき、ステートに入ったタイミングでエディタがポーズ状態になります。
		/// </summary>
#else
		/// <summary>
		/// Break point.
		/// When this property is true, the editor is in a pause state at the timing of entering the state.
		/// </summary>
#endif
		public bool breakPoint
		{
			get
			{
				return _BreakPoint;
			}
			set
			{
				_BreakPoint = value;
			}
		}

		private uint _TransitionCount;

#if ARBOR_DOC_JA
		/// <summary>
		/// transitionCountが変更されたときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when transitionCount changes
		/// </summary>
#endif
		public event System.Action onChangedTransitionCount;

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移回数。
		/// </summary>
#else
		/// <summary>
		/// Transition count.
		/// </summary>
#endif
		public uint transitionCount
		{
			get
			{
				return _TransitionCount;
			}
			set
			{
				if (_TransitionCount != value)
				{
					_TransitionCount = value;

					onChangedTransitionCount?.Invoke();
				}
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

		internal State(ArborFSMInternal stateMachine, int nodeID, bool resident, IList<System.Type> types) : base(stateMachine, nodeID)
		{
			_Resident = resident;
			name = "New State";

			if (types != null)
			{
				for (int i = 0; i < types.Count; i++)
				{
					System.Type type = types[i];
					AddBehaviour(type);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <param name="behaviour">追加するStateBehaviour</param>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <param name="behaviour">Add StateBehaviour</param>
#endif
		public void AddBehaviour(StateBehaviour behaviour)
		{
			var stateMachine = this.stateMachine; // improved get property performance.

			ComponentUtility.RecordObject(stateMachine, "Add Behaviour");

			_Behaviours.Add(behaviour);
			SetDirtyCallbackEntry();

			ComponentUtility.SetDirty(stateMachine);

			behaviour.OnAttachToNode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <param name="type">追加するStateBehaviourの型</param>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <param name="type">Type of add StateBehaviour</param>
		/// <returns>Added StateBehaviour</returns>
#endif
		public StateBehaviour AddBehaviour(System.Type type)
		{
			StateBehaviour behaviour = StateBehaviour.CreateStateBehaviour(this, type);

			AddBehaviour(behaviour);

			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <typeparam name="T">追加するStateBehaviourの型</typeparam>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of add StateBehaviour</typeparam>
		/// <returns>Added StateBehaviour</returns>
#endif
		public T AddBehaviour<T>() where T : StateBehaviour
		{
			T behaviour = StateBehaviour.CreateStateBehaviour<T>(this);

			AddBehaviour(behaviour);

			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを挿入。
		/// </summary>
		/// <param name="index">挿入先インデックス</param>
		/// <param name="behaviour">挿入するStateBehaviour</param>
#else
		/// <summary>
		/// Insert the behaviour.
		/// </summary>
		/// <param name="index">Insertion destination index</param>
		/// <param name="behaviour">Insert StateBehaviour</param>
#endif
		public void InsertBehaviour(int index, StateBehaviour behaviour)
		{
			var stateMachine = this.stateMachine; // improved get property performance.

			ComponentUtility.RecordObject(stateMachine, "Insert Behaviour");

			_Behaviours.Insert(index, behaviour);
			SetDirtyCallbackEntry();

			ComponentUtility.SetDirty(stateMachine);

			behaviour.OnAttachToNode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを挿入。
		/// </summary>
		/// <param name="index">挿入先インデックス</param>
		/// <param name="type">追加するStateBehaviourの型</param>
		/// <returns>挿入したStateBehaviour</returns>
#else
		/// <summary>
		/// Insert the behaviour.
		/// </summary>
		/// <param name="index">Insertion destination index</param>
		/// <param name="type">Type of add StateBehaviour</param>
		/// <returns>Inserted StateBehaviour</returns>
#endif
		public StateBehaviour InsertBehaviour(int index, System.Type type)
		{
			StateBehaviour behaviour = StateBehaviour.CreateStateBehaviour(this, type);

			InsertBehaviour(index, behaviour);

			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <typeparam name="T">挿入するStateBehaviourの型</typeparam>
		/// <param name="index">挿入先インデックス</param>
		/// <returns>挿入したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of insert StateBehaviour</typeparam>
		/// <param name="index">Insertion destination index</param>
		/// <returns>Inserted StateBehaviour</returns>
#endif
		public T InsertBehaviour<T>(int index) where T : StateBehaviour
		{
			T behaviour = StateBehaviour.CreateStateBehaviour<T>(this);

			InsertBehaviour(index, behaviour);

			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourをindexから取得。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>StateBehaviour</returns>
#else
		/// <summary>
		/// Get StateBehavior from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>StateBehaviour</returns>
#endif
		public StateBehaviour GetBehaviourFromIndex(int index)
		{
			return GetBehaviourObjectFromIndex(index) as StateBehaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourのObjectをindexから取得。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>Object</returns>
#else
		/// <summary>
		/// Get Object of StateBehavior from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Object</returns>
#endif
		public Object GetBehaviourObjectFromIndex(int index)
		{
			return _Behaviours[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public StateBehaviour GetBehaviour(System.Type type)
		{
			for (int i = 0; i < _Behaviours.Count; i++)
			{
				var behaviour = _Behaviours[i];
				System.Type classType = behaviour.GetType();
				if (classType == type || TypeUtility.IsSubclassOf(classType, type))
				{
					return behaviour as StateBehaviour;
				}
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public T GetBehaviour<T>() where T : StateBehaviour
		{
			System.Type type = typeof(T);

			for (int i = 0; i < _Behaviours.Count; i++)
			{
				var behaviour = _Behaviours[i];
				System.Type classType = behaviour.GetType();
				if (classType == type || TypeUtility.IsSubclassOf(classType, type))
				{
					return behaviour as T;
				}
			}
			return null;
		}

		private System.Array GetBehavioursInternal(System.Type type, bool useSearchTypeAsArrayReturnType)
		{
			ArrayList array = new ArrayList();

			for (int behaviourIndex = 0; behaviourIndex < _Behaviours.Count; behaviourIndex++)
			{
				Object behaviour = _Behaviours[behaviourIndex];
				System.Type classType = behaviour.GetType();

				if (TypeUtility.IsAssignableFrom(type, classType))
				{
					array.Add(behaviour as StateBehaviour);
				}
			}

			if (useSearchTypeAsArrayReturnType)
			{
				return array.ToArray(type);
			}
			else
			{
				return array.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public StateBehaviour[] GetBehaviours(System.Type type)
		{
			return (StateBehaviour[])GetBehavioursInternal(type, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public T[] GetBehaviours<T>() where T : StateBehaviour
		{
			return (T[])GetBehavioursInternal(typeof(T), true);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourが含まれているかどうか。
		/// </summary>
		/// <param name="behaviour">判定するStateBehaviour。</param>
		/// <returns>含まれているかどうか。</returns>
#else
		/// <summary>
		/// Whether StateBehaviour are included.
		/// </summary>
		/// <param name="behaviour">Judges StateBehaviour.</param>
		/// <returns>Whether included are.</returns>
#endif
		public bool Contains(StateBehaviour behaviour)
		{
			return _Behaviours.Contains(behaviour);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourのインデックスを返す。
		/// </summary>
		/// <param name="behaviourObj">検索するStateBehaviour</param>
		/// <returns>見つかった場合はインデックス、ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Return index of StateBehaviour.
		/// </summary>
		/// <param name="behaviourObj">The StateBehaviour to locate in the State.</param>
		/// <returns>Returns an index if found, -1 otherwise.</returns>
#endif
		public int IndexOfBehaviour(Object behaviourObj)
		{
			return _Behaviours.IndexOf(behaviourObj);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを削除する。インスタンスは削除されないため、<see cref="StateBehaviour.Destroy" />を使用すること。
		/// </summary>
		/// <param name="behaviourObj">削除するStateBehaviour。</param>
#else
		/// <summary>
		/// I want to remove the StateBehaviour. For instance is not deleted, that you use the <see cref = "StateBehaviour.Destroy" />.
		/// </summary>
		/// <param name="behaviourObj">StateBehaviour you want to remove.</param>
#endif
		public void RemoveBehaviour(Object behaviourObj)
		{
			RemoveBehaviourAt(_Behaviours.IndexOf(behaviourObj));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを削除する。インスタンスは削除されないため、<see cref="StateBehaviour.Destroy" />を使用すること。
		/// </summary>
		/// <param name="behaviourIndex">削除するStateBehaviourのインデックス。</param>
#else
		/// <summary>
		/// I want to remove the StateBehaviour. For instance is not deleted, that you use the <see cref = "StateBehaviour.Destroy" />.
		/// </summary>
		/// <param name="behaviourIndex">Index of StateBehaviour to remove.</param>
#endif
		public void RemoveBehaviourAt(int behaviourIndex)
		{
			if (0 <= behaviourIndex && behaviourIndex < _Behaviours.Count)
			{
				var stateMachine = this.stateMachine; // improved get property performance.

				ComponentUtility.RecordObject(stateMachine, "Remove Behaviour");
				_Behaviours.RemoveAt(behaviourIndex);
				ComponentUtility.SetDirty(stateMachine);

				SetDirtyCallbackEntry();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourの順番を入れ替える。
		/// </summary>
		/// <param name="fromIndex">入れ替えたいインデックス。</param>
		/// <param name="toIndex">入れ替え先インデックス。</param>
#else
		/// <summary>
		/// Swap the order of StateBehaviour.
		/// </summary>
		/// <param name="fromIndex">The swapping want index.</param>
		/// <param name="toIndex">Exchange destination index.</param>
#endif
		public void SwapBehaviour(int fromIndex, int toIndex)
		{
			Object behaviour = _Behaviours[toIndex];
			_Behaviours[toIndex] = _Behaviours[fromIndex];
			_Behaviours[fromIndex] = behaviour;

			SetDirtyCallbackEntry();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourの順番を移動する。
		/// </summary>
		/// <param name="fromIndex">移動させたいインデックス。</param>
		/// <param name="toState">移動先のState。</param>
		/// <param name="toIndex">移動先のインデックス。</param>
#else
		/// <summary>
		/// Move the order of StateBehaviour.
		/// </summary>
		/// <param name="fromIndex">The moving want index.</param>
		/// <param name="toState">The destination State.</param>
		/// <param name="toIndex">The destination index.</param>
#endif
		public void MoveBehaviour(int fromIndex, State toState, int toIndex)
		{
			if (toState == null)
			{
				return;
			}

			if (this == toState)
			{
				Object behaviour = _Behaviours[fromIndex];
				_Behaviours.RemoveAt(fromIndex);
				_Behaviours.Insert(toIndex, behaviour);

				SetDirtyCallbackEntry();
			}
			else
			{
				Object behaviour = _Behaviours[fromIndex];

				StateBehaviour stateBehaviour = behaviour as StateBehaviour;
				if (stateBehaviour != null)
				{
					_Behaviours.RemoveAt(fromIndex);
					SetDirtyCallbackEntry();

					ComponentUtility.RecordObject(stateBehaviour, "Move Behaviour");

					stateBehaviour.Initialize(nodeGraph, toState.nodeID);

					ComponentUtility.SetDirty(stateBehaviour);

					toState._Behaviours.Insert(toIndex, behaviour);
					toState.SetDirtyCallbackEntry();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを設定する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <param name="behaviour">StateBehaviour</param>
		/// <remarks>
		/// 元のStateBehaviourは破棄しないため注意。
		/// StateBehaviourを追加したい場合はAddBehaviourを使用して下さい。
		/// また、破棄したい場合はDestroyBehaviourを使用して下さい。
		/// </remarks>
#else
		/// <summary>
		/// Set StateBehaviour.
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="behaviour">StateBehaviour</param>
		/// <remarks>
		/// Note that the original StateBehaviour does not Destroy.
		/// If you want to add StateBehaviour, please use AddBehaviour.
		/// If you want to do Destroy please use DestroyBehaviour.
		/// </remarks>
#endif
		public void SetBehaviour(int index, StateBehaviour behaviour)
		{
			_Behaviours[index] = behaviour;
			SetDirtyCallbackEntry();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeが所属するNodeGraphが変わった際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when the NodeGraph to which the Node belongs has changed.
		/// </summary>
#endif
		protected override void OnGraphChanged()
		{
			Object[] sourceBehaviours = _Behaviours.ToArray();
			_Behaviours.Clear();
			SetDirtyCallbackEntry();

			for (int i = 0; i < sourceBehaviours.Length; i++)
			{
				Object obj = sourceBehaviours[i];
				StateBehaviour sourceBehaviour = obj as StateBehaviour;
				if (sourceBehaviour != null)
				{
					ComponentUtility.MoveBehaviour(this, sourceBehaviour);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを含んでいるかをチェックする。
		/// </summary>
		/// <param name="behaviour">チェックするNodeBehaviour</param>
		/// <returns>NodeBehaviourを含んでいる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check if it contains NodeBehaviour.
		/// </summary>
		/// <param name="behaviour">Check NodeBehaviour</param>
		/// <returns>Returns true if it contains NodeBehaviour.</returns>
#endif
		public override bool IsContainsBehaviour(NodeBehaviour behaviour)
		{
			StateBehaviour stateBehaviour = behaviour as StateBehaviour;
			if (stateBehaviour != null)
			{
				return Contains(stateBehaviour);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部処理用。
		/// </summary>
		/// <param name="stateID">切断するステートID</param>
#else
		/// <summary>
		/// For internal.
		/// </summary>
		/// <param name="stateID">State ID to disconnect</param>
#endif
		public void DisconnectState(int stateID)
		{
			int behaviourCount = _Behaviours.Count;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = _Behaviours[behaviourIndex] as StateBehaviour;
				if (behaviour != null)
				{
					behaviour.DisconnectState(stateID);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを破棄する。
		/// </summary>
		/// <param name="behaviourObj">破棄したいStateBehaviour.</param>
#else
		/// <summary>
		/// Destroy StateBehaviour
		/// </summary>
		/// <param name="behaviourObj">The StateBehavior you want to destroy.</param>
#endif
		public void DestroyBehaviour(Object behaviourObj)
		{
			int behaviourIndex = IndexOfBehaviour(behaviourObj);

			DestroyBehaviourAt(behaviourIndex);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを破棄する。
		/// </summary>
		/// <param name="behaviourIndex">破棄したいStateBehaviourのインデックス。</param>
#else
		/// <summary>
		/// Destroy StateBehaviour
		/// </summary>
		/// <param name="behaviourIndex">Index of StateBehaviour you want to destroy.</param>
#endif
		public void DestroyBehaviourAt(int behaviourIndex)
		{
			if (!(0 <= behaviourIndex && behaviourIndex < _Behaviours.Count))
			{
				return;
			}

			Object behaviourObj = GetBehaviourObjectFromIndex(behaviourIndex);

			stateMachine.DisconnectDataBranch(behaviourObj);

			RemoveBehaviourAt(behaviourIndex);

			NodeBehaviour nodeBehaviour = behaviourObj as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				NodeBehaviour.Destroy(nodeBehaviour);
			}
			else if (behaviourObj != null)
			{
				ComponentUtility.Destroy(behaviourObj);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部処理用。
		/// </summary>
#else
		/// <summary>
		/// For internal.
		/// </summary>
#endif
		public void DestroyBehaviours()
		{
			int behaviourCount = _Behaviours.Count;
			for (int behaviourIndex = behaviourCount - 1; behaviourIndex >= 0; behaviourIndex--)
			{
				DestroyBehaviourAt(behaviourIndex);
			}
		}

		private bool _IsAwake = false;

		internal void Awake()
		{
			if (_IsAwake)
			{
				return;
			}

			_IsAwake = true;

			UpdateCallbackEntry();
		}

		internal void Activate(bool enable)
		{
			if (_IsActive == enable)
			{
				return;
			}

			_IsActive = enable;

			if (enable)
			{
				Awake();
			}
			
			UpdateCallbackEntry();
			
			StateBehaviour[] behaviours = _CallbackEntry.enableBehaviours;
			if (behaviours != null)
			{
				int behaviourCount = behaviours.Length;
				if (behaviourCount > 0)
				{
					// improved get property performance.
					var stateMachine = this.stateMachine;
					bool enterStoppingPlayState = stateMachine._EnterStoppingPlayState;
					bool activateNormalState = enable && !_Resident;
					int nodeID = this.nodeID;

					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						StateBehaviour behaviour = behaviours[behaviourIndex];

						behaviour.Activate(enable, true);

						if (activateNormalState)
						{
							if ((!enterStoppingPlayState && stateMachine.playState != PlayState.Playing) || stateMachine.IsLeavedState(nodeID))
							{
								break;
							}
						}
					}
				}
			}

			if (enable)
			{
				// improved get property performance.
				uint transitionCount = this.transitionCount;
				if (transitionCount < uint.MaxValue)
				{
					transitionCount++;
				}
				this.transitionCount = transitionCount;
				if (_BreakPoint)
				{
					nodeGraph.BreakNode(this);
				}
			}
		}

		internal void Pause()
		{
			int behaviourCount = _Behaviours.Count;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = _Behaviours[behaviourIndex] as StateBehaviour;
				if (behaviour != null && behaviour.behaviourEnabled)
				{
					behaviour.Pause();
				}
			}
		}

		internal void Resume()
		{
			var stateMachine = this.stateMachine; // improved get property performance.

			int behaviourCount = _Behaviours.Count;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = _Behaviours[behaviourIndex] as StateBehaviour;
				if (behaviour != null && behaviour.behaviourEnabled)
				{
					behaviour.Resume();
					if (!_Resident)
					{
						if (stateMachine.playState != PlayState.Playing || stateMachine.IsLeavedState(nodeID))
						{
							break;
						}
					}
				}
			}
		}

		internal void Stop()
		{
			int behaviourCount = _Behaviours.Count;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = _Behaviours[behaviourIndex] as StateBehaviour;
				if (behaviour != null && behaviour.behaviourEnabled)
				{
					behaviour.Stop();
				}
			}
		}

		internal void SetDirtyCallbackEntry()
		{
			if (!_IsAwake)
			{
				return;
			}

			if (_CallbackEntry != null)
			{
				_CallbackEntry.isDirty = true;
			}
		}

		private void UpdateCallbackEntry()
		{
			if (!_IsAwake)
			{
				return;
			}

			if (_CallbackEntry == null)
			{
				_CallbackEntry = new CallbackEntry();
			}

			_CallbackEntry.Update(_Behaviours);
		}

		internal void UpdateBehaviours()
		{
			var stateMachine = this.stateMachine; // improved get property performance.
			if (!stateMachine.isActiveAndEnabled)
			{
				return;
			}

			UpdateCallbackEntry();
			StateBehaviour[] behaviours = _CallbackEntry.enableBehaviours;
			if (behaviours == null)
			{
				return;
			}

			int nodeID = this.nodeID; // improved get property performance.

			int behaviourCount = behaviours.Length;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = behaviours[behaviourIndex];

				behaviour.Update();

				if (!_Resident && stateMachine.IsLeavedState(nodeID))
				{
					return;
				}
			}
		}

		internal void LateUpdateBehaviours()
		{
			var stateMachine = this.stateMachine; // improved get property performance.
			if (!stateMachine.isActiveAndEnabled)
			{
				return;
			}

			UpdateCallbackEntry();
			StateBehaviour[] behaviours = _CallbackEntry.enableBehaviours;
			if (behaviours == null)
			{
				return;
			}

			int nodeID = this.nodeID;  // improved get property performance.
			int behaviourCount = behaviours.Length;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = behaviours[behaviourIndex];

				var callbackInfo = behaviour.callbackInfo;

				behaviour.LateUpdate();

				if (!_Resident && stateMachine.IsLeavedState(nodeID))
				{
					return;
				}
			}
		}

		internal void FixedUpdateBehaviours()
		{
			var stateMachine = this.stateMachine; // improved get property performance.
			if (!stateMachine.isActiveAndEnabled)
			{
				return;
			}

			UpdateCallbackEntry();
			StateBehaviour[] behaviours = _CallbackEntry.enableBehaviours;
			if (behaviours == null)
			{
				return;
			}

			int nodeID = this.nodeID;  // improved get property performance.
			int behaviourCount = behaviours.Length;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = behaviours[behaviourIndex];

				behaviour.FixedUpdate();

				if (!_Resident && stateMachine.IsLeavedState(nodeID))
				{
					return;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーメッセージを送信する。<see cref="StateBehaviour.OnStateTrigger"/>が呼び出される。
		/// </summary>
		/// <param name="message">送信するメッセージ</param>
#else
		/// <summary>
		/// Send a trigger message.<see cref = "StateBehaviour.OnStateTrigger" /> is called.
		/// </summary>
		/// <param name="message">Message to be sent</param>
#endif
		public void SendTrigger(string message)
		{
			var stateMachine = this.stateMachine; // improved get property performance.
			if (!stateMachine.isActiveAndEnabled)
			{
				return;
			}

			UpdateCallbackEntry();

			StateBehaviour[] behaviours = _CallbackEntry.enableBehaviours;
			if (behaviours == null)
			{
				return;
			}

			int behaviourCount = behaviours.Length;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour behaviour = behaviours[behaviourIndex];
				behaviour.CallSendTriggerInternal(message);
			}
		}

		int INodeBehaviourContainer.GetNodeBehaviourCount()
		{
			return behaviourCount;
		}

		T INodeBehaviourContainer.GetNodeBehaviour<T>(int index)
		{
			return GetBehaviourObjectFromIndex(index) as T;
		}

		void INodeBehaviourContainer.SetNodeBehaviour(int index, NodeBehaviour behaviour)
		{
			SetBehaviour(index, behaviour as StateBehaviour);
		}
	}
}
