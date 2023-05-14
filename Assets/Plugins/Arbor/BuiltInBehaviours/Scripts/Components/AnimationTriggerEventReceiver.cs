//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Events;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AnimationEventから呼ばれたイベントを別のメソッドに通達するためのコンポーネント
	/// </summary>
#else
	/// <summary>
	/// A component for notifying another method of an event called from AnimationEvent
	/// </summary>
#endif
	[AddComponentMenu("Arbor/AnimationTriggerEventReceiver")]
	[HelpURL(ArborReferenceUtility.componentUrl + "Arbor/animationtriggereventreceiver.html")]
	[BuiltInComponent]
	[Internal.DocumentManual("/manual/builtin/animationtriggereventreceiver.md")]
	public sealed class AnimationTriggerEventReceiver : MonoBehaviour
	{
		[System.Serializable]
		internal class StringEvent : UnityEvent<string>
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimationEventが呼び出されたときに通達するイベント先
		/// </summary>
#else
		/// <summary>
		/// Event destination notified when AnimationEvent is called
		/// </summary>
#endif
		[SerializeField]
		private StringEvent _OnTrigger = new StringEvent();

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーを受け取るアクションを追加する。
		/// </summary>
		/// <param name="onTrigger">呼び出されるアクション</param>
#else
		/// <summary>
		/// Add an action that receives a trigger.
		/// </summary>
		/// <param name="onTrigger">Action to be called</param>
#endif
		public void AddListener(UnityAction<string> onTrigger)
		{
			_OnTrigger.AddListener(onTrigger);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーを受け取るアクションを削除する。
		/// </summary>
		/// <param name="onTrigger">削除するアクション</param>
#else
		/// <summary>
		/// Remove the action that receives the trigger.
		/// </summary>
		/// <param name="onTrigger">Action to remove</param>
#endif
		public void RemoveListener(UnityAction<string> onTrigger)
		{
			_OnTrigger.RemoveListener(onTrigger);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimationEventから呼び出すメソッド
		/// </summary>
		/// <param name="triggerName">トリガー名</param>
#else
		/// <summary>
		/// Method called from AnimationEvent
		/// </summary>
		/// <param name="triggerName">Trigger Name</param>
#endif
		public void TriggerEvent(string triggerName)
		{
			_OnTrigger.Invoke(triggerName);
		}
	}
}