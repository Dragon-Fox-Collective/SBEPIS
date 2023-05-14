//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なSpace型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Space type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleSpace : FlexibleField<Space>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSpaceデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleSpace default constructor
		/// </summary>
#endif
		public FlexibleSpace()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSpaceコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleSpace constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleSpace(Space value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSpaceコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleSpace constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleSpace(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSpaceコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleSpace constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleSpace(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSpaceをSpaceにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleSpace</param>
		/// <returns>Spaceにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleSpace to Space.
		/// </summary>
		/// <param name="flexible">FlexibleSpace</param>
		/// <returns>Returns the result of casting to Space.</returns>
#endif
		public static explicit operator Space(FlexibleSpace flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// SpaceをFlexibleSpaceにキャスト。
		/// </summary>
		/// <param name="value">Space</param>
		/// <returns>FlexibleSpaceにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Space to FlexibleSpace.
		/// </summary>
		/// <param name="value">Space</param>
		/// <returns>Returns the result of casting to FlexibleSpace.</returns>
#endif
		public static explicit operator FlexibleSpace(Space value)
		{
			return new FlexibleSpace(value);
		}
	}
}