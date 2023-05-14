//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// インデクサを使用してQuaternionの成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the components of Quaternion using an indexer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.GetIndexer")]
	[BehaviourTitle("Quaternion.GetIndexer")]
	[BuiltInBehaviour]
	public sealed class QuaternionGetIndexerCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

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
			_Output.SetValue(_Quaternion.value[_Index.value]);
		}
	}
}
