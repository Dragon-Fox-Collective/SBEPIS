//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor.DynamicReflection
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 動的な型のユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Dynamic type utility class
	/// </summary>
#endif
	public static class DynamicUtility
	{
		private static readonly System.Func<object, object> s_MemberwiseClone = null;

		static DynamicUtility()
		{
			MethodInfo memberwiseCloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
			s_MemberwiseClone = (System.Func<object, object>)memberwiseCloneMethod.CreateDelegate(typeof(System.Func<object, object>));
		}

		internal static object CastInternal(object obj, System.Type type)
		{
			if (obj == null || type == null || type == typeof(void))
			{
				return null;
			}

			var objType = obj.GetType();

			if (objType == type)
			{
				return obj;
			}

			if (!TypeUtility.IsValueType(type))
			{
				if (TypeUtility.IsAssignableFrom(type, objType))
				{
					// Upcast
					return obj;
				}
				if (TypeUtility.IsAssignableFrom(objType, type))
				{
					// Downcast failure
					return null;
				}

				// The type is different and cannot be cast.
				throw new System.InvalidCastException();
			}

			if (EnumFieldUtility.IsEnum(type))
			{
				return System.Enum.ToObject(type, obj);
			}

			return System.Convert.ChangeType(obj, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをキャストする。
		/// </summary>
		/// <param name="obj">キャストするオブジェクト</param>
		/// <param name="type">キャストする型</param>
		/// <param name="result">キャストされた値</param>
		/// <returns>キャスト出来た場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Cast the object.
		/// </summary>
		/// <param name="obj">The object to cast</param>
		/// <param name="type">Casting type</param>
		/// <param name="result">Casted value</param>
		/// <returns>Returns true if the cast is successful.</returns>
#endif
		public static bool TryCast(object obj, System.Type type, out object result)
		{
			try
			{
				result = CastInternal(obj, type);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをキャストする。
		/// </summary>
		/// <param name="obj">キャストするオブジェクト</param>
		/// <param name="type">キャストする型</param>
		/// <param name="exceptionMode">例外モード<br />Throw以外の場合、例外発生時にdefaultを返す。</param>
		/// <returns>キャストされた値</returns>
#else
		/// <summary>
		/// Cast the object.
		/// </summary>
		/// <param name="obj">The object to cast</param>
		/// <param name="type">Casting type</param>
		/// <param name="exceptionMode">Exception mode<br />If other than Throw, return default when an exception occurs.</param>
		/// <returns>Casted value</returns>
#endif
		public static object Cast(object obj, System.Type type, ExceptionMode exceptionMode)
		{
			try
			{
				return CastInternal(obj, type);
			}
			catch (System.Exception ex)
			{
				switch (exceptionMode)
				{
					case ExceptionMode.Throw:
						throw;
					case ExceptionMode.Log:
						Debug.Log(ex);
						break;
					case ExceptionMode.Ignore:
						break;
				}

				return GetDefault(type);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをキャストする。
		/// </summary>
		/// <param name="obj">キャストするオブジェクト</param>
		/// <param name="type">キャストする型</param>
		/// <param name="ignoreThrowException">例外を無視するフラグ。<br />trueにした場合は例外発生時に例外メッセージをログに出力しdefaultを返す。</param>
		/// <returns>キャストされた値</returns>
#else
		/// <summary>
		/// Cast the object.
		/// </summary>
		/// <param name="obj">The object to cast</param>
		/// <param name="type">Casting type</param>
		/// <param name="ignoreThrowException">Flag to ignore exceptions.<br />If set to true, it will output an exception message to the log and return default when an exception occurs.</param>
		/// <returns>Casted value</returns>
#endif
		public static object Cast(object obj, System.Type type, bool ignoreThrowException = true)
		{
			return Cast(obj, type, ignoreThrowException ? ExceptionMode.Log : ExceptionMode.Throw);
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
		public static object GetDefault(System.Type type)
		{
			if (type == null || type == typeof(void) || !TypeUtility.IsValueType(type) || System.Nullable.GetUnderlyingType(type) != null)
			{
				return null;
			}
			return System.Activator.CreateInstance(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// objectを再ボックス化し値をコピーする。
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns>再ボックス化したobject</returns>
		/// <remarks>値型ではない場合はコピーされない。</remarks>
#else
		/// <summary>
		/// Rebox object and copy the value.
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns>Reboxed object</returns>
		/// <remarks>If it is not a value type, it is not copied.</remarks>
#endif
		public static object Rebox(object obj)
		{
			if (obj == null)
			{
				return null;
			}

			System.Type type = obj.GetType();

			if (type == null || !TypeUtility.IsValueType(type))
			{
				return obj;
			}

			try
			{
				obj = s_MemberwiseClone(obj);
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex);
			}

			return obj;
		}
	}
}