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
	/// Listの要素が格納されているインデックスを取得する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Gets the index where the List elements are stored.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/List.IndexOf")]
	[BehaviourTitle("List.IndexOf", useNicifyName = false)]
	[BuiltInBehaviour]
	public sealed class ListIndexOfCalculator : ListElementCalculatorBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 結果の出力スロット。インデックスが見つからなかった場合は-1を返す。
		/// </summary>
#else
		/// <summary>
		/// Resulting output slot. Returns -1 if no index was found.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotInt _Output = new OutputSlotInt();

		// Use this for calculate
		protected override void OnExecute(ListMediator mediator, IList list, IValueGetter valueContainer)
		{
			if (valueContainer != null)
			{
				int index = mediator.IndexOf(list, valueContainer);
				_Output.SetValue(index);
			}
			else
			{
				_Output.SetValue(-1);
			}
		}
	}
}