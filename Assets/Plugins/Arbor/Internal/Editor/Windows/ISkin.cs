//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace ArborEditor
{
	[System.Obsolete]
	public interface ISkin
	{
		bool isDarkSkin
		{
			get;
		}

		void Begin();
		void End();
	}
}