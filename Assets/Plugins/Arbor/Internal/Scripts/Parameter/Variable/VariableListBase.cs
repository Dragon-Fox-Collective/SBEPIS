//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// VariableListの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class of VariableList.
	/// </summary>
#endif
	public abstract class VariableListBase : InternalVariableBase
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
			System.Type targetClassType = typeof(VariableListBase);

			System.Type baseType = TypeUtility.GetBaseType(variableClassType);
			while (baseType != null && TypeUtility.IsSubclassOf(baseType, targetClassType))
			{
				if (TypeUtility.IsGeneric(baseType, typeof(VariableList<>)))
				{
					return ListUtility.GetIListType(TypeUtility.GetGenericArguments(baseType)[0]);
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
		/// <returns>作成したVariableListBaseを返す。</returns>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <param name="container">ParameterContainer</param>
		/// <param name="type">Type</param>
		/// <returns>Returns the created VariableListBase.</returns>
#endif
		public static VariableListBase Create(ParameterContainerInternal container, System.Type type)
		{
			System.Type classType = typeof(VariableListBase);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `VariableListBase' in order to use it as parameter `type'", "type");
			}

			s_CreatingParameterContainer = container;

			System.Type valueType = GetDataType(type);

			VariableListBase variableList = ComponentUtility.AddComponent(container.gameObject, type) as VariableListBase;

			ComponentUtility.RecordObject(variableList, "Add " + TypeUtility.GetTypeName(valueType));

			variableList.Initialize(container);

			ComponentUtility.SetDirty(variableList);

			s_CreatingParameterContainer = null;

			return variableList;
		}
	}
}