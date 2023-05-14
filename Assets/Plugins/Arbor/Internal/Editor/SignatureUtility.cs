//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace ArborEditor
{
	public static class SignatureUtility
	{
		static CodeDomProvider provider;
		static CodeNamespaceImportCollection imports;

		static SignatureUtility()
		{
			provider = CodeDomProvider.CreateProvider("c#");
			imports = new CodeNamespaceImportCollection();
			imports.Add(new CodeNamespaceImport("UnityEngine"));
		}

		const string k_ref = "ref ";
		const string k_out = "out ";

		static void ParametersSignature(StringBuilder sigBuilder, MethodBase method)
		{
			bool firstParam = true;

			// Add method generics
			if (method.IsGenericMethod)
			{
				sigBuilder.Append("<");
				Type[] arg = method.GetGenericArguments();
				int argCount = arg.Length;
				for (int argIndex = 0; argIndex < argCount; argIndex++)
				{
					Type g = arg[argIndex];
					if (firstParam)
					{
						firstParam = false;
					}
					else
					{
						sigBuilder.Append(", ");
					}
					sigBuilder.Append(GetTypeName(g));
				}
				sigBuilder.Append(">");
			}

			sigBuilder.Append("(");
			firstParam = true;
			ParameterInfo[] parameters = method.GetParameters();
			int paramCount = parameters.Length;
			for (int paramIndex = 0; paramIndex < paramCount; paramIndex++)
			{
				ParameterInfo param = parameters[paramIndex];
				Type paramType = param.ParameterType;
				if (firstParam)
				{
					firstParam = false;
				}
				else
				{
					sigBuilder.Append(", ");
				}
				if (paramType.IsByRef)
				{
					sigBuilder.Append(k_ref);
				}
				else if (param.IsOut)
				{
					sigBuilder.Append(k_out);
				}
				sigBuilder.Append(GetTypeName(paramType));
				sigBuilder.Append(' ');
				sigBuilder.Append(param.Name);
			}
			sigBuilder.Append(")");
		}

		private static StringBuilder sigBuilder = new StringBuilder();

		public static string GetSignature(ConstructorInfo constructor)
		{
			sigBuilder.Append(constructor.ReflectedType.Name);

			ParametersSignature(sigBuilder, constructor);

			string sig = sigBuilder.ToString();
			sigBuilder.Length = 0;

			return sig;
		}

		/// <summary>
		/// Return the method signature as a string.
		/// </summary>
		/// <param name="method">The Method</param>
		/// <returns>Method signature</returns>
		public static string GetSignature(MethodInfo method)
		{
			sigBuilder.Append(method.Name);

			ParametersSignature(sigBuilder, method);

			string sig = sigBuilder.ToString();
			sigBuilder.Length = 0;

			return sig;
		}

		static Dictionary<RuntimeTypeHandle, string> _TypeNames = new Dictionary<RuntimeTypeHandle, string>();

		/// <summary>
		/// Get full type name with full namespace names
		/// </summary>
		/// <param name="type">Type. May be generic or nullable</param>
		/// <returns>Full type name, fully qualified namespaces</returns>
		public static string GetTypeName(Type type)
		{
			string typeName = string.Empty;
			RuntimeTypeHandle typeHandle = type.TypeHandle;
			if (_TypeNames.TryGetValue(typeHandle, out typeName))
			{
				return typeName;
			}

			Type nullableType = Nullable.GetUnderlyingType(type);
			if (nullableType != null)
			{
				CodeTypeReference typeRef = new CodeTypeReference(nullableType);
				Shortify(typeRef, type);
				typeName = provider.GetTypeOutput(typeRef) + "?";
			}
			else
			{
				CodeTypeReference typeRef = new CodeTypeReference(type);
				Shortify(typeRef, type);
				typeName = provider.GetTypeOutput(typeRef);
			}

			_TypeNames.Add(typeHandle, typeName);

			return typeName;
		}

		private static void Shortify(CodeTypeReference typeReference, Type type)
		{
			if (typeReference.ArrayRank > 0)
			{
				Shortify(typeReference.ArrayElementType, type);
				return;
			}

			if (type.Namespace != null &&
				imports.Cast<CodeNamespaceImport>().Any(cni => cni.Namespace == type.Namespace))
			{
				string prefix = type.Namespace + '.';

				if (prefix != null)
				{
					int pos = typeReference.BaseType.IndexOf(prefix, StringComparison.Ordinal);
					if (pos == 0)
					{
						typeReference.BaseType = typeReference.BaseType.Substring(prefix.Length);
					}
				}
			}
		}
	}
}
