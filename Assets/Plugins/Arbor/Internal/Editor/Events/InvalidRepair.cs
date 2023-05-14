//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace ArborEditor.Events
{
	internal class InvalidRepair
	{
		public virtual string GetMessage()
		{
			return null;
		}

		public virtual void OnRepair()
		{
		}
	}
}