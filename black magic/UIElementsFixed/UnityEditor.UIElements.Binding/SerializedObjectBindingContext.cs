using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements.Bindings;

internal class SerializedObjectBindingContext
{
	private class TrackedValue
	{
		public uint contentHash;

		public SerializedProperty prop;

		public Action<object, SerializedProperty> callback;

		public object cookie;

		public bool isValid;

		public TrackedValue(SerializedProperty property, Action<object, SerializedProperty> cb)
		{
			contentHash = property.contentHash;
			prop = property.Copy();
			callback = cb;
		}

		public void Update(SerializedObjectBindingContext context)
		{
			uint num = prop.contentHash;
			if (contentHash != num)
			{
				callback(cookie, prop);
			}
		}
	}

	private class TrackedValues
	{
		private List<TrackedValue> m_List = new List<TrackedValue>();

		public void Add(SerializedProperty prop, object cookie, Action<object, SerializedProperty> callback)
		{
			TrackedValue trackedValue = new TrackedValue(prop, callback);
			trackedValue.cookie = cookie;
			m_List.Add(trackedValue);
		}

		public void Remove(SerializedProperty prop, object cookie)
		{
			for (int num = m_List.Count - 1; num >= 0; num--)
			{
				TrackedValue trackedValue = m_List[num];
				if (trackedValue.cookie == cookie && SerializedProperty.EqualContents(prop, trackedValue.prop))
				{
					m_List.RemoveAt(num);
				}
			}
		}

