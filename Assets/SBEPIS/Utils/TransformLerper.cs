using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class TransformLerper : ValidatedMonoBehaviour
    {
	    [SerializeField, Self(Flag.Optional)]
	    private new Rigidbody rigidbody;
		
    	public float timeToComplete = 1;
    	public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    	public UnityEvent onEnd = new();
		
    	public Transform start;
    	public Transform target;
	    
    	private float time = 0;
		
    	private void Awake()
    	{
    		enabled = false;
    		if (rigidbody)
    			rigidbody.Disable();
    	}
		
    	private void OnEnable()
    	{
    		time = 0;
    	}
		
    	private void Update()
    	{
    		time += Time.deltaTime / timeToComplete;
    		transform.SetPositionAndRotation(
    			Vector3.Lerp(start.position, target.position, curve.Evaluate(time)),
    			Quaternion.Lerp(start.rotation, target.rotation, curve.Evaluate(time)));
    
    		if (time >= 1)
    		{
    			Destroy(this);
    			if (rigidbody)
    				rigidbody.Enable();
    			onEnd.Invoke();
    		}
    	}
		
    	public TransformLerper Chain(Transform next)
    	{
    		TransformLerper newLerper = gameObject.AddComponent<TransformLerper>();
    		newLerper.start = target;
    		newLerper.target = next;
    		newLerper.timeToComplete = timeToComplete;
    		newLerper.curve = curve;
    		onEnd.AddListener(() => newLerper.enabled = true);
    		return newLerper;
    	}
    }
}
