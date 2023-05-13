//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なTransform型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Transform type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleTransform : FlexibleComponent<Transform>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleTransform default constructor
		/// </summary>
#endif
		public FlexibleTransform()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleTransform constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleTransform(Transform value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleTransform constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleTransform(TransformParameterReference parameter) : base((ComponentParameterReference)parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleTransform constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleTransform(InputSlotTransform slot) : base((InputSlotComponent)slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleTransform constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleTransform(FlexibleHierarchyType hierarchyType) : base(hierarchyType)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransformをTransformにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleTransform</param>
		/// <returns>Transformにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleTransform to Transform.
		/// </summary>
		/// <param name="flexible">FlexibleTransform</param>
		/// <returns>Returns the result of casting to Transform.</returns>
#endif
		public static explicit operator Transform(FlexibleTransform flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TransformをFlexibleTransformにキャスト。
		/// </summary>
		/// <param name="value">Transform</param>
		/// <returns>FlexibleTransformにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Transform to FlexibleTransform.
		/// </summary>
		/// <param name="value">Transform</param>
		/// <returns>Returns the result of casting to FlexibleTransform.</returns>
#endif
		public static explicit operator FlexibleTransform(Transform value)
		{
			return new FlexibleTransform(value);
		}
	}
}
