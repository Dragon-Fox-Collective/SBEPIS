//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntのX成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the X component of Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.SetX")]
	[BehaviourTitle("Vector3Int.SetX")]
	[BuiltInBehaviour]
	public sealed class Vector3IntSetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分
		/// </summary>
#else
		/// <summary>
		/// X component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _X = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Result = new OutputSlotVector3Int();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector3Int value = _Vector3Int.value;
			value.x = _X.value;
			_Result.SetValue(value);
		}
	}
}