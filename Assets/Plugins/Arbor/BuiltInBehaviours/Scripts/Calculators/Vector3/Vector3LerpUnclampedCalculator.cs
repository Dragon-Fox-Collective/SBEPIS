//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromとToのベクトルの間を補間パラメータTで線形補間する。
	/// </summary>
#else
	/// <summary>
	/// Linear interpolation is performed between the vectors of From and To with the interpolation parameter T.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.LerpUnclamped")]
	[BehaviourTitle("Vector3.LerpUnclamped")]
	[BuiltInBehaviour]
	public sealed class Vector3LerpUnclampedCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始ベクトル
		/// </summary>
#else
		/// <summary>
		/// Starting vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _From = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了ベクトル
		/// </summary>
#else
		/// <summary>
		/// End vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _To = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 補間パラメータ
		/// </summary>
#else
		/// <summary>
		/// The interpolation parameter
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _T = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector3.LerpUnclamped(_From.value, _To.value, _T.value));
		}
	}
}
