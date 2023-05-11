//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// インデクサを使用してColorの成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the components of Color using an indexer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.SetIndexer")]
	[BehaviourTitle("Color.SetIndexer")]
	[BuiltInBehaviour]
	public sealed class ColorSetIndexerCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// インデックス
		/// </summary>
#else
		/// <summary>
		/// Index
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _Index = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値
		/// </summary>
#else
		/// <summary>
		/// Value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Value = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Result = new OutputSlotColor();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Color value = _Color.value;
			value[_Index.value] = _Value.value;
			_Result.SetValue(value);
		}
	}
}
