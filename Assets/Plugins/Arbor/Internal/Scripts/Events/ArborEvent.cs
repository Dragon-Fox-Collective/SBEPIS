//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ArborEditorで引数の設定も可能な永続的なコールバック
	/// </summary>
#else
	/// <summary>
	/// Persistent callbacks that can also set arguments in ArborEditor
	/// </summary>
#endif
	[System.Serializable]
	public sealed class ArborEvent
	{
		[SerializeField]
		private List<PersistentCall> _Calls = new List<PersistentCall>();

		private System.Text.StringBuilder s_WarningBuilder = new System.Text.StringBuilder();
		private string _WarningMessage = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// メソッドの呼び出しに警告がある場合にメッセージを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns a message if there is a warning on the method call.
		/// </summary>
#endif
		public string warningMessage
		{
			get
			{
				if (string.IsNullOrEmpty(_WarningMessage))
				{
					s_WarningBuilder.Length = 0;
					for (int callIndex = 0; callIndex < _Calls.Count; callIndex++)
					{
						PersistentCall call = _Calls[callIndex];
						string message = call.GetWarningMessage();
						if (string.IsNullOrEmpty(message))
						{
							continue;
						}

						if (s_WarningBuilder.Length > 0)
						{
							s_WarningBuilder.AppendLine();
						}
						s_WarningBuilder.AppendLine(message);
					}

					_WarningMessage = s_WarningBuilder.ToString();
				}
				return _WarningMessage;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// メソッドの呼び出し
		/// </summary>
#else
		/// <summary>
		/// Calling methods
		/// </summary>
#endif
		public void Invoke()
		{
			for (int i = 0, count = _Calls.Count; i < count; ++i)
			{
				PersistentCall call = _Calls[i];
				call.Invoke();
			}
		}
	}
}