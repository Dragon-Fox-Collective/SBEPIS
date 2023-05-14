//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
	using Arbor.Events;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listに要素が含まれているかを判定する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Determines if the List contains elements.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/List.Contains")]
	[BehaviourTitle("List.Contains", useNicifyName = false)]
	[BuiltInBehaviour]
	public sealed class ListContainsCalculator : ListElementCalculatorBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 結果の出力スロット
		/// </summary>
#else
		/// <summary>
		/// Resulting output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotBool _Output = new OutputSlotBool();

		protected override void OnExecute(ListMediator mediator, IList list, IValueGetter valueContainer)
		{
			if (valueContainer != null)
			{
				bool contains = mediator.Contains(list, valueContainer);
				_Output.SetValue(contains);
			}
			else
			{
				_Output.SetValue(false);
			}
		}
	}
}