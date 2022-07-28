using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace SBEPIS.Interaction
{
	public class Recenterer : MonoBehaviour
	{
		public void BindToXRSubsystem()
		{
			XRInputSubsystem subsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();
			subsystem.boundaryChanged += Recenter;
		}

		private void Recenter(XRInputSubsystem subsystem)
		{
			List<Vector3> boundaryPoints = new();
			subsystem.TryGetBoundaryPoints(boundaryPoints);
			print($"Boundary changed {boundaryPoints.ToDelimString()}");
		}
	}
}
