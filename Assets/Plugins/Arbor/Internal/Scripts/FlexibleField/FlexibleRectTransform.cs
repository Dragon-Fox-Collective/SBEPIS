//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なRectTransform型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible RectTransform type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleRectTransform : FlexibleComponent<RectTransform>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleRectTransform default constructor
		/// </summary>
#endif
		public FlexibleRectTransform()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleRectTransform constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleRectTransform(RectTransform value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleRectTransform constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleRectTransform(RectTransformParameterReference parameter) : base((ComponentParameterReference)parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleRectTransform constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleRectTransform(InputSlotRectTransform slot) : base((InputSlotComponent)slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleRectTransform constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleRectTransform(FlexibleHierarchyType hierarchyType) : base(hierarchyType)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectTransformをRectTransformにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleRectTransform</param>
		/// <returns>RectTransformにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleRectTransform to RectTransform.
		/// </summary>
		/// <param name="flexible">FlexibleRectTransform</param>
		/// <returns>Returns the result of casting to RectTransform.</returns>
#endif
		public static explicit operator RectTransform(FlexibleRectTransform flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransformをFlexibleRectTransformにキャスト。
		/// </summary>
		/// <param name="value">RectTransform</param>
		/// <returns>FlexibleRectTransformにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast RectTransform to FlexibleRectTransform.
		/// </summary>
		/// <param name="value">RectTransform</param>
		/// <returns>Returns the result of casting to FlexibleRectTransform.</returns>
#endif
		public static explicit operator FlexibleRectTransform(RectTransform value)
		{
			return new FlexibleRectTransform(value);
		}
	}
}
