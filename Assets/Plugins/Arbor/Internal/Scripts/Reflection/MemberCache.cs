//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using System.Reflection;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MemberInfoのキャッシュ
	/// </summary>
#else
	/// <summary>
	/// MemberInfo cache
	/// </summary>
#endif
	public static class MemberCache
	{
		struct MethodKey : System.IEquatable<MethodKey>
		{
			private System.Type _Type;
			private string _MethodName;
			private System.Type[] _Arguments;

			private int _HashCode;

			public MethodKey(System.Type type, string methodName, System.Type[] arguments)
			{
				_Type = type;
				_MethodName = methodName;
				_Arguments = arguments;

				_HashCode = _Type.GetHashCode() ^ _MethodName.GetHashCode();

				for (int argIndex = 0; argIndex < _Arguments.Length; argIndex++)
				{
					System.Type argType = _Arguments[argIndex];
					_HashCode ^= argType.GetHashCode();
				}
			}

			public MethodInfo GetMethodInfo()
			{
				return _Type.GetMethod(_MethodName, _Arguments);
			}

			public bool Equals(MethodKey other)
			{
				if (!other._Type.Equals(_Type))
				{
					return false;
				}

				if (!other._MethodName.Equals(_MethodName))
				{
					return false;
				}

				return ListUtility.Equals<System.Type>(_Arguments, other._Arguments);
			}

			public override bool Equals(object other)
			{
				if (other is MethodKey)
					return Equals((MethodKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		struct FieldKey : System.IEquatable<FieldKey>
		{
			private System.Type _Type;
			private string _FieldName;

			private int _HashCode;

			public FieldKey(System.Type type, string fieldName)
			{
				_Type = type;
				_FieldName = fieldName;

				_HashCode = _Type.GetHashCode() ^ _FieldName.GetHashCode();
			}

			public FieldInfo GetFieldInfo()
			{
				return _Type.GetField(_FieldName);
			}

			public bool Equals(FieldKey other)
			{
				if (!other._Type.Equals(_Type))
				{
					return false;
				}

				if (!other._FieldName.Equals(_FieldName))
				{
					return false;
				}

				return true;
			}

			public override bool Equals(object other)
			{
				if (other is FieldKey)
					return Equals((FieldKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		struct PropertyKey : System.IEquatable<PropertyKey>
		{
			private System.Type _Type;
			private string _PropertyName;

			private int _HashCode;

			public PropertyKey(System.Type type, string propertyName)
			{
				_Type = type;
				_PropertyName = propertyName;

				_HashCode = _Type.GetHashCode() ^ _PropertyName.GetHashCode();
			}

			public PropertyInfo GetPropertyInfo()
			{
				return _Type.GetProperty(_PropertyName);
			}

			public bool Equals(PropertyKey other)
			{
				if (!other._Type.Equals(_Type))
				{
					return false;
				}

				if (!other._PropertyName.Equals(_PropertyName))
				{
					return false;
				}

				return true;
			}

			public override bool Equals(object other)
			{
				if (other is PropertyKey)
					return Equals((PropertyKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		struct RenamedMethodKey : System.IEquatable<RenamedMethodKey>
		{
			private string _MethodName;
			private System.Type[] _Arguments;

			private int _HashCode;

			public RenamedMethodKey(string methodName, System.Type[] arguments)
			{
				_MethodName = methodName;
				_Arguments = arguments;

				_HashCode = _MethodName.GetHashCode();

				for (int argIndex = 0; argIndex < _Arguments.Length; argIndex++)
				{
					System.Type argType = _Arguments[argIndex];
					_HashCode ^= argType.GetHashCode();
				}
			}

			public bool Equals(RenamedMethodKey other)
			{
				if (!other._MethodName.Equals(_MethodName))
				{
					return false;
				}

				return ListUtility.Equals<System.Type>(_Arguments, other._Arguments);
			}

			public override bool Equals(object other)
			{
				if (other is RenamedMethodKey)
					return Equals((RenamedMethodKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		struct RenamedFieldKey : System.IEquatable<RenamedFieldKey>
		{
			private System.Type _FieldType;
			private string _FieldName;

			private int _HashCode;

			public RenamedFieldKey(System.Type fieldType, string fieldName)
			{
				_FieldType = fieldType;
				_FieldName = fieldName;

				_HashCode = _FieldType.GetHashCode() ^ _FieldName.GetHashCode();
			}

			public bool Equals(RenamedFieldKey other)
			{
				if (!other._FieldType.Equals(_FieldType))
				{
					return false;
				}

				if (!other._FieldName.Equals(_FieldName))
				{
					return false;
				}

				return true;
			}

			public override bool Equals(object other)
			{
				if (other is RenamedFieldKey)
					return Equals((RenamedFieldKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		struct RenamedPropertyKey : System.IEquatable<RenamedPropertyKey>
		{
			private System.Type _PropertyType;
			private string _PropertyName;

			private int _HashCode;

			public RenamedPropertyKey(System.Type propertyType, string propertyName)
			{
				_PropertyType = propertyType;
				_PropertyName = propertyName;

				_HashCode = _PropertyType.GetHashCode() ^ _PropertyName.GetHashCode();
			}

			public bool Equals(RenamedPropertyKey other)
			{
				if (!other._PropertyType.Equals(_PropertyType))
				{
					return false;
				}

				if (!other._PropertyName.Equals(_PropertyName))
				{
					return false;
				}

				return true;
			}

			public override bool Equals(object other)
			{
				if (other is RenamedPropertyKey)
					return Equals((RenamedPropertyKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _HashCode;
			}
		}

		private static Dictionary<MethodKey, MethodInfo> s_MethodCache = new Dictionary<MethodKey, MethodInfo>();
		private static Dictionary<FieldKey, FieldInfo> s_FieldCache = new Dictionary<FieldKey, FieldInfo>();
		private static Dictionary<PropertyKey, PropertyInfo> s_PropertyCache = new Dictionary<PropertyKey, PropertyInfo>();
		private static Dictionary<System.Type, Dictionary<RenamedMethodKey, MethodInfo>> s_RenamedMethodCache = new Dictionary<System.Type, Dictionary<RenamedMethodKey, MethodInfo>>();
		private static Dictionary<System.Type, Dictionary<RenamedFieldKey, FieldInfo>> s_RenamedFieldCache = new Dictionary<System.Type, Dictionary<RenamedFieldKey, FieldInfo>>();
		private static Dictionary<System.Type, Dictionary<RenamedPropertyKey, PropertyInfo>> s_RenamedPropertyCache = new Dictionary<System.Type, Dictionary<RenamedPropertyKey, PropertyInfo>>();

#if ARBOR_DOC_JA
		/// <summary>
		/// キャッシュをクリア
		/// </summary>
#else
		/// <summary>
		/// Clear cache
		/// </summary>
#endif
		public static void Clear()
		{
			s_MethodCache.Clear();
			s_FieldCache.Clear();
			s_PropertyCache.Clear();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// MethodInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="methodName">メソッド名</param>
		/// <param name="arguments">引数の型</param>
		/// <returns>取得するMethodInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get MethodInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="methodName">Method name</param>
		/// <param name="arguments">Argument types</param>
		/// <returns>MethodInfo to get. If there is none, it returns null.</returns>
#endif
		public static MethodInfo GetMethodInfo(System.Type type, string methodName, System.Type[] arguments)
		{
			MethodKey key = new MethodKey(type, methodName, arguments);

			MethodInfo methodInfo = null;
			if (!s_MethodCache.TryGetValue(key, out methodInfo))
			{
				methodInfo = key.GetMethodInfo();
				s_MethodCache.Add(key, methodInfo);
			}

			return methodInfo;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// リネームされたMethodInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="methodName">メソッド名</param>
		/// <param name="arguments">引数の型</param>
		/// <returns>取得するMethodInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the renamed MethodInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="methodName">Method name</param>
		/// <param name="arguments">Argument types</param>
		/// <returns>MethodInfo to get. If there is none, it returns null.</returns>
#endif
		public static MethodInfo GetMethodInfoRenamedFrom(System.Type type, string methodName, System.Type[] arguments)
		{
			Dictionary<RenamedMethodKey, MethodInfo> methodCache = null;
			if (!s_RenamedMethodCache.TryGetValue(type, out methodCache))
			{
				methodCache = new Dictionary<RenamedMethodKey, MethodInfo>();

				var methods = type.GetMethods();
				for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++)
				{
					MethodInfo info = methods[methodIndex];
					RenamedFromAttribute attr = AttributeHelper.GetAttribute<RenamedFromAttribute>(info);
					if (attr == null)
					{
						continue;
					}

					ParameterInfo[] parameters = info.GetParameters();

					System.Type[] argumentTypes = new System.Type[parameters.Length];
					for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
					{
						argumentTypes[parameterIndex] = parameters[parameterIndex].ParameterType;
					}

					RenamedMethodKey oldKey = new RenamedMethodKey(attr.oldName, argumentTypes);
					methodCache.Add(oldKey, info);
				}

				s_RenamedMethodCache.Add(type, methodCache);
			}

			if (methodCache == null)
			{
				return null;
			}

			MethodInfo methodInfo = null;
			RenamedMethodKey key = new RenamedMethodKey(methodName, arguments);
			if (methodCache.TryGetValue(key, out methodInfo))
			{
				return methodInfo;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FieldInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="fieldName">フィールド名</param>
		/// <returns>取得するFieldInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get FieldInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="fieldName">Field name</param>
		/// <returns>FieldInfo to get. If there is none, it returns null.</returns>
#endif
		public static FieldInfo GetFieldInfo(System.Type type, string fieldName)
		{
			FieldKey key = new FieldKey(type, fieldName);

			FieldInfo fieldInfo = null;
			if (!s_FieldCache.TryGetValue(key, out fieldInfo))
			{
				fieldInfo = key.GetFieldInfo();
				s_FieldCache.Add(key, fieldInfo);
			}

			return fieldInfo;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// リネームされたFieldInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="fieldName">フィールド名</param>
		/// <param name="fieldType">フィールドの型</param>
		/// <returns>取得するFieldInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the renamed FieldInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="fieldName">Field name</param>
		/// <param name="fieldType">Field type</param>
		/// <returns>FieldInfo to get. If there is none, it returns null.</returns>
#endif
		public static FieldInfo GetFieldInfoRenamedFrom(System.Type type, string fieldName, System.Type fieldType)
		{
			Dictionary<RenamedFieldKey, FieldInfo> fieldCache = null;
			if (!s_RenamedFieldCache.TryGetValue(type, out fieldCache))
			{
				fieldCache = new Dictionary<RenamedFieldKey, FieldInfo>();

				var fields = type.GetFields();
				for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					FieldInfo info = fields[fieldIndex];
					RenamedFromAttribute attr = AttributeHelper.GetAttribute<RenamedFromAttribute>(info);
					if (attr == null)
					{
						continue;
					}

					RenamedFieldKey oldKey = new RenamedFieldKey(info.FieldType, attr.oldName);
					fieldCache.Add(oldKey, info);
				}

				s_RenamedFieldCache.Add(type, fieldCache);
			}

			if (fieldCache == null)
			{
				return null;
			}

			RenamedFieldKey fieldKey = new RenamedFieldKey(fieldType, fieldName);
			FieldInfo fieldInfo = null;
			if (fieldCache.TryGetValue(fieldKey, out fieldInfo))
			{
				return fieldInfo;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PropertyInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="propertyName">プロパティ名</param>
		/// <returns>取得するPropertyInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get PropertyInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="propertyName">Property name</param>
		/// <returns>PropertyInfo to get. If there is none, it returns null.</returns>
#endif
		public static PropertyInfo GetPropertyInfo(System.Type type, string propertyName)
		{
			PropertyKey key = new PropertyKey(type, propertyName);

			PropertyInfo propertyInfo = null;
			if (!s_PropertyCache.TryGetValue(key, out propertyInfo))
			{
				propertyInfo = key.GetPropertyInfo();
				s_PropertyCache.Add(key, propertyInfo);
			}

			return propertyInfo;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// リネームされたPropertyInfoを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="propertyName">プロパティ名</param>
		/// <param name="propertyType">プロパティの型</param>
		/// <returns>取得するPropertyInfo。無かった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the renamed PropertyInfo.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="propertyName">Property name</param>
		/// <param name="propertyType">Property type</param>
		/// <returns>PropertyInfo to get. If there is none, it returns null.</returns>
#endif
		public static PropertyInfo GetPropertyInfoRenamedFrom(System.Type type, string propertyName, System.Type propertyType)
		{
			Dictionary<RenamedPropertyKey, PropertyInfo> propertyCache = null;
			if (!s_RenamedPropertyCache.TryGetValue(type, out propertyCache))
			{
				propertyCache = new Dictionary<RenamedPropertyKey, PropertyInfo>();

				var properties = type.GetProperties();
				for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
				{
					PropertyInfo info = properties[propertyIndex];
					RenamedFromAttribute attr = AttributeHelper.GetAttribute<RenamedFromAttribute>(info);
					if (attr == null)
					{
						continue;
					}

					RenamedPropertyKey oldKey = new RenamedPropertyKey(info.PropertyType, attr.oldName);
					propertyCache.Add(oldKey, info);
				}

				s_RenamedPropertyCache.Add(type, propertyCache);
			}

			if (propertyCache == null)
			{
				return null;
			}

			RenamedPropertyKey propertyKey = new RenamedPropertyKey(propertyType, propertyName);
			PropertyInfo propertyInfo = null;
			if (propertyCache.TryGetValue(propertyKey, out propertyInfo))
			{
				return propertyInfo;
			}

			return null;
		}
	}
}