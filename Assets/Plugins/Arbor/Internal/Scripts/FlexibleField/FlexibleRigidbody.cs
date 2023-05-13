//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なRigidbody型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Rigidbody type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleRigidbody : FlexibleComponent<Rigidbody>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleRigidbody default constructor
		/// </summary>
#endif
		public FlexibleRigidbody()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleRigidbody constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleRigidbody(Rigidbody value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleRigidbody constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleRigidbody(RigidbodyParameterReference parameter) : base((ComponentParameterReference)parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleRigidbody constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleRigidbody(InputSlotRigidbody slot) : base((InputSlotComponent)slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleRigidbody constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleRigidbody(FlexibleHierarchyType hierarchyType) : base(hierarchyType)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRigidbodyをRigidbodyにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleRigidbody</param>
		/// <returns>Rigidbodyにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleRigidbody to Rigidbody.
		/// </summary>
		/// <param name="flexible">FlexibleRigidbody</param>
		/// <returns>Returns the result of casting to Rigidbody.</returns>
#endif
		public static explicit operator Rigidbody(FlexibleRigidbody flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RigidbodyをFlexibleRigidbodyにキャスト。
		/// </summary>
		/// <param name="value">Rigidbody</param>
		/// <returns>FlexibleRigidbodyにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Rigidbody to FlexibleRigidbody.
		/// </summary>
		/// <param name="value">Rigidbody</param>
		/// <returns>Returns the result of casting to FlexibleRigidbody.</returns>
#endif
		public static explicit operator FlexibleRigidbody(Rigidbody value)
		{
			return new FlexibleRigidbody(value);
		}
	}
}
