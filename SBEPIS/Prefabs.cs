using BepuPhysics.Collidables;
using Echidna2.Core;
using Echidna2.Physics;
using Echidna2.Rendering3D;
using Mesh = Echidna2.Rendering.Mesh;

namespace SBEPIS;

public partial class Football : INotificationPropagator, INamed
{
	[ExposeMembersInClass] public Named Named { get; set; }
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public DynamicBody Body { get; set; }
	public GravityAffector Gravity { get; set; }
	
	public Football(WorldSimulation simulation)
	{
		Named = new Named("Football");
		Transform = new Transform3D { IsGlobal = true };
		Sphere sphere = new(0.5f);
		MeshRenderer = new PBRMeshRenderer(Transform) { Mesh = Mesh.Sphere };
		Body = new DynamicBody(simulation, Transform, BodyShape.Of(sphere), sphere.ComputeInertia(1));
		Gravity = new GravityAffector(Body);
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Body, Gravity, MeshRenderer);
	}
}