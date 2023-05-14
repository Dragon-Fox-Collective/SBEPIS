//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なプリミティブ型を扱うクラス。継承して使用する。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible primitive type reference method there is more than one. Inherit and use it.
	/// </summary>
#endif
	[System.Serializable]
	public abstract class FlexiblePrimitiveBase : IFlexibleField
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// プリミティブ型の参照タイプ
		/// </summary>
#else
		/// <summary>
		/// Reference type of primitive type
		/// </summary>
#endif
		[SerializeField]
		protected FlexiblePrimitiveType _Type = FlexiblePrimitiveType.Constant;

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeを返す
		/// </summary>
#else
		/// <summary>
		/// It returns a type
		/// </summary>
#endif
		public FlexiblePrimitiveType type
		{
			get
			{
				return _Type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public abstract object GetValueObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public abstract void Disconnect();
	}
}