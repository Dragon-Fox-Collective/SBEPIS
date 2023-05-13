//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Calculatorを再演算する際のモード
	/// </summary>
#else
	/// <summary>
	/// Mode when recalculating Calculator
	/// </summary>
#endif
	[Internal.Documentable]
	public enum RecalculateMode
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットやパラメータの変更を監視し必要な時のみ再計算する。
		/// </summary>
		/// <remarks>参照しているインスタンスのメンバーについては監視しないため、必要に応じてOnCheckDirtyを定義すること。</remarks>
#else
		/// <summary>
		/// Monitor changes in input slots and parameters and recalculate only when necessary.
		/// </summary>
		/// <remarks>Since the members of the referenced instance are not monitored, OnCheckDirty should be defined as necessary.</remarks>
#endif
		Dirty,

#if ARBOR_DOC_JA
		/// <summary>
		/// フレームが更新されたら再計算する。
		/// </summary>
		/// <remarks>同一フレーム内で計算済みだった場合は、RecalculateMode.Dirtyと同等の動作となる。</remarks>
#else
		/// <summary>
		/// Recalculate when the frame is updated.
		/// </summary>
		/// <remarks>If it was already calculated in the same frame, the behavior is equivalent to RecalculateMode. </remarks>
#endif
		Frame,

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculateScope単位で再計算する。
		/// </summary>
		/// <remarks>
		/// グラフ用コールバックからのCalculator呼び出しであれば、UpdateやLateUpdateの呼び出し単位で再計算されるようになる。<br />
		/// ユーザースクリプトでUpdateなどのMonoBehaviourメッセージを処理する場合はグループ化されていないため、常に再計算される。<br />
		/// スコープ内で計算済みだった場合は、RecalculateMode.Dirtyと同等の動作となる。
		/// </remarks>
#else
		/// <summary>
		/// Recalculate in CalculatorScope units.
		/// </summary>
		/// <remarks>
		/// If it is a Calculator call from a callback for graphs, it will be recalculated in units of Update and LateUpdate calls.<br />
		/// When processing MonoBehaviour messages such as Update in user scripts, they are not grouped, so they are always recalculated.<br />
		/// If it has already been calculated in the scope, the behavior will be the same as RecalculateMode.Dirty.
		/// </remarks>
#endif
		Scope,

#if ARBOR_DOC_JA
		/// <summary>
		/// 常に再計算する。
		/// </summary>
		/// <remarks>複数の出力スロットを持つCalculatorでは、スロット接続単位で再計算される点に注意。</remarks>
#else
		/// <summary>
		/// Always recalculate.
		/// </summary>
		/// <remarks>Note that for calculators with multiple output slots, the calculation is recalculated in units of slot connections.</remarks>
#endif
		Always,
	}
}