//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突した UV テクスチャの座標
	/// </summary>
#else
	/// <summary>
	/// The uv texture coordinate at the collision location.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.TextureCoord")]
	[BehaviourTitle("RaycastHit.TextureCoord")]
	[BuiltInBehaviour]
	public sealed class RaycastHitTextureCoordCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったUV座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit UV coordinate
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _TextureCoord = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_TextureCoord.SetValue(raycastHit.textureCoord);
			}
		}
	}
}
