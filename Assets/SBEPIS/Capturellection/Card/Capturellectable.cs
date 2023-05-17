using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SBEPIS.Capturellection
{
	public class Capturellectable : MonoBehaviour
	{
		public CaptureEvent onCapture;
		public CaptureEvent onFetch;

		private bool isBeingCaptured;
		public bool IsBeingCaptured
		{
			get => isBeingCaptured;
			set
			{
				isBeingCaptured = value;
				if (isBeingCaptured)
				{
					RenderPipelineManager.beginContextRendering += SwapToCaptureLayer;
					RenderPipelineManager.endContextRendering += SwapToOriginalLayer;
				}
				else
				{
					RenderPipelineManager.beginContextRendering -= SwapToCaptureLayer;
					RenderPipelineManager.endContextRendering -= SwapToOriginalLayer;
				}
			}
		}
		
		private int originalLayer;
		private int captureLayer;
		
		private void Awake()
		{
			captureLayer = LayerMask.NameToLayer("Capturing");
		}
		
		private void SwapToCaptureLayer(ScriptableRenderContext context, List<Camera> cameras)
		{
			originalLayer = gameObject.layer;
			gameObject.SetLayerRecursively(captureLayer);
		}
		
		private void SwapToOriginalLayer(ScriptableRenderContext context, List<Camera> cameras)
		{
			gameObject.SetLayerRecursively(originalLayer);
		}
		
		private void OnDestroy()
		{
			IsBeingCaptured = false;
		}
	}
}
