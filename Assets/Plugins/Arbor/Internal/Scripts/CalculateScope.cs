//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 演算ノードのスコープ。演算ノードの再演算モードをスコープにしている場合、同一スコープ内の再計算をできうる限り抑制してパフォーマンスを改善するのに使用する。
	/// </summary>
#else
	/// <summary>
	/// Scope of the calculator node. When the recalculation mode of a calculator node is scoped, it is used to improve performance by suppressing recalculations in the same scope as much as possible.
	/// </summary>
#endif
	public class CalculateScope
	{
		private static Stack<CalculateScope> s_CalculateScopes = new Stack<CalculateScope>(64);
		private static Queue<CalculateScope> s_ReserveCalculateScopes = new Queue<CalculateScope>(64);
		private static Queue<UsingScope> s_ReserveUsingScopes = new Queue<UsingScope>(64);

		private static readonly int kReserveCount = 10;

		static CalculateScope()
		{
			for (int i = 0; i < kReserveCount; i++)
			{
				s_ReserveCalculateScopes.Enqueue(new CalculateScope());
				s_ReserveUsingScopes.Enqueue(new UsingScope(true));
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スコープを開始する。
		/// </summary>
#else
		/// <summary>
		/// Begin scope.
		/// </summary>
#endif
		public static void Begin()
		{
			CalculateScope scope = s_ReserveCalculateScopes.Count > 0? s_ReserveCalculateScopes.Dequeue() : new CalculateScope();
			s_CalculateScopes.Push(scope);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スコープを終了する。
		/// </summary>
#else
		/// <summary>
		/// End scope.
		/// </summary>
#endif
		public static void End()
		{
			if (s_CalculateScopes.Count > 0)
			{
				CalculateScope scope = s_CalculateScopes.Pop();
				scope.Clear();
				s_ReserveCalculateScopes.Enqueue(scope);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スコープを持っているかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether it has a scope.
		/// </summary>
#endif
		public static bool hasScope
		{
			get
			{
				return s_CalculateScopes.Count > 0;
			}
		}

		internal static CalculateScope currentScope
		{
			get
			{
				if (s_CalculateScopes.Count > 0)
				{
					return s_CalculateScopes.Peek();
				}

				return null;
			}
		}

		internal static void SetCalculated(Calculator calculator)
		{
			var scope = currentScope;
			if (scope != null)
			{
				scope.Add(calculator);
			}
		}

		internal static bool IsCalculated(Calculator calculator)
		{
			var scope = currentScope;
			if (scope != null)
			{
				return scope.Contains(calculator);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スコープを開く。usingステートメントで使用する。
		/// </summary>
		/// <returns>開いたスコープ。</returns>
#else
		/// <summary>
		/// Open the scope. Used in the using statement.
		/// </summary>
		/// <returns>Open scope.</returns>
#endif
		public static System.IDisposable OpenScope()
		{
			UsingScope scope = null;
			if (s_ReserveUsingScopes.Count > 0)
			{
				scope = s_ReserveUsingScopes.Dequeue();
			}
			else
			{
				scope = new UsingScope(true);
			}
			scope.Begin();
			return scope;
		}

		private class UsingScope : System.IDisposable
		{
			private bool _IsStarted;
			private int _ScopeCount;
			private bool _IsCache;

			internal UsingScope(bool isCache)
			{
				_IsCache = isCache;
			}

			internal void Begin()
			{
				if (_IsStarted)
				{
					return;
				}

				_IsStarted = true;

				_ScopeCount = s_CalculateScopes.Count;
				CalculateScope.Begin();
			}

			public void Dispose()
			{
				if (!_IsStarted)
				{
					return;
				}

				_IsStarted = false;

				End();

				int currentCount = s_CalculateScopes.Count;
				if (currentCount > _ScopeCount)
				{
					Debug.LogError("CalculateScope Error: You are pushing more CalculateScopes than you are popping. Make sure they are balanced.");
				}
				else if (currentCount < _ScopeCount)
				{
					Debug.LogError("CalculateScope Error: You are popping more CalculateScopes than you are pushing. Make sure they are balanced.");
				}

				// Clear extraneous Scopes
				while (s_CalculateScopes.Count > _ScopeCount)
				{
					End();
				}

				if (_IsCache)
				{
					s_ReserveUsingScopes.Enqueue(this);
				}
			}
		}

		private HashSet<int> _Calculators = new HashSet<int>();

		private CalculateScope()
		{
		}

		private void Add(Calculator calculator)
		{
			_Calculators.Add(calculator.GetInstanceID());
		}

		private bool Contains(Calculator calculator)
		{
			return _Calculators.Contains(calculator.GetInstanceID());
		}

		private void Clear()
		{
			_Calculators.Clear();
		}
	}
}