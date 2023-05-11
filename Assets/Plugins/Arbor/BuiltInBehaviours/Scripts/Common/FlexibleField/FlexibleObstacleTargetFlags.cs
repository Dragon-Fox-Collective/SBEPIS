//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なObstacleTargetFlags型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible ObstacleTargetFlags type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleObstacleTargetFlags : FlexibleField<ObstacleTargetFlags>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleTargetFlagsデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleObstacleTargetFlags default constructor
		/// </summary>
#endif
		public FlexibleObstacleTargetFlags()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleTargetFlagsコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleObstacleTargetFlags constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleObstacleTargetFlags(ObstacleTargetFlags value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleTargetFlagsコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleObstacleTargetFlags constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleObstacleTargetFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleTargetFlagsコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleObstacleTargetFlags constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleObstacleTargetFlags(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleObstacleTargetFlagsをObstacleTargetFlagsにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleObstacleTargetFlags</param>
		/// <returns>ObstacleTargetFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleObstacleTargetFlags to ObstacleTargetFlags.
		/// </summary>
		/// <param name="flexible">FlexibleObstacleTargetFlags</param>
		/// <returns>Returns the result of casting to ObstacleTargetFlags.</returns>
#endif
		public static explicit operator ObstacleTargetFlags(FlexibleObstacleTargetFlags flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ObstacleTargetFlagsをFlexibleObstacleTargetFlagsにキャスト。
		/// </summary>
		/// <param name="value">ObstacleTargetFlags</param>
		/// <returns>FlexibleObstacleTargetFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast ObstacleTargetFlags to FlexibleObstacleTargetFlags.
		/// </summary>
		/// <param name="value">ObstacleTargetFlags</param>
		/// <returns>Returns the result of casting to FlexibleObstacleTargetFlags.</returns>
#endif
		public static explicit operator FlexibleObstacleTargetFlags(ObstacleTargetFlags value)
		{
			return new FlexibleObstacleTargetFlags(value);
		}
	}
}