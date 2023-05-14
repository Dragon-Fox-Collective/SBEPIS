//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rayを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Ray.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Ray/Ray.Compose")]
	[BehaviourTitle("Ray.Compose")]
	[BuiltInBehaviour]
	public sealed class RayComposeCalculator : Calculator 
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 原点
		/// </summary>
#else
		/// <summary>
		/// Origin
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Origin = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向
		/// </summary>
#else
		/// <summary>
		/// Direction
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Direction = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotRay _Result = new OutputSlotRay();

		// Use this for calculate
		public override void OnCalculate() 
		{
			_Result.SetValue(new Ray(_Origin.value, _Direction.value));
		}
	}
}