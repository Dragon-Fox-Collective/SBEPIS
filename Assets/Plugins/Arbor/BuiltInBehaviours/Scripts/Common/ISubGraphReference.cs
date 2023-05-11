//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public interface ISubGraphReference
	{
		bool IsRuntimeGraphInScene();
		void ReleaseRuntimeGraphInScene();
	}
}