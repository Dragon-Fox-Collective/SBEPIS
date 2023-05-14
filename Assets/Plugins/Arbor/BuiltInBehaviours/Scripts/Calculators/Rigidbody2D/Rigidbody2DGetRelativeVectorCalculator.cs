//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// リジッドボディのローカル空間において指定した RelativeVector のグローバル空間ベクトルを取得する。
	/// </summary>
#else
	/// <summary>
	/// Get a global space vector given the vector RelativeVector in rigidBody local space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.GetRelativeVector")]
	[BehaviourTitle("Rigidbody2D.GetRelativeVector")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DGetRelativeVectorCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカル空間でのベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector in local space
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _RelativeVector = new FlexibleVector2();

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

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody2D rigidbody2D = _Rigidbody2D.value;
			if (rigidbody2D != null)
			{
				_Result.SetValue(rigidbody2D.GetRelativeVector(_RelativeVector.value));
			}
		}
	}
}
