//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityPlayerLoop = UnityEngine.LowLevel.PlayerLoop;
using UnityPlayerLoopTypes = UnityEngine.PlayerLoop;

namespace Arbor.PlayerLoop
{
#if ARBOR_DOC_JA
	/// <summary>
	/// PlayerLoopのヘルパークラス
	/// </summary>
#else
	/// <summary>
	/// PlayerLoop helper class
	/// </summary>
#endif
	public static class PlayerLoopHelper
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="UnityPlayerLoopTypes.FixedUpdate.ScriptRunDelayedFixedFrameRate"/>の後に呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called after <see cref="UnityPlayerLoopTypes.FixedUpdate.ScriptRunDelayedFixedFrameRate"/>.
		/// </summary>
#endif
		public static System.Action onDelayedFixedFrameRate;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnBeforeSceneLoad()
		{
			PlayerLoopSystem root = UnityPlayerLoop.GetCurrentPlayerLoop();

			var loop = new PlayerLoopSystem()
			{
				type = typeof(PlayerLoopTypes.ArborDelayedFixedFrameRate),
				updateDelegate = OnDelayFixedFrameRate,
			};

			PlayerLoopUtility.Insert<UnityPlayerLoopTypes.FixedUpdate.ScriptRunDelayedFixedFrameRate>(loop, true, ref root);

			UnityPlayerLoop.SetPlayerLoop(root);
		}

		static void OnDelayFixedFrameRate()
		{
			onDelayedFixedFrameRate?.Invoke();
		}
	}
}