//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 方向タイプ
	/// </summary>
#else
	/// <summary>
	/// Direction type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum DirectionType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector
		/// </summary>
#endif
		Vector,

#if ARBOR_DOC_JA
		/// <summary>
		/// オイラー角
		/// </summary>
#else
		/// <summary>
		/// Euler angle
		/// </summary>
#endif
		EulerAngle,
	}
}