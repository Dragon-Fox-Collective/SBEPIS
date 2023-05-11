//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ビューポート座標をワールド座標に変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert viewport coordinates to world coordinates.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Camera/Camera.ViewportToWorldPoint")]
	[BehaviourTitle("Camera.ViewportToWorldPoint")]
	[BuiltInBehaviour]
	public sealed class CameraViewportToWorldPointCalculator : Calculator
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
		/// カメラアイ
		/// </summary>
#else
		/// <summary>
		/// Camera eye
		/// </summary>
#endif
		[SerializeField]
		private FlexibleMonoOrStereoscopicEye _Eye = new FlexibleMonoOrStereoscopicEye(Camera.MonoOrStereoscopicEye.Mono);

#if ARBOR_DOC_JA
		/// <summary>
		/// 変換結果のワールド座標を出力する。
		/// </summary>
#else
		/// <summary>
		/// Output the world coordinates of the conversion result.
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

			_Result.SetValue(camera.ViewportToWorldPoint(_Position.value, _Eye.value));
		}
	}
}