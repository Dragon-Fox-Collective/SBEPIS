//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// インデクサを使用してVector3の成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the components of Vector3 using an indexer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.GetIndexer")]
	[BehaviourTitle("Vector3.GetIndexer")]
	[BuiltInBehaviour]
	public sealed class Vector3GetIndexerCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

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
		[SerializeField] private OutputSlotFloat _Output = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Output.SetValue(_Vector3.value[_Index.value]);
		}
	}
}
