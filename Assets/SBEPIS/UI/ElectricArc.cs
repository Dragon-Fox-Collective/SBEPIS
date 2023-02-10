using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SBEPIS.UI
{
	public class ElectricArc : MonoBehaviour
	{
		public Transform otherPoint;
		public int numPoints = 5;
		public float randomnessFactor = 1f;
		public Material arcMaterial;
		
		private LineRenderer line;
		private Vector3[] points;
		
		private Vector2 mainTextureScale = Vector2.one;
		private Vector2 mainTextureOffset = Vector2.one;
		
		private float time;
		
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		private void Awake()
		{
			line = gameObject.AddComponent<LineRenderer>();
			line.material = arcMaterial;
			points = new Vector3[numPoints];
			line.positionCount = numPoints;
			line.enabled = enabled;
		}
		
		private void Update()
		{
			if (!otherPoint)
				return;
			
			float distanceBetweenTwoPoints = Vector3.Distance(transform.position, otherPoint.position) / (points.Length - 1);
			mainTextureScale.x = distanceBetweenTwoPoints;
			line.material.SetTextureScale(MainTex, mainTextureScale);

			float randomness = distanceBetweenTwoPoints * randomnessFactor;
			mainTextureOffset.x = Random.Range(-randomness, randomness);
			line.material.SetTextureOffset(MainTex, mainTextureOffset);
			
			for (int i = 0; i < points.Length; i++)
				points[i] = Vector3.Lerp(transform.position, otherPoint.position, i / (points.Length - 1f));
			for (int i = 1; i < points.Length - 1; i++)
				points[i] += new Vector3(
					Random.Range(-randomness, randomness),
					Random.Range(-randomness, randomness),
					Random.Range(-randomness, randomness));
			
			line.SetPositions(points);
		}

		private void OnEnable()
		{
			line.enabled = true;
		}

		private void OnDisable()
		{
			line.enabled = false;
		}
	}
}
