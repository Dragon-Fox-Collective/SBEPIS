//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 経路を設定するためのコンポーネント
	/// </summary>
#else
	/// <summary>
	/// Components for setting routes
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/Waypoint", 50)]
	[BuiltInComponent]
	[HelpURL(ArborReferenceUtility.componentUrl + "Arbor/Navigation/waypoint.html")]
	[Internal.DocumentManual("/manual/builtin/waypoint.md")]
	public sealed class Waypoint : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 通過ポイント
		/// </summary>
#else
		/// <summary>
		/// Passing point
		/// </summary>
#endif
		[SerializeField]
		private List<Transform> _Points = new List<Transform>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Gizmosの色
		/// </summary>
#else
		/// <summary>
		/// Gizmos color
		/// </summary>
#endif
		[SerializeField]
		private Color _GizmosColor = Color.green;

#if ARBOR_DOC_JA
		/// <summary>
		/// 通過ポイントの数
		/// </summary>
#else
		/// <summary>
		/// Number of passing points
		/// </summary>
#endif
		public int pointCount
		{
			get
			{
				return _Points.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 通過ポイントの取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>通過ポイント</returns>
#else
		/// <summary>
		/// Get a passing point
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Passing point</returns>
#endif
		public Transform GetPoint(int index)
		{
			return _Points[index];
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = _GizmosColor;
			for (int i = 0; i < _Points.Count - 1; i++)
			{
				Transform p1 = _Points[i];
				Transform p2 = _Points[i + 1];
				if (p1 == null || p2 == null)
				{
					break;
				}
				Gizmos.DrawLine(p1.position, p2.position);
			}
		}
	}
}