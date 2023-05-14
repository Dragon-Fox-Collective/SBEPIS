//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rendererの色を設定する。
	/// </summary>
	/// <remarks>設定にはMaterialPropertyBlockが使用される。</remarks>
#else
	/// <summary>
	/// Set the color of the Renderer.
	/// </summary>
	/// <remarks>MaterialPropertyBlock is used for setting.</remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/SetRendererColor")]
	[BuiltInBehaviour]
	public class SetRendererColor : StateBehaviour
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
		/// プロパティ名。
		/// </summary>
#else
		/// <summary>
		/// Property name.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _PropertyName = new FlexibleString("_Color");

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定する値。
		/// </summary>
#else
		/// <summary>
		/// The value to set.
		/// </summary>
#endif
		[SerializeField] 
		private FlexibleColor _Color = new FlexibleColor(Color.white);

		public override void OnStateBegin()
		{
			Renderer renderer = _Renderer.value as Renderer;
			if (renderer != null)
			{
				RendererPropertyBlock block = RendererPropertyBlock.Get(renderer, _MaterialIndex.value);

				block.Update();
				block.SetColor(_PropertyName.value, _Color.value);
				block.Apply();
			}
		}
	}
}