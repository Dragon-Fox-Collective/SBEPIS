//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using System.Reflection;

namespace Arbor.Serialization
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FieldInfoのキャッシュ
	/// </summary>
#else
	/// <summary>
	/// FieldInfo cache
	/// </summary>
#endif
	public static class FieldCache
	{
		private static Dictionary<System.Type, FieldInfo[]> s_TypeCaches = new Dictionary<System.Type, FieldInfo[]>();

		private static List<FieldInfo> s_FieldBuilder = new List<FieldInfo>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 型に定義されているフィールドを取得する。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>型に定義されているフィールド</returns>
#else
		/// <summary>
		/// Get the fields defined in the type.
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Fields defined in the type</returns>
#endif
		public static FieldInfo[] GetFields(System.Type type)
		{
			FieldInfo[] fields = null;
			if (!s_TypeCaches.TryGetValue(type, out fields))
			{
				fields = CreateFields(type);
				s_TypeCaches.Add(type, fields);
			}

			return fields;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// キャッシュをクリアする。
		/// </summary>
#else
		/// <summary>
		/// Cache clear
		/// </summary>
#endif
		public static void Clear()
		{
			s_TypeCaches.Clear();
		}

		static FieldInfo[] CreateFields(System.Type type)
		{
			var fields = TypeUtility.GetFields(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
			for (int i = 0, count = fields.Length; i < count; i++)
			{
				var fieldInfo = fields[i];

				if (!SerializationUtility.IsSerializableField(fieldInfo))
				{
					continue;
				}

				s_FieldBuilder.Add(fieldInfo);
			}

			var serializeFields = s_FieldBuilder.ToArray();
			s_FieldBuilder.Clear();

			return serializeFields;
		}
	}
}