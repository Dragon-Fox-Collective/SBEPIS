//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
//#define ARBOR_CLASSLIST_DEBUG

using UnityEditor;
using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CompilationPipeline = UnityEditor.Compilation.CompilationPipeline;
using CompilationAssembly = UnityEditor.Compilation.Assembly;
using AssemblyFlags = UnityEditor.Compilation.AssemblyFlags;

namespace ArborEditor
{
	using Arbor;

	[InitializeOnLoad]
	public static class ClassList
	{
		private const bool kUseThread = true;

		public static ReadOnlyCollection<Assembly> runtimeAssemblies
		{
			get;
			private set;
		}

		static HashSet<Assembly> _Assemblies = new HashSet<Assembly>();
		static Dictionary<string, NamespaceItem> _NamespaceDic = new Dictionary<string, NamespaceItem>();
		static Dictionary<AssemblyName, Assembly> _AssemblieDic = new Dictionary<AssemblyName, Assembly>();

		static List<NamespaceItem> _Namespaces = new List<NamespaceItem>();
		static List<TypeItem> _TypeItems = new List<TypeItem>();

		static Thread _CreateMethodThread = null;

		enum BuildStatus
		{
			None,
			DelayBuild,
			Building,
			BuildingForce,
			Ready,
			Canceling,
		}

		private static volatile BuildStatus _BuildStatus = BuildStatus.None;

		public static bool isReady
		{
			get
			{
				if (_BuildStatus == BuildStatus.DelayBuild)
				{
					_BuildStatus = BuildStatus.BuildingForce;
					Build();
				}
				return _BuildStatus == BuildStatus.Ready;
			}
		}

		public static int namespaceCount
		{
			get
			{
				return _Namespaces.Count;
			}
		}

		static ClassList()
		{
			_BuildStatus = BuildStatus.DelayBuild;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			switch (_BuildStatus)
			{
				case BuildStatus.DelayBuild:
					if (!EditorApplication.isPlayingOrWillChangePlaymode)
					{
						_BuildStatus = BuildStatus.Building;
						Build();
					}
					break;
				case BuildStatus.Building:
					if (EditorApplication.isPlayingOrWillChangePlaymode)
					{
						_BuildStatus = BuildStatus.Canceling;
					}
					break;
				case BuildStatus.Ready:
					{
						EditorApplication.update -= OnUpdate;
					}
					break;
			}
		}

		#region Build

		static HashSet<string> s_IgnoreAssemblies = new HashSet<string>()
		{
			"Boo.Lang",
			"Boo.Lang.Compiler",
			"Boo.Lang.Parser",
			"ExCSS.Unity",
			"I18N",
			"I18N.CJK",
			"ICSharpCode.NRefactory",
			"Mono.Cecil",
			"Mono.Data.Tds",
			"Mono.Security",
			"nunit.core",
			"nunit.core.interfaces",
			"nunit.framework",
			"UnityEditor.iOS.Extensions.Xcode",
			"UnityScript",
			"UnityScript.Lang",
			"Unity.Cecil",
			"Unity.CompilationPipeline.Common",
			"Unity.DataContract",
			"Unity.IvyParser",
			"Unity.Legacy.NRefactory",
			"Unity.Locator",
			"Unity.SerializationLogic",
			"SyntaxTree.VisualStudio.Unity.Messaging",
			"System.Configuration",
			"System.Transactions",
		};

		static bool IsValidPlugin(PluginImporter pluginImporter)
		{
			string extension = System.IO.Path.GetExtension(pluginImporter.assetPath).ToLower();
			if (extension != ".dll")
			{
				return false;
			}

			if (pluginImporter.isNativePlugin)
			{
				return false;
			}

			if (pluginImporter.GetCompatibleWithAnyPlatform())
			{
				return true;
			}

			if (!pluginImporter.GetCompatibleWithEditor())
			{
				return true;
			}

			BuildTarget[] buildTargets = EnumUtility.GetValues<BuildTarget>();
			for (int i = 0; i < buildTargets.Length; i++)
			{
				BuildTarget target = buildTargets[i];
				if (target <= 0)
				{
					continue;
				}

				if (pluginImporter.GetCompatibleWithPlatform(target))
				{
					return true;
				}
			}

			return false;
		}

