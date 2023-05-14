//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// UnityEventを使用してオブジェクトのメンバーを呼び出す。
	/// </summary>
	/// <remarks>詳細は<a href="https://docs.unity3d.com/ja/current/Manual/UnityEvents.html">UnityEvent</a>を参照してください。<br/>制限のないメンバー呼び出しを行いたい場合は<a href="https://arbor-docs.caitsithware.com/ja/inspector/behaviours/Events/InvokeMethod.html">InvokeMethod</a>の利用を推奨します。</remarks>
#else
	/// <summary>
	/// Invoke a member of the object using UnityEvent.
	/// </summary>
	/// <remarks>See <a href="https://docs.unity3d.com/Manual/UnityEvents.html">UnityEvents</a> for details.<br/>It is recommended to use <a href="https://arbor-docs.caitsithware.com/en/inspector/behaviours/Events/InvokeMethod.html">InvokeMethod</a> for unlimited member calls.</remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SendEventGameObject")]
	[BuiltInBehaviour]
	public sealed class SendEventGameObject : StateBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateAwakeによるイベント送信
		/// </summary>
#else
		/// <summary>
		/// Send event by OnStateAwake
		/// </summary>
#endif
		[SerializeField]
		private UnityEvent _OnStateAwake = new UnityEvent();

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateBeginによるイベント送信
		/// </summary>
#else
		/// <summary>
		/// Send event by OnStateBegin
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("_Event")]
		private UnityEvent _OnStateBegin = new UnityEvent();

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateEndによるイベント送信
		/// </summary>
#else
		/// <summary>
		/// Send event by OnStateEnd
		/// </summary>
#endif
		[SerializeField]
		private UnityEvent _OnStateEnd = new UnityEvent();

		#endregion // Serialize fields

		// Use this for awake state
		public override void OnStateAwake()
		{
			if (_OnStateAwake != null)
			{
				_OnStateAwake.Invoke();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_OnStateBegin != null)
			{
				_OnStateBegin.Invoke();
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_OnStateEnd != null)
			{
				_OnStateEnd.Invoke();
			}
		}
	}
}
