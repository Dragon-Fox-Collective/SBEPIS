using UnityEngine;

/*
Copyright 2016 Max Kaufmann (max.kaufmann@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

public static class QuaternionUtil
{

	public static Quaternion AngVelToDeriv(Quaternion current, Vector3 angVel)
	{
		Quaternion spin = new(angVel.x, angVel.y, angVel.z, 0f);
		Quaternion result = spin * current;
		return new Quaternion(0.5f * result.x, 0.5f * result.y, 0.5f * result.z, 0.5f * result.w);
	}

	public static Vector3 DerivToAngVel(Quaternion current, Quaternion deriv)
	{
		Quaternion result = deriv * Quaternion.Inverse(current);
		return new Vector3(2f * result.x, 2f * result.y, 2f * result.z);
	}

	public static Quaternion IntegrateRotation(Quaternion rotation, Vector3 angularVelocity, float deltaTime)
	{
		if (deltaTime < Mathf.Epsilon) return rotation;
		Quaternion deriv = AngVelToDeriv(rotation, angularVelocity);
		Vector4 pred = new Vector4(
				rotation.x + deriv.x * deltaTime,
				rotation.y + deriv.y * deltaTime,
				rotation.z + deriv.z * deltaTime,
				rotation.w + deriv.w * deltaTime
		).normalized;
		return new Quaternion(pred.x, pred.y, pred.z, pred.w);
	}

	public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
	{
		if (Time.deltaTime < Mathf.Epsilon) return rot;
		// account for double-cover
		float dot = Quaternion.Dot(rot, target);
		float multi = dot > 0f ? 1f : -1f;
		target.x *= multi;
		target.y *= multi;
		target.z *= multi;
		target.w *= multi;
		// smooth damp (nlerp approx)
		Vector4 result = new Vector4(
			Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
			Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
			Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
			Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
		).normalized;

		// ensure deriv is tangent
		Vector4 derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), result);
		deriv.x -= derivError.x;
		deriv.y -= derivError.y;
		deriv.z -= derivError.z;
		deriv.w -= derivError.w;

		return new Quaternion(result.x, result.y, result.z, result.w);
	}
}