		static bool IsValidAssembly(Assembly a)
		{
			if (a == null || a.IsDynamic)
			{
				return false;
			}

			string assemblyName = a.GetName().Name;

			if (s_IgnoreAssemblies.Contains(assemblyName))
			{
				return false;
			}

			string path = PathUtility.ChangeDirectorySeparator(a.Location);
			PluginImporter importer = AssetImporter.GetAtPath(path) as PluginImporter;
			if (importer == null)
			{
				path = FileUtil.GetProjectRelativePath(path);
				if (!string.IsNullOrEmpty(path))
				{
					importer = AssetImporter.GetAtPath(path) as PluginImporter;
				}
			}
			if (importer != null)
			{
				if (!IsValidPlugin(importer))
				{
					return false;
				}
			}
			else
			{
				if (!TypeUtility.IsRuntimeAssembly(a))
				{
					return false;
				}
			}

			return true;
		}

#if ARBOR_CLASSLIST_DEBUG
		static System.Text.StringBuilder s_sb = new System.Text.StringBuilder();
		static System.Text.StringBuilder s_sbLoaded = new System.Text.StringBuilder();
#endif

		[System.Diagnostics.Conditional("ARBOR_CLASSLIST_DEBUG")]
		static void DebugBegin()
		{
#if ARBOR_CLASSLIST_DEBUG
			s_sb.Length = 0;
			s_sbLoaded.Length = 0;
#endif
		}

		[System.Diagnostics.Conditional("ARBOR_CLASSLIST_DEBUG")]
		static void Debug(string message)
		{
#if ARBOR_CLASSLIST_DEBUG
			s_sb.AppendLine(message);
#endif
		}

		[System.Diagnostics.Conditional("ARBOR_CLASSLIST_DEBUG")]
		static void Debug(Assembly assembly, bool added)
		{
#if ARBOR_CLASSLIST_DEBUG
			if (added)
			{
				s_sb.AppendLine(assembly.GetName().Name);
			}
			else
			{
				s_sbLoaded.AppendLine(assembly.GetName().Name);
			}
#endif
		}

		[System.Diagnostics.Conditional("ARBOR_CLASSLIST_DEBUG")]
		static void DebugApplyLoaded()
		{
#if ARBOR_CLASSLIST_DEBUG
			if (s_sbLoaded.Length != 0)
			{
				s_sb.AppendLine("[Loaded]");
				s_sb.AppendLine(s_sbLoaded.ToString());
				s_sbLoaded.Length = 0;
			}
			s_sb.AppendLine();
#endif
		}

		[System.Diagnostics.Conditional("ARBOR_CLASSLIST_DEBUG")]
		static void DebugEnd()
		{
#if ARBOR_CLASSLIST_DEBUG
			UnityEngine.Debug.Log(s_sb.ToString());
			s_sb.Length = 0;
#endif
		}

