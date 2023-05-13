//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// FlexibleFieldのインターフェイス
	/// </summary>
#else
	/// <summary>
	/// FlexibleField interface
	/// </summary>
#endif
	public interface IFlexibleField : IValueGetter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		void Disconnect();
	}
}