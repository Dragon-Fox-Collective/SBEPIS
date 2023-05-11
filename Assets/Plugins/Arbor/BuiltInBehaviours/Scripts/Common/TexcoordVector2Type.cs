//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2のテクスチャ座標タイプ
	/// </summary>
#else
	/// <summary>
	/// Texcoord Vector2 Type.
	/// </summary>
#endif
	[Arbor.Internal.Documentable]
	public enum TexcoordVector2Type
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4のxyを使用
		/// </summary>
#else
		/// <summary>
		/// Use xy of Vector4
		/// </summary>
#endif
		XY,

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4のzwを使用
		/// </summary>
#else
		/// <summary>
		/// Use zw of Vector4
		/// </summary>
#endif
		ZW,
	}
}