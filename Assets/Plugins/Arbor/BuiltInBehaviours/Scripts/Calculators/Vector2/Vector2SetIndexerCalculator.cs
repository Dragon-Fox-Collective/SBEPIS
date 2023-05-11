//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// インデクサを使用してVector2の成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the components of Vector2 using an indexer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.SetIndexer")]
	[BehaviourTitle("Vector2.SetIndexer")]
	[BuiltInBehaviour]
	public sealed class Vector2SetIndexerCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2
		/// </summary>
		[SerializeField] private FlexibleVector2 _Vector2 = new FlexibleVector2();

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
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector2 vector2 = _Vector2.value;
			vector2[_Index.value] = _Value.value;
			_Result.SetValue(vector2);
		}
	}
}
