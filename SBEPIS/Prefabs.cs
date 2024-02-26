using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using Echidna2.Serialization;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Mesh = Echidna2.Rendering.Mesh;

namespace SBEPIS;

public partial class Football : INotificationPropagator, IPhysicsUpdate
{
	[SerializedReference] public Transform3D? Transform { get; set; }
	[SerializedReference] public PBRMeshRenderer? MeshRenderer { get; set; }
	[SerializedReference] public DynamicBody? Body { get; set; }
	[SerializedReference] public GravityAffector? Gravity { get; set; }
	
	public Vector3 GlobalPosition
	{
		get => Body!.GlobalPosition;
		set => Body!.GlobalPosition = value;
	}
	public event Transform3D.TransformChangedHandler? TransformChanged
	{
		add => Transform!.TransformChanged += value;
		remove => Transform!.TransformChanged -= value;
	}
	
	private double radius = 1.0;
	[SerializedValue] public double Radius
	{
		get => radius;
		set
		{
			radius = value;
			if (Body is not null)
				Body.Shape = BodyShape.Of(new Sphere((float)radius));
		}
	}
	[SerializedValue] public Vector2 MovementDirection = Vector2.Zero;
	[SerializedValue] public double Speed = 20;
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Body!, Gravity!, MeshRenderer!);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		Body!.Reference.Velocity.Angular = MovementDirection == Vector2.Zero ? Vector3.Zero : MovementDirection.WithZ(0).Normalized.Cross(Vector3.Down) * Speed;
	}
	
	public static Football NewWithDefaults(double radius = 1.0)
	{
		Transform3D transform = new() { LocalScale = Vector3.One * radius };
		Sphere sphere = new((float)radius);
		PBRMeshRenderer meshRenderer = new() { Transform = transform, Mesh = Mesh.Sphere };
		DynamicBody body = new()
		{
			Transform = transform,
			Shape = BodyShape.Of(sphere),
			Inertia = new BodyInertia { InverseMass = 1.0f },
			PhysicsMaterial = new PhysicsMaterial { Friction = 10 },
			CollisionFilter = new CollisionFilter { Membership = 1L, Collision = ~2L },
		};
		GravityAffector gravity = new() { Body = body };
		return new Football
		{
			Transform = transform,
			MeshRenderer = meshRenderer,
			Body = body,
			Gravity = gravity,
			Radius = radius,
		};
	}
}

public partial class FootballWithShinGuard : INotificationPropagator, INotificationHook<IInitializeIntoSimulation.Notification>
{
	[ExposeMembersInClass] public Football Football { get; set; }
	[SerializedReference] public Transform3D ShinGuardTransform { get; set; }
	[SerializedReference] public PBRMeshRenderer ShinGuardRenderer { get; set; }
	[SerializedReference] public DynamicBody ShinGuardBody { get; set; }
	[SerializedReference] public GravityAffector ShinGuardGravity { get; set; }
	
	public double ShinGuardRadius = 1.0;
	public double FootballGap = 0.1;
	
	public Vector3 GlobalPosition
	{
		get => Football.GlobalPosition;
		set
		{
			Vector3 shinGuardOffset = ShinGuardBody.GlobalPosition - Football.GlobalPosition;
			Football.GlobalPosition = value;
			ShinGuardBody.GlobalPosition = value + shinGuardOffset;
		}
	}
	
	public FootballWithShinGuard()
	{
		Football = Football.NewWithDefaults(radius: 0.9);
		
		ShinGuardTransform = new Transform3D { LocalPosition = Vector3.Up * 0.5, LocalScale = Vector3.One * ShinGuardRadius };
		ShinGuardRenderer = new PBRMeshRenderer { Transform = ShinGuardTransform, Mesh = Mesh.Sphere };
		ShinGuardBody = new DynamicBody
		{
			Transform =	ShinGuardTransform,
			Shape = BodyShape.Of(new Sphere((float)ShinGuardRadius)),
			Inertia = new BodyInertia { InverseMass = 1.0f },
			PhysicsMaterial = new PhysicsMaterial { Friction = 1 },
			CollisionFilter = new CollisionFilter { Membership = 2L, Collision = ~1L },
		};
		ShinGuardGravity = new GravityAffector { Body = ShinGuardBody};
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, ShinGuardBody, ShinGuardRenderer, ShinGuardGravity);
	}
	
	public void OnPreNotify(IInitializeIntoSimulation.Notification notification)
	{
		
	}
	
	public void OnPostNotify(IInitializeIntoSimulation.Notification notification)
	{
		
	}
	
	public void OnPostPropagate(IInitializeIntoSimulation.Notification notification)
	{
		if (Football.Body is null)
			throw new NullReferenceException();
		
		notification.Simulation.AddJoint(
			Football.Body,
			ShinGuardBody,
			new BallSocket
			{
				LocalOffsetA = Vector3.Zero,
				LocalOffsetB = Vector3.Down * (ShinGuardRadius - Radius + FootballGap),
				SpringSettings = new SpringSettings(60, 1),
			}
		);
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
	
	public Player(INotificationPropagator cameraRoot)
	{
		Football = new FootballWithShinGuard();
		
		Camera = new Camera3D(cameraRoot, new Transform3D()) { FieldOfView = 100 };
		Camera.Transform.LocalRotation = Quaternion.FromEulerAngles(cameraPitch, cameraYaw, 0);
		
		Football.TransformChanged += () => Camera.Transform.GlobalPosition = Football.GlobalPosition + Vector3.Up * 3;
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
	[ExposeMembersInClass] public Transform3D Transform { get; set; }
	
	public Vector3 LocalPosition
	{
		get => Transform.LocalPosition;
		set
		{
			Transform.LocalPosition = value;
			Football.GlobalPosition = Transform.Parent?.GlobalTransform.TransformPoint(value) ?? value;
		}
	}
	public Vector3 GlobalPosition
	{
		get => Transform.GlobalPosition;
		set
		{
			Transform.GlobalPosition = value;
			Football.GlobalPosition = value;
		}
	}
	
	public Transform3D MeshRendererTransform { get; set; }
	public PBRMeshRenderer MeshRenderer { get; set; }
	public FootballWithShinGuard Football { get; set; }
	
	private Vector2 targetPosition = Vector2.Zero;
	
	public Consort()
	{
		Transform = new Transform3D();
		
		MeshRendererTransform = new Transform3D { Parent = Transform, LocalPosition = Vector3.Up * 2 };
		MeshRenderer = new PBRMeshRenderer { Transform = MeshRendererTransform, Mesh = Mesh.Cube };
		
		Football = new FootballWithShinGuard();
		Football.TransformChanged += () => Transform.GlobalPosition = Football.GlobalPosition;
		
		RegenerateTarget();
	}
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, MeshRenderer);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		if (Football.GlobalPosition.XY.DistanceTo(targetPosition) < 0.1)
			RegenerateTarget();
		Football.MovementDirection = (targetPosition - Football.GlobalPosition.XY).Normalized;
	}
	
	public void RegenerateTarget()
	{
		targetPosition = new Vector2(Random.Shared.NextDouble() * 100 - 50, Random.Shared.NextDouble() * 100 - 50);
	}
}