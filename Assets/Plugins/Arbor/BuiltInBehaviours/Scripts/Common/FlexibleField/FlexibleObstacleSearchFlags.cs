//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なObstacleSearchFlags型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible ObstacleSearchFlags type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleObstacleSearchFlags : FlexibleField<ObstacleSearchFlags>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleSearchFlagsデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleObstacleSearchFlags default constructor
		/// </summary>
#endif
		public FlexibleObstacleSearchFlags()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleSearchFlagsコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleObstacleSearchFlags constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleObstacleSearchFlags(ObstacleSearchFlags value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleSearchFlagsコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleObstacleSearchFlags constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleObstacleSearchFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleSearchFlagsコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleObstacleSearchFlags constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleObstacleSearchFlags(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleSearchFlagsをObstacleSearchFlagsにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleObstacleSearchFlags</param>
		/// <returns>ObstacleSearchFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleObstacleSearchFlags to ObstacleSearchFlags.
		/// </summary>
		/// <param name="flexible">FlexibleObstacleSearchFlags</param>
		/// <returns>Returns the result of casting to ObstacleSearchFlags.</returns>
#endif
		public static explicit operator ObstacleSearchFlags(FlexibleObstacleSearchFlags flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ObstacleSearchFlagsをFlexibleObstacleSearchFlagsにキャスト。
		/// </summary>
		/// <param name="value">ObstacleSearchFlags</param>
		/// <returns>FlexibleObstacleSearchFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast ObstacleSearchFlags to FlexibleObstacleSearchFlags.
		/// </summary>
		/// <param name="value">ObstacleSearchFlags</param>
		/// <returns>Returns the result of casting to FlexibleObstacleSearchFlags.</returns>
#endif
		public static explicit operator FlexibleObstacleSearchFlags(ObstacleSearchFlags value)
		{
			return new FlexibleObstacleSearchFlags(value);
		}
	}
}