		public void Update(SerializedObjectBindingContext context)
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				TrackedValue trackedValue = m_List[i];
				if (trackedValue.isValid)
				{
					trackedValue.Update(context);
				}
			}
		}

		public void UpdateValidProperties(HashSet<int> validPropertyPaths)
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				TrackedValue trackedValue = m_List[i];
				trackedValue.isValid = SerializedPropertyHelper.IsPropertyValidFaster(validPropertyPaths, trackedValue.prop);
			}
		}
	}

	private bool m_DelayBind = false;

	private long m_BindingOperationStartTimeMs;

	private const int k_MaxBindingTimeMs = 50;

	private static readonly string k_EnabledOverrideSet = "EnabledSetByBindings";

	private static readonly PropertyName FindContextPropertyKey = "__UnityBindingContext";

	private TrackedValues m_ValueTracker;

	private static HashSet<int> m_ValidPropertyPaths;

	private HashSet<SerializedObjectBindingBase> m_RegisteredBindings = new HashSet<SerializedObjectBindingBase>();

	public ulong lastRevision { get; private set; }

	public SerializedObject serializedObject { get; private set; }

	private bool wasUpdated { get; set; }

	public SerializedObjectBindingContext(SerializedObject so)
	{
		serializedObject = so;
		lastRevision = so.objectVersion;
	}

	public void Bind(VisualElement element)
	{
		element.SetProperty(FindContextPropertyKey, this);
		ContinueBinding(element, null);
	}

	internal void ContinueBinding(VisualElement element, SerializedProperty parentProperty)
	{
		try
		{
			m_BindingOperationStartTimeMs = Panel.TimeSinceStartupMs();
			m_DelayBind = false;
			BindTree(element, parentProperty);
		}
		finally
		{
			m_DelayBind = false;
		}
	}

	private bool ShouldDelayBind()
	{
		if (!m_DelayBind && !VisualTreeBindingsUpdater.disableBindingsThrottling)
		{
			m_DelayBind = Panel.TimeSinceStartupMs() - m_BindingOperationStartTimeMs > 50;
		}
		return m_DelayBind;
	}

	public void Unbind(VisualElement element)
	{
		object property = element.GetProperty(FindContextPropertyKey);
		if (property != null)
		{
			if (property != this)
			{
				return;
			}
			element.SetProperty(FindContextPropertyKey, null);
		}
		RemoveBinding(element as IBindable);
		int childCount = element.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Unbind(element.hierarchy[i]);
		}
	}

	private static bool SendBindingEvent<TEventType>(TEventType evt, VisualElement target) where TEventType : EventBase<TEventType>, new()
	{
		evt.target = target;
		target.HandleEventAtTargetAndDefaultPhase(evt);
		return evt.isPropagationStopped;
	}

	internal void BindTree(VisualElement element, SerializedProperty parentProperty)
	{
		if (ShouldDelayBind())
		{
			DefaultSerializedObjectBindingImplementation.BindingRequest req = DefaultSerializedObjectBindingImplementation.BindingRequest.CreateDelayBinding(this, parentProperty);
			VisualTreeBindingsUpdater.AddBindingRequest(element, req);
			return;
		}
		IBindable bindable = element as IBindable;
		if (element.HasEventCallbacksOrDefaultActions(EventBase<SerializedObjectBindEvent>.EventCategory))
		{
			using SerializedObjectBindEvent evt = SerializedObjectBindEvent.GetPooled(serializedObject);
			if (SendBindingEvent(evt, element))
			{
				return;
			}
		}
		if (bindable != null && !string.IsNullOrEmpty(bindable.bindingPath))
		{
			SerializedProperty serializedProperty = BindPropertyRelative(bindable, parentProperty);
			if (serializedProperty != null)
			{
				parentProperty = serializedProperty;
			}
		}
		int childCount = element.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			BindTree(element.hierarchy[i], parentProperty);
		}
	}

	private static void SyncEditableState(VisualElement fieldElement, bool shouldBeEditable)
	{
		if (fieldElement.enabledSelf == shouldBeEditable)
		{
			return;
		}
		if (shouldBeEditable)
		{
			if (fieldElement.GetProperty(k_EnabledOverrideSet) != null)
			{
				fieldElement.SetEnabled(value: true);
			}
		}
		else
		{
			fieldElement.SetProperty(k_EnabledOverrideSet, fieldElement.enabledSelf);
			fieldElement.SetEnabled(value: false);
		}
	}

	internal SerializedProperty BindPropertyRelative(IBindable field, SerializedProperty parentProperty)
	{
		bool unsafeMode = parentProperty?.unsafeMode ?? false;
		if (parentProperty != null)
		{
			parentProperty.unsafeMode = true;
		}
		SerializedProperty serializedProperty = parentProperty?.FindPropertyRelative(field.bindingPath);
		if (parentProperty != null)
		{
			parentProperty.unsafeMode = unsafeMode;
		}
		if (serializedProperty == null)
		{
			serializedProperty = serializedObject?.FindProperty(field.bindingPath);
		}
		RemoveBinding(field);
		VisualElement visualElement = field as VisualElement;
		if (serializedProperty == null || visualElement == null)
		{
			return serializedProperty;
		}
		SyncEditableState(visualElement, serializedProperty.editable);
		if (visualElement.HasEventCallbacksOrDefaultActions(EventBase<SerializedPropertyBindEvent>.EventCategory))
		{
			using SerializedPropertyBindEvent evt = SerializedPropertyBindEvent.GetPooled(serializedProperty);
			if (SendBindingEvent(evt, visualElement))
			{
				return serializedProperty;
			}
		}
		CreateBindingObjectForProperty(visualElement, serializedProperty);
		return serializedProperty;
	}

	private void CreateBindingObjectForProperty(VisualElement element, SerializedProperty prop)
	{
		if (element is Foldout)
		{
			Foldout field = element as Foldout;
			SerializedObjectBinding<bool>.CreateBind(field, this, prop, (SerializedProperty p) => p.isExpanded, delegate(SerializedProperty p, bool v)
			{
				p.isExpanded = v;
			}, SerializedPropertyHelper.ValueEquals);
			return;
		}
		if (element is Label && element.GetProperty(PropertyField.foldoutTitleBoundLabelProperty) != null)
		{
			Label field2 = element as Label;
			SerializedObjectBinding<string>.CreateBind(field2, this, prop, (SerializedProperty p) => p.localizedDisplayName, delegate
			{
			}, SerializedPropertyHelper.ValueEquals<string>);
			return;
		}
		if (element is ListView)
		{
			BindListView(element as ListView, prop);
			return;
		}
		switch (prop.propertyType)
		{
		case SerializedPropertyType.Integer:
			if (prop.type == "long" || prop.type == "ulong")
			{
				if (element is INotifyValueChanged<long> || element is INotifyValueChanged<string>)
				{
					DefaultBind(element, prop, SerializedPropertyHelper.GetLongPropertyValue, SerializedPropertyHelper.SetLongPropertyValue, SerializedPropertyHelper.ValueEquals);
				}
				else if (element is INotifyValueChanged<ulong>)
				{
					DefaultBind(element, prop, (SerializedProperty x) => (ulong)SerializedPropertyHelper.GetLongPropertyValue(x), delegate(SerializedProperty x, ulong v)
					{
						SerializedPropertyHelper.SetLongPropertyValue(x, (long)v);
					}, (ulong a, SerializedProperty b, Func<SerializedProperty, ulong> c) => SerializedPropertyHelper.ValueEquals((long)a, b, (SerializedProperty x) => (long)c(x)));
				}
				else if (element is INotifyValueChanged<int>)
				{
					DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValue, SerializedPropertyHelper.SetIntPropertyValue, SerializedPropertyHelper.ValueEquals);
				}
				else if (element is INotifyValueChanged<float>)
				{
					DefaultBind(element, prop, SerializedPropertyHelper.GetLongPropertyValueAsFloat, SerializedPropertyHelper.SetFloatPropertyValue, SerializedPropertyHelper.ValueEquals);
				}
			}
			else if (element is INotifyValueChanged<int> || element is INotifyValueChanged<string>)
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValue, SerializedPropertyHelper.SetIntPropertyValue, SerializedPropertyHelper.ValueEquals);
			}
			else if (element is INotifyValueChanged<long>)
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetLongPropertyValue, SerializedPropertyHelper.SetLongPropertyValue, SerializedPropertyHelper.ValueEquals);
			}
			else if (element is INotifyValueChanged<float>)
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValueAsFloat, SerializedPropertyHelper.SetFloatPropertyValue, SerializedPropertyHelper.ValueEquals);
			}
			break;
		case SerializedPropertyType.Boolean:
			DefaultBind(element, prop, SerializedPropertyHelper.GetBoolPropertyValue, SerializedPropertyHelper.SetBoolPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Float:
			if (prop.type == "float")
			{
				if (element is INotifyValueChanged<double>)
				{
					DefaultBind(element, prop, SerializedPropertyHelper.GetFloatPropertyValueAsDouble, SerializedPropertyHelper.SetFloatPropertyValueFromDouble, SerializedPropertyHelper.ValueEquals);
				}
				else
				{
					DefaultBind(element, prop, SerializedPropertyHelper.GetFloatPropertyValue, SerializedPropertyHelper.SetFloatPropertyValue, SerializedPropertyHelper.ValueEquals);
				}
			}
			else if (element is INotifyValueChanged<float>)
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetDoublePropertyValueAsFloat, SerializedPropertyHelper.SetDoublePropertyValueFromFloat, SerializedPropertyHelper.ValueEquals);
			}
			else
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetDoublePropertyValue, SerializedPropertyHelper.SetDoublePropertyValue, SerializedPropertyHelper.ValueEquals);
			}
			break;
		case SerializedPropertyType.String:
			DefaultBind(element, prop, SerializedPropertyHelper.GetStringPropertyValue, SerializedPropertyHelper.SetStringPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Color:
			DefaultBind(element, prop, SerializedPropertyHelper.GetColorPropertyValue, SerializedPropertyHelper.SetColorPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.ObjectReference:
			DefaultBind(element, prop, SerializedPropertyHelper.GetObjectRefPropertyValue, SerializedPropertyHelper.SetObjectRefPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		/*case SerializedPropertyType.ManagedReference:
			DefaultBind(element, prop, SerializedPropertyHelper.GetManagedRefPropertyValue, SerializedPropertyHelper.SetManagedRefPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.ExposedReference:
			DefaultBind(element, prop, SerializedPropertyHelper.GetExposedRefPropertyValue, SerializedPropertyHelper.SetExposedRefPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;*/
		case SerializedPropertyType.Generic:
			DefaultBind(element, prop, SerializedPropertyHelper.GetGenericPropertyValue, SerializedPropertyHelper.SetGenericPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.LayerMask:
			DefaultBind(element, prop, SerializedPropertyHelper.GetLayerMaskPropertyValue, SerializedPropertyHelper.SetLayerMaskPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Enum:
			CreateEnumBindingObject(element, prop);
			break;
		case SerializedPropertyType.Vector2:
			DefaultBind(element, prop, SerializedPropertyHelper.GetVector2PropertyValue, SerializedPropertyHelper.SetVector2PropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Vector3:
			DefaultBind(element, prop, SerializedPropertyHelper.GetVector3PropertyValue, SerializedPropertyHelper.SetVector3PropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Vector4:
			DefaultBind(element, prop, SerializedPropertyHelper.GetVector4PropertyValue, SerializedPropertyHelper.SetVector4PropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Rect:
			DefaultBind(element, prop, SerializedPropertyHelper.GetRectPropertyValue, SerializedPropertyHelper.SetRectPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.ArraySize:
			DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValue, SerializedPropertyHelper.SetIntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.AnimationCurve:
			DefaultBind(element, prop, SerializedPropertyHelper.GetAnimationCurvePropertyValue, SerializedPropertyHelper.SetAnimationCurvePropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Bounds:
			DefaultBind(element, prop, SerializedPropertyHelper.GetBoundsPropertyValue, SerializedPropertyHelper.SetBoundsPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Gradient:
			DefaultBind(element, prop, SerializedPropertyHelper.GetGradientPropertyValue, SerializedPropertyHelper.SetGradientPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Quaternion:
			DefaultBind(element, prop, SerializedPropertyHelper.GetQuaternionPropertyValue, SerializedPropertyHelper.SetQuaternionPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.FixedBufferSize:
			DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValue, SerializedPropertyHelper.SetIntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Vector2Int:
			DefaultBind(element, prop, SerializedPropertyHelper.GetVector2IntPropertyValue, SerializedPropertyHelper.SetVector2IntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Vector3Int:
			DefaultBind(element, prop, SerializedPropertyHelper.GetVector3IntPropertyValue, SerializedPropertyHelper.SetVector3IntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.RectInt:
			DefaultBind(element, prop, SerializedPropertyHelper.GetRectIntPropertyValue, SerializedPropertyHelper.SetRectIntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.BoundsInt:
			DefaultBind(element, prop, SerializedPropertyHelper.GetBoundsIntPropertyValue, SerializedPropertyHelper.SetBoundsIntPropertyValue, SerializedPropertyHelper.ValueEquals);
			break;
		case SerializedPropertyType.Character:
			if (element is INotifyValueChanged<string>)
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetCharacterPropertyValueAsString, SerializedPropertyHelper.SetCharacterPropertyValueFromString, SerializedPropertyHelper.ValueEquals);
			}
			else
			{
				DefaultBind(element, prop, SerializedPropertyHelper.GetCharacterPropertyValue, SerializedPropertyHelper.SetCharacterPropertyValue, SerializedPropertyHelper.ValueEquals);
			}
			break;
		default:
			Debug.LogWarning($"Binding is not supported for {prop.type} properties \"{prop.propertyPath}\"");
			break;
		}
	}

	private void DefaultBind<TValue>(VisualElement element, SerializedProperty prop, Func<SerializedProperty, TValue> propertyReadFunc, Action<SerializedProperty, TValue> propertyWriteFunc, Func<TValue, SerializedProperty, Func<SerializedProperty, TValue>, bool> valueComparerFunc)
	{
		INotifyValueChanged<TValue> notifyValueChanged = element as INotifyValueChanged<TValue>;
		if (element is INotifyValueChanged<string> && typeof(TValue) != typeof(string))
		{
			SerializedObjectStringConversionBinding<TValue>.CreateBind(element as INotifyValueChanged<string>, this, prop, propertyReadFunc, propertyWriteFunc, valueComparerFunc);
		}
		else if (notifyValueChanged != null)
		{
			SerializedObjectBinding<TValue>.CreateBind(notifyValueChanged, this, prop, propertyReadFunc, propertyWriteFunc, valueComparerFunc);
		}
		else if (prop.propertyType == SerializedPropertyType.Generic)
		{
			Debug.LogWarning($"Field type {element.GetType().FullName} is not compatible with {prop.type} ({prop.propertyType}) property \"{prop.propertyPath}\" (Generic property fields should be of type System.Object, not {prop.type})");
		}
		else
		{
			Debug.LogWarning($"Field type {element.GetType().FullName} is not compatible with {prop.type} ({prop.propertyType}) property \"{prop.propertyPath}\"");
		}
	}

	private void CreateEnumBindingObject(VisualElement element, SerializedProperty prop)
	{
		if (element is PopupField<string>)
		{
			SerializedDefaultEnumBinding.CreateBind((PopupField<string>)element, this, prop);
		}
		else if (element is EnumFlagsField || element is EnumField)
		{
			SerializedManagedEnumBinding.CreateBind((BaseField<Enum>)element, this, prop);
		}
		else if (element is INotifyValueChanged<int>)
		{
			DefaultBind(element, prop, SerializedPropertyHelper.GetIntPropertyValue, SerializedPropertyHelper.SetIntPropertyValue, SerializedPropertyHelper.ValueEquals);
		}
		else
		{
			DefaultBind(element, prop, SerializedPropertyHelper.GetEnumPropertyValueAsString, SerializedPropertyHelper.SetEnumPropertyValueFromString, SerializedPropertyHelper.SlowEnumValueEquals);
		}
	}

	private bool BindListView(ListView listView, SerializedProperty prop)
	{
		if (prop.propertyType == SerializedPropertyType.Generic)
		{
			SerializedProperty serializedProperty = prop.FindPropertyRelative("Array.size");
			if (serializedProperty == null)
			{
				Debug.LogWarning($"Binding ListView failed: can't find array size for property \"{prop.propertyPath}\"");
				return false;
			}
			ListViewSerializedObjectBinding.CreateBind(listView, this, prop);
			return true;
		}
		Debug.LogWarning($"Binding ListView is not supported for {prop.type} properties \"{prop.propertyPath}\"");
		return false;
	}

	private void RemoveBinding(IBindable bindable)
	{
		if (bindable != null && bindable.IsBound() && bindable.binding is SerializedObjectBindingBase serializedObjectBindingBase && serializedObjectBindingBase.bindingContext == this)
		{
			bindable.binding?.Release();
			bindable.binding = null;
		}
	}

	internal static SerializedObjectBindingContext GetBindingContextFromElement(VisualElement element)
	{
		if (element is IBindable bindable && bindable.binding is SerializedObjectBindingBase serializedObjectBindingBase)
		{
			return serializedObjectBindingBase.bindingContext;
		}
		if (element.GetProperty(FindContextPropertyKey) is SerializedObjectBindingContext result)
		{
			return result;
		}
		return null;
	}

	internal static SerializedObjectBindingContext FindBindingContext(VisualElement element, SerializedObject obj)
	{
		while (element != null)
		{
			SerializedObjectBindingContext bindingContextFromElement = GetBindingContextFromElement(element);
			if (bindingContextFromElement != null && bindingContextFromElement.serializedObject == obj)
			{
				return bindingContextFromElement;
			}
			element = element.hierarchy.parent;
		}
		return null;
	}

	internal void UpdateRevision()
	{
		ulong num = lastRevision;
		lastRevision = serializedObject.objectVersion;
		if (num != lastRevision)
		{
			UpdateValidProperties();
			UpdateTrackedValues();
		}
	}

	internal bool IsValid()
	{
		if (serializedObject == null || serializedObject.m_NativeObjectPtr == IntPtr.Zero)
		{
			return false;
		}
		return serializedObject.isValid;
	}

	internal void UpdateIfNecessary()
	{
		if (!wasUpdated)
		{
			if (IsValid())
			{
				serializedObject.UpdateIfRequiredOrScript();
				UpdateRevision();
			}
			wasUpdated = true;
		}
	}

	internal void ResetUpdate()
	{
		wasUpdated = false;
	}

	private void UpdateTrackedValues()
	{
		m_ValueTracker?.Update(this);
	}

	public bool RegisterSerializedPropertyChangeCallback(object cookie, SerializedProperty property, Action<object, SerializedProperty> valueChangedCallback)
	{
		TrackedValues orCreateTrackedValues = GetOrCreateTrackedValues();
		orCreateTrackedValues.Add(property, cookie, valueChangedCallback);
		return true;
	}

	public void UnregisterSerializedPropertyChangeCallback(object cookie, SerializedProperty property)
	{
		TrackedValues orCreateTrackedValues = GetOrCreateTrackedValues();
		orCreateTrackedValues.Remove(property, cookie);
	}

	private TrackedValues GetOrCreateTrackedValues()
	{
		if (m_ValueTracker == null)
		{
			m_ValueTracker = new TrackedValues();
		}
		return m_ValueTracker;
	}

	public SerializedObjectBindingContextUpdater AddBindingUpdater(VisualElement element)
	{
		IBinding additionalBinding = VisualTreeBindingsUpdater.GetAdditionalBinding(element);
		SerializedObjectBindingContextUpdater serializedObjectBindingContextUpdater = additionalBinding as SerializedObjectBindingContextUpdater;
		if (additionalBinding == null)
		{
			serializedObjectBindingContextUpdater = SerializedObjectBindingContextUpdater.Create(element, this);
			VisualTreeBindingsUpdater.SetAdditionalBinding(element, serializedObjectBindingContextUpdater);
		}
		else if (serializedObjectBindingContextUpdater == null || serializedObjectBindingContextUpdater.bindingContext != this)
		{
			throw new NotSupportedException("An element can track properties on only one serializedObject at a time");
		}
		return serializedObjectBindingContextUpdater;
	}

	internal void RegisterBindingObject(SerializedObjectBindingBase b)
	{
		m_RegisteredBindings.Add(b);
		b.SetPropertyValid(isValid: true);
	}

	internal void UnregisterBindingObject(SerializedObjectBindingBase b)
	{
		m_RegisteredBindings.Remove(b);
		b.SetPropertyValid(isValid: false);
	}

	private void UpdateValidProperties()
	{
		if (m_ValidPropertyPaths == null)
		{
			m_ValidPropertyPaths = new HashSet<int>();
		}
		m_ValidPropertyPaths.Clear();
		SerializedProperty iterator = serializedObject.GetIterator();
		iterator.unsafeMode = true;
		while (iterator.NextVisible(enterChildren: true))
		{
			m_ValidPropertyPaths.Add(iterator.hashCodeForPropertyPath);
		}
		foreach (SerializedObjectBindingBase registeredBinding in m_RegisteredBindings)
		{
			registeredBinding.SetPropertyValid(SerializedPropertyHelper.IsPropertyValidFaster(m_ValidPropertyPaths, registeredBinding.boundProperty));
		}
		m_ValueTracker?.UpdateValidProperties(m_ValidPropertyPaths);
	}
}
internal sealed class SerializedObjectBindingContextUpdater : SerializedObjectBindingBase
{
	public static ObjectPool<SerializedObjectBindingContextUpdater> s_Pool = new ObjectPool<SerializedObjectBindingContextUpdater>(() => new SerializedObjectBindingContextUpdater(), 32);

	private VisualElement owner;

	private ulong lastUpdatedRevision = ulong.MaxValue;

	private List<SerializedProperty> trackedProperties { get; }

	public event Action<object, SerializedObject> registeredCallbacks;

	public static SerializedObjectBindingContextUpdater Create(VisualElement owner, SerializedObjectBindingContext context)
	{
		SerializedObjectBindingContextUpdater serializedObjectBindingContextUpdater = s_Pool.Get();
		serializedObjectBindingContextUpdater.bindingContext = context;
		serializedObjectBindingContextUpdater.owner = owner;
		serializedObjectBindingContextUpdater.lastUpdatedRevision = context.lastRevision;
		return serializedObjectBindingContextUpdater;
	}

	public SerializedObjectBindingContextUpdater()
	{
		trackedProperties = new List<SerializedProperty>();
	}

	public void AddTracking(SerializedProperty prop)
	{
		trackedProperties.Add(prop);
	}

	public override void Update()
	{
		ResetUpdate();
		if (lastUpdatedRevision != base.bindingContext.lastRevision)
		{
			lastUpdatedRevision = base.bindingContext.lastRevision;
			this.registeredCallbacks?.Invoke(owner, base.bindingContext.serializedObject);
		}
	}

	public override void Release()
	{
		if (base.isReleased)
		{
			return;
		}
		if (owner != null)
		{
			foreach (SerializedProperty trackedProperty in trackedProperties)
			{
				base.bindingContext.UnregisterSerializedPropertyChangeCallback(owner, trackedProperty);
			}
		}
		trackedProperties.Clear();
		owner = null;
		base.bindingContext = null;
		boundProperty = null;
		this.registeredCallbacks = null;
		ResetCachedValues();
		base.isReleased = true;
		s_Pool.Release(this);
	}

	protected override void ResetCachedValues()
	{
		lastUpdatedRevision = ulong.MaxValue;
		lastContentHash = uint.MaxValue;
	}
}
