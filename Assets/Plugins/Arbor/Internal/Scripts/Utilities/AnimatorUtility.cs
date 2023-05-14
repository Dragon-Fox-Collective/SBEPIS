//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Utilities
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Animator utility class
	/// </summary>
#endif
	public static class AnimatorUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// レイヤー名からインデックスを取得する。
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="layerName">レイヤー名</param>
		/// <returns>見つかったインデックス。見つからなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get the index from the layer name.
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="layerName">Layer Name</param>
		/// <returns>Index found. If not found, returns -1.</returns>
#endif
		public static int GetLayerIndex(Animator animator, string layerName)
		{
			for (int i = 0; i < animator.layerCount; i++)
			{
				if (animator.GetLayerName(i) == layerName)
				{
					return i;
				}
			}

			return -1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移中か判定する。
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="layerIndex">レイヤーのインデックス</param>
		/// <param name="stateName">ステート名</param>
		/// <returns>ステートへの遷移中であればtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if the transition is in progress.
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="layerIndex">Layer index</param>
		/// <param name="stateName">State name</param>
		/// <returns>Returns true if transitioning to state.</returns>
#endif
		public static bool IsInTransition(Animator animator, int layerIndex, string stateName)
		{
			if (animator.IsInTransition(layerIndex))
			{
				AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(layerIndex);
				return nextState.IsName(stateName);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータがあるか判定する。
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="name">パラメータ名</param>
		/// <returns>パラメータがある場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if there are parameters.
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="name">Parameter name</param>
		/// <returns>Returns true if there are parameters.</returns>
#endif
		public static bool HasParameter(Animator animator, string name)
		{
			var parameters = animator.parameters;
			for (int i = 0, count = parameters.Length; i < count; i++)
			{
				var parameter = parameters[i];
				if (parameter.name == name)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// パラメータが存在しているか確認する。
		/// </summary>
		/// <param name="animator">Animator</param>
		/// <param name="name">パラメータ名</param>
		/// <returns>パラメータがある場合にtrueを返す。</returns>
		/// <remarks>
		/// 確認はUnityEditor上でのプレイかDevelopmentBuildの時にのみ行われる。<br/>
		/// Scripting Define Symbolsに"ARBOR_DISABLE_CHECK_ANIMATOR_PARAMETER"を追加すると常に無効化できる。
		/// </remarks>
		public static bool CheckExistsParameter(Animator animator, string name)
		{
#if ARBOR_DISABLE_CHECK_ANIMATOR_PARAMETER
			return true;
#else
			if (!Application.isEditor && !Debug.isDebugBuild)
			{
				return true;
			}

			if (!string.IsNullOrEmpty(name) && HasParameter(animator, name))
			{
				return true;
			}

			Debug.LogError($"Parameter '{name}' does not exist.");
			return false;
#endif
		}
	}
}