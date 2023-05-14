//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterContainerを識別するための基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class to identify the ParameterContainer
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class ParameterContainerBase : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 実体のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		/// It returns the ParameterContainer entity.
		/// </summary>
#endif
		public ParameterContainerInternal container
		{
			get
			{
				GlobalParameterContainerInternal globalParameterContainer = this as GlobalParameterContainerInternal;
				if ((object)globalParameterContainer != null)
				{
					return globalParameterContainer.instance;
				}

				return this as ParameterContainerInternal;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 元のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		/// It returns the original ParameterContainer.
		/// </summary>
#endif
		public ParameterContainerInternal defaultContainer
		{
			get
			{
				GlobalParameterContainerInternal globalParameterContainer = this as GlobalParameterContainerInternal;
				if ((object)globalParameterContainer != null)
				{
					return globalParameterContainer.prefab;
				}

				return this as ParameterContainerInternal;
			}
		}
	}
}
