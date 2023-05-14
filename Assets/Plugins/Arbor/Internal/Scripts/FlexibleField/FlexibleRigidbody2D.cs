//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なRigidbody2D型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Rigidbody2D type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleRigidbody2D : FlexibleComponent<Rigidbody2D>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2Dデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleRigidbody2D default constructor
		/// </summary>
#endif
		public FlexibleRigidbody2D()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2Dコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleRigidbody2D constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleRigidbody2D(Rigidbody2D value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2Dコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleRigidbody2D constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleRigidbody2D(Rigidbody2DParameterReference parameter) : base((ComponentParameterReference)parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2Dコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleRigidbody2D constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleRigidbody2D(InputSlotRigidbody2D slot) : base((InputSlotComponent)slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2Dコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleRigidbody2D constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleRigidbody2D(FlexibleHierarchyType hierarchyType) : base(hierarchyType)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbody2DをRigidbody2Dにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleRigidbody2D</param>
		/// <returns>Rigidbody2Dにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleRigidbody2D to Rigidbody2D.
		/// </summary>
		/// <param name="flexible">FlexibleRigidbody2D</param>
		/// <returns>Returns the result of casting to Rigidbody2D.</returns>
#endif
		public static explicit operator Rigidbody2D(FlexibleRigidbody2D flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2DをFlexibleRigidbody2Dにキャスト。
		/// </summary>
		/// <param name="value">Rigidbody2D</param>
		/// <returns>FlexibleRigidbody2Dにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Rigidbody2D to FlexibleRigidbody2D.
		/// </summary>
		/// <param name="value">Rigidbody2D</param>
		/// <returns>Returns the result of casting to FlexibleRigidbody2D.</returns>
#endif
		public static explicit operator FlexibleRigidbody2D(Rigidbody2D value)
		{
			return new FlexibleRigidbody2D(value);
		}
	}
}
