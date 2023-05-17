using System;
using KBCore.Refs;
using UnityEngine;

public class RenderTextureProxy : ValidatedMonoBehaviour
{
	[SerializeField, Self] private new Camera camera;
	[SerializeField] private Material material;
	
	private RenderTexture renderTexture;
	
	private static readonly int TextureKey = Shader.PropertyToID("_Texture");
	
	private void Awake()
	{
		CreateRenderTexture();
		camera.enabled = true;
	}
	
	private void Update()
	{
		if (renderTexture.width != Screen.width || renderTexture.height != Screen.height)
		{
			DestroyRenderTexture();
			CreateRenderTexture();
		}
	}
	
	private void OnDestroy()
	{
		DestroyRenderTexture();
	}
	
	private void CreateRenderTexture()
	{
		renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		camera.targetTexture = renderTexture;
		material.SetTexture(TextureKey, renderTexture);
	}
	
	private void DestroyRenderTexture()
	{
		Destroy(renderTexture);
	}
}
