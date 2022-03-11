using UnityEngine;

public class SampleMaterialChanger : MonoBehaviour
{
	public new Renderer renderer;
	public Material newMaterial;

	private Material oldMaterial;

	public void SetMaterial()
	{
		oldMaterial = renderer.material;
		renderer.material = newMaterial;
	}

	public void ResetMaterial()
	{
		renderer.material = oldMaterial;
		oldMaterial = null;
	}
}
