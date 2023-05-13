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
	/// CompositeNodeとActionNodeを装飾を定義するクラス。継承して利用する。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="AddBehaviourMenu" /></description></item>
	/// <item><description><see cref="HideBehaviour" /></description></item>
	/// <item><description><see cref="BehaviourTitle" /></description></item>
	/// <item><description><see cref="BehaviourHelp" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// A class that defines decoration for composite nodes and action nodes. Inherit and use it.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="AddBehaviourMenu" /></description></item>
	/// <item><description><see cref="HideBehaviour" /></description></item>
	/// <item><description><see cref="BehaviourTitle" /></description></item>
	/// <item><description><see cref="BehaviourHelp" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[Internal.DocumentManual("/manual/scripting/behaviourtree/decorator.md")]
	public class Decorator : TreeNodeBehaviour
	{
		#region Serialize fields

		[SerializeField]
		[HideInInspector]
		private bool _BehaviourEnabled = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// 中断フラグ。
		/// </summary>
#else
		/// <summary>
		/// Abort Flags.
		/// </summary>
#endif
		[SerializeField]
		[HideInInspector, Internal.Documentable]
		private AbortFlags _AbortFlags = (AbortFlags)0;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンディション判定結果の論理演算方法
		/// </summary>
#else
		/// <summary>
		/// Logical operation method of condition check result
		/// </summary>
#endif
		[SerializeField]
		[HideInInspector, Internal.Documentable]
		private LogicalCondition _LogicalCondition = new LogicalCondition();

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Decoratorの有効状態を取得/設定。
		/// </summary>
		/// <value>
		///   <c>true</c> 有効; その他、 <c>false</c>.
		/// </value>
#else
		/// <summary>
		/// Gets or sets a value indicating whether [behaviour enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [behaviour enabled]; otherwise, <c>false</c>.
		/// </value>
#endif
		public bool behaviourEnabled
		{
			get
			{
				return _BehaviourEnabled;
			}
			set
			{
				if (_BehaviourEnabled != value)
				{
					_BehaviourEnabled = value;
					if (treeNode.isActive)
					{
						if (_BehaviourEnabled)
						{
							this.Activate(true, !this.IsActive());
						}
						else
						{
							this.Activate(false, false);
						}
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 中止フラグ
		/// </summary>
#else
		/// <summary>
		/// Abort flags
		/// </summary>
#endif
		public AbortFlags abortFlags
		{
			get
			{
				return _AbortFlags;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 複数のDecoratorがあるに一つ前のDecoratorの結果との論理演算方法を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the logical operation method with the result of the previous Decorator when there are multiple Decorators.
		/// </summary>
#endif
		public LogicalOperation logicalOperation
		{
			get
			{
				return _LogicalCondition.logicalOperation;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再評価を行うかを返す。
		/// </summary>
#else
		/// <summary>
		/// It returns whether to reevaluate.
		/// </summary>
#endif
		public bool isRevaluation
		{
			get
			{
				return behaviourEnabled &&
					behaviourTree.IsRevaluation(treeNode) &&
					(treeNode.isActive && CompareAbortFlags(AbortFlags.Self) ||
					!treeNode.isActive && CompareAbortFlags(AbortFlags.LowerPriority));
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デコレータのコンディション
		/// </summary>
#else
		/// <summary>
		/// The condition of the decorator
		/// </summary>
#endif
		public enum Condition
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// コンディションなし
			/// </summary>
#else
			/// <summary>
			/// No condition
			/// </summary>
#endif
			None,

#if ARBOR_DOC_JA
			/// <summary>
			/// 成功
			/// </summary>
#else
			/// <summary>
			/// Success
			/// </summary>
#endif
			Success,

#if ARBOR_DOC_JA
			/// <summary>
			/// 失敗
			/// </summary>
#else
			/// <summary>
			/// Failure
			/// </summary>
#endif
			Failure,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// currentConditionが変更されたときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when the currentCondition changes
		/// </summary>
#endif
		public System.Action onConditionChanged;

		private Condition _CurrentCondition;

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在のコンディション
		/// </summary>
#else
		/// <summary>
		/// Current condition
		/// </summary>
#endif
		public Condition currentCondition
		{
			get
			{
				return _CurrentCondition;
			}
			private set
			{
				if (_CurrentCondition != value)
				{
					_CurrentCondition = value;

					onConditionChanged?.Invoke();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Decoratorを作成する。
		/// </summary>
		/// <param name="node">Node</param>
		/// <param name="type">Decoratorの型</param>
		/// <returns>作成したDecorator。</returns>
#else
		/// <summary>
		/// Create Decorator
		/// </summary>
		/// <param name="node">Node</param>
		/// <param name="type">Decorator type</param>
		/// <returns>Created Decorator.</returns>
#endif
		public static Decorator Create(Node node, System.Type type)
		{
			System.Type classType = typeof(Decorator);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `Decorator' in order to use it as parameter `type'", "type");
			}

			return CreateNodeBehaviour(node, type) as Decorator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Decoratorを作成する。
		/// </summary>
		/// <typeparam name="Type">Decoratorの型</typeparam>
		/// <param name="node">Node</param>
		/// <returns>作成したDecorator。</returns>
#else
		/// <summary>
		/// Create Decorator
		/// </summary>
		/// <typeparam name="Type">Decorator type</typeparam>
		/// <param name="node">Node</param>
		/// <returns>Created Decorator.</returns>
#endif
		public static Type Create<Type>(Node node) where Type : Decorator
		{
			return CreateNodeBehaviour<Type>(node);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ConditionCheckを行うか判定する。
		/// </summary>
		/// <remarks>
		/// デフォルトではtrueを返す。
		/// 無効にする場合は、overrideしてfalseを返してください。
		/// </remarks>
		/// <returns>ConditionCheckを行う場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It is judged whether Condition Check should be performed.
		/// </summary>
		/// <remarks>
		/// It returns true by default.
		/// To invalidate, please override and return false.
		/// </remarks>
		/// <returns>Returns true when performing ConditionCheck.</returns>
#endif
		public virtual bool HasConditionCheck()
		{
			return true;
		}

		bool CallHasConditionCheck()
		{
			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("HasConditionCheck()")))
#endif
				{
					return HasConditionCheck();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
				return false;
			}
		}

		internal bool CallConditionCheckInternal()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			if (!CallHasConditionCheck())
			{
				return true;
			}

			bool isCondition = false;

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnConditionCheck()")))
#endif
				{
					isCondition = OnConditionCheck();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}

			if (_LogicalCondition.notOp)
			{
				isCondition = !isCondition;
			}

			currentCondition = isCondition ? Condition.Success : Condition.Failure;

			return isCondition;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ConditionCheckを行う。
		/// </summary>
		/// <returns>条件に一致する場合はtrue、一致しなければfalseを返す。</returns>
#else
		/// <summary>
		/// Perform ConditionCheck.
		/// </summary>
		/// <returns>Returns true if it matches the condition, false if it does not match.</returns>
#endif
		protected virtual bool OnConditionCheck()
		{
			return true;
		}

		internal bool CallFinishExecuteInternal(bool result)
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnFinishExecute()")))
#endif
				{
					return OnFinishExecute(result);
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
				return result;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FinishExecuteのコールバック。<br />
		/// 実行結果を変更できます。
		/// </summary>
		/// <param name="result">実行結果。</param>
		/// <returns>変更した結果。</returns>
#else
		/// <summary>
		/// FinishExecute callback.<br />
		/// You can change the result of the execution result.
		/// </summary>
		/// <param name="result">Execution result.</param>
		/// <returns>Changed result.</returns>
#endif
		protected virtual bool OnFinishExecute(bool result)
		{
			return result;
		}

		internal bool CallRepeatCheckInternal(bool nodeResult)
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnRepeatCheck()")))
#endif
				{
					return OnRepeatCheck(nodeResult);
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
				return false;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 自ノードが終了した際に再度繰り返すかを判定するコールバック。
		/// </summary>
		/// <returns>繰り返す場合はtrue、しない場合はfalseを返す。</returns>
#else
		/// <summary>
		/// Callback to decide whether to repeat again when the self node finishes.
		/// </summary>
		/// <returns>Returns true if to repeat, false otherwise.</returns>
#endif
		protected virtual bool OnRepeatCheck()
		{
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 自ノードが終了した際に再度繰り返すかを判定するコールバック。
		/// </summary>
		/// <param name="nodeResult">ノードの処理結果</param>
		/// <returns>繰り返す場合はtrue、しない場合はfalseを返す。</returns>
#else
		/// <summary>
		/// Callback to decide whether to repeat again when the self node finishes.
		/// </summary>
		/// <param name="nodeResult">Node processing result</param>
		/// <returns>Returns true if to repeat, false otherwise.</returns>
#endif
		protected virtual bool OnRepeatCheck(bool nodeResult)
		{
			return OnRepeatCheck();
		}

		internal bool CompareAbortFlags(AbortFlags abortFlags)
		{
			if (abortFlags == 0)
			{
				return true;
			}
			return (_AbortFlags & abortFlags) == abortFlags;
		}

		internal void ClearCondition()
		{
			currentCondition = Condition.None;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再評価対象に入った時に呼び出される
		/// </summary>
#else
		/// <summary>
		/// Called when entering the re-evaluation target
		/// </summary>
#endif
		protected virtual void OnRevaluationEnter()
		{
		}

		internal void CallRevaluationEnter()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnRevaluationEnter()")))
#endif
				{
					OnRevaluationEnter();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再評価対象から抜けた時に呼び出される
		/// </summary>
#else
		/// <summary>
		/// Called when exiting the re-evaluation target
		/// </summary>
#endif
		protected virtual void OnRevaluationExit()
		{
		}

		internal void CallRevaluationExit()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnRevaluationExit()")))
#endif
				{
					OnRevaluationExit();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}
	}
}