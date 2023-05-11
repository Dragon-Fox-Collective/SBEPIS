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
	/// Rendererのマテリアルを取得する
	/// </summary>
#else
	/// <summary>
	/// Get Renderer material
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/GetRendererMaterial")]
	[BuiltInBehaviour]
	public sealed class GetRendererMaterial : Calculator
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
		[SlotType(typeof(Renderer))]
		private FlexibleComponent _Renderer = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のマテリアルのインデックス<br/>
		/// インデックスがどのマテリアルであるかは各種RendererのMaterials(例えば<a href="https://docs.unity3d.com/ja/current/Manual/class-MeshRenderer.html#materials">MeshRendererのMaterials</a>)を参照してください。
		/// </summary>
#else
		/// <summary>
		/// Index of target material<br/>
		/// See the various Renderer Materials (eg <a href="https://docs.unity3d.com/Manual/class-MeshRenderer.html#materials">Mesh Renderer Materials</a>) to see which material the index is.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _MaterialIndex = new FlexibleInt();

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
		[SlotType(typeof(Material))]
		private OutputSlotAny _Output = new OutputSlotAny();

		// Use this for calculate
		public override void OnCalculate()
		{
			Renderer renderer = _Renderer.value as Renderer;
			if (renderer != null)
			{
				using (Arbor.Pool.ListPool<Material>.Get(out var materials))
				{
					renderer.GetSharedMaterials(materials);
					var material = materials[_MaterialIndex.value];
					_Output.SetValue(material);
				}
			}
		}
	}
}