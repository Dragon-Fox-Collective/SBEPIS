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
	[AddBehaviourMenu("Vector4/Vector4.Lerp")]
	[BehaviourTitle("Vector4.Lerp")]
	[BuiltInBehaviour]
	public sealed class Vector4LerpCalculator : Calculator
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
		[SerializeField] private FlexibleVector4 _From = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了ベクトル
		/// </summary>
#else
		/// <summary>
		/// End vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _To = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 補間パラメータ<br />
		/// [0, 1]の間にクランプされる。
		/// </summary>
#else
		/// <summary>
		/// The interpolation parameter<br />
		/// Clamped between [0, 1].
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
		[SerializeField] private OutputSlotVector4 _Result = new OutputSlotVector4();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector4.Lerp(_From.value, _To.value, _T.value));
		}
	}
}
