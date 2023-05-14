//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型のユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Type utility class
	/// </summary>
#endif
	public static class TypeUtility
	{
		static readonly Type[] s_DummyTypes = new Type[0];

		static readonly Dictionary<Type, string> s_TypeAliases = new Dictionary<Type, string>()
		{
			{ typeof(void), "void" },
			{ typeof(string), "string" },
			{ typeof(int), "int" },
			{ typeof(byte), "byte" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(short), "short" },
			{ typeof(ushort), "ushort" },
			{ typeof(long), "long" },
			{ typeof(uint), "uint" },
			{ typeof(ulong), "ulong" },
			{ typeof(float), "float" },
			{ typeof(double), "double" },
			{ typeof(decimal), "decimal" },
			{ typeof(object), "object" },
			{ typeof(bool), "bool" },
			{ typeof(char), "char" }
		};

		private static Dictionary<Type, string> s_TypeNameCache = new Dictionary<Type, string>();

		private static Dictionary<Type, string> s_AssemblyTypeNameCache = new Dictionary<Type, string>();

		private static Dictionary<string, Type> s_TypeCache = new Dictionary<string, Type>();

		static string GetNullableTypeName(Type type)
		{
			type = GetGenericArguments(type)[0];

			return string.Format("{0}?", GetTypeName(type));
		}

		static string GetGenericTypeName(Type type)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

			string typeName = type.Name;
			int index = typeName.IndexOf('`');
			if (index > -1)
			{
				typeName = typeName.Substring(0, index);
			}

			var genericArguments = GetGenericArguments(type);
			for (int i = 0; i < genericArguments.Length; i++)
			{
				Type t = genericArguments[i];
				if (builder.Length > 0)
				{
					builder.Append(",");
				}
				builder.Append(GetTypeName(t));
			}

			return string.Format("{0}<{1}>", typeName, builder.ToString());
		}

		static string GetArrayTypeName(Type type)
		{
			type = type.GetElementType();

			return string.Format("{0}[]", GetTypeName(type));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// エイリアス名を取得する。
		/// </summary>
		/// <param name="type">エイリアス名を取得する型。</param>
		/// <param name="typeName">取得した型の出力引数。</param>
		/// <returns>エイリアス名がある場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the alias name.
		/// </summary>
		/// <param name="type">The type to get the alias name.</param>
		/// <param name="typeName">Output argument of the retrieved type.</param>
		/// <returns>Returns true if there is an alias name.</returns>
#endif
		public static bool TryGetAliasName(Type type, out string typeName)
		{
			return s_TypeAliases.TryGetValue(type, out typeName);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型の名前を返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>型の名前</returns>
#else
		/// <summary>
		/// Returns the name of the type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>型の名前</returns>
#endif
		public static string GetTypeName(Type type)
		{
			if (type == null)
			{
				return "null";
			}

			if (type.IsByRef)
			{
				type = type.GetElementType();
			}

			string typeName = null;
			if (!s_TypeNameCache.TryGetValue(type, out typeName))
			{
				if (IsGenericType(type))
				{
					if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						typeName = GetNullableTypeName(type);
					}
					else
					{
						typeName = GetGenericTypeName(type);
					}
				}
				else if (type.IsArray)
				{
					typeName = GetArrayTypeName(type);
				}
				else if (!TryGetAliasName(type, out typeName))
				{
					typeName = type.Name;
				}

				s_TypeNameCache.Add(type, typeName);
			}

			return typeName;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型の名前を返す（データスロット用）
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>型の名前</returns>
#else
		/// <summary>
		/// Return type name (for data slot)
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Type name</returns>
#endif
		public static string GetSlotTypeName(Type type)
		{
			if (type != null)
			{
				return GetTypeName(type);
			}

			return "Any";
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeをシリアライズ可能な文字列に変換する。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <returns>シリアライズ可能な文字列</returns>
#else
		/// <summary>
		/// Convert Type to a serializable string.
		/// </summary>
		/// <param name="type">Type value</param>
		/// <returns>Serializable string</returns>
#endif
		public static string TidyAssemblyTypeName(Type type)
		{
			if (type == null)
			{
				return string.Empty;
			}

			string name = "";
			if (s_AssemblyTypeNameCache.TryGetValue(type, out name))
			{
				return name;
			}

			name = type.AssemblyQualifiedName;
			if (!string.IsNullOrEmpty(name))
			{
				if (IsGenericType(type) && !IsGenericTypeDefinition(type))
				{
					var arguments = GetGenericArguments(type);
					if (arguments != null)
					{
						HashSet<System.Type> hash = new HashSet<Type>();
						for (int index = 0; index < arguments.Length; index++)
						{
							System.Type t = arguments[index];
							if (t == null || hash.Contains(t))
							{
								continue;
							}

							name = name.Replace(t.AssemblyQualifiedName, TidyAssemblyTypeName(t));
							hash.Add(t);
						}
					}
				}

				int min = int.MaxValue;
				int i = name.LastIndexOf(", Version", StringComparison.Ordinal);
				if (i != -1)
				{
					min = Math.Min(i, min);
				}
				i = name.LastIndexOf(", Culture=", StringComparison.Ordinal);
				if (i != -1)
				{
					min = Math.Min(i, min);
				}
				i = name.LastIndexOf(", PublicKeyToken=", StringComparison.Ordinal);
				if (i != -1)
				{
					min = Math.Min(i, min);
				}

				if (min != int.MaxValue)
				{
					name = name.Substring(0, min);
				}

				// Strip module assembly name.
				// The non-modular version will always work, due to type forwarders.
				// This way, when a type gets moved to a differnet module, previously serialized UnityEvents still work.
				i = name.LastIndexOf(", UnityEngine.", StringComparison.Ordinal);
				if (i != -1 && name.EndsWith("Module", StringComparison.Ordinal))
					name = name.Substring(0, i) + ", UnityEngine";
			}

			s_AssemblyTypeNameCache.Add(type, name);

			return name;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 文字列からSystem.Typeを取得する。
		/// </summary>
		/// <param name="assemblyTypeName">型名</param>
		/// <returns>System.Type。assemblyTypeNameが空かnullの場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get System.Type from string.
		/// </summary>
		/// <param name="assemblyTypeName">Type name</param>
		/// <returns>System.Type. If assemblyTypeName is empty or null, it returns null.</returns>
#endif
		public static Type GetAssemblyType(string assemblyTypeName)
		{
			if (string.IsNullOrEmpty(assemblyTypeName))
			{
				return null;
			}

			Type type = null;
			if (!s_TypeCache.TryGetValue(assemblyTypeName, out type))
			{
				type = Type.GetType(assemblyTypeName, false);
				s_TypeCache.Add(assemblyTypeName, type);
			}

			return type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Assemblyに定義された型を返す。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Assemblyに定義された型の配列。失敗した場合は空の配列を返す。</returns>
#else
		/// <summary>
		/// Returns the type defined in Assembly.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>An array of types defined in Assembly. Returns an empty array on failure.</returns>
#endif
		public static Type[] GetTypesFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return s_DummyTypes;
			}
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException)
			{
				return s_DummyTypes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型が宣言されるAssemblyを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>型が宣言されるAssembly</returns>
#else
		/// <summary>
		/// Gets the Assembly in which the type is declared.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Assembly in which the type is declared</returns>
#endif
		public static Assembly GetAssembly(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().Assembly;
#else
			return type.Assembly;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 基底型を返す
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>基底型</returns>
#else
		/// <summary>
		/// Return base type
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Base type</returns>
#endif
		public static Type GetBaseType(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().BaseType;
#else
			return type.BaseType;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// typeがジェネリック型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ジェネリック型であればtrueを返す</returns>
#else
		/// <summary>
		/// Returns whether type is a generic type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>ジェネリック型であればtrueを返す</returns>
#endif
		public static bool IsGenericType(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsGenericType;
#else
			return type.IsGenericType;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ジェネリック型の定義であるかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ジェネリック型の定義であればtrueを返す</returns>
#else
		/// <summary>
		/// Returns whether it is a generic type definition.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a generic type definition</returns>
#endif
		public static bool IsGenericTypeDefinition(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsGenericTypeDefinition;
#else
			return type.IsGenericTypeDefinition;
#endif
		}

		private static Dictionary<Type, Type[]> s_GenericArguments = new Dictionary<Type, Type[]>();

		static Type[] Internal_GetGenericArguments(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().GenericTypeArguments;
#else
			return type.GetGenericArguments();
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ジェネリック型の型引数の配列を返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ジェネリック型の型引数の配列</returns>
#else
		/// <summary>
		/// Return an array of generic type arguments.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>An array of generic type arguments</returns>
#endif
		public static Type[] GetGenericArguments(Type type)
		{
			Type[] args = null;
			if (!s_GenericArguments.TryGetValue(type, out args))
			{
				args = Internal_GetGenericArguments(type);
				s_GenericArguments.Add(type, args);
			}

			return args;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したジェネリック型定義から構築されたジェネリック型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="genericTypeDefinition">ジェネリック型定義</param>
		/// <returns>typeがgenericTypeDefinitionから構築されたジェネリック型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether it is a generic type constructed from the specified generic type definition.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="genericTypeDefinition">Generic type definition</param>
		/// <returns>Return true if type is a generic type constructed from genericTypeDefinition.</returns>
#endif
		public static bool IsGeneric(Type type, Type genericTypeDefinition)
		{
			return IsGenericType(type) && !IsGenericTypeDefinition(type) && type.GetGenericTypeDefinition() == genericTypeDefinition;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プリミティブ型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>プリミティブ型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is a primitive type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a primitive type.</returns>
#endif
		public static bool IsPrimitive(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsPrimitive;
#else
			return type.IsPrimitive;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 列挙体かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>列挙体であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is an enumeration.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Return true if it is an enumeration.</returns>
#endif
		public static bool IsEnum(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// クラス型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>クラス型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is a class type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a class type.</returns>
#endif
		public static bool IsClass(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsClass;
#else
			return type.IsClass;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// インターフェイス型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>インターフェイス型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is a interface type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a interface type.</returns>
#endif
		public static bool IsInterface(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsInterface;
#else
			return type.IsInterface;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>値型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is a value type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a value type.</returns>
#endif
		public static bool IsValueType(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsValueType;
#else
			return type.IsValueType;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライズ可能かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>シリアライズ可能であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether it is serializable.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if serializable.</returns>
#endif
		public static bool IsSerializable(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsSerializable;
#else
			return type.IsSerializable;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 抽象型かどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>抽象型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it is an abstract type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is an abstract type.</returns>
#endif
		public static bool IsAbstract(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsAbstract;
#else
			return type.IsAbstract;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ジェネリック型パラメータを含んでいるかどうかを返す。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ジェネリック型パラメータを含んでいればtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it contains a generic type parameter.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it contains a generic type parameter.</returns>
#endif
		public static bool ContainsGenericParameters(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().ContainsGenericParameters;
#else
			return type.ContainsGenericParameters;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// MemberInfoを取得する
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>MemberInfo</returns>
#else
		/// <summary>
		/// Get MemberInfo
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>MemberInfo</returns>
#endif
		public static MemberInfo GetMemberInfo(Type type)
		{
			return type.GetTypeInfo();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 派生した型かどうかを判断する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="c">比較する型</param>
		/// <returns>typeがcから派生している場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine whether it is a derived type or not.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="c">Type to be compared</param>
		/// <returns>Returns true if type is derived from c.</returns>
#endif
		public static bool IsSubclassOf(Type type, Type c)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsSubclassOf(c);
#else
			return type.IsSubclassOf(c);
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り当てられる型かどうかを判断する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="c">比較する型</param>
		/// <returns>cのインスタンスをtypeのインスタンスに割り当てられる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It is judged whether or not it is an assignable type.
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="c">Type to be compared</param>
		/// <returns>Returns true if an instance of c can be assigned to an instance of type.</returns>
#endif
		public static bool IsAssignableFrom(Type type, Type c)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
#else
			return type.IsAssignableFrom(c);
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型のフィールドを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="bindingAttr">フィールドの検索方法を制御するフラグ</param>
		/// <returns>FieldInfoの配列</returns>
#else
		/// <summary>
		/// Get type fields.
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="bindingAttr">Flag that controls how the field is searched</param>
		/// <returns>Array of FieldInfo</returns>
#endif
		public static FieldInfo[] GetFields(Type type, BindingFlags bindingAttr)
		{
			// If NETFX_CORE is defined then System.Reflection.TypeExtensions is used.
			return type.GetFields(bindingAttr);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// クラスがsealedで宣言されているかを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>クラスがsealedで宣言されている場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Gets whether the class is declared sealed.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if the class is declared sealed.</returns>
#endif
		public static bool IsSealed(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsSealed;
#else
			return type.IsSealed;
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// staticクラスであるかどうかを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>staticクラスであればtrueを返す。</returns>
#else
		/// <summary>
		/// Get whether it is a static class.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Returns true if it is a static class.</returns>
#endif
		public static bool IsStatic(Type type)
		{
			return type != null && IsClass(type) && IsAbstract(type) && IsSealed(type);
		}

		private static Dictionary<string, Type> s_RenamedTypes = null;

		struct ReferenceableKey : IEquatable<ReferenceableKey>
		{
			private Assembly _Assembly;
			private Assembly _Reference;

			public ReferenceableKey(Assembly assembly, Assembly reference)
			{
				_Assembly = assembly;
				_Reference = reference;
			}

			public bool Equals(ReferenceableKey other)
			{
				return other._Assembly == _Assembly && other._Reference == _Reference;
			}

			public override bool Equals(object other)
			{
				if (other is ReferenceableKey)
					return Equals((ReferenceableKey)other);
				return false;
			}

			public override int GetHashCode()
			{
				return _Assembly.GetHashCode() ^ _Reference.GetHashCode();
			}
		}

		private static Dictionary<ReferenceableKey, bool> s_ReferenceableTypes = new Dictionary<ReferenceableKey, bool>();

		private static Assembly TryToLoad(AssemblyName assemblyName)
		{
			try
			{
				return Assembly.Load(assemblyName);
			}
			catch (Exception)
			{
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照しているAssemblyを取得する。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>参照しているAssemblyNameの配列</returns>
#else
		/// <summary>
		/// Get Referenced Assemblies.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Array of referenced AssemblyName</returns>
#endif
		public static AssemblyName[] GetReferencedAssemblies(Assembly assembly)
		{
#if NETFX_CORE
			var method = assembly.GetType().GetMethod("GetReferencedAssemblies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return null;
			}
			try
			{
				return (AssemblyName[])method.Invoke(assembly, null);
			}
			catch (System.Exception)
			{
				return null;
			}
#else
			return assembly.GetReferencedAssemblies();
#endif
		}

		private static bool Internal_IsReferenceable(Assembly assembly, Assembly reference)
		{
			if (assembly == reference)
			{
				return true;
			}

			var referenceName = reference.GetName();

			var assemblies = GetReferencedAssemblies(assembly);
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					AssemblyName assemblyName = assemblies[i];
					if (AssemblyName.ReferenceMatchesDefinition(assemblyName, referenceName))
					{
						try
						{
							Assembly referencedAssembly = TryToLoad(assemblyName);
							if (referencedAssembly != null && referencedAssembly == reference)
							{
								return true;
							}
						}
						catch
						{
							continue;
						}
					}
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Assemblyが参照されているかどうかを判定する。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="reference">参照されているか判定するAssembly</param>
		/// <returns>参照されている場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if the Assembly is referenced.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="reference">Assembly to determine if it is referenced</param>
		/// <returns>Returns true if referenced.</returns>
#endif
		public static bool IsReferenceable(Assembly assembly, Assembly reference)
		{
			if (assembly == null || reference == null)
			{
				return false;
			}

			ReferenceableKey key = new ReferenceableKey(assembly, reference);
			bool isReferenceable = false;
			if (!s_ReferenceableTypes.TryGetValue(key, out isReferenceable))
			{
				isReferenceable = Internal_IsReferenceable(assembly, reference);
				s_ReferenceableTypes.Add(key, isReferenceable);
			}

			return isReferenceable;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssemblyがTypeを参照できるかどうかを判断する。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="type">Type</param>
		/// <returns>AssemblyがTypeを参照できる場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if the Assembly can reference the Type.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="type">Type</param>
		/// <returns>Returns true if Assembly can refer to Type.</returns>
#endif
		public static bool IsReferenceableType(Assembly assembly, Type type)
		{
			if (assembly == null || type == null)
			{
				return false;
			}

			Assembly typeAssembly = GetAssembly(type);
			return IsReferenceable(assembly, typeAssembly);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Assemblyが動的に生成されたかどうかを判断する。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Assemblyが動的に生成された場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if the Assembly was dynamically generated.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Returns true if the Assembly is dynamically generated.</returns>
#endif
		[Obsolete("use Assembly.IsDynamic")]
		public static bool IsDynamic(Assembly assembly)
		{
			return assembly.IsDynamic;
		}

		private static bool IsEditorAssembly(Assembly assembly)
		{
			return assembly.IsDefined(typeof(UnityEngine.AssemblyIsEditorAssembly)) ||
				IsEditorAssembly(assembly.GetName());
		}

		private static bool IsEditorAssembly(AssemblyName assemblyName)
		{
			string name = assemblyName.Name;
			return name == "Assembly-CSharp-Editor" || name == "Assembly-CSharp-Editor-firstpass" || name == "ArborEditor";
		}

		private static bool IsEditorDependentAssembly(Assembly assembly)
		{
			if (IsEditorAssembly(assembly))
				return true;
			var assemblies = GetReferencedAssemblies(assembly);
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					AssemblyName referencedAssembly = assemblies[i];
					if (IsEditorAssembly(referencedAssembly))
						return true;
				}
			}
			return false;
		}

		private static bool IsUserAssembly(AssemblyName assemblyName)
		{
			string name = assemblyName.Name;
			return name == "Assembly-CSharp" || name == "Assembly-CSharp-firstpass";
		}

		private static bool IsUserAssembly(Assembly assembly)
		{
			return IsUserAssembly(assembly.GetName());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ランタイムAssemblyかどうかを判定する。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>ランタイムAssemblyであればtrueを返す。</returns>
		/// <remarks>UnityEditorなどのAssemblyを参照していない場合にランタイムAssemblyとしている。</remarks>
#else
		/// <summary>
		/// Determine if it is a runtime assembly.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Returns true if it is a runtime assembly.</returns>
		/// <remarks>When the assembly such as UnityEditor is not referenced, it is set as the runtime assembly.</remarks>
#endif
		public static bool IsRuntimeAssembly(Assembly assembly)
		{
			return IsUserAssembly(assembly) || !IsEditorDependentAssembly(assembly);
		}

#if WINDOWS_UWP
		private static async System.Threading.Tasks.Task<Assembly[]> GetAssembliesAsync()
		{
			var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
			var files = await installedLocation.GetFilesAsync().AsTask().ConfigureAwait(false);
			if (files == null || files.Count == 0)
			{
				return new Assembly[0];
			}

			List<Assembly> assemblies = new List<Assembly>();
			for (int i = 0; i < files.Count; i++)
			{
				var file = files[i];
				string fileType = file.FileType;
				if (!(fileType == ".dll" || fileType == ".exe"))
				{
					continue;
				}
				var assemblyName = new AssemblyName(System.IO.Path.GetFileNameWithoutExtension(file.Name));
				var assembly = TryToLoad(assemblyName);
				if (assembly != null)
				{
					assemblies.Add(assembly);
				}
			}

			return assemblies.ToArray();
		}
#endif

#if ARBOR_DOC_JA
		/// <summary>
		/// アプリケーションに読み込まれているアセンブリを取得する。
		/// </summary>
		/// <returns>アプリケーションに読み込まれているアセンブリの配列を返す。</returns>
#else
		/// <summary>
		/// Get the assembly loaded in the application.
		/// </summary>
		/// <returns>Returns an array of assemblies loaded into the application.</returns>
#endif
		public static Assembly[] GetAssemblies()
		{
#if WINDOWS_UWP
			var task = GetAssembliesAsync();
			try
			{
				task.Wait();
			}
			catch (AggregateException ex)
			{
				UnityEngine.Debug.LogException(ex);
			}
			return task.Result;
#else
			return AppDomain.CurrentDomain.GetAssemblies();
#endif
		}

		private static List<Assembly> s_RuntimeAssembies = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// Arborを参照しているランタイムAssemblyを取得する。
		/// </summary>
		/// <returns>ランタイムAssemblyのリスト</returns>
#else
		/// <summary>
		/// Get the runtime assemblies that references the Arbor.
		/// </summary>
		/// <returns>List of runtime assemblies</returns>
#endif
		public static List<Assembly> GetRuntimeAssembies()
		{
			if (s_RuntimeAssembies != null)
			{
				return s_RuntimeAssembies;
			}

			s_RuntimeAssembies = new List<Assembly>();

			Assembly thisAssembly = GetAssembly(typeof(TypeUtility));

			var assemblies = GetAssemblies();
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					Assembly assembly = assemblies[i];
					if (assembly.IsDynamic ||
						!IsReferenceable(assembly, thisAssembly) ||
						!IsRuntimeAssembly(assembly))
					{
						continue;
					}

					s_RuntimeAssembies.Add(assembly);
				}
			}

			return s_RuntimeAssembies;
		}

		private static List<Type> s_RuntimeTypes = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// ランタイムに参照できる型を取得する。
		/// </summary>
		/// <returns>ランタイムに参照できる型のリスト</returns>
#else
		/// <summary>
		/// Get the type that can be referenced at runtime.
		/// </summary>
		/// <returns>List of types that can be referenced at runtime</returns>
#endif
		public static List<Type> GetRuntimeTypes()
		{
			if (s_RuntimeTypes != null)
			{
				return s_RuntimeTypes;
			}

			s_RuntimeTypes = new List<Type>();

			var assemblies = GetRuntimeAssembies();
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Count; i++)
				{
					Assembly assembly = assemblies[i];

					var types = GetTypesFromAssembly(assembly);
					for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
					{
						Type type = types[typeIndex];
						if (IsAssignableFrom(typeof(Attribute), type))
						{
							continue;
						}

						if (AttributeHelper.HasAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>(type))
						{
							continue;
						}

						s_RuntimeTypes.Add(type);
					}
				}
			}

			return s_RuntimeTypes;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// リネーム前の型名から型を取得する。
		/// </summary>
		/// <param name="typeName">リネーム前の型名</param>
		/// <returns>型</returns>
#else
		/// <summary>
		/// Get the type from the type name before renaming.
		/// </summary>
		/// <param name="typeName">Type name before renaming</param>
		/// <returns>Type</returns>
#endif
		public static Type GetTypeRenamedFrom(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return null;
			}

			if (s_RenamedTypes == null)
			{
				s_RenamedTypes = new Dictionary<string, Type>();

				var types = GetRuntimeTypes();
				for (int typeIndex = 0; typeIndex < types.Count; typeIndex++)
				{
					Type type = types[typeIndex];

					var attributes = AttributeHelper.GetAttributes<RenamedFromAttribute>(type);
					for (int attrIndex = 0; attrIndex < attributes.Length; attrIndex++)
					{
						RenamedFromAttribute attr = attributes[attrIndex];
						s_RenamedTypes[attr.oldName] = type;
					}
				}
			}

			Type newType = null;
			if (s_RenamedTypes.TryGetValue(typeName, out newType))
			{
				return newType;
			}
			return null;
		}

		private static Dictionary<string, Type> s_FindTypes = new Dictionary<string, Type>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Assembly名を含まない型名から型を見つける。
		/// </summary>
		/// <param name="typeName">Assembly名を含まない型名</param>
		/// <returns>見つかった型。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Find the type from the type name that does not include the Assembly name.
		/// </summary>
		/// <param name="typeName">Type name without Assembly name</param>
		/// <returns>The type found. If not, returns null.</returns>
#endif
		public static Type FindType(string typeName)
		{
			Type foundType = null;
			if (!s_FindTypes.TryGetValue(typeName, out foundType))
			{
				var types = GetRuntimeTypes();
				for (int i = 0; i < types.Count; i++)
				{
					Type t = types[i];
					if (t.FullName == typeName)
					{
						foundType = t;
						break;
					}
				}

				s_FindTypes.Add(typeName, foundType);
			}

			return foundType;
		}
	}
}
