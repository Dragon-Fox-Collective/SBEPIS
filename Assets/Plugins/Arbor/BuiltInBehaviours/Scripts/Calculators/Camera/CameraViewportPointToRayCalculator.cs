//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ビューポート座標をレイに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert viewport coordinates to rays.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Camera/Camera.ViewportPointToRay")]
	[BehaviourTitle("Camera.ViewportPointToRay")]
	[BuiltInBehaviour]
	public sealed class CameraViewportPointToRayCalculator : Calculator
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
		private FlexibleVector3 _Pos = new FlexibleVector3();

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
		/// 変換結果のレイを出力する。
		/// </summary>
#else
		/// <summary>
		/// Output the conversion result ray.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotRay _Result = new OutputSlotRay();

		// Use this for calculate
		public override void OnCalculate()
		{
			Camera camera = _Camera.value as Camera;
			if (camera == null)
			{
				return;
			}

			_Result.SetValue(camera.ViewportPointToRay(_Pos.value, _Eye.value));
		}
	}
}