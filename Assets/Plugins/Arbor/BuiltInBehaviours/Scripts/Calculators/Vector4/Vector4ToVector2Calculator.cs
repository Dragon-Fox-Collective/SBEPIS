//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4をVector2に変換する。
	/// </summary>
#else
	/// <summary>
	/// Vector4 is converted to Vector2.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.ToVector2")]
	[BehaviourTitle("Vector4.ToVector2")]
	[BuiltInBehaviour]
	public sealed class Vector4ToVector2Calculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

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
			_Result.SetValue((Vector2)_Vector4.value);
		}
	}
}
