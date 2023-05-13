//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Text;
using System.Reflection;

namespace Arbor.Events
{
	using Arbor.DynamicReflection;

#if ARBOR_DOC_JA
	/// <summary>
	/// ArborEventのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// ArborEvent utility class
	/// </summary>
#endif
	public static class ArborEventUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// メソッド名を返す。
		/// </summary>
		/// <param name="methodInfo">メソッド名を返すMethodInfo</param>
		/// <returns>メソッド名</returns>
#else
		/// <summary>
		/// Returns the method name.
		/// </summary>
		/// <param name="methodInfo">MethodInfo that returns method name</param>
		/// <returns>Method name</returns>
#endif
		public static string GetMethodName(MethodInfo methodInfo)
		{
			StringBuilder args = new StringBuilder();
			ParameterInfo[] parameters = methodInfo.GetParameters();
			int paramCount = parameters.Length;
			for (int paramIndex = 0; paramIndex < paramCount; paramIndex++)
			{
				ParameterInfo parameter = parameters[paramIndex];
				System.Type parameterType = parameter.ParameterType;
				if (parameter.IsOut)
				{
					args.Append("out ");
				}
				else if (parameterType.IsByRef)
				{
					args.Append("ref ");
				}

				args.Append(TypeUtility.GetTypeName(parameterType));

				if (paramIndex < paramCount - 1)
					args.Append(", ");
			}

			return string.Format("{0} ({1})", methodInfo.Name, args.ToString());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールド名を返す。
		/// </summary>
		/// <param name="fieldInfo">メソッド名を返すFieldInfo</param>
		/// <returns>フィールド名</returns>
#else
		/// <summary>
		/// Returns the field name.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo that returns field name</param>
		/// <returns>Field name</returns>
#endif
		public static string GetFieldName(FieldInfo fieldInfo)
		{
			return string.Format("{0} {1}", TypeUtility.GetTypeName(fieldInfo.FieldType), fieldInfo.Name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プロパティ名を返す。
		/// </summary>
		/// <param name="propertyInfo">プロパティ名を返すPropertyInfo</param>
		/// <returns>プロパティ名</returns>
#else
		/// <summary>
		/// Returns the property name.
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo that returns property name</param>
		/// <returns>Property name</returns>
#endif
		public static string GetPropertyName(PropertyInfo propertyInfo)
		{
			return string.Format("{0} {1}", TypeUtility.GetTypeName(propertyInfo.PropertyType), propertyInfo.Name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// メンバー名を返す。
		/// </summary>
		/// <param name="memberInfo">メンバー名を返すMemberInfo</param>
		/// <returns>メンバー名</returns>
#else
		/// <summary>
		/// Returns the member name.
		/// </summary>
		/// <param name="memberInfo">MemberInfo that returns member name</param>
		/// <returns>Member name</returns>
#endif
		public static string GetMemberName(MemberInfo memberInfo)
		{
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return GetMethodName(methodInfo);
			}

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return GetFieldName(fieldInfo);
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return GetPropertyName(propertyInfo);
			}

			return memberInfo.Name;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スタティックなメンバーであるかを返す。
		/// </summary>
		/// <param name="memberInfo">MemberInfo</param>
		/// <returns>スタティックメンバーであればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether it is a static member.
		/// </summary>
		/// <param name="memberInfo">MemberInfo</param>
		/// <returns>Returns true if it is a static member.</returns>
#endif
		public static bool IsStatic(MemberInfo memberInfo)
		{
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return methodInfo.IsStatic;
			}

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.IsStatic;
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.GetAccessors()[0].IsStatic;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータのタイプを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="unknownToSlot">不明な型の場合にスロットとするフラグ</param>
		/// <returns>パラメータのタイプ</returns>
#else
		/// <summary>
		/// Returns the type of parameter.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="unknownToSlot">Flag to slot for unknown type</param>
		/// <returns>Type of parameter</returns>
#endif
		public static ParameterType GetParameterType(System.Type type, bool unknownToSlot)
		{
			if (type == null)
			{
				return ParameterType.Unknown;
			}

			if (type.IsByRef)
			{
				type = type.GetElementType();
			}

			if (type == typeof(int))
			{
				return ParameterType.Int;
			}
			else if (type == typeof(long))
			{
				return ParameterType.Long;
			}
			else if (type == typeof(float))
			{
				return ParameterType.Float;
			}
			else if (type == typeof(bool))
			{
				return ParameterType.Bool;
			}
			else if (type == typeof(string))
			{
				return ParameterType.String;
			}
			else if (type == typeof(Vector2))
			{
				return ParameterType.Vector2;
			}
			else if (type == typeof(Vector3))
			{
				return ParameterType.Vector3;
			}
			else if (type == typeof(Quaternion))
			{
				return ParameterType.Quaternion;
			}
			else if (type == typeof(Rect))
			{
				return ParameterType.Rect;
			}
			else if (type == typeof(Bounds))
			{
				return ParameterType.Bounds;
			}
			else if (type == typeof(Color))
			{
				return ParameterType.Color;
			}
			else if (type == typeof(Vector4))
			{
				return ParameterType.Vector4;
			}
			else if (type == typeof(Vector2Int))
			{
				return ParameterType.Vector2Int;
			}
			else if (type == typeof(Vector3Int))
			{
				return ParameterType.Vector3Int;
			}
			else if (type == typeof(RectInt))
			{
				return ParameterType.RectInt;
			}
			else if (type == typeof(BoundsInt))
			{
				return ParameterType.BoundsInt;
			}
			else if (type == typeof(GameObject))
			{
				return ParameterType.GameObject;
			}
			else if (TypeUtility.IsAssignableFrom(typeof(Component), type))
			{
				return ParameterType.Component;
			}
			else if (TypeUtility.IsAssignableFrom(typeof(Object), type))
			{
				return ParameterType.AssetObject;
			}
			else if (EnumFieldUtility.IsEnum(type))
			{
				return ParameterType.Enum;
			}

			return unknownToSlot ? ParameterType.Slot : ParameterType.Unknown;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 選択可能なメソッドか判定する。
		/// </summary>
		/// <param name="methodInfo">判定するMethodInfo</param>
		/// <returns>選択可能な場合にtrueを返す</returns>
#else
		/// <summary>
		/// It is judged whether it is a selectable method.
		/// </summary>
		/// <param name="methodInfo">MethodInfo to determine</param>
		/// <returns>Returns true if it is selectable</returns>
#endif
		public static bool IsSelectableMethod(MethodInfo methodInfo)
		{
			if (methodInfo.IsSpecialName)
			{
				return false;
			}

			if (methodInfo.IsGenericMethod || methodInfo.IsGenericMethodDefinition)
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<ShowEventAttribute>(methodInfo))
			{
				return true;
			}

			if (AttributeHelper.HasAttribute<System.ObsoleteAttribute>(methodInfo))
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<HideEventAttribute>(methodInfo))
			{
				return false;
			}

			//var parameters = methodInfo.GetParameters();
			//for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
			//{
			//	ParameterInfo parameter = parameters[parameterIndex];
			//	if (!parameter.IsOut && GetParameterType(parameter.ParameterType, false) == ParameterType.Unknown)
			//	{
			//		return false;
			//	}
			//}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 選択可能なフィールドか判定する。
		/// </summary>
		/// <param name="fieldInfo">判定するFieldInfo</param>
		/// <returns>選択可能な場合にtrueを返す</returns>
#else
		/// <summary>
		/// It is judged whether it is a selectable field.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo to determine</param>
		/// <returns>Returns true if it is selectable</returns>
#endif
		public static bool IsSelectableField(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsSpecialName)
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<ShowEventAttribute>(fieldInfo))
			{
				return true;
			}

			if (AttributeHelper.HasAttribute<System.ObsoleteAttribute>(fieldInfo))
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<HideEventAttribute>(fieldInfo))
			{
				return false;
			}

			//if (GetParameterType(fieldInfo.FieldType) == ParameterType.Unknown)
			//{
			//	return false;
			//}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// getアクセサーがあるかを返す。
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo</param>
		/// <returns>getアクセサーがある場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether there is a get accessor.
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo</param>
		/// <returns>Returns true if there is a get accessor.</returns>
#endif
		public static bool IsGetProperty(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.CanRead)
			{
				return false;
			}

			MethodInfo getMethod = propertyInfo.GetGetMethod();
			if (getMethod == null || !getMethod.IsPublic)
			{
				return false;
			}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// setアクセサーがあるかを返す。
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo</param>
		/// <returns>setアクセサーがある場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether there is a set accessor.
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo</param>
		/// <returns>Returns true if there is a set accessor.</returns>
#endif
		public static bool IsSetProperty(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.CanWrite)
			{
				return false;
			}

			MethodInfo setMethod = propertyInfo.GetSetMethod();
			if (setMethod == null || !setMethod.IsPublic)
			{
				return false;
			}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 選択可能なプロパティか判定する。
		/// </summary>
		/// <param name="propertyInfo">判定するPropertyInfo</param>
		/// <returns>選択可能な場合にtrueを返す</returns>
#else
		/// <summary>
		/// It is judged whether it is a selectable property.
		/// </summary>
		/// <param name="propertyInfo">PropertyInfo to determine</param>
		/// <returns>Returns true if it is selectable</returns>
#endif
		public static bool IsSelectableProperty(PropertyInfo propertyInfo)
		{
			if (propertyInfo.IsSpecialName)
			{
				return false;
			}

			// ignore indexer
			ParameterInfo[] parameters = propertyInfo.GetIndexParameters();
			if (parameters != null && parameters.Length > 0)
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<ShowEventAttribute>(propertyInfo))
			{
				return true;
			}

			if (AttributeHelper.HasAttribute<System.ObsoleteAttribute>(propertyInfo))
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<HideEventAttribute>(propertyInfo))
			{
				return false;
			}

			//if (GetParameterType(propertyInfo.PropertyType) == ParameterType.Unknown)
			//{
			//	return false;
			//}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをキャストする。
		/// </summary>
		/// <param name="obj">キャストするオブジェクト</param>
		/// <param name="type">キャストする型</param>
		/// <returns>キャストされた値</returns>
#else
		/// <summary>
		/// Cast the object.
		/// </summary>
		/// <param name="obj">The object to cast</param>
		/// <param name="type">Casting type</param>
		/// <returns>Casted value</returns>
#endif
		[System.Obsolete("use Arbor.DynamicReflection.DynamicUtility.Cast()")]
		public static object Cast(object obj, System.Type type)
		{
			return DynamicUtility.Cast(obj, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型のデフォルト値を返す。
		/// </summary>
		/// <param name="type">デフォルト値の型</param>
		/// <returns>デフォルト値</returns>
#else
		/// <summary>
		/// Returns the default value of type.
		/// </summary>
		/// <param name="type">Default value type</param>
		/// <returns>Default value</returns>
#endif
		[System.Obsolete("use Arbor.DynamicReflection.DynamicUtility.GetDefault()")]
		public static object GetDefault(System.Type type)
		{
			return DynamicUtility.GetDefault(type);
		}
	}
}