//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.TaskSystem
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 進捗を定義するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface that defines progress
	/// </summary>
#endif
	public interface IProgress
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 0fから1fまでの進捗度
		/// </summary>
#else
		/// <summary>
		/// Progress from 0f to 1f
		/// </summary>
#endif
		float progress
		{
			get;
		}
	}
}