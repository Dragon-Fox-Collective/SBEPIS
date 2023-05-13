//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Extensions;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// VariableとVariableListの基底クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class of Variable　and VariableList.
	/// </summary>
#endif
	[HideType(true)]
	public abstract class InternalVariableBase : MonoBehaviour, IValueGetter
	{
		#region Serialize Fields

		[SerializeField]
		[HideInInspector]
		private ParameterContainerInternal _ParameterContainer;

		#endregion // Serialize Fields

		internal static ParameterContainerInternal s_CreatingParameterContainer;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値の型
		/// </summary>
#else
		/// <summary>
		/// Value type of parameter
		/// </summary>
#endif
		public abstract System.Type valueType
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値のオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Value object
		/// </summary>
#endif
		public abstract object valueObject
		{
			get;
			set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the NodeGraph.
		/// </summary>
#endif
		public ParameterContainerInternal parameterContainer
		{
			get
			{
				if (_ParameterContainer == null)
				{
					var containers = gameObject.GetComponentsTemp<ParameterContainerInternal>();
					for (int containerIndex = 0; containerIndex < containers.Count; containerIndex++)
					{
						ParameterContainerInternal container = containers[containerIndex];
						Parameter parameter = container.FindParameterContainsVariable(this);
						if (parameter != null)
						{
							_ParameterContainer = container;
							break;
						}
					}
				}
				if (_ParameterContainer == null && s_CreatingParameterContainer != null)
				{
					_ParameterContainer = s_CreatingParameterContainer;
				}
				return _ParameterContainer;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
		/// <param name="container">ParameterContainerInternal</param>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <param name="container">ParameterContainerInternal</param>
#endif
		public void Initialize(ParameterContainerInternal container)
		{
#if !ARBOR_DEBUG
			hideFlags |= HideFlags.HideInInspector | HideFlags.HideInHierarchy;
#endif

			_ParameterContainer = container;
		}

		object IValueGetter.GetValueObject()
		{
			return valueObject;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトがロードされた時やインスペクターの値が変更されたときに呼び出される（この呼出はエディター上のみ）
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script is loaded or when the inspector value changes (this call is only in the editor)
		/// </summary>
#endif
		protected virtual void OnValidate()
		{
			if (parameterContainer != null)
			{
				return;
			}

			if (s_DestroyUnuseVariables == null)
			{
				s_DestroyUnuseVariables = new HashSet<InternalVariableBase>();
			}
			if (!s_DestroyUnuseVariables.Contains(this))
			{
				s_DestroyUnuseVariables.Add(this);
			}
			if (!s_CallDelayDestroy)
			{
				s_CallDelayDestroy = true;
				ComponentUtility.DelayCall(OnDestroyUnuseVariablesDelay);
			}
		}

		private static bool s_CallDelayDestroy = false;
		private static HashSet<InternalVariableBase> s_DestroyUnuseVariables = null;

		static void OnDestroyUnuseVariablesDelay()
		{
			s_CallDelayDestroy = false;

			if (s_DestroyUnuseVariables == null || s_DestroyUnuseVariables.Count == 0)
			{
				return;
			}

			int count = 0;

			foreach (var variable in s_DestroyUnuseVariables)
			{
				if (variable == null)
				{
					continue;
				}
				GameObject obj = variable.gameObject;
				DestroyImmediate(variable);
				ComponentUtility.SetDirty(obj);

				count++;
			}

			if (count > 0)
			{
				Debug.LogWarning(string.Format("Arbor : Cleaned {0} Variable(s) that were not destroyed by the bug.", count));
			}

			s_DestroyUnuseVariables.Clear();
		}
	}
}