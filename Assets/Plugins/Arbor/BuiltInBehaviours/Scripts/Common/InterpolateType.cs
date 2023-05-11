//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 補間タイプ
	/// </summary>
#else
	/// <summary>
	/// Lerp type
	/// </summary>
#endif
	[Arbor.Internal.Documentable]
	public enum InterpolateType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 線形補間
		/// </summary>
#else
		/// <summary>
		/// Linearly interpolates
		/// </summary>
#endif
		Lerp,

#if ARBOR_DOC_JA
		/// <summary>
		/// 球面線形補間
		/// </summary>
#else
		/// <summary>
		/// Spherically interpolates
		/// </summary>
#endif
		Slerp,
	}
}