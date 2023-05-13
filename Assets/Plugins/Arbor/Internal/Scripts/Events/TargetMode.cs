//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ターゲットのインスタンスを指定する
	/// </summary>
#else
	/// <summary>
	/// Specify target instance
	/// </summary>
#endif
	public enum TargetMode
	{
		/// <summary>
		/// Component
		/// </summary>
		Component,

		/// <summary>
		/// GameObject
		/// </summary>
		GameObject,

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentとGameObject以外のObject
		/// </summary>
#else
		/// <summary>
		/// Object other than Component and GameObject
		/// </summary>
#endif
		AssetObject,

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロット
		/// </summary>
#else
		/// <summary>
		/// Input slot
		/// </summary>
#endif
		Slot,

#if ARBOR_DOC_JA
		/// <summary>
		/// スタティッククラス
		/// </summary>
#else
		/// <summary>
		/// Static class
		/// </summary>
#endif
		Static,
	}
}