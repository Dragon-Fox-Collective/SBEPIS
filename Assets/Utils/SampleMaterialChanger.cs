using UnityEngine;

public class SampleMaterialChanger : MonoBehaviour
{
	public new Renderer renderer;
	public Material newMaterial;

	private Material oldMaterial;

	private void Awake()
	{
		oldMaterial = renderer.material;
	}

	public void SetMaterial()
	{
		renderer.material = newMaterial;
	}

	public void ResetMaterial()
	{
		renderer.material = oldMaterial;
	}
}
