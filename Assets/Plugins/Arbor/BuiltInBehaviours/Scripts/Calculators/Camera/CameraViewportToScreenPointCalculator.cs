//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ビューポート座標をスクリーン座標に変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert viewport coordinates to screen coordinates.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Camera/Camera.ViewportToScreenPoint")]
	[BehaviourTitle("Camera.ViewportToScreenPoint")]
	[BuiltInBehaviour]
	public sealed class CameraViewportToScreenPointCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// カメラ
		/// </summary>
#else
		/// <summary>
		/// Camera
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Camera))]
		private FlexibleComponent _Camera = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ビューポート座標
		/// </summary>
#else
		/// <summary>
		/// Viewport coordinates
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Position = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 変換結果のスクリーン座標を出力する。
		/// </summary>
#else
		/// <summary>
		/// Output the screen coordinates of the conversion result.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Result = new OutputSlotVector3();

		// Use this for calculate
		public override void OnCalculate()
		{
			Camera camera = _Camera.value as Camera;
			if (camera == null)
			{
				return;
			}

			_Result.SetValue(camera.ViewportToScreenPoint(_Position.value));
		}
	}
}