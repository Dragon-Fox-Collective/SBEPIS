//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Variableの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class of Variable.
	/// </summary>
#endif
	public abstract class VariableBase : InternalVariableBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Varableクラスが持つデータ型を取得する。
		/// </summary>
		/// <param name="variableClassType">Variableクラスの型</param>
		/// <returns>データ型</returns>
#else
		/// <summary>
		/// Get the data type possessed by the Variable class.
		/// </summary>
		/// <param name="variableClassType">Variable class type</param>
		/// <returns>Data type</returns>
#endif
		public static System.Type GetDataType(System.Type variableClassType)
		{
			System.Type targetClassType = typeof(VariableBase);

			System.Type baseType = TypeUtility.GetBaseType(variableClassType);
			while (baseType != null && TypeUtility.IsSubclassOf(baseType, targetClassType))
			{
				if (TypeUtility.IsGeneric(baseType, typeof(Variable<>)))
				{
					return TypeUtility.GetGenericArguments(baseType)[0];
				}
				baseType = TypeUtility.GetBaseType(baseType);
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
		/// <param name="container">ParameterContainer</param>
		/// <param name="type">型</param>
		/// <returns>作成したVariableBaseを返す。</returns>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <param name="container">ParameterContainer</param>
		/// <param name="type">Type</param>
		/// <returns>Returns the created VariableBase.</returns>
#endif
		public static VariableBase Create(ParameterContainerInternal container, System.Type type)
		{
			System.Type classType = typeof(VariableBase);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `VariableBase' in order to use it as parameter `type'", "type");
			}

			s_CreatingParameterContainer = container;

			System.Type valueType = GetDataType(type);

			VariableBase variable = ComponentUtility.AddComponent(container.gameObject, type) as VariableBase;

			ComponentUtility.RecordObject(variable, "Add " + TypeUtility.GetTypeName(valueType));

			variable.Initialize(container);

			ComponentUtility.SetDirty(variable);

			s_CreatingParameterContainer = null;

			return variable;
		}
	}
}