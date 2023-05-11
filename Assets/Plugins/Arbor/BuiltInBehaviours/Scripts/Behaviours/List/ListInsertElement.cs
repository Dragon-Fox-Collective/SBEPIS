﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
	using System;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listに要素を挿入する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Insert an element into the List.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/List.InsertElement")]
	[BehaviourTitle("List.InsertElement", useNicifyName = false)]
	[BuiltInBehaviour]
	public sealed class ListInsertElement : ListElementBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 挿入するインデックス
		/// </summary>
#else
		/// <summary>
		/// The index to insert
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Index = new FlexibleInt();

		protected override object OnExecute(ListMediator mediator, IList list, IValueGetter container, ListInstanceType outputType)
		{
			int index = _Index.value;

			return mediator.InsertElement(list, index, container, outputType);
		}
	}
}