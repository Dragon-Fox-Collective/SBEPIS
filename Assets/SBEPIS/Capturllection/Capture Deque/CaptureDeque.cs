using System;
using System.Collections.Generic;
using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Thaumaturgy;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(GravitySum))]
	[RequireComponent(typeof(CollisionTrigger), typeof(CouplingPlug))]
	public class CaptureDeque : MonoBehaviour
	{
		public Transform cardStart;
		public Transform cardTarget;
		
		public DequeLayer definition;

		public Material dequeMaterial;
		public List<Renderer> renderers;

		public Grabbable grabbable { get; private set; }
		public GravitySum gravitySum { get; private set; }
		public CollisionTrigger collisionTrigger { get; private set; }
		public CouplingPlug plug { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			gravitySum = GetComponent<GravitySum>();
			collisionTrigger = GetComponent<CollisionTrigger>();
			plug = GetComponent<CouplingPlug>();
		}

		private void Start()
		{
			renderers.PerformOnMaterial(dequeMaterial, material =>
			{
				Texture2D firstTexture = definition.deques[0].dequeTexture;
				material.SetTexture("_Fallback_Texture", firstTexture);
				
				Texture2DArray texture = new(firstTexture.width, firstTexture.height, definition.deques.Count, firstTexture.format, firstTexture.mipmapCount > 1);
				for (int i = 0; i < definition.deques.Count; i++)
					Graphics.CopyTexture(definition.deques[i].dequeTexture, 0, texture, i);
				material.SetTexture("_Textures", texture);
				
				material.SetFloat("_Num_Textures", definition.deques.Count);
			});
		}

		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturllector capturllector = grabber.GetComponent<Capturllector>();
			if (!capturllector)
				return;

			DequeOwner dequeOwner = capturllector.dequeOwner;
			dequeOwner.deque = this;
		}
	}
}
