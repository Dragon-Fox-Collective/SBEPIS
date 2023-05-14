//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Object型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Object type.
		/// </summary>
#endif
		public Object objectReferenceValue
		{
			get
			{
				if (IsObjectType())
				{
					return Internal_GetObject();
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!IsObjectType())
				{
					throw new ParameterTypeMismatchException();
				}

				Internal_SetObject(value);
			}
		}

		#region Object

		internal void Internal_SetObject(Object value)
		{
			if (container._ObjectParameters[_ParameterIndex] != value)
			{
				container._ObjectParameters[_ParameterIndex] = value;
				DoChanged();
			}
		}

		internal Object Internal_GetObject()
		{
			return container._ObjectParameters[_ParameterIndex];
		}

		bool IsObjectType()
		{
			return type == Type.GameObject ||
				type == Type.Component ||
				type == Type.Transform ||
				type == Type.RectTransform ||
				type == Type.Rigidbody ||
				type == Type.Rigidbody2D ||
				type == Type.AssetObject ||
				type == Type.Variable ||
				type == Type.VariableList;
		}

		#endregion //Object
	}
}