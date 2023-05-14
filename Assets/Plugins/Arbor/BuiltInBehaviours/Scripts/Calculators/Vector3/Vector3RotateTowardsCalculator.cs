//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 現在の位置Current からTargetに向けてベクトルを回転する。
	/// </summary>
#else
	/// <summary>
	/// Rotates a vector Current towards Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.RotateTowards")]
	[BehaviourTitle("Vector3.RotateTowards")]
	[BuiltInBehaviour]
	public sealed class Vector3RotateTowardsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在ベクトル
		/// </summary>
#else
		/// <summary>
		/// Current vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Current = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標ベクトル
		/// </summary>
#else
		/// <summary>
		/// Target vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Target = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大のラジアン変化量
		/// </summary>
#else
		/// <summary>
		/// The maximum radian of the amount of change
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxRadiansDelta = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大の長さ変化量
		/// </summary>
#else
		/// <summary>
		/// The maximum length of the amount of change
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxMagnitudeDelta = new FlexibleFloat();

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
			_Result.SetValue(Vector3.RotateTowards(_Current.value, _Target.value, _MaxRadiansDelta.value, _MaxMagnitudeDelta.value));
		}
	}
}
