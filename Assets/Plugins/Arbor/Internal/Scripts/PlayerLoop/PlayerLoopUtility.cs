//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Text;
using UnityEngine.LowLevel;

namespace Arbor.PlayerLoop
{
#if ARBOR_DOC_JA
	/// <summary>
	/// PlayerLoopのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// PlayerLoop utility class
	/// </summary>
#endif
	public static class PlayerLoopUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// PlayerLoopを挿入する。
		/// </summary>
		/// <typeparam name="T">挿入先のPlayerLoopの型</typeparam>
		/// <param name="subSystem">挿入するPlayerLoopSystem</param>
		/// <param name="insertAfter">挿入先のPlayerLoopの型の後ろに挿入するかどうか</param>
		/// <param name="parentSystem">挿入する親のPlayerLoopSystem</param>
		/// <returns>挿入できたならtrueを返す。</returns>
#else
		/// <summary>
		/// Insert PlayerLoop.
		/// </summary>
		/// <typeparam name="T">Insertion destination Player Loop type</typeparam>
		/// <param name="subSystem">PlayerLoopSystem to insert</param>
		/// <param name="insertAfter">Whether to insert after the type of PlayerLoop to insert</param>
		/// <param name="parentSystem">Insert parent PlayerLoopSystem</param>
		/// <returns>If it can be inserted, it returns true.</returns>
#endif
		public static bool Insert<T>(PlayerLoopSystem subSystem, in bool insertAfter, ref PlayerLoopSystem parentSystem) where T : struct
		{
			var subSystemList = parentSystem.subSystemList;
			if (subSystemList == null)
			{
				return false;
			}

			for (int i = 0; i < subSystemList.Length; i++)
			{
				var s = subSystemList[i];
				if (s.type == typeof(T))
				{
					parentSystem.subSystemList = ListUtility.InsertToArray(subSystemList, i + (insertAfter ? 1 : 0), subSystem);
					return true;
				}
				else if (Insert<T>(subSystem, insertAfter, ref s))
				{
					subSystemList[i] = s;
					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PlayerLoopを削除する。
		/// </summary>
		/// <typeparam name="T">削除するPlayerLoopの型</typeparam>
		/// <param name="parentSystem">削除する親のPlayerLoopSystem</param>
		/// <returns>削除したならtrueを返す。</returns>
#else
		/// <summary>
		/// Remove PlayerLoop.
		/// </summary>
		/// <typeparam name="T">Player Loop type to remove</typeparam>
		/// <param name="parentSystem">The parent PlayerLoopSystem to remove</param>
		/// <returns>Returns true if removed.</returns>
#endif
		public static bool Remove<T>(ref PlayerLoopSystem parentSystem) where T : struct
		{
			var subSystemList = parentSystem.subSystemList;
			if (subSystemList == null)
			{
				return false;
			}

			for (int i = 0; i < subSystemList.Length; i++)
			{
				var s = subSystemList[i];
				if (s.type == typeof(T))
				{
					parentSystem.subSystemList = ListUtility.RemoveAtToArray(subSystemList, i);
					return true;
				}
				else if (Remove<T>(ref s))
				{
					subSystemList[i] = s;
					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PlayerLoopが含まれているか判定する。
		/// </summary>
		/// <typeparam name="T">判定するPlayerLoopの型</typeparam>
		/// <param name="parentSystem">判定する親のPlayerLoop</param>
		/// <returns>含まれていたならtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if PlayerLoop is included.
		/// </summary>
		/// <typeparam name="T">Player Loop type to judge</typeparam>
		/// <param name="parentSystem">Judging parent Player Loop</param>
		/// <returns>Returns true if it was included.</returns>
#endif
		public static bool Contains<T>(in PlayerLoopSystem parentSystem) where T : struct
		{
			var subSystemList = parentSystem.subSystemList;
			if (subSystemList == null)
			{
				return false;
			}

			for (int i = 0; i < subSystemList.Length; i++)
			{
				if (subSystemList[i].type == typeof(T) || Contains<T>(subSystemList[i]))
				{
					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PlayerLoopSystemをダンプする。
		/// </summary>
		/// <param name="loop">PlayerLoopSystem</param>
		/// <returns>ダンプした結果の文字列を返す。</returns>
#else
		/// <summary>
		/// Dump PlayerLoopSystem.
		/// </summary>
		/// <param name="loop">PlayerLoopSystem</param>
		/// <returns>Returns the string of the dumped result.</returns>
#endif
		public static string Dump(in PlayerLoopSystem loop)
		{
			StringBuilder sb = new StringBuilder();

			Dump(loop, sb, 0);

			return sb.ToString();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PlayerLoopSystemをダンプする。
		/// </summary>
		/// <param name="loop">PlayerLoopSystem</param>
		/// <param name="sb">ダンプ先のStringBuilder</param>
#else
		/// <summary>
		/// Dump PlayerLoopSystem.
		/// </summary>
		/// <param name="loop">PlayerLoopSystem</param>
		/// <param name="sb">Dump destination StringBuilder</param>
#endif
		public static void Dump(in PlayerLoopSystem loop, StringBuilder sb)
		{
			Dump(loop, sb, 0);
		}

		static void Dump(in PlayerLoopSystem loop, StringBuilder sb, int depth)
		{
			var subSystemList = loop.subSystemList;
			if (loop.subSystemList == null)
			{
				return;
			}

			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				Indent(sb, depth);
				var s = loop.subSystemList[i];
				sb.AppendLine($"* {s.type.Name}");
				Dump(s, sb, depth + 1);
			}
		}

		static void Indent(StringBuilder sb, int depth)
		{
			for (int i = 0; i < depth; i++)
			{
				sb.Append("    ");
			}
		}
	}
}