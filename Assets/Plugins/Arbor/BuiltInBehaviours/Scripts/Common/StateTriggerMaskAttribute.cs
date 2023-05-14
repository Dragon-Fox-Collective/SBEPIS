//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
	public sealed class StateTriggerMaskAttribute : System.Attribute
	{
		public readonly StateTriggerFlags mask;

		public StateTriggerMaskAttribute(StateTriggerFlags mask)
		{
			this.mask = mask;
		}
	}
}