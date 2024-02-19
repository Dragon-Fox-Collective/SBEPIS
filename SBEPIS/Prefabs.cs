using BepuPhysics;
using BepuPhysics.Collidables;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Mesh = Echidna2.Rendering.Mesh;

namespace SBEPIS;

public partial class Football : INotificationPropagator, INamed, INotificationListener<IKeyDown.Notification>, INotificationListener<IKeyUp.Notification>, IPhysicsUpdate
{
	[ExposeMembersInClass] public Named Named { get; set; }
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public DynamicBody Body { get; set; }
	public GravityAffector Gravity { get; set; }
	
	private Vector2 movementDirection = Vector2.Zero;
	
	public Football(WorldSimulation simulation)
	{
		Named = new Named("Football");
		Transform = new Transform3D { IsGlobal = true };
		Sphere sphere = new(0.5f);
		MeshRenderer = new PBRMeshRenderer(Transform) { Mesh = Mesh.Sphere };
		Body = new DynamicBody(simulation, Transform, BodyShape.Of(sphere), new BodyInertia { InverseMass = 1.0f });
		Gravity = new GravityAffector(Body);
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Body, Gravity, MeshRenderer);
	}
	
	public void OnNotify(IKeyDown.Notification notification)
	{
		movementDirection += notification.Key switch
		{
			Keys.W => Vector2.North,
			Keys.A => Vector2.West,
			Keys.S => Vector2.South,
			Keys.D => Vector2.East,
			_ => Vector2.Zero
		};
	}
	
	public void OnNotify(IKeyUp.Notification notification)
	{
		movementDirection -= notification.Key switch
		{
			Keys.W => Vector2.North,
			Keys.A => Vector2.West,
			Keys.S => Vector2.South,
			Keys.D => Vector2.East,
			_ => Vector2.Zero
		};
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		Body.Reference.Velocity.Angular = movementDirection == Vector2.Zero ? Vector3.Zero : movementDirection.WithZ(0).Normalized.Cross(Vector3.Down) * 20;
	}
}