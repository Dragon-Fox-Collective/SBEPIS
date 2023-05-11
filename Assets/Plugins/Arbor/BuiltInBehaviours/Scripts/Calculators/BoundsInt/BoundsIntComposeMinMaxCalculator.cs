//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxからBoundsIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create BoundsInt from Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.ComposeMinMax")]
	[BehaviourTitle("BoundsInt.ComposeMinMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntComposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _XMin = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMin = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _ZMin = new FlexibleInt();


#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _XMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _ZMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBoundsInt _Result = new OutputSlotBoundsInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt boundsInt = new BoundsInt();
			boundsInt.SetMinMax(new Vector3Int(_XMin.value, _YMin.value, _ZMin.value), new Vector3Int(_XMax.value, _YMax.value, _ZMax.value));
			_Result.SetValue(boundsInt);
		}
	}
}