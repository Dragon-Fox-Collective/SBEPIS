//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace ArborEditor
{
	using Arbor;

	internal class SubGraphTreeViewItem : GraphTreeViewItem
	{
		public ISubGraphBehaviour subGraphReference
		{
			get;
			private set;
		}

		public override bool isExternal
		{
			get
			{
				return subGraphReference.isExternal;
			}
		}

		public SubGraphTreeViewItem(int id, ISubGraphBehaviour subGraphyReference) : base(id, subGraphyReference.GetSubGraph())
		{
			this.subGraphReference = subGraphyReference;
		}
	}
}