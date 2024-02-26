using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using Echidna2.Serialization;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SBEPIS;

public partial class Football : INotificationPropagator, IPhysicsUpdate
{
	[SerializedReference] public Transform3D Transform { get; set; } = null!;
	[SerializedReference] public PBRMeshRenderer MeshRenderer { get; set; } = null!;
	[SerializedReference] public DynamicBody Body { get; set; } = null!;
	[SerializedReference] public GravityAffector Gravity { get; set; } = null!;
	
	public Vector3 GlobalPosition
	{
		get => Body.GlobalPosition;
		set => Body.GlobalPosition = value;
	}
	public event Transform3D.TransformChangedHandler? TransformChanged
	{
		add => Transform.TransformChanged += value;
		remove => Transform.TransformChanged -= value;
	}
	
	private double radius = 1.0;
	[SerializedValue] public double Radius
	{
		get => radius;
		set
		{
			radius = value;
			Body.Transform.LocalScale = Vector3.One * value;
			Body.Shape = BodyShape.Of(new Sphere((float)value));
		}
	}
	[SerializedValue] public Vector2 MovementDirection = Vector2.Zero;
	[SerializedValue] public double Speed = 20;
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Body, Gravity, MeshRenderer);
	}
	
	public void OnPhysicsUpdate(double deltaTime)
	{
		Body.Reference.Velocity.Angular = MovementDirection == Vector2.Zero ? Vector3.Zero : MovementDirection.WithZ(0).Normalized.Cross(Vector3.Down) * Speed;
	}
}

public partial class FootballWithShinGuard : INotificationPropagator, INotificationHook<IInitializeIntoSimulation.Notification>
{
	[SerializedReference, ExposeMembersInClass] public Football Football { get; set; } = null!;
	[SerializedReference] public Transform3D ShinGuardTransform { get; set; } = null!;
	[SerializedReference] public PBRMeshRenderer ShinGuardMeshRenderer { get; set; } = null!;
	[SerializedReference] public DynamicBody ShinGuardBody { get; set; } = null!;
	[SerializedReference] public GravityAffector ShinGuardGravity { get; set; } = null!;
	
	private double shinGuardRadius = 1.0;
	[SerializedValue] public double ShinGuardRadius
	{
		get => shinGuardRadius;
		set
		{
			shinGuardRadius = value;
			ShinGuardBody.Transform.LocalScale = Vector3.One * value;
			ShinGuardBody.Shape = BodyShape.Of(new Sphere((float)value));
		}
	}
	[SerializedValue] public double FootballGap = 0.1;
	
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
	
	public void Notify<T>(T notification) where T : notnull
	{
		INotificationPropagator.Notify(notification, Football, ShinGuardBody, ShinGuardMeshRenderer, ShinGuardGravity);
	}
	
	public void OnPreNotify(IInitializeIntoSimulation.Notification notification)
	{
		
	}
	
	public void OnPostNotify(IInitializeIntoSimulation.Notification notification)
	{
		
	}
	
	public void OnPostPropagate(IInitializeIntoSimulation.Notification notification)
	{
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

public partial class Player : INotificationPropagator, IKeyDown, IKeyUp, IMouseMoved, IPhysicsUpdate, IInitialize
{
	[SerializedReference] public FootballWithShinGuard Football { get; set; } = null!;
	[SerializedReference] public Camera3D Camera { get; set; } = null!;
	
	[SerializedValue] public double CameraSensitivity = 0.003;
	
	private Vector2 inputDirection = Vector2.Zero;
	private double cameraPitch = -Math.PI / 2;
	private double cameraYaw = 0;
	
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
	
	public void OnInitialize()
	{
		Football.TransformChanged += () => Camera.Transform.GlobalPosition = Football.GlobalPosition + Vector3.Up * 3;
		Camera.Transform.LocalRotation = Quaternion.FromEulerAngles(cameraPitch, cameraYaw, 0);
	}
}

public partial class Consort : INotificationPropagator, IPhysicsUpdate, IInitialize
{
	[SerializedReference, ExposeMembersInClass] public Transform3D Transform { get; set; } = null!;
	[SerializedReference] public Transform3D MeshRendererTransform { get; set; } = null!;
	[SerializedReference] public PBRMeshRenderer MeshRenderer { get; set; } = null!;
	[SerializedReference] public FootballWithShinGuard Football { get; set; } = null!;
	
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
	
	private Vector2 targetPosition = Vector2.Zero;
	
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
	
	public void OnInitialize()
	{
		Football.TransformChanged += () => Transform.GlobalPosition = Football.GlobalPosition;
		RegenerateTarget();
	}
}

public partial class Scene : INotificationPropagator, IHasChildren, ICanAddChildren
{
	[SerializedReference, ExposeMembersInClass] public Hierarchy Hierarchy { get; set; } = null!;
	[SerializedReference] public Player Player { get; set; } = null!;
}