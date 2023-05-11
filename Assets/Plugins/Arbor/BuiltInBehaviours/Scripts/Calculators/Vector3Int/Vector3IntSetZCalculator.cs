//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntのZ成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Z component of Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.SetZ")]
	[BehaviourTitle("Vector3Int.SetZ")]
	[BuiltInBehaviour]
	public sealed class Vector3IntSetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分
		/// </summary>
#else
		/// <summary>
		/// Z component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _Z = new FlexibleInt();

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
			value.z = _Z.value;
			_Result.SetValue(value);
		}
	}
}