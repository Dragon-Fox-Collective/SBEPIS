//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したセカンダリ UV テクスチャの座標
	/// </summary>
#else
	/// <summary>
	/// The secondary uv texture coordinate at the impact point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.TextureCoord2")]
	[BehaviourTitle("RaycastHit.TextureCoord2")]
	[BuiltInBehaviour]
	public sealed class RaycastHitTextureCoord2Calculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったセカンダリUV座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit secondary UV coordinates
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _TextureCoord2 = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_TextureCoord2.SetValue(raycastHit.textureCoord2);
			}
		}
	}
}
