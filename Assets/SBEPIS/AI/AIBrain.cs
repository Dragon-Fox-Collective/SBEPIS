using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace SBEPIS.AI
{
	public class AIBrain : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private NavMeshAgent agent;
		
		[SerializeField] private AIPointBehaviour moveTo;
		
		private void Start()
		{
			agent.updatePosition = false;
			agent.updateRotation = false;
			agent.destination = moveTo.transform.position;
		}
		
		private void Update()
		{
			transform.position += agent.velocity * Time.deltaTime;
			//agent.nextPosition = transform.position;
		}
	}
}
