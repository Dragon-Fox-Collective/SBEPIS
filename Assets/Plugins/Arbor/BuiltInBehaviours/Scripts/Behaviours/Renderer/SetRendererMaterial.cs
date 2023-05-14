//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rendererのマテリアルを設定する。
	/// </summary>
	/// <remarks>マテリアルはRenderer.sharedMaterialsに設定される。</remarks>
#else
	/// <summary>
	/// Set the Renderer material.
	/// </summary>
	/// <remarks>Materials are set in Renderer.sharedMaterials.</remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/SetRendererMaterial")]
	[BuiltInBehaviour]
	public class SetRendererMaterial : StateBehaviour
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
		FlexibleComponent _Renderer = new FlexibleComponent(FlexibleHierarchyType.Self);

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
		/// 設定するマテリアル。
		/// </summary>
#else
		/// <summary>
		/// Material to set.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Material))]
		FlexibleAssetObject _Material = new FlexibleAssetObject();

		public override void OnStateBegin()
		{
			Renderer renderer = _Renderer.value as Renderer;
			if (renderer != null)
			{
				var materials = renderer.sharedMaterials;
				materials[_MaterialIndex.value] = _Material.value as Material;
				renderer.sharedMaterials = materials;
			}
		}
	}
}