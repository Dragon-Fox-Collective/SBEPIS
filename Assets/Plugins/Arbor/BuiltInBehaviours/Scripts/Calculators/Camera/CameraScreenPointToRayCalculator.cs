//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// スクリーン座標をレイに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert screen coordinates to rays.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Camera/Camera.ScreenPointToRay")]
	[BehaviourTitle("Camera.ScreenPointToRay")]
	[BuiltInBehaviour]
	public sealed class CameraScreenPointToRayCalculator : Calculator
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
		/// スクリーン座標
		/// </summary>
#else
		/// <summary>
		/// Screen coordinates
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

			_Result.SetValue(camera.ScreenPointToRay(_Pos.value, _Eye.value));
		}
	}
}