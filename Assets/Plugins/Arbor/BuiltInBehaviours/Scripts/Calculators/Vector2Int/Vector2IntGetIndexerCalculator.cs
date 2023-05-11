//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// インデクサを使用してVector2Intの成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the components of Vector2Int using an indexer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.GetIndexer")]
	[BehaviourTitle("Vector2Int.GetIndexer")]
	[BuiltInBehaviour]
	public sealed class Vector2IntGetIndexerCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2Int
		/// </summary>
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

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
		/// 成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Output = new OutputSlotInt();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Output.SetValue(_Vector2Int.value[_Index.value]);
		}
	}
}