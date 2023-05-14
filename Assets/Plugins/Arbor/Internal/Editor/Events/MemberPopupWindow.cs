//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;
	using ArborEditor.IMGUI.Controls;

	internal sealed class MemberPopupWindow : TreePopupWindow<MemberInfo>, ITreeFilter
	{
		public const string kNoFunctionText = "No Function";

		private static Dictionary<System.Type, List<MemberInfo>> s_MembersCache = new Dictionary<System.Type, List<MemberInfo>>();

		private System.Type _Type;

		private UniqueIDGenerator _UniqueIDGenerator = new UniqueIDGenerator();

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.methodSearch;
			}
			set
			{
				ArborEditorCache.methodSearch = value;
			}
		}

		internal static void Open(Rect buttonRect, int controlID, MemberInfo selected, System.Type type, bool hasFilter, MemberFilterFlags memberFlags)
		{
			MemberPopupWindow window = s_Instance as MemberPopupWindow;
			if (window == null)
			{
				window = ScriptableObject.CreateInstance<MemberPopupWindow>();
				s_Instance = window;
			}
			window.Init(buttonRect, controlID, selected, type, hasFilter, memberFlags);
		}

		static int BitCount(int flags)
		{
			var x = flags - ((flags >> 1) & 0x55555555);
			x = ((x >> 2) & 0x33333333) + (x & 0x33333333);
			x = (x >> 4) + x & 0x0f0f0f0f;
			x += x >> 8;
			return (x >> 16) + x & 0xff;
		}

		void Init(Rect buttonRect, int controlID, MemberInfo selected, System.Type type, bool hasFilter, MemberFilterFlags memberFlags)
		{
			_Type = type;
			_HasFilter = hasFilter;
			_UseFilter = hasFilter && BitCount((int)memberFlags) > 1;
			if (ArborEditorCache.memberFilterMask != memberFlags)
			{
				ArborEditorCache.memberFilterMask = memberFlags;
				ArborEditorCache.memberFilterFlags = memberFlags;
			}

			Init(buttonRect, controlID, selected);
		}

		private sealed class MemberComparer : IComparer<MemberInfo>
		{
			private System.Type _Type;

			private Dictionary<System.Type, int> _Depth = new Dictionary<System.Type, int>();

			public MemberComparer(System.Type type)
			{
				_Type = type;
			}

			int GetBaseDepth(System.Type searchType)
			{
				int depth = 0;

				if (_Depth.TryGetValue(searchType, out depth))
				{
					return depth;
				}

				System.Type baseType = _Type;

				while (baseType != null && searchType != baseType)
				{
					depth++;
					baseType = baseType.BaseType;
				}

				_Depth.Add(searchType, depth);

				return depth;
			}

			static int Order(MemberInfo memberInfo)
			{
				if (memberInfo is MethodInfo)
				{
					return 0;
				}
				else if (memberInfo is FieldInfo)
				{
					return 1;
				}
				else if (memberInfo is PropertyInfo)
				{
					return 2;
				}
				return -1;

			}

			public int Compare(MemberInfo m1, MemberInfo m2)
			{
				System.Type t1 = m1.DeclaringType;
				System.Type t2 = m2.DeclaringType;

				if (t1 == t2)
				{
					int order1 = Order(m1);
					int order2 = Order(m2);
					if (order1 != order2)
					{
						return order1.CompareTo(order2);
					}

					return m1.Name.CompareTo(m2.Name);
				}

				int depth1 = GetBaseDepth(t1);
				int depth2 = GetBaseDepth(t2);

				return depth1.CompareTo(depth2);
			}
		}

		TreeViewValueItem<MemberInfo> CreateMemberTree(MemberInfo memberInfo, string name)
		{
			using (new ProfilerScope(name))
			{
				if (memberInfo != null && _HasFilter && !IsValidMember(memberInfo, ArborEditorCache.memberFilterMask))
				{
					return null;
				}

				int id = _UniqueIDGenerator.CreateID();
				TreeViewValueItem<MemberInfo> valueElement = new TreeViewValueItem<MemberInfo>(id, name, memberInfo, Icons.GetMemberIcon(memberInfo) as Texture2D);

				if (selectedValue == memberInfo)
				{
					SetSelectedItem(valueElement, true);
					SetExpanded(valueElement, true);
				}

				return valueElement;
			}
		}

		bool IsSelectableMember(MemberInfo memberInfo)
		{
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return ArborEventUtility.IsSelectableMethod(methodInfo);
			}

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return ArborEventUtility.IsSelectableField(fieldInfo);
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return ArborEventUtility.IsSelectableProperty(propertyInfo);
			}

			return false;
		}

		protected override void OnCreateTree(TreeViewItem root)
		{
			_TreeView.noneValueItem = CreateMemberTree(null, kNoFunctionText);
			root.AddChild(_TreeView.noneValueItem);

			List<MemberInfo> members;
			if (!s_MembersCache.TryGetValue(_Type, out members))
			{
				members = _Type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Where(IsSelectableMember).ToList();

				MemberComparer comparer = new MemberComparer(_Type);
				members.Sort(comparer);

				s_MembersCache.Add(_Type, members);
			}

			System.Type currentDeclaringType = null;
			TreeViewItem groupItem = null;
			bool addedGroup = false;

			bool isStatic = _Type.IsAbstract && _Type.IsSealed;

			int memberCount = members.Count;
			for (int memberIndex = 0; memberIndex < memberCount; memberIndex++)
			{
				MemberInfo memberInfo = members[memberIndex];

				System.Type declaringType = memberInfo.DeclaringType;

				if (isStatic && declaringType != _Type)
				{
					continue;
				}

				if (currentDeclaringType == null || groupItem == null || currentDeclaringType != declaringType)
				{
					currentDeclaringType = declaringType;

					int id = _UniqueIDGenerator.CreateID();
					groupItem = new TreeViewItem(id, TypeUtility.GetTypeName(currentDeclaringType), Icons.GetTypeIcon(currentDeclaringType) as Texture2D);
					addedGroup = false;
				}

				TreeViewValueItem<MemberInfo> methodElement = CreateMemberTree(memberInfo, ArborEventUtility.GetMemberName(memberInfo));
				if (methodElement != null)
				{
					if (IsExpanded(methodElement))
					{
						SetExpanded(groupItem, true);
					}
					groupItem.AddChild(methodElement);
					if (!addedGroup)
					{
						root.AddChild(groupItem);
						addedGroup = true;
					}
				}
			}
		}

		private bool _HasFilter;
		private bool _UseFilter;

		bool ITreeFilter.useFilter
		{
			get
			{
				return _UseFilter;
			}
		}

		bool ITreeFilter.openFilter
		{
			get
			{
				return ArborEditorCache.memberFilterEnable;
			}
			set
			{
				ArborEditorCache.memberFilterEnable = value;
			}
		}

		private sealed class FilterMenu
		{
			public MemberFilterFlags flags;
			public GUIContent content;
		};

		private static readonly FilterMenu[] s_FilterMenu =
		{
			new FilterMenu()
			{
				flags = MemberFilterFlags.Method,
				content = new GUIContent("Method"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.Field,
				content = new GUIContent("Field"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.ReadOnlyField,
				content = new GUIContent("ReadOnlyField"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.GetProperty,
				content = new GUIContent("GetProperty"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.SetProperty,
				content = new GUIContent("SetProperty"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.Instance,
				content = new GUIContent("Instance"),
			},
			new FilterMenu()
			{
				flags = MemberFilterFlags.Static,
				content = new GUIContent("Static"),
			},
		};

		VisualElement ITreeFilter.CreateFilterSettingsElement()
		{
			VisualElement element = new VisualElement()
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					flexWrap = Wrap.Wrap,
				},
			};

			for (int filterMenuIndex = 0; filterMenuIndex < s_FilterMenu.Length; filterMenuIndex++)
			{
				FilterMenu filterMenu = s_FilterMenu[filterMenuIndex];
				MemberFilterFlags flags = filterMenu.flags;
				if ((ArborEditorCache.memberFilterMask & flags) == 0)
				{
					continue;
				}

				bool isFlag = (ArborEditorCache.memberFilterFlags & flags) != 0;
				var toggle = new Toggle()
				{
					text = filterMenu.content.text,
				};
				toggle.SetValueWithoutNotify(isFlag);
				toggle.RegisterValueChangedCallback(e =>
				{
					if (e.newValue)
					{
						ArborEditorCache.memberFilterFlags |= flags;
					}
					else
					{
						ArborEditorCache.memberFilterFlags &= ~flags;
					}
					RebuildSearch();
				});

				element.Add(toggle);
			}

			return element;
		}

		static bool IsValidMember(MemberInfo value, MemberFilterFlags memberFilterFlags)
		{
			if (ArborEventUtility.IsStatic(value))
			{
				if ((memberFilterFlags & MemberFilterFlags.Static) == 0)
				{
					return false;
				}
			}
			else if ((memberFilterFlags & MemberFilterFlags.Instance) == 0)
			{
				return false;
			}

			if (value.MemberType == MemberTypes.Method)
			{
				return (memberFilterFlags & MemberFilterFlags.Method) != 0;
			}
			else if (value.MemberType == MemberTypes.Field)
			{
				FieldInfo fieldInfo = value as FieldInfo;
				if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
				{
					return (memberFilterFlags & MemberFilterFlags.ReadOnlyField) != 0;
				}

				return (memberFilterFlags & MemberFilterFlags.Field) != 0;
			}
			else if (value.MemberType == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = value as PropertyInfo;
				if (ArborEventUtility.IsGetProperty(propertyInfo))
				{
					if ((memberFilterFlags & MemberFilterFlags.GetProperty) != 0)
					{
						return true;
					}
				}
				if (ArborEventUtility.IsSetProperty(propertyInfo))
				{
					if ((memberFilterFlags & MemberFilterFlags.SetProperty) != 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		bool ITreeFilter.IsValid(TreeViewItem item)
		{
			var valueItem = item as TreeViewValueItem<MemberInfo>;
			if (valueItem == null)
			{
				return false;
			}

			return IsValidMember(valueItem.value, ArborEditorCache.memberFilterFlags);
		}
	}
}
