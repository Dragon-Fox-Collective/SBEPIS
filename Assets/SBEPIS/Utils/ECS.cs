using System;
using System.Collections;
using System.Collections.Generic;
using SBEPIS.Utils.Linq;

namespace SBEPIS.Utils.ECS
{
	public class ECSEntity<TComponentBase> : IEnumerable<TComponentBase>
	{
		private Dictionary<Type, TComponentBase> components = new();
		protected IEnumerable<KeyValuePair<Type, TComponentBase>> Components => components;
		
		public ECSEntity() { }
		public ECSEntity(Dictionary<Type, TComponentBase> components)
		{
			this.components = components;
		}
		
		public void Add<TComponent>(TComponent stateComponent) where TComponent : TComponentBase => components.Add(typeof(TComponent), stateComponent);
		
		public TComponent Get<TComponent>() where TComponent : TComponentBase => (TComponent)components[typeof(TComponent)];
		public void Set<TComponent>(TComponent component) where TComponent : TComponentBase => components[typeof(TComponent)] = component;
		
		public IEnumerator<TComponentBase> GetEnumerator() => components.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override string ToString() => ", ".Join(components.Values);
	}
}