//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Arbor.Playables
{
	using Arbor.Threading.Tasks;

	internal class YieldDispatcher : IActionDispatcher
	{
		private List<System.Action> _Actions;

		public bool hasActions
		{
			get
			{
				return _Actions != null && _Actions.Count > 0;
			}
		}

		public void Register(System.Action action)
		{
			if (_Actions == null)
			{
				_Actions = Pool.ListPool<System.Action>.Get();
			}

			_Actions.Add(action);
		}

		public void Invoke()
		{
			if (_Actions != null)
			{
				List<System.Action> actions = _Actions;
				_Actions = null;

				for (int i = 0; i < actions.Count; i++)
				{
					var action = actions[i];
					action?.Invoke();
				}

				Pool.ListPool<System.Action>.Release(actions);
			}
		}
	}
}