//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collision2DからヒットしたTransformを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Transform hit from Collision2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/Collision2D.Transform")]
	[BehaviourTitle("Collision2D.Transform")]
	[BuiltInBehaviour]
	public sealed class Collision2DTransformCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision2D
		/// </summary>
		[SerializeField] private InputSlotCollision2D _Collision2D = new InputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotTransform _Transform = new OutputSlotTransform();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision2D collision2D = null;
			_Collision2D.GetValue(ref collision2D);
			if (collision2D != null)
			{
				_Transform.SetValue(collision2D.transform);
			}
		}
	}
}
