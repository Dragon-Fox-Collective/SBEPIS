//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4を作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Compose")]
	[BehaviourTitle("Vector4.Compose")]
	[BuiltInBehaviour]
	public sealed class Vector4ComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// X座標の値
		/// </summary>
#else
		/// <summary>
		/// X coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _X = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y座標の値
		/// </summary>
#else
		/// <summary>
		/// Y coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z座標の値
		/// </summary>
#else
		/// <summary>
		/// Z coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Z = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// W座標の値
		/// </summary>
#else
		/// <summary>
		/// W coordinate value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _W = new FlexibleFloat();

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

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(new Vector4(_X.value, _Y.value, _Z.value, _W.value));
		}
	}
}
