//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Tweenの移動タイプ
	/// </summary>
#else
	/// <summary>
	/// Tween movement type
	/// </summary>
#endif
	[Arbor.Internal.Documentable]
	public enum TweenMoveType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 絶対値。
		/// </summary>
#else
		/// <summary>
		/// Absolute value.
		/// </summary>
#endif
		Absolute,

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時点の値からの相対値。
		/// </summary>
#else
		/// <summary>
		/// Relative value from starting value.
		/// </summary>
#endif
		Relative,

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時点の値から絶対値へ。(Fromフィールドは使わない)
		/// </summary>
#else
		/// <summary>
		/// From the value at the start to the absolute value. (From field is not used)
		/// </summary>
#endif
		ToAbsolute,
	}
}