		internal static ReadOnlyCollection<Assembly> GetAssemblies()
		{
			HashSet<Assembly> loadAssemblies = new HashSet<Assembly>();

			DebugBegin(); // Conditional("ARBOR_CLASSLIST_DEBUG")

			Debug("[CompilationPipeline.GetAssemblies]"); // Conditional("ARBOR_CLASSLIST_DEBUG")
			var compilationAssemblies = CompilationPipeline.GetAssemblies();
			for (int assemblyIndex = 0; assemblyIndex < compilationAssemblies.Length; assemblyIndex++)
			{
				CompilationAssembly a = compilationAssemblies[assemblyIndex];
				if (a.flags == AssemblyFlags.EditorAssembly)
				{
					s_IgnoreAssemblies.Add(a.name);
				}
				else
				{
					var assembly = LoadAssemblyFile(a.outputPath);
					if (assembly == null)
					{
						continue;
					}

					bool added = loadAssemblies.Add(assembly);
					Debug(assembly, added); // Conditional("ARBOR_CLASSLIST_DEBUG")
				}
			}
			DebugApplyLoaded(); // Conditional("ARBOR_CLASSLIST_DEBUG")

			Debug("[PluginImporter.GetAllImporters]"); // Conditional("ARBOR_CLASSLIST_DEBUG")
			PluginImporter[] pluginImporters = PluginImporter.GetAllImporters();
			for (int pluginIndex = 0; pluginIndex < pluginImporters.Length; pluginIndex++)
			{
				PluginImporter pluginImporter = pluginImporters[pluginIndex];
				if (!IsValidPlugin(pluginImporter))
				{
					continue;
				}

				Assembly a = LoadAssemblyFile(pluginImporter.assetPath);
				if (a == null)
				{
					continue;
				}

				string assemblyName = a.GetName().Name;

				if (s_IgnoreAssemblies.Contains(assemblyName))
				{
					continue;
				}

				bool added = loadAssemblies.Add(a);
				Debug(a, added); // Conditional("ARBOR_CLASSLIST_DEBUG")
			}
			DebugApplyLoaded(); // Conditional("ARBOR_CLASSLIST_DEBUG")

			Debug("[CompilationPipeline.GetPrecompiledAssemblyPaths]"); // Conditional("ARBOR_CLASSLIST_DEBUG")
			var assemblyPaths = CompilationPipeline.GetPrecompiledAssemblyPaths(
				CompilationPipeline.PrecompiledAssemblySources.UnityEngine |
				CompilationPipeline.PrecompiledAssemblySources.SystemAssembly |
				CompilationPipeline.PrecompiledAssemblySources.UserAssembly);
			for (int pathIndex = 0; pathIndex < assemblyPaths.Length; pathIndex++)
			{
				string path = assemblyPaths[pathIndex];
				var a = LoadAssemblyFile(path);
				if (a == null)
				{
					a = LoadAssembly(System.IO.Path.GetFileName(path));
				}
				if (a == null)
				{
					continue;
				}

				if (!IsValidAssembly(a))
				{
					continue;
				}

				bool added = loadAssemblies.Add(a);
				Debug(a, added); // Conditional("ARBOR_CLASSLIST_DEBUG")
			}
			DebugApplyLoaded(); // Conditional("ARBOR_CLASSLIST_DEBUG")

			DebugEnd(); // Conditional("ARBOR_CLASSLIST_DEBUG")

			List<Assembly> result = new List<Assembly>();
			foreach (var a in loadAssemblies)
			{
				result.Add(a);
			}

			return result.AsReadOnly();
		}

		static void Build()
		{
			runtimeAssemblies = GetAssemblies();
			
			BeginCreateList();
		}

		static void BeginCreateList()
		{
#pragma warning disable 0162
			if (kUseThread)
			{
				_CreateMethodThread = new Thread(new ThreadStart(CreateMethodList));
				_CreateMethodThread.Start();
			}
			else
			{
				CreateMethodList();
			}
#pragma warning restore 0162
		}

		static Assembly LoadAssembly(string assemblyName)
		{
			try
			{
				return Assembly.Load(assemblyName);
			}
			catch { }

			return null;
		}

		static Assembly LoadAssemblyFile(string path)
		{
			try
			{
				return Assembly.LoadFrom(path);
			}
			catch { }

			return null;
		}

		static bool ListUpTypes(Assembly assembly)
		{
			if (assembly == null)
			{
				return false;
			}

			if (!_Assemblies.Contains(assembly))
			{
				_Assemblies.Add(assembly);

				var types = TypeUtility.GetTypesFromAssembly(assembly);
				for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
				{
					Type type = types[typeIndex];
					if (type.IsVisible && !type.IsSubclassOf(typeof(Attribute)) && !type.IsGenericType && !type.IsNested)
					{
						string Namespace = type.Namespace;
						if (string.IsNullOrEmpty(Namespace))
						{
							Namespace = "<unnamed>";
						}

						NamespaceItem namespaceItem = null;
						if (!_NamespaceDic.TryGetValue(Namespace, out namespaceItem))
						{
							namespaceItem = new NamespaceItem();
							namespaceItem.name = Namespace;

							_NamespaceDic.Add(Namespace, namespaceItem);
							_Namespaces.Add(namespaceItem);
						}

						namespaceItem.typeIndices.Add(AddType(type));
					}

					if (_BuildStatus == BuildStatus.Canceling)
					{
						break;
					}
				}

				return true;
			}

			return false;
		}

		static void ClearAssemblyHashes()
		{
			_Assemblies.Clear();
			_NamespaceDic.Clear();
			_AssemblieDic.Clear();
		}

		static int SortTypeIndices(int lhs, int rhs)
		{
			TypeItem lhsType = _TypeItems[lhs];
			TypeItem rhsType = _TypeItems[rhs];
			return lhsType.CompareTo(rhsType);
		}

