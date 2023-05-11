//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collision2DからヒットしたGameObjectを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output GameObject hit from Collision2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/Collision2D.GameObject")]
	[BehaviourTitle("Collision2D.GameObject")]
	[BuiltInBehaviour]
	public sealed class Collision2DGameObjectCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collision2D
		/// </summary>
		[SerializeField] private InputSlotCollision2D _Collision2D = new InputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// ヒットしたGameObject。
		/// </summary>
#else
		/// <summary>
		/// GameObject that hit.
		/// </summary>
#endif
		[SerializeField] private OutputSlotGameObject _GameObject = new OutputSlotGameObject();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Collision2D collision2D = null;
			_Collision2D.GetValue(ref collision2D);
			if (collision2D != null)
			{
				_GameObject.SetValue(collision2D.gameObject);
			}
		}
	}
}
