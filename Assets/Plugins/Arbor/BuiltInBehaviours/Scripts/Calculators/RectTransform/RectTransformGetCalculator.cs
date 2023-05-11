//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectにアタッチされているRectTransformを取得する。
	/// </summary>
#else
	/// <summary>
	/// Gets the RectTransform attached to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectTransform/RectTransform.Get")]
	[BehaviourTitle("RectTransform.Get")]
	[BuiltInBehaviour]
	public sealed class RectTransformGetCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// GameObject
		/// </summary>
		[SerializeField] private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 取得したRectTransform
		/// </summary>
#else
		/// <summary>
		/// Get the RectTransform
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectTransform _RectTransform = new OutputSlotRectTransform();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject != null)
			{
				_RectTransform.SetValue(gameObject.transform as RectTransform);
			}
		}
	}
}
