//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Attributeのヘルパークラス。
	/// </summary>
#else
	/// <summary>
	/// A helper class for Attribute.
	/// </summary>
#endif
	public static class AttributeHelper
	{
		private static readonly Dictionary<MemberInfo, Attribute[]> _MemberAttributes = new Dictionary<MemberInfo, Attribute[]>();
		private static readonly Dictionary<Assembly, Attribute[]> _AssemblyAttributes = new Dictionary<Assembly, Attribute[]>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attributes</returns>
#else
		/// <summary>
		/// Get Attributes.
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attributes</returns>
#endif
		public static Attribute[] GetAttributes(Assembly assembly)
		{
			Attribute[] attributes = null;
			if (!_AssemblyAttributes.TryGetValue(assembly, out attributes))
			{
				attributes = assembly.GetCustomAttributes().ToArray();
				_AssemblyAttributes.Add(assembly, attributes);
			}

			return attributes;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Attribute</returns>
#endif
		public static Attribute GetAttribute(Assembly assembly, System.Type targetType)
		{
			if (assembly == null)
			{
				return null;
			}

			Attribute[] attributes = GetAttributes(assembly);
			int attributeCount = attributes.Length;
			for (int index = 0; index < attributeCount; index++)
			{
				Attribute attribute = attributes[index];

				System.Type attributeType = attribute.GetType();

				if (attributeType == targetType || TypeUtility.IsSubclassOf(attributeType, targetType))
				{
					return attribute;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attribute</returns>
#endif
		public static T GetAttribute<T>(Assembly assembly) where T : Attribute
		{
			if (assembly == null)
			{
				return null;
			}

			Attribute[] attributes = GetAttributes(assembly);
			int attributeCount = attributes.Length;
			for (int index = 0; index < attributeCount; index++)
			{
				Attribute attribute = attributes[index];

				if (attribute is T)
				{
					return attribute as T;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeの配列を取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attributeの配列</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attribute</returns>
#endif
		public static T[] GetAttributes<T>(Assembly assembly) where T : Attribute
		{
			return AttributeCache<T>.GetAttributes(assembly);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute(Assembly assembly, System.Type targetType)
		{
			return (GetAttribute(assembly, targetType) != null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute<T>(Assembly assembly) where T : Attribute
		{
			return (GetAttribute<T>(assembly) != null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得。
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attributes</returns>
#else
		/// <summary>
		/// Get Attributes.
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attributes</returns>
#endif
		public static Attribute[] GetAttributes(MemberInfo member)
		{
			Attribute[] attributes = null;
			if (!_MemberAttributes.TryGetValue(member, out attributes))
			{
				try
				{
					attributes = member.GetCustomAttributes(typeof(Attribute), false) as Attribute[];
				}
				catch
				{
					attributes = new Attribute[0];
				}

				_MemberAttributes.Add(member, attributes);
			}

			return attributes;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得。
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Attributes</returns>
#else
		/// <summary>
		/// Get Attributes.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Attributes</returns>
#endif
		public static Attribute[] GetAttributes(Type type)
		{
			return GetAttributes(TypeUtility.GetMemberInfo(type));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Attribute</returns>
#endif
		public static Attribute GetAttribute(MemberInfo member, System.Type targetType)
		{
			if (member == null)
			{
				return null;
			}

			Attribute[] attributes = GetAttributes(member);
			int attributeCount = attributes.Length;
			for (int index = 0; index < attributeCount; index++)
			{
				Attribute attribute = attributes[index];

				System.Type attributeType = attribute.GetType();

				if (attributeType == targetType || TypeUtility.IsSubclassOf(attributeType, targetType))
				{
					return attribute;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Attribute</returns>
#endif
		public static Attribute GetAttribute(Type type, Type targetType)
		{
			return GetAttribute(TypeUtility.GetMemberInfo(type), targetType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attribute</returns>
#endif
		public static T GetAttribute<T>(MemberInfo member) where T : Attribute
		{
			if (member == null)
			{
				return null;
			}

			Attribute[] attributes = GetAttributes(member);
			int attributeCount = attributes.Length;
			for (int index = 0; index < attributeCount; index++)
			{
				Attribute attribute = attributes[index];

				if (attribute is T)
				{
					return attribute as T;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeを取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Attribute</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Attribute</returns>
#endif
		public static T GetAttribute<T>(Type type) where T : Attribute
		{
			return GetAttribute<T>(TypeUtility.GetMemberInfo(type));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeの配列を取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attributeの配列</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attribute</returns>
#endif
		public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
		{
			return AttributeCache<T>.GetAttributes(member);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeの配列を取得 (generic)
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Attributeの配列</returns>
#else
		/// <summary>
		/// Get Attribute (generic)
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Attribute</returns>
#endif
		public static T[] GetAttributes<T>(Type type) where T : Attribute
		{
			return GetAttributes<T>(TypeUtility.GetMemberInfo(type));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <param name="member">MemberInfo</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute(MemberInfo member, Type targetType)
		{
			return (GetAttribute(member, targetType) != null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="targetType">取得する型</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="targetType">Target Type</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute(Type type, Type targetType)
		{
			return HasAttribute(TypeUtility.GetMemberInfo(type), targetType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="member">MemberInfo</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute<T>(MemberInfo member) where T : Attribute
		{
			return (GetAttribute<T>(member) != null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Attributeがあるかどうか。
		/// </summary>
		/// <typeparam name="T">取得する型</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Attributeがあるかどうか。</returns>
#else
		/// <summary>
		/// Whether has Attribute
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="type">Type</param>
		/// <returns>Whether has attribute.</returns>
#endif
		public static bool HasAttribute<T>(Type type) where T : Attribute
		{
			return HasAttribute<T>(TypeUtility.GetMemberInfo(type));
		}

		static class AttributeCache<T> where T : Attribute
		{
			private static Dictionary<Assembly, T[]> s_AssemblyAttributes = new Dictionary<Assembly, T[]>();
			private static Dictionary<MemberInfo, T[]> s_MemberAttributes = new Dictionary<MemberInfo, T[]>();

			internal static T[] GetAttributes(Assembly assembly)
			{
				if (assembly == null)
				{
					return null;
				}

				T[] result = null;
				if (s_AssemblyAttributes.TryGetValue(assembly, out result))
				{
					return result;
				}

				List<T> list = new List<T>();

				Attribute[] attributes = AttributeHelper.GetAttributes(assembly);
				int attributeCount = attributes.Length;
				for (int index = 0; index < attributeCount; index++)
				{
					T attribute = attributes[index] as T;

					if (attribute != null)
					{
						list.Add(attribute);
					}
				}

				result = list.ToArray();

				s_AssemblyAttributes.Add(assembly, result);

				return result;
			}

			internal static T[] GetAttributes(MemberInfo member)
			{
				if (member == null)
				{
					return null;
				}

				T[] result = null;
				if (s_MemberAttributes.TryGetValue(member, out result))
				{
					return result;
				}

				List<T> list = new List<T>();

				Attribute[] attributes = AttributeHelper.GetAttributes(member);
				int attributeCount = attributes.Length;
				for (int index = 0; index < attributeCount; index++)
				{
					T attribute = attributes[index] as T;

					if (attribute != null)
					{
						list.Add(attribute);
					}
				}

				result = list.ToArray();

				s_MemberAttributes.Add(member, result);

				return result;
			}
		}
	}
}
