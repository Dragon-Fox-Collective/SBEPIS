//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listから指定したインデックスの要素を削除する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Removes the element with the specified index from List.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/List.RemoveAtIndex")]
	[BehaviourTitle("List.RemoveAtIndex", useNicifyName = false)]
	[BuiltInBehaviour]
	public sealed class ListRemoveAtIndex : ListBehaviourBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 削除するインデックス
		/// </summary>
#else
		/// <summary>
		/// Index to remove
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Index = new FlexibleInt();

		protected sealed override object OnExecute(System.Type elementType, IList list, ListInstanceType outputType)
		{
			int index = _Index.value;

			System.Type listElementType = ListUtility.GetElementType(list.GetType());
			if (listElementType != null)
			{
				elementType = listElementType;
			}

			return ValueMediator.Get(elementType).listMediator.RemoveAtIndex(list, index, outputType);
		}
	}
}