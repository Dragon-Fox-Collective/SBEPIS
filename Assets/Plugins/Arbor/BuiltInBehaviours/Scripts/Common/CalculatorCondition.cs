//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Calculatorの状態チェッククラス
	/// </summary>
	/// <param name="Value1">比較する値</param>
	/// <param name="Value2">比較する値</param>
#else
	/// <summary>
	/// Condition check class of Calculator
	/// </summary>
	/// <param name="Value1">Value to compare</param>
	/// <param name="Value2">Value to compare</param>
#endif
	[System.Serializable]
	[Arbor.Internal.Documentable]
	public sealed class CalculatorCondition
	{
		#region enum

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の型
		/// </summary>
#else
		/// <summary>
		/// Value type
		/// </summary>
#endif
		[Arbor.Internal.Documentable]
		public enum Type
		{
			/// <summary>
			/// FlexibleInt
			/// </summary>
			Int,

			/// <summary>
			/// FlexileFloat
			/// </summary>
			Float,

			/// <summary>
			/// FlexibleBool
			/// </summary>
			Bool,
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の型
		/// </summary>
#else
		/// <summary>
		/// Value type
		/// </summary>
#endif
		[SerializeField]
		[Internal.HideInDocument]
		internal Type _Type = Type.Int;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンディションの論理演算
		/// </summary>
#else
		/// <summary>
		/// Logical operation of condition
		/// </summary>
#endif
		[SerializeField]
		internal LogicalCondition _LogicalCondition = new LogicalCondition();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ
		/// </summary>
#else
		/// <summary>
		/// Compare type
		/// </summary>
#endif
		[SerializeField]
		internal CompareType _CompareType = CompareType.Equals;

		[SerializeField]
		[HideInInspector]
		internal int _ParameterIndex = 0;

		#endregion // Serialize fields

		public ConditionResult conditionResult
		{
			get;
			private set;
		}

		[System.NonSerialized]
		private CalculatorConditionList _Owner = null;

		public CalculatorConditionList owner
		{
			get
			{
				return _Owner;
			}
			internal set
			{
				_Owner = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を比較する.
		/// </summary>
		/// <returns>比較結果</returns>
#else
		/// <summary>
		/// Compare the values.
		/// </summary>
		/// <returns>Comparison result</returns>
#endif
		public bool Compare()
		{
			bool result = false;
			switch (_Type)
			{
				case Type.Int:
					CalculatorConditionList.IntParameter intParameter = owner._IntParameters[_ParameterIndex];
					result = intParameter.Compare(_CompareType);
					break;
				case Type.Float:
					CalculatorConditionList.FloatParameter floatParameter = owner._FloatParameters[_ParameterIndex];
					result = floatParameter.Compare(_CompareType);
					break;
				case Type.Bool:
					CalculatorConditionList.BoolParameter boolParameter = owner._BoolParameters[_ParameterIndex];
					result = boolParameter.Compare();
					break;
			}

			if (_LogicalCondition.notOp)
			{
				result = !result;
			}

			conditionResult = result ? ConditionResult.Success : ConditionResult.Failure;

			return result;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorConditionコンストラクタ
		/// </summary>
		/// <param name="type">値の型</param>
#else
		/// <summary>
		/// CalculatorCondition constructor
		/// </summary>
		/// <param name="type">Value type</param>
#endif
		public CalculatorCondition(Type type)
		{
			_Type = type;
		}

		internal void ClearConditionResult()
		{
			conditionResult = ConditionResult.None;
		}
	}
}