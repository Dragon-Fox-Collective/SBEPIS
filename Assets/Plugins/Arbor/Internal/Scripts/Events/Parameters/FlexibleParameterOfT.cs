//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Events.Legacy
{
	[System.Serializable]
	internal class FlexibleParameter<TFlexible> where TFlexible : IFlexibleField, new()
	{
		[SerializeField]
		private TFlexible _Value = new TFlexible();

		public TFlexible GetFlexibleField()
		{
			return _Value;
		}
	}
}