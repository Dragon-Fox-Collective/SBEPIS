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
	/// Rendererのfloat値を取得する
	/// </summary>
	/// <remarks>取得にはMaterialPropertyBlockが使用される。</remarks>
#else
	/// <summary>
	/// Get the Renderer float value
	/// </summary>
	/// <remarks>MaterialPropertyBlock is used for get.</remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/GetRendererFloat")]
	[BuiltInBehaviour]
	public sealed class GetRendererFloat : Calculator
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
		private FlexibleString _PropertyName = new FlexibleString("");

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
		private OutputSlotFloat _Output = new OutputSlotFloat();

		// Use this for calculate
		public override void OnCalculate()
		{
			Renderer renderer = _Renderer.value as Renderer;
			if (renderer != null)
			{
				int propertyID = Shader.PropertyToID(_PropertyName.value);

				RendererPropertyBlock block = RendererPropertyBlock.Get(renderer, _MaterialIndex.value);

				block.Update();

				float value = block.GetFloat(propertyID);

				_Output.SetValue(value);
			}
		}
	}
}