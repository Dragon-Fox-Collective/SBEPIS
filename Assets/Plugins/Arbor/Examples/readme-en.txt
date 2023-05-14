# Example list

## ExampleSelector

A scene to select a sample scene from the list.
(If you want to build, you need to add each scene to Scenes In Build in Build Settings)

## 01(Basic FSM)

Basic state machine usage example.
ArborFSM of MainMenu object performs menu transition using OnGUI.

### Contents

* Input
    * Any Key
	  Example of transition by any input
	  (AnyKeyDownTransition, AnyKeyTransition)

	* Key
	  Example of transition by keyboard input
	  (KeyDownTransition, KeyUpTransition)

	* Mouse Button
	  Example of transition by mouse button input
	  (MouseButtonDownTransition, MouseButtonUpTransition)

* Time
  Example of transition over time
  (TimeTransition)

* Collision
  Example of transition by OnTriggerEnter event, OnTriggerExit event
  (InstantiateGameObject, OnTriggerEnterTransition, OnTriggerExitTransition, OnTriggerEnterDestroy)

* Trigger
  Example of transition by sending and receiving trigger message between FSMs
  (SendTrigger, TriggerTransition)



## 02(OnMouse)

Example of a state machine that uses the OnMouse event.

### Contents

* MouseEnter/Exit
  Example of transition by OnMouseEnter event, OnMouseExit event
  (OnMouseEnterTransition, OnMouseExitTransition)

* MouseDown/Up
  Example of transition by OnMouseDown event, OnMouseUp event
  (OnMouseDownTransition, OnMouseUpTransition)



## 03(EventSystems)

An example state machine using EventSystems.

### Contents

* Left Cube (PointerDown/Up)
  Example of transition by OnPointerDown、OnPointerUp event
  (OnPointerDownTransition, OnPointerUpTransition)

* Center Cube (PointerEnter/Exit)
  Example of transition by OnPointerEnter、OnPointerExit event
  (OnPointerEnterTransition, OnPointerExitTransition)

* Right Cube (PointerClick)
  Example of transition by OnPointerClick event
  (OnPointerClickTransition)



## 04(UI)

Example of a state machine using UnityUI.

### Contents

* Button
  Make transition to button click and set text to Text.
  It also uses UITween to perform appearing animation and click animation.
  (UIButtonTransition, UISetText, UITweenSize, UITweenColor, UITweenPosition)

* Toggle
  Perform transition by switching Toggle.
  (UIToggleTransition, UISetText)

* Slider
  Make a transition according to the value of Slider.
  (UISliderTransition, UISetText)



## 05(ParameterContainer)

An example of transition conditioning by parameter using ParameterContainer.

### Contents

* ParameterContainer
  Owns an int type Counter parameter.

* ArborFSM
  The Counter parameter is increased from 0 by 1 to complete Once reached 10 or more.
  (CalcParameter, ParameterTransition, UISetTextFromParameter)



## 06(GlobalParameterContainer)

An example of handling parameters that are not discarded even if the scene is changed using GlobalParameterContainer.

### Contents

* GlobalParameterContainer
  Holds a reference to the shared ParameterContainer prefab.

* ArborFSM
  An example via GlobalParameterContainer in Container reference of CalcParameter.
  (CalcParameter, UISetTextFromParameter, TweenCancasGroupAlpha)



## 07(DataFlow)

Example of performing data input / output by data flow.

### Contents

* DataFlowExampleData.cs
  Script that defines the self-made structure DataFlowExampleData.
  InputSlot and OutputSlot are also defined together to handle self-made structures in data flow.

* DataFlowExampleBehaviour.cs
  StateBehaviour script that displays the value of DataFlowExampleData received from the input slot in Text.

* DataFlowExampleNewDataCalculator.cs
  Calculator script that outputs new DataFlowExampleData.
  Each field of data uses FlexibeString and FlexibleInt, and is an example of setting an input value from a constant or data flow.

* ArborFSM object
  An example of displaying data received from InputSlot and Calculator which creates a self-made structure.
  (DataFlowExampleBehaviour, DataFlowExampleNewDataCalculator, Random.RangeInt)



## 08(Variable)

Example of adding a custom data type to ParameterContainer.

### Contents

* VariableExampleDataVariable.cs
  Example of generation of Variable for ParameterContainer by Variable Generator.

* VariableExampleBehaviour.cs
  An example StateBehaviour script that displays a value using the generated FlexibleVariableExampleData type.

* VariableExampleSetNameCalculator.cs
  An example Calculator script that handles self-made Variables in a data flow using the generated InputSlotVariableExampleData and OutputSlotVariableExampleData types.

* ParameterContainer object
　An example of using generated VariableExampleData as a parameter.

* ArborFSM object
  An example of a state machine that uses a custom Variable.
  (VariableExampleBehaviour, VariableExampleSetNameCalculator, GetParameter)



## 09(DataLink)

An example of a new function that uses DataLink attributes to input from a data flow.

### Contents

* DataLinkExampleData.cs
  Script that defines the self-made structure DataLinkExampleData.
  We also define OutputSlot to handle self-made structures in data flow.

* DataLinkExampleBehaviour.cs
  StateBehaviour script that displays the value of DataLinkExampleData set using DataLink attribute in Text.

* DataLinkExampleNewDataCalculator.cs
  Calculator script that outputs new DataLinkExampleData.
  Each field of data uses DataLink attribute, and is an example of setting input value from constant or data flow.

* ArborFSM object
  An example of displaying a data received from a data flow with a calculator that creates a self-made structure.
  (DataLinkExampleBehaviour, DataLinkExampleNewDataCalculator, Random.RangeInt)



## 10(Animator)

An example of linking Animator's state machine with ArborFSM.

