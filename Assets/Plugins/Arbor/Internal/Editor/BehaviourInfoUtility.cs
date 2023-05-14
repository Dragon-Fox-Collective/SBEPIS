//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	public static class BehaviourInfoUtility
	{
		static BehaviourInfo _InvalidBehaviourInfo = null;
		static Dictionary<System.Type, BehaviourInfo> _BehaviourInfos = new Dictionary<System.Type, BehaviourInfo>();

		public static event System.Action onChanged;

		static BehaviourInfoUtility()
		{
			ArborSettings.onChangedLanguage += OnChanedLanguage;
		}

		static void OnChanedLanguage()
		{
			_BehaviourInfos.Clear();

			onChanged?.Invoke();
		}

		public static BehaviourInfo GetBehaviourInfo(System.Type classType)
		{
			BehaviourInfo behaviourInfo;
			if (!_BehaviourInfos.TryGetValue(classType, out behaviourInfo))
			{
				behaviourInfo = new BehaviourInfo(classType);
				_BehaviourInfos.Add(classType, behaviourInfo);
			}

			return behaviourInfo;
		}

		public static BehaviourInfo GetBehaviourInfo(Object behaviourObj)
		{
			if (ComponentUtility.IsValidObject(behaviourObj))
			{
				System.Type classType = behaviourObj.GetType();
				return GetBehaviourInfo(classType);
			}

			if (_InvalidBehaviourInfo == null)
			{
				_InvalidBehaviourInfo = new BehaviourInfo(null);
			}
			return _InvalidBehaviourInfo;
		}
	}
}