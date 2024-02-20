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

public partial class Football : INotificationPropagator, INamed, IPhysicsUpdate
{
	[ExposeMembersInClass] public Named Named { get; set; }
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public DynamicBody Body { get; set; }
	public GravityAffector Gravity { get; set; }
	
	public Vector2 MovementDirection = Vector2.Zero;
	
	public Football(WorldSimulation simulation)
	{
		Named = new Named("Football");
		Transform = new Transform3D();
		Sphere sphere = new(1);
		MeshRenderer = new PBRMeshRenderer(Transform) { Mesh = Mesh.Sphere };
		Body = new DynamicBody(Transform, BodyShape.Of(sphere), new BodyInertia { InverseMass = 1.0f })
		{
			PhysicsMaterial = new PhysicsMaterial { Friction = 10 },
			Simulation = simulation
		};
		Gravity = new GravityAffector(Body);
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Body, Gravity, MeshRenderer);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		Body.Reference.Velocity.Angular = MovementDirection == Vector2.Zero ? Vector3.Zero : MovementDirection.WithZ(0).Normalized.Cross(Vector3.Down) * 20;
	}
}

public partial class Player : INotificationPropagator, INamed, IKeyDown, IKeyUp, IMouseMoved, IPhysicsUpdate
{
	[ExposeMembersInClass] public Named Named { get; set; }
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	public Football Football { get; set; }
	public Camera3D Camera { get; set; }
	
	public double CameraSensitivity = 0.003;
	
	private Vector2 inputDirection = Vector2.Zero;
	private double cameraPitch = -Math.PI / 2;
	private double cameraYaw = 0;
	
	public Player(WorldSimulation simulation, INotificationPropagator cameraRoot)
	{
		Named = new Named("Player");
		Transform = new Transform3D();
		Football = new Football(simulation) { IsGlobal = true };
		Football.LocalTransformChanged += () => LocalPosition = Football.LocalPosition - Vector3.Down;
		
		Camera = new Camera3D(cameraRoot, new Transform3D { IsGlobal = true }) { FieldOfView = 100 };
		Camera.Transform.LocalRotation = Quaternion.FromEulerAngles(cameraPitch, cameraYaw, 0);
		LocalTransformChanged += () => Camera.Transform.LocalPosition = LocalPosition + Vector3.Up * 3;
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, Camera);
	}
	
	public void OnKeyDown(Keys key)
	{
		inputDirection += key switch
		{
			Keys.W => Vector2.North,
			Keys.A => Vector2.West,
			Keys.S => Vector2.South,
			Keys.D => Vector2.East,
			_ => Vector2.Zero
		};
	}
	
	public void OnKeyUp(Keys key)
	{
		inputDirection -= key switch
		{
			Keys.W => Vector2.North,
			Keys.A => Vector2.West,
			Keys.S => Vector2.South,
			Keys.D => Vector2.East,
			_ => Vector2.Zero
		};
	}
	
	public void OnMouseMoved(Vector2 position, Vector2 delta, Vector3 globalPosition)
	{
		cameraPitch = Math.Clamp(cameraPitch - delta.Y * CameraSensitivity, -Math.PI, 0);
		cameraYaw = (cameraYaw + delta.X * CameraSensitivity) % (2 * Math.PI);
		Camera.Transform.LocalRotation = Quaternion.FromEulerAngles(cameraPitch, cameraYaw, 0);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		Football.MovementDirection = Camera.Transform.LocalRotation * inputDirection;
	}
}