### Contents

* Animator Parameter Controller
  Simple AnimatorController that only transitions by Trigger parameter

* Animator object
  An example of linking Animator component and ArborFSM.
  (CalcAnimatorParamerter, AnimatorStateTransition)



## 11(Agent)

An example of moving a character using NavMeshAgent.

### Contents

* PatrolAgent object
  A character that rouses around the initial position.
  (AgentMoveToRandomPosition)

* FollowAgent object
  A character that follows the player.
  (AgentFollow)

* EscapeAgent object
  A character that escapes from the player.
  (AgentEscape)

* Patrol-FollowAgent object
  Follow the player within 5 meters with the player. Otherwise it roams around the initial position.
  (AgentMoveToRandomPosition, AgentFollow, DistanceTransition)

* Patrol-EscapeAgent object
  Escape from the player within 5 meters of the player. Otherwise it roams around the initial position.
  (AgentMoveToRandomPosition, AgentEscape, DistanceTransition)

* Waypoint-FollowAgent object
  Follow the player within 5 meters with the player. Otherwise, move on Waypoint.
  (AgentMoveOnWaypoint, AgentFollow, DistanceTransition)



## 12(RaycastFollow)

An example of using Raycast to determine the object that NavMeshAgent follows.

### Contents

* Patrol-EscapeAgent object
  Escape from RaycastFollowAgent within 3 meters with RaycastFollowAgent. Otherwise it roams around the initial position,
  (AgentMoveToRandomPosition, AgentEscape, DistanceTransition)

* RaycastFollowAgent object
  Raycast from your own position while moving on Waypoint, and follow if there is a hit.
  (AgentMoveOnWaypoint, RaycastTransition, RaycastHitTransformCalculator, AgentFollow, AgentMoveToPosition)



## 13(BT Agent)

An example of moving NavMeshAgent using BehaviourTree.

### Contents

* EnemyAnget object
  Follow the player if the distance to the player is less than 5 meters. Otherwise, move on Waypoint.
  (Selector, Sequencer, CalculatorCheck, AgentMoveToTransition, AgentMoveOnWaypoint)



## 14(Graph Hierarcy)

Example of graph hierarchy.

### Contents

* RootFSM object
  This ArborFSM has a child BehaviourTree and a child ArborFSM.
  (SubBehaviourTree, SubStateMachine, EndStateMachine)



## 15(External Graph)

An example of executing a prefabricated graph as a child graph.

### Contents

* RootFSM object
  This ArborFSM instantiates and executes an external BehaviourTree and ArborFSM.
  (SubBehaviourTreeReference, SubStateMachineReference)

* ExternalFSM prefab
  FSM object instantiated from RootFSM with SubStateMachineReference.

* ExternalBT prefab
  BT object instantiated from RootFSM with SubBehaviourTreeReference.
  From here, also execute External FSM with SubStateMachineReference.



## 16(RandomTransition)

Example of random transition with weighted StateLink.

### Contents

* ArborFSM object
  Transition randomly using RandomTransition.
  (RandomTransition)



## 17(Coin Pusher)

An example of creating a simple game (coin pusher) using ArborFSM.

### Contents

* Stage object
  Manage the progress of the game using Arbor FSM.
  Start by pressing the Start button, determine the hand-held score and display the result screen.

* ParameterContainer object
  In possession of Score parameters.

* Pusher object
　Perform move processing front and back.
  (TweenRigidbodyPosition)

* Spawner object
  If space key is input, coin is instantiated if there is hand score.

* Saucer object
  If a coin falls, add a score.



## 18(Roll a Ball)

An example of creating a ball rolling game.

### Contents

* Player object
  A ball object that becomes a player.

* PickUp object
  Items to pick up. You win if you pick them all up.

* Obstacle object
  Obstacle. If you hit, you lose.


## 19(OffMeshLink)

An example of using the OffMeshLink traverse setting of AgentController.

### Contents

* Level object
  The stage where OffMeshLink is set.

* ClickMoveAgent object
  An agent that moves to the clicked position.

* WaypointAgent object
  An agent that moves along a waypoint.

## 20(ObjectPool)

An example of using ObjectPool.

### Contents

* MenuFSM object
  The object that controls the menu.

* BulletSpawnFSM object
  An object that shoots a bullet at the clicked position.
  The object hit by the bullet is stored in the pool.

* ObjectPool object in DontDestroyOnLoad
  The body of the object pool.
  Objects stored in the pool are children of this object.
  If the lifetime is set, the pooled objects will be completely deleted due to the passage of time or scene switching.
  When creating an object from the pool, reuse it if it already exists in the pool.

* Bullet prefab
  Prefab of bullets.
  Tag is set to Finish, and the hit object is deleted (stored in the pool).

* Cube_SceneUnloadedFSM prefab
  Cube with LifeTime Flags set to Scene Unloaded.
  If the scene is unloaded while the pool is stored, it will be permanently deleted.
  Instantiate with "FSM: Scene Unloaded" in the menu.

* Cube_TimeElapsedFSM prefab
  Cube with LifeTimeFlags set to TimeElapsed.
  It will be completely deleted after 5 seconds when the pool is stored.
  Instantiate with "FSM: Time Elapsed" in the menu.

* Cylinder_SceneUnloadedBT prefab
  Cylinder with LifeTimeFlags set to SceneUnloaded.
  If the scene is unloaded while the pool is stored, it will be permanently deleted.
  Instantiate with "BT: Scene Unloaded" in the menu.

* Cylinder_TimeElapsedBT prefab
  Cylinder with LifeTimeFlags set to TimeElapsed.
  It will be completely deleted after 5 seconds when the pool is stored.
  Instantiate with "BT: Time Elapsed" in the menu.
