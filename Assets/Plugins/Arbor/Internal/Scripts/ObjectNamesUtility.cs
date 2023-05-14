//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ObjectNamesのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// ObjectNames utility class
	/// </summary>
#endif
	public static class ObjectNamesUtility
	{
		static bool IsAlphabet(char ch)
		{
			return 'A' <= ch && ch <= 'Z' ||
				'a' <= ch && ch <= 'z';
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 変数名を表示可能な名前に加工する。
		/// </summary>
		/// <param name="name">変数名</param>
		/// <returns>表示可能な名前を返す。</returns>
		/// <remarks>大文字の前にスペースを挿入し、先頭の<code>m_</code>, <code>_</code>または大文字の前の<code>k</code>を削除する。</remarks>
#else
		/// <summary>
		/// Process the variable name into a displayable name.
		/// </summary>
		/// <param name="name">Variable name</param>
		/// <returns>Returns a displayable name.</returns>
		/// <remarks>Insert a space before the capital letter and remove the leading <code>m_</code>, <code>_</code> or the <code>k</code> before the capital letter.</remarks>
#endif
		public static string NicifyVariableName(string name)
		{
			int startIndex = 0;

			if (name.Length >= 1 && (name[0] == '_' ||
				(name[0] == 'k' && name.Length >= 2 && char.IsUpper(name[1]))))
			{
				startIndex += 1;
			}
			else if (name.StartsWith("m_"))
			{
				startIndex += 2;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			char prev = '\0';

			for (int i = startIndex; i < name.Length; i++)
			{
				char ch = name[i];
				if (i == startIndex)
				{
					if (char.IsLetter(ch))
					{
						ch = char.ToUpper(ch);
					}
				}
				else if (char.IsUpper(ch) && char.IsLetter(prev) && char.IsLower(prev) || 
					char.IsDigit(ch) && IsAlphabet(prev))
				{
					sb.Append(" ");
				}
				sb.Append(ch);

				prev = ch;
			}

			return sb.ToString();
		}
	}
}