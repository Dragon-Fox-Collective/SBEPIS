//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if !(UNITY_ANDROID || UNITY_IOS || UNITY_WSA || UNITY_TVOS)
#define ARBOR_ENABLE_ONMOUSE_EVENT
#endif
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnMouseUpAdButtonが呼ばれたときにステートを遷移する。
	/// </summary>
	/// <remarks>
	/// デフォルトでは小型デバイス(Android, iOS, Windows Store Apps, Apple TV)で動作しない設定になっています。有効にするには"Scripting Define Symbols"にARBOR_ENABLE_ONMOUSE_EVENTを追加してください。
	/// </remarks>
#else
	/// <summary>
	/// It will transition the state when the OnMouseUpAdButton is called.
	/// </summary>
	/// <remarks>
	/// By default, it does not work on handheld devices(Android, iOS, Windows Store Apps, Apple TV).\nTo enable it, add ARBOR_ENABLE_ONMOUSE_EVENT to "Scripting Define Symbols".
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseUpAsButtonTransition")]
	[BuiltInBehaviour]
#if !ARBOR_ENABLE_ONMOUSE_EVENT
	[System.Obsolete("Does not work on handheld devices.\nTo enable it, add ARBOR_ENABLE_ONMOUSE_EVENT to \"Scripting Define Symbols\".")]
#endif
	public sealed class OnMouseUpAsButtonTransition : OnMouseBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnMouseUpAdButton
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnMouseUpAdButton
		/// </summary>
#endif
#pragma warning disable 0414
		[SerializeField] private StateLink _NextState = new StateLink();
#pragma warning restore 0414

		#endregion // Serialize fields

#if ARBOR_ENABLE_ONMOUSE_EVENT
		void OnMouseUpAdButton()
		{
			Transition(_NextState);
		}
#endif
	}
}
