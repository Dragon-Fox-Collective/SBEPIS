//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// SpriteRendererのスプライトを取得する。
	/// </summary>
#else
	/// <summary>
	/// Get the Sprite of SpriteRenderer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/GetSprite")]
	[BuiltInBehaviour]
	public sealed class GetSprite : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるRenderer。
		/// </summary>
#else
		/// <summary>
		/// Renderer of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(SpriteRenderer))]
		private FlexibleComponent _Renderer = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Sprite))]
		private OutputSlotAny _Output = new OutputSlotAny();

		// Use this for calculate
		public override void OnCalculate()
		{
			SpriteRenderer renderer = _Renderer.value as SpriteRenderer;
			if (renderer != null)
			{
				_Output.SetValue(renderer.sprite);
			}
		}
	}
}