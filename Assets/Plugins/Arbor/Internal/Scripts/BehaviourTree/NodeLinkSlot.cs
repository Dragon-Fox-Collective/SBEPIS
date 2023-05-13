//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Nodeとリンクするためのスロットクラス。
	/// </summary>
#else
	/// <summary>
	/// Slot class for linking with Node.
	/// </summary>
#endif
	[System.Serializable]
	public abstract class NodeLinkSlot
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 接続が変更されたときのコールバックイベント
		/// </summary>
#else
		/// <summary>
		/// Callback event when connection is changed
		/// </summary>
#endif
		public event System.Action onConnectionChanged;

		private protected void ConnectionChanged()
		{
			onConnectionChanged?.Invoke();
		}
	}
}