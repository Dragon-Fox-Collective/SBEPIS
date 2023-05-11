//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromDirectionからToDirectionへのQuaternionを作成する。
	/// </summary>
#else
	/// <summary>
	/// Creates a rotation which rotates from FromDirection to ToDirection.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.FromToRotation")]
	[BehaviourTitle("Quaternion.FromToRotation")]
	[BuiltInBehaviour]
	public sealed class QuaternionFromToRotationCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始方向
		/// </summary>
#else
		/// <summary>
		/// Starting direction
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _FromDirection = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了方向
		/// </summary>
#else
		/// <summary>
		/// End direction
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _ToDirection = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _Result = new OutputSlotQuaternion();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Quaternion.FromToRotation(_FromDirection.value, _ToDirection.value));
		}
	}
}
