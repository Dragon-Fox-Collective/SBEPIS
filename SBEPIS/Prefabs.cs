using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Mesh = Echidna2.Rendering.Mesh;

namespace SBEPIS;

public partial class Football : INotificationPropagator, IPhysicsUpdate
{
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public DynamicBody Body { get; set; }
	public GravityAffector Gravity { get; set; }
	
	public Vector2 MovementDirection = Vector2.Zero;
	public double Speed = 20;
	
	public Football(WorldSimulation simulation, double radius = 1.0)
	{
		Transform = new Transform3D { LocalScale = Vector3.One * radius };
		Sphere sphere = new((float)radius);
		MeshRenderer = new PBRMeshRenderer(Transform) { Mesh = Mesh.Sphere };
		Body = new DynamicBody(Transform, BodyShape.Of(sphere), new BodyInertia { InverseMass = 1.0f })
		{
			PhysicsMaterial = new PhysicsMaterial { Friction = 10 },
			CollisionFilter = new CollisionFilter { Membership = 1, Collision = ~2 },
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
		Body.Reference.Velocity.Angular = MovementDirection == Vector2.Zero ? Vector3.Zero : MovementDirection.WithZ(0).Normalized.Cross(Vector3.Down) * Speed;
	}
}

public partial class FootballWithShinGuard : INotificationPropagator
{
	[ExposeMembersInClass] public Football Football { get; set; }
	public Transform3D ShinGuardTransform { get; set; }
	public PBRMeshRenderer ShinGuardRenderer { get; set; }
	public DynamicBody ShinGuardBody { get; set; }
	
	public FootballWithShinGuard(WorldSimulation simulation, double footballRadius = 0.9, double shinGuardRadius = 1.0, double footballGap = 0.1)
	{
		Football = new Football(simulation, footballRadius);
		
		ShinGuardTransform = new Transform3D { IsGlobal = true, LocalPosition = Vector3.Up * 0.5, LocalScale = Vector3.One * shinGuardRadius };
		ShinGuardRenderer = new PBRMeshRenderer(ShinGuardTransform) { Mesh = Mesh.Sphere };
		ShinGuardBody = new DynamicBody(ShinGuardTransform, BodyShape.Of(new Sphere((float)shinGuardRadius)), new BodyInertia { InverseMass = 1.0f })
		{
			PhysicsMaterial = new PhysicsMaterial { Friction = 1 },
			CollisionFilter = new CollisionFilter { Membership = 2, Collision = ~1 },
			Simulation = simulation
		};
		
		simulation.AddJoint(
			Football.Body,
			ShinGuardBody,
			new BallSocket
			{
				LocalOffsetA = Vector3.Zero,
				LocalOffsetB = Vector3.Down * (shinGuardRadius - footballRadius + footballGap),
				SpringSettings = new SpringSettings(60, 1),
			}
		);
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, ShinGuardBody, ShinGuardRenderer);
	}
}

public partial class Player : INotificationPropagator, IKeyDown, IKeyUp, IMouseMoved, IPhysicsUpdate
{
	public FootballWithShinGuard Football { get; set; }
	public Camera3D Camera { get; set; }
	
	public double CameraSensitivity = 0.003;
	
	private Vector2 inputDirection = Vector2.Zero;
	private double cameraPitch = -Math.PI / 2;
	private double cameraYaw = 0;
	
	public Player(WorldSimulation simulation, INotificationPropagator cameraRoot)
	{
		Football = new FootballWithShinGuard(simulation) { IsGlobal = true };
		
		Camera = new Camera3D(cameraRoot, new Transform3D { IsGlobal = true }) { FieldOfView = 100 };
		Camera.Transform.LocalRotation = Quaternion.FromEulerAngles(cameraPitch, cameraYaw, 0);
		
		Football.LocalTransformChanged += () => Camera.Transform.LocalPosition = Football.LocalPosition + Vector3.Up * 3;
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
		Vector2 forward = Camera.Transform.GlobalTransform.Out.XY;
		double angle = Vector2.AngleBetween(forward, Vector2.Forward);
		if (double.IsNaN(angle))
		{
			forward = Camera.Transform.GlobalTransform.Forward.XY;
			angle = Vector2.AngleBetween(forward, Vector2.Forward);
		}
		if (forward.X < 0) angle = -angle;
		Vector2 rotatedInput = inputDirection.RotatedBy(-angle);
		Football.MovementDirection = rotatedInput;
	}
}

public partial class Consort : INotificationPropagator, IPhysicsUpdate
{
	public Transform3D MeshRendererTransform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public FootballWithShinGuard Football { get; set; }
	
	private Vector2 targetPosition = Vector2.Zero;
	
	public Consort(WorldSimulation simulation)
	{
		MeshRendererTransform = new Transform3D { IsGlobal = true };
		MeshRenderer = new PBRMeshRenderer(MeshRendererTransform) { Mesh = Mesh.Cube };
		
		Football = new FootballWithShinGuard(simulation) { IsGlobal = true };
		Football.LocalTransformChanged += () => MeshRendererTransform.LocalPosition = Football.LocalPosition + Vector3.Up * 2;
		
		RegenerateTarget();
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, MeshRenderer);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		if (Football.LocalPosition.XY.DistanceTo(targetPosition) < 0.1)
			RegenerateTarget();
		Football.MovementDirection = (targetPosition - Football.LocalPosition.XY).Normalized;
		
	}
	
	public void RegenerateTarget()
	{
		targetPosition = new Vector2(Random.Shared.NextDouble() * 100 - 50, Random.Shared.NextDouble() * 100 - 50);
	}
}