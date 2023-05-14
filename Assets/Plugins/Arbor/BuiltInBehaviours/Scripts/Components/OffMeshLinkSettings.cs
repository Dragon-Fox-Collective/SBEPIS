//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentControllerが<see cref="OffMeshLink"/>を横切る方法の設定を行う。
	/// </summary>
#else
	/// <summary>
	/// Set the method for the AgentController to traverse <see cref="OffMeshLink"/>.
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/OffMeshLinkSettings")]
	[BuiltInComponent]
	[RequireComponent(typeof(OffMeshLink))]
	[Internal.DocumentManual("/manual/builtin/offmeshlinksettings.md")]
	public class OffMeshLinkSettings : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OffMeshLink"/>を通過する方法の設定
		/// </summary>
#else
		/// <summary>
		/// Setting how to traverse <see cref="OffMeshLink"/>
		/// </summary>
#endif
		[SerializeField]
		private OffMeshLinkTraverseData _TraverseData = new OffMeshLinkTraverseData();

#if ARBOR_DOC_JA
		/// <summary>
		/// OffMeshLinkTraverseDataを取得する
		/// </summary>
#else
		/// <summary>
		/// Get OffMeshLinkTraverseData
		/// </summary>
#endif
		public OffMeshLinkTraverseData traverseData
		{
			get
			{
				return _TraverseData;
			}
		}
	}
}