		static void CreateMethodList()
		{
			try
			{
				_Assemblies.Clear();
				_Namespaces.Clear();
				_TypeItems.Clear();

				for (int assemblyIndex = 0; assemblyIndex < runtimeAssemblies.Count; assemblyIndex++)
				{
					Assembly assembly = runtimeAssemblies[assemblyIndex];
					ListUpTypes(assembly);

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_Namespaces.Sort();

				for (int namespaceIndex = 0; namespaceIndex < _Namespaces.Count; namespaceIndex++)
				{
					NamespaceItem namespaceItem = _Namespaces[namespaceIndex];
					namespaceItem.typeIndices.Sort(SortTypeIndices);

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_BuildStatus = BuildStatus.Ready;
			}
			finally
			{
				ClearAssemblyHashes();

				GC.Collect();

				if (_BuildStatus == BuildStatus.Canceling)
				{
					_BuildStatus = BuildStatus.DelayBuild;
				}
			}
		}

		static int AddType(Type type)
		{
			_TypeItems.Add(new TypeItem(type));
			return _TypeItems.Count - 1;
		}

		public static TypeItem GetType(int index)
		{
			return _TypeItems[index];
		}

		#endregion

		public static NamespaceItem GetNamespaceItem(int index)
		{
			return _Namespaces[index];
		}

		public static TypeItem GetTypeItem(Type type)
		{
			if (type == null)
			{
				return null;
			}

			int namespaceCount = _Namespaces.Count;
			for (int namespaceIndex = 0; namespaceIndex < namespaceCount; namespaceIndex++)
			{
				NamespaceItem namespaceItem = _Namespaces[namespaceIndex];
				int typeCount = namespaceItem.typeIndices.Count;
				for (int typeIndex = 0; typeIndex < typeCount; typeIndex++)
				{
					TypeItem typeItem = GetType(namespaceItem.typeIndices[typeIndex]);
					TypeItem findTypeItem = typeItem.GetTypeItem(type);
					if (findTypeItem != null)
					{
						return findTypeItem;
					}
				}
			}

			return null;
		}

		#region Item classes

		public class Item : System.IComparable
		{
			public string name;

			public int CompareTo(object obj)
			{
				return name.CompareTo((obj as Item).name);
			}
		}

		public sealed class TypeItem : Item
		{
			private RuntimeTypeHandle _TypeHandle;
			public Type type
			{
				get
				{
					return Type.GetTypeFromHandle(_TypeHandle);
				}
			}

			private List<int> _NestedTypeIndices = new List<int>();
			public List<int> nestedTypeIndices
			{
				get
				{
					return _NestedTypeIndices;
				}
			}

			public TypeItem(Type type)
			{
				this.name = Arbor.TypeUtility.GetTypeName(type);
				_TypeHandle = type.TypeHandle;

				CreateNestedTypes(type);
			}

			void CreateNestedTypes(Type type)
			{
				_NestedTypeIndices.Clear();

				Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public);
				int nestedTypeCount = nestedTypes.Length;
				for (int nestedTypeIndex = 0; nestedTypeIndex < nestedTypeCount; nestedTypeIndex++)
				{
					Type nestedType = nestedTypes[nestedTypeIndex];

					_NestedTypeIndices.Add(AddType(nestedType));

					if (_BuildStatus == BuildStatus.Canceling)
					{
						return;
					}
				}

				_NestedTypeIndices.Sort(SortTypeIndices);
			}

			public TypeItem GetTypeItem(Type type)
			{
				if (type == null)
				{
					return null;
				}

				if (type == this.type)
				{
					return this;
				}

				int nestedTypeCount = _NestedTypeIndices.Count;
				for (int nestedTypeIndex = 0; nestedTypeIndex < nestedTypeCount; nestedTypeIndex++)
				{
					TypeItem nestedType = ClassList.GetType((_NestedTypeIndices[nestedTypeIndex]));
					TypeItem findTypeItem = nestedType.GetTypeItem(type);
					if (findTypeItem != null)
					{
						return findTypeItem;
					}
				}

				return null;
			}
		}

		public sealed class NamespaceItem : Item
		{
			public List<int> typeIndices = new List<int>();
		}

		#endregion
	}
}
