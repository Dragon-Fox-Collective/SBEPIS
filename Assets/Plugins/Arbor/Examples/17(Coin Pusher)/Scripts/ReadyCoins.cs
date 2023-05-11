//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

namespace Arbor.Examples
{
	[AddBehaviourMenu("Examples/ReadyCoins")]
	[AddComponentMenu("")]
	public sealed class ReadyCoins : StateBehaviour
	{
		[SerializeField]
		FlexibleGameObject _Prefab = new FlexibleGameObject();

		[SerializeField]
		FlexibleTransform _Parent = new FlexibleTransform();

		[SerializeField]
		FlexibleBounds _Area = new FlexibleBounds();

		[SerializeField]
		FlexibleInt _Count = new FlexibleInt();

		[SerializeField]
		FlexibleFloat _SimulateStep = new FlexibleFloat();

		void PhysicsSimulate(float timer)
		{
			while (timer >= 0)
			{
				timer -= Time.fixedDeltaTime;
				Physics.Simulate(Time.fixedDeltaTime);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			int count = _Count.value;
			Bounds area = _Area.value;

			Vector3 min = area.min;
			Vector3 max = area.max;

			GameObject prefab = _Prefab.value;
			Transform parent = _Parent.value;

			float simulateStep = _SimulateStep.value;

#if UNITY_2022_2_OR_NEWER
			var simulationMode = Physics.simulationMode;
			Physics.simulationMode = SimulationMode.Script;
#else
			bool autoSimulation = Physics.autoSimulation;
			Physics.autoSimulation = false;
#endif

			for (int i = 0; i < count; i++)
			{
				GameObject obj = Instantiate(prefab, parent);

				obj.transform.position = new Vector3(
					Random.Range(min.x, max.x),
					Random.Range(min.y,max.y),
					Random.Range(min.z,max.z)
				);

				PhysicsSimulate(simulateStep);
			}

#if UNITY_2022_2_OR_NEWER
			Physics.simulationMode = simulationMode;
#else
			Physics.autoSimulation = autoSimulation;
#endif
		}
	}
}