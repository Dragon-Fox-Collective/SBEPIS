# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/).

## [3.9.5] - 2023-04-10

### Improved

#### Arbor Editor

- Implemented the opposite behavior for "Mouse Wheel Mode" setting when using Ctrl + mouse wheel (Command + mouse wheel on Mac).
- Added support for horizontal scrolling with Shift + mouse wheel on Windows.
- Improved the behavior of maintaining the scroll position and zoom value of the graph until Unity is closed.
- Changed the display width of node comments to automatically adjust to the width of the comment text.
- Improved the profiler measurement when playing in the graph view to minimize the impact of editor processing.

#### Welcome Window

- Changed the setting for automatically opening the welcome window. ([Welcome Window](https://arbor-docs.caitsithware.com/en/manual/window/welcomewindow.html))

#### Scripts

- Calculator: Improved the event parameter subscription to only subscribe to parameters that were actually used during OnCalculate.

### Fixed

#### Arbor Editor

- Fixed an issue where the displayed graph in the graph view did not change when switching to a new external graph that was referenced as a subgraph.
- Fixed an issue where the drag target would shift when performing automatic scrolling while dragging with a changed zoom level.



## [3.9.4] - 2023-03-03

### Improved

#### Arbor Editor

- Improved source code

#### Scripts

- Calculator: improved to subscribe to parameter change events after calling OnCalculate

### Fixed

#### Arbor Editor

- Fixed a bug that the graph tab tree was not updated when an external graph referenced as a subgraph was deleted.
- Fixed so that the scroll position is centered when the graph is displayed for the first time.
- Fixed a bug that the process for updating the window is called even if the ArborEditor window is closed during execution.
- Fixed a bug that the position of the mouse cursor and the position of the node being moved are misaligned when the position is adjusted by the automatic alignment function of the group node while moving the node.

#### Behaviour selection window

- [Unity2022.2 or lator] Fixed a bug where clicking on a highlighted item would not select it.

#### Built in StateBehaviour

- Fixed a bug that an exception occurs when the reference type of External FSM of [SubStateMachineReference](https://arbor-docs.caitsithware.com/en/inspector/behaviours/StateMachine/substatemachinereference.html) is set to Hierarchy.
- Fixed a bug that an exception occurs when the reference type of External BT of [SubBehaviourTreeReference](https://arbor-docs.caitsithware.com/en/inspector/behaviours/BehaviourTree/subbehaviourtreereference.html) is set to Hierarchy.

#### Built in ActionBehaviour

- Fixed a bug that an exception occurs when the reference type of External FSM of [SubStateMachineReference](https://arbor-docs.caitsithware.com/en/inspector/behaviourtree/actions/StateMachine/substatemachinereference.html) is set to Hierarchy.
- Fixed a bug that an exception occurs when the reference type of External BT of [SubBehaviourTreeReference](https://arbor-docs.caitsithware.com/en/inspector/behaviourtree/actions/BehaviourTree/subbehaviourtreereference.html) is set to Hierarchy.

#### Unity support

- Fixed compilation warning in Unity2023.1.0b5.



## [3.9.3] - 2023-01-24

### Fixed

#### Arbor Editor

- Fixed a bug that Failed to unpersist error occurs when playing with an inactive scene graph open in Arbor Editor.
- Fixed unnecessary GC Alloc when stopping at a node breakpoint, even if the Arbor Editor was not opened.

#### Scripts

- Fixed unnecessary GC Alloc when calling the set accessor of NodeGraph.ownerBehaviourObject.



## [3.9.2] - 2022-12-12

### Fixed

#### Arbor Editor

- Fixed a bug where node icon changes were not reflected in the node list tab of the side panel.

### Others

#### Examples

- Changed not to use Unity built-in materials.
  (For URP and HDRP projects, converting the materials under the Examples folder will work properly)



## [3.9.1] - 2022-09-26

### Improved

#### Arbor Editor

- Improved to display an ellipsis when the title name is cut off when the width of the behaviour title bar is narrow.

#### ArborFSM

- Improved to allow immediate transition at the transition destination of OnGraphStopTransition.

#### Data flow

- GC Alloc reduction when passing Ray, Ray2D, RaycastHit, RaycastHit2D.

### Fixed

#### ArborFSM

- Fixed a bug that only the first StateBehaviour is executed at the transition destination of OnGraphStopTransition.
- Fixed a bug that the transitioned connection line was still highlighted when stopping the state machine.

#### Behaviour Tree

- Fixed a bug that the "Disconnect" text in the node connection line menu does not reflect when switching language settings.

#### Parameter Container

- Fixed a bug that the same group name set with the AddVariableName attribute was not combined into one.

#### Built in Calculator

- Fixed Ray.Decompose menu name.

#### Scripts

- Fixed namespace of IValueSetter to Arbor.ValueFlow.



## [3.9.0] - 2022-07-29

### Added

#### Arbor Editor

- Arbor Editor window now supports UI Elements.  
  (In the custom editor of the script for the node, you can implement the UIElements compatible editor by implementing the CreateInspectorGUI () method.)
- Added the function to open the target node with Arbor Editor when it stops at the breakpoint of the node.  
  (Enable by opening the settings menu in the Arbor Editor window and turning on "Open node at breakpoint")
- Added "Highlight Script" to the settings menu in the behavior title bar.
- Added "Highlight Editor Script" to the settings menu in the behavior title bar.

#### Hierarchy

- Added the ability to display Arbor related component icons in Hierarchy GameObject.  
  (Enable by opening the Settings menu in the Arbor Editor window and turning on "Show icons in Hierarchy")

#### ObjectPool

- Added lifetime settings for objects stored in the pool.

#### Behaviour Tree

- Added a function to logically calculate the judgment results of multiple Decorators.

#### Built in Component

- Added behavior to [AgentController](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/agentcontroller.html)  
  See [Agent behavior control](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/agentcontroller/behaviours.html) for more information.
  - Hide that moves to a position hidden by obstacles
  - Pursuit that moves to the predicted position considering the movement speed of the target
  - Interpose that moves to a position that interrupts between two objects in consideration of the moving speed
  - Wander that moves to a position that randomly changes direction with respect to the current direction of travel
  - Evade that moves away from the predicted position considering the movement speed of the target
- Added behavior control when passing OffMeshLink to AgentController.  
  For details, see [AgentController#Traverse Data](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/agentcontroller.html#TraverseData) and [Setup example](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/agentcontroller/example.html).
- Added clearVelocity argument to [AgentController.Stop](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor/Types/AgentController/M-Stop.html).
- Added checkRaycast argument to [AgentController.MoveToRandomPosition](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor/Types/AgentController/M-MoveToRandomPosition.html).
- Added component [MovingEntity](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/movingentity.html) for predicting the destination in consideration of moving speed.
  - Added [MovingEntityCharacterController](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/movingentity/movingentitycharactercontroller.html)
  - Added [MovingEntityNavMeshAgent](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/movingentity/movingentitynavmeshagent.html)
  - Added [MovingEntityRigidbody](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/movingentity/movingentityrigidbody.html)
  - Added [MovingEntityTransform](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/movingentity/movingentitytransform.html)
- Added [OffMeshLinkSettings](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/offmeshlinksettings.html)
- Added [AnimationTriggerEventReceiver](https://arbor-docs.caitsithware.com/en/3.9.0/manual/builtin/animationtriggereventreceiver.html)

#### Built in StateBehaviour

- Added [SetRendererColor](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderercolor.html)
- Added [SetRendererFloat](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrendererfloat.html)
- Added [SetRendererMaterial](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderermaterial.html)
- Added [SetRendererTexture](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderertexture.html)
- Added [SetRendererTextureOffset](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderertextureoffset.html)
- Added [SetRendererTextureScale](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderertexturescale.html)
- Added [SetRendererVector](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Renderer/setrenderervector.html)
- Added [AnimatorPlay](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Animator/animatorplay.html)
- Added [AddComponent](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Component/addcomponent.html)
- Added [DestroyComponent](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Component/destroycomponent.html)
- Added [RestartScene](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Scene/restartscene.html)
- Added [PauseNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/NodeGraph/pausenodegraph.html)
- Added [PlayNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/NodeGraph/playnodegraph.html)
- Added [ResumeNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/NodeGraph/resumenodegraph.html)
- Added [StopNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/NodeGraph/stopnodegraph.html)
- Added [AgentEscapeFromPosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentescapefromposition.html)
- Added [AgentEvade](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentevade.html)
- Added [AgentHideFromPosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agenthidefromposition.html)
- Added [AgentHideFromTransform](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agenthidefromtransform.html)
- Added [AgentInterpose](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentinterpose.html)
- Added [AgentPursuit](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentpursuit.html)
- Added [AgentWander](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentwander.html)
- Added pool lifetime settings (Life Time Flags and Life Duration fields) to [InstantiateGameObject](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/GameObject/instantiategameobject.html).
- Added Ignore UI field to MouseButton**Transition.  
  The related scripts are:
  - [MouseButtonDownTransition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Transition/Input/mousebuttondowntransition.html)
  - [MouseButtonTransition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Transition/Input/mousebuttontransition.html)
  - [MouseButtonUpTransition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Transition/Input/mousebuttonuptransition.html)
- Added Clear Velocity field to [AgentStop](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentstop.html).
- Added Clear Velocity On Stop field to Agent scripts.
- Added CantMove StateLink to Agent scripts.
- Added Check Raycast field to [AgentMoveToRandomPosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentmovetorandomposition.html).
- Added Radius field to [AgentMoveOnWaypoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentmoveonwaypoint.html).
- Added MaterialIndex field to Renderer related Tween scripts  
  The related scripts are:
  - [TweenColor](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweencolor.html)
  - [TweenColorSimple](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweencolorsimple.html)
  - [TweenMaterialFloat](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweenmaterialfloat.html)
  - [TweenMaterialVector2](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweenmaterialvector2.html)
  - [TweenTextureOffset](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweentextureoffset.html)
  - [TweenTextureScale](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Tween/tweentexturescale.html)

#### Built in ActionBehaviour

- Added [Idle](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Classes/idle.html)
- Added [PauseNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/NodeGraph/pausenodegraph.html)
- Added [PlayNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/NodeGraph/playnodegraph.html)
- Added [ResumeNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/NodeGraph/resumenodegraph.html)
- Added [StopNodeGraph](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/NodeGraph/stopnodegraph.html)
- Added [AgentEvade](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentevade.html)
- Added [AgentHideFromPosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agenthidefromposition.html)
- Added [AgentHideFromTransform](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agenthidefromtransform.html)
- Added [AgentInterpose](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentinterpose.html)
- Added [AgentPursuit](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentpursuit.html)
- Added [AgentWander](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentwander.html)
- Added Clear Velocity field to [AgentStop](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentstop.html).
- Added Clear Velocity On Stop field to Agent scripts.
- Added Check Raycast field to [AgentMoveToRandomPosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentmovetorandomposition.html).
- Added Radius field to [AgentMoveOnWaypoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/actions/Agent/agentmoveonwaypoint.html).

#### Built in Decorator

- Added [DistanceCheck](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/decorators/Classes/distancecheck.html)

#### Built in Calculator

- Added [Enum.Equals](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Enum/enumequalscalculator.html)
- Added [Enum.NotEquals](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Enum/enumnotequalscalculator.html)
- Added [Enum.ToInt](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Enum/enumtointcalculator.html)
- Added [Enum.ToString](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Enum/enumtostringcalculator.html)
- Added [Enum.TryParse](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Enum/enumtryparsecalculator.html)
- Added [Int.ToEnum](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Int/inttoenumcalculator.html)
- Added [GetRendererColor](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderercolor.html)
- Added [GetRendererFloat](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrendererfloat.html)
- Added [GetRendererMaterial](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderermaterial.html)
- Added [GetRendererTexture](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderertexture.html)
- Added [GetRendererTextureOffset](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderertextureoffset.html)
- Added [GetRendererTextureScale](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderertexturescale.html)
- Added [GetRendererVector](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getrenderervector.html)
- Added [GetSprite](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Renderer/getsprite.html)
- Added [Camera.ScreenPointToRay](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/camerascreenpointtoraycalculator.html)
- Added [Camera.ScreenToViewportPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/camerascreentoviewportpointcalculator.html)
- Added [Camera.ScreenToWorldPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/camerascreentoworldpointcalculator.html)
- Added [Camera.ViewportPointToRay](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/cameraviewportpointtoraycalculator.html)
- Added [Camera.ViewportToScreenPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/cameraviewporttoscreenpointcalculator.html)
- Added [Camera.ViewportToWorldPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/cameraviewporttoworldpointcalculator.html)
- Added [Camera.WorldToScreenPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/cameraworldtoscreenpointcalculator.html)
- Added [Camera.WorldToViewportPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Camera/cameraworldtoviewportpointcalculator.html)
- Added [Input.GetMousePosition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Input/inputgetmousepositioncalculator.html)
- Added [Ray.Compose](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raycomposecalculator.html)
- Added [Ray.Decompose](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raydecomposecalculator.html)
- Added [Ray.GetDirection](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raygetdirectioncalculator.html)
- Added [Ray.GetOrigin](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raygetorigincalculator.html)
- Added [Ray.GetPoint](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raygetpointcalculator.html)
- Added [Ray.SetDirection](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raysetdirectioncalculator.html)
- Added [Ray.SetOrigin](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raysetorigincalculator.html)
- Added [Ray.ToString](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/calculators/Ray/raytostringcalculator.html)

#### Built in scripts

- Added a function to logically operate the judgment results of multiple elements in the Calculator Condition.  
  The related scripts are:
  - [(StateBehaviour)CalculatorTransition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Transition/calculatortransition.html)
  - [(Decorator)CalculatorCheck](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/decorators/Classes/calculatorcheck.html)
  - [(Decorator)CalculatorConditionalLoop](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/decorators/Classes/calculatorconditionalloop.html)
- Added a function to logically operate the judgment results of multiple elements in Parameter Condition.  
  The related scripts are:
  - [(StateBehaviour)ParameterTransition](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Transition/parametertransition.html)
  - [(Decorator)ParameterCheck](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/decorators/Classes/parametercheck.html)
  - [(Decorator)ParameterConditionalLoop](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviourtree/decorators/Classes/parameterconditionalloop.html)

#### Scripts

- Supports async / await.  
  See [async/await](https://arbor-docs.caitsithware.com/en/3.9.0/manual/scripting/async_await.html) for more information.
  - Wait until the next OnUpdate call with the [PlayableBehaviour.Yield](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.Playables/Types/PlayableBehaviour/M-Yield.html) method.
  - Wait until the next OnExecute call with the [ActionBehaviour.WaitForExecute](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.BehaviourTree/Types/ActionBehaviour/M-WaitForExecute.html) method.  
    (If you wait with the WaitForExecute method, it is essentially the same as calling inside OnExecute, so you can use the FinishExecute method.)
  - Added [PlayableBehaviour.CancellationTokenOnEnd](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.Playables/Types/PlayableBehaviour/P-CancellationTokenOnEnd.html) property.
  - UniTask compatible.  
    If UniTask is installed, it can be converted to UniTask with Yield().ToUniTask()  
	For details, refer to [Use of UniTask](https://arbor-docs.caitsithware.com/en/3.9.0/manual/extra/unitask.html).
- Added [AnimatorParameterReference.nameHash](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor/Types/AnimatorParameterReference/P-nameHash.html) property.
- Added [Decotator.OnRevaluationEnter](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.BehaviourTree/Types/Decorator/M-OnRevaluationEnter.html) method.
- Added [Decorator.OnRevaluationExit](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.BehaviourTree/Types/Decorator/M-OnRevaluationExit.html) method.

#### Examples

- Added 19(OffMeshLink) as an example of OffMeshLink traverse setting of AgentController.
- Added 20(ObjectPool) as an example of ObjectPool.

### Changed

#### Arbor Editor

- Supports node name change for all node types.
- Changed to display the behavior title bar on the calculator node.

#### Behaviour Tree

- Changed to automatically display AbortFlags field and Boolean field even if Decorator's custom editor is implemented.
- Changed the default behavior of ActionBehaviour.OnExecute from "return failure" to "do nothing".

#### ObjectPool

- Changed to stop when the graph is pooled.
- Changed to start playback if playOnStart is true when the graph returns from the pool.
- Changed to sleep when Rigidbody is pooled.
- Changed to wake up when Rigidbody resumes from the pool.
- Changed to sleep when Rigidbody2D is pooled.
- Changed to wake up when Rigidbody2D resumes from the pool.

#### Built in Component

- Changed the Add Component menu of Agent Controller to "Arbor > Navigation > Agent Controller".
- Changed Waypoint's Add Component menu to "Arbor > Navigation > Waypoint".
- Changed AgentController to derive from MovingEntity class.
- Renamed AgentController.Follow to [MoveTo](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor/Types/AgentController/M-MoveTo.html)

#### Built in StateBehaviour

- Renamed AgentEscape to [AgentEscapeFromTransform](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Agent/agentescapefromtransform.html)
- Renamed LoadLevel to [LoadScene](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Scene/loadscene.html)
- Renamed UnloadLevel to [UnloadScene](https://arbor-docs.caitsithware.com/en/3.9.0/inspector/behaviours/Scene/unloadscene.html)

#### Built in Decorator

- Changed so that it does not interrupt even if AbortFlags.LowerPriority is set in TimeLimit.

#### Parameter Container

- Changed the parameter addition menu to the Add Component menu format.

#### Unity support

- Raised Unity's minimum action version to 2019.4.0f1.
- Support for Unity2022.2.0b2

#### Examples

- Unified namespace to "Arbor.Examples".
- Unified the group of AddComponent menu to "Arbor > Examples".
- Unify the group of behavior addition menu to "Examples".

### Improved

#### Data flow

- Improved enum type boxing
  (To minimize the boxing of enum types, register the target type with [ValueMediator.RegisterEnum<T>()](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor.ValueFlow/Types/ValueMediator/MS-RegisterEnum.html))

#### Built in Component

- Adjusted the behavior so that it does not fit as much as possible when cornered by the wall with [AgentController.Escape](https://arbor-docs.caitsithware.com/en/3.9.0/scriptreference/Arbor/Types/AgentController/M-Escape.html).

#### Built in scripts

- Improved boxing of SetParameter
- Added support to disable related scripts when the Unity UI package is removed.
- When Active Input Handling is set to "Input System Package (New)", the related script is disabled.



## [3.8.11] - 2022-06-27

### Fixed

#### Arbor Editor

- Fixed a bug that the scroll position becomes an invalid value when the graph is selected when the graph setting window is opened while the graph is not selected.

#### Behaviour Tree

- Fixed a bug that when the Loop decorator is used, the decorator is executed without being re-evaluated when the child node is re-executed.



## [3.8.10] - 2022-03-14

### Fixed

#### Built in scripts

- Fixed a bug that an exception occurs in related scripts when AnimatorOverrideController is specified for Animator.
  - AgentController
  - AnimatorCrossFade
  - AnimatorSetLayerWeight
  - AnimatorStateTransition
  - CalcAnimatorParameter
  
  

## [3.8.9] - 2021-12-27

### Added

#### Parameter Container

- Added deletion of Parameter Container associated with the graph.

### Improved

#### Type selection window

- Improved so that groups without selectable elements are not displayed when a filter is set.

#### Member selection window

- Improved so that groups without selectable elements are not displayed when a filter is set.

#### Built in scripts

- Improved to set type constraints at runtime.

### Fixed

#### Arbor Editor

- [Unity2022.1.0b2] Fixed a bug that nothing is displayed in Arbor Editor.

#### Parameter Container

- Fixed a bug that internal objects are not deleted when a parameter of VariableList type is deleted.

#### Built in scripts

- [Built-in script that can change the type that can be connected to the data slot] Fixed a bug that the operation that disconnects the data slot cannot be restored even if it is undone.



## [3.8.8] - 2021-12-02

### Improved

#### Arbor Editor

- Improved to switch to the additional selection mode with the Shift key and the exclusion mode with the Ctrl key while selecting the rectangle of the node.

### Fixed

#### Arbor Editor

- Fixed a bug that the logo changes its size due to the influence of the zoom of the graph when the logo is always displayed.
- Fixed a bug that the graph is not switched to unselected when the referenced graph is discarded.
- Fixed a bug that node comment is displayed when node comment is deleted and another node is created immediately.
- Fixed a bug that the display does not switch even if Undo / Redo is performed after switching the display of node comments.
- Fixed to exit rename mode when the window is out of focus while renaming a node.

#### Built in scripts

- Fixed a bug that when Once is specified in the Tween system, it will return to the state at the start of Tween if it does not transition after the lapse of time.



## [3.8.7] - 2021-11-08

### Fixed

#### Arbor Editor

- [Unity 2021.2 or later] Fixed a bug that the display range of the graph was shifted when zooming the graph.
- [Unity 2021.2 or later] Fixed a bug that the warning message "Should not be capturing when there is a hotcontrol" was displayed when dragging in the graph after selecting the transition destination from the node list, and the dragging state was not released even if the left button was released.
- [Unity2021.2 or later] Fixed a bug that NullReferenceException occurs after compiling when clicking in the graph during compilation.
- Fixed a bug where zooming a graph would also change the drag auto-scroll area.
- Fixed a bug that some parts of the auto-scrolling dragging area of graphs were not responding.

#### Built in scripts

- Fixed a bug that ParameterTypeMismatchException occurs when executing with List type specified in the argument of SubStateMachine.



## [3.8.6] - 2021-10-01

### Fixed

#### Arbor Editor

- Fixed a bug that drag scroll mode remains when a parameter is dragged from Parameter Container and released outside the graph area.
- Fixed a bug that the State behavior insertion button disappears when the parameter is dragged from the Parameter Container and released outside the graph area.

#### Parameter Container

- [Unity 2021.1 or later] Fixed a bug that parameters are not displayed when scrolling Parameter Container.

#### Built in scripts

- [Unity2021.1 or later] Fixed a bug that an exception occurs when changing the parameter referenced in ParameterTransition.



## [3.8.5] - 2021-09-02

### Fixed

#### Data flow

- Fixed a bug where the data value displayed at runtime was of type Color regardless of the type.

#### Parameter Container

- Fixed a bug in which Variable data for internal parameters remained when SubStateMachine or SubBehaviourTree was deleted.
- Fixed a bug that an exception occurs when adding an element when a type other than Component is specified for the ComponentList parameter.
- [Unity2020.2 or later] Fixed a bug that an exception occurs when searching for parameters.



## [3.8.4] - 2021-08-13

### Changed

#### Scripts

- Changed "bool GetValue<T>(ref T)" of InputSlotBase class to "bool TryGetValue<T>(out T)".

### Improved

#### Built in scripts

- Reduced boxing of List related scripts such as ListAddElement.
- Improved to use the same graph in "Use Directory In Scene" of SubStateMachineReference and SubBehaviourTreeReference.

#### Scripts

- Improved to set virtual to Unity callback method of non-sealed class.

### Fixed

#### ArborFSM

- Fixed a bug that an infinite loop occurs when SendTrigger is sent to the own FSM and an attempt is made to transition from the resident state TriggerTransition with TrantisionTiming.Immediate.

#### Scripts

- Fixed a bug that a compile error occurs when ARBOR_DISABLE_DEFAULT_EDITOR is set in Scripting Define Symbols.



## [3.8.3] - 2021-07-16

### Fixed

#### Arbor Editor

- Fixed a bug that an exception occurs when displaying a node comment.



## [3.8.2] - 2021-07-01

### Fixed

#### Type selection window

- Fixed TypeLoadException when clicking type selection of Component parameter of ParameterContainer.

#### Welcome window

- Fixed a bug that the Welcome window was displayed when UnityEditor was started in batch mode.

#### Unity support

- [Unity2021.2.0b1] Fixed a bug that Arbor windows did not work properly due to an exception.

#### Documentation

- Fixed that the text was partly Test.



## [3.8.1] - 2021-06-18

### Added

#### Built in scripts

- Added FixedUnscaledTime to WaypointTimeType used by TransformMoveOnWaypoint.

#### Scripts

- Added Timer class to measure elapsed time.
- Added FixedTime and FixedUnscaledTime to TimeType.

### Changed

#### BehaviourTree

- Changed to call OnGraphPause / OnGraphResume of the decorator to be re-evaluated.

#### Scripts

- Changed to call OnDrawGizmos / OnDrawGizmosSelected of NodeBehaviour.

### Improved

#### Graph

- Improved graph to disable paused time lapse
  - SpecifySeconds in UpdateSettings.
  - TimeTransition, Tween, Agent, MoveOnWaypoint
  - Wait action, Agent action, TimeLimit decorator, Cooldown decorator

### Fixed

#### Documentation

- Fixed some properties not being documented.



## [3.8.0] - 2021-05-20

### Added

#### Arbor Editor

- Added a minimap to the side panel.

#### ArborFSM

- The following types have been added to TransitionTiming.
  - NextUpdateOverwrite
  - NextUpdateDontOverwrite(Change the initial value to this)

#### ParameterContainer

- Added the following types of parameters.
  - Vector4
  - Vector2Int
  - Vector3Int
  - RectInt
  - BoundsInt
  - Vector4List
  - Vector2IntList
  - Vector3IntList
  - RectIntList
  - BoundsIntList

#### Data flow

- Added recalculate mode setting to calculation nodes.

#### Built-in Calculator

- Add an calculator node that uses a member of each of the following types
  - int
  - long
  - float
  - bool
  - string
  - Vector2
  - Vector3
  - Vector4
  - Quaternion
  - Rect
  - Bounds
  - Color
  - Vector2Int
  - Vector3Int
  - RectInt
  - BoundsInt

#### Scripts

- Added OnStateFixedUpdate method to StateBehaviour.
- Added OnFixedUpdate method to TreeNodeBehaviour.

#### Welcome Window

- Added zip download button for documents.

### Changed

#### Arbor Editor

- Changed the graph tab in the side panel to a graph tree.
- Separate the node list in the side panel into tabs.
- Changed the menu displayed from the Create Graph button to a selection window.
- Changed to be able to distinguish between unconnected State reroute nodes and data reroute nodes.

#### ArborFSM

- Changed StateLink to show the settings window by right-clicking on it.

#### Built-in Scripts

- Replace Update(), LateUpdate(), and FixedUpdate() with callback methods for Arbor.
- AgentController
  - Rename the Patrol method to MoveToRandomPosition.
  - Changed the IsDivAgentSpeed parameter to a SpeedType enum parameter.
  - Added SpeedDivValue parameter.
  - Abolished the isDivAgentSpeed property and replaced it with the speedType property.

#### Built-in StateBehaviour

- Added UseDirectlyInScene flag to allow SubStateMachineReference and SubBehaviourTreeReference to treat other graphs in the scene as subgraphs.
- Rename AgentPatrol to AgentMoveToRandomPosition.

#### Built-in ActionBehaviour

- Added UseDirectlyInScene flag to allow SubStateMachineReference and SubBehaviourTreeReference to treat other graphs in the scene as subgraphs.
- Rename AgentPatrol to AgentMoveToRandomPosition.

#### Scripts

- Changed the animator field of AnimatorParameterReference to be specified by FlexibleComponent.

#### Examples

- Added models for agents.

#### Unity support

- Raised Unity's minimum action version to 2018.4.0f1.

### Improved

#### Data flow

- Reduced boxing when outputting value types to dataflow.

#### Built-in Calculator

- Reduced boxing of List system calculation nodes.

#### Scripts

- Reduce the number of foreaches used internally.
- Reduced the number of lambda expressions used internally.
- Reduced GC Alloc by GetComponent, GetComponents, GetComponentsInChildren, etc.
- Improved the internal string determination to use StringComparison.

### Fixed

#### Arbor Editor

- Fixed a bug that an exception was raised when adding behavior during compilation.
- Fixed a bug that an exception was generated when copy-pasting nodes of different types of graphs.
- Fixed a bug that the selected state of a graph did not return after Undoing the Remove Component of a graph component while it was selected in the Arbor Editor.
- Fixed a bug that only the name of a node was returned when it was created and undone.

#### ArborFSM

- Fixed that TriggerTransition does not respond to SendTrigger immediately after the start of execution when two FSMs are started simultaneously.

### Deprecated

#### Scripts

- Changed DataSlotField class to Obsolete.

#### Documentation

- Removed the documentation zip from the package.



## [3.7.9] - 2021-03-24

### Fixed

#### Arbor Editor

- [Unity2017.2 - 2018.4] Fixed a bug that scrolling in the graph does not work after capturing the graph.
- Fixed a bug that the color of the graph captured image changes when the Color Space of Project Settings is set to Linear.
- Fixed a bug that an exception occurs when dragging and dropping a state with SubStateMachine to another state's SubStateMachine.

#### ParameterContainer

- Fixed a bug that garbage data remains when the parameters of EnumList, ComponentList, AssetObjectList are deleted.

#### Unity support

- [Unity2021.1.0f1 or later] Supports deletion of elements in Waypoint, WeightList, etc.



## [3.7.8] - 2020-12-21

### Fixed

#### ArborFSM

- Fixed a bug that an exception occurs when StateLink is displayed at the top of the node.

#### Type selection window

- Fixed a bug that some Assembly types are not displayed.
- Fixed a bug that the hierarchy does not switch when opening and closing with the left and right arrow keys.

#### Package Manager

- Fixed a bug that could not be imported as a package.



## [3.7.7] - 2020-12-12

### Fixed

#### Data flow

- Fixed the reference being cut off when the type referenced in ClassTypeReference is moved to another assembly.
- Fixed RenamedFromAttribute to work with UWP (.NET) builds.



## [3.7.6] - 2020-12-08

### Fixed

#### BehaviourTree

- Fixed "Paste" judgment in the Insert menu of Decorator and Service.



## [3.7.5] - 2020-10-27

### Fixed

#### Arbor Editor

- Fixed a bug that drag was interrupted illegally when right-clicking while dragging.
- Fixed a bug that changing the color of the group node changes the color of the button on the header.

#### Scripts

- Fixed to separate the methods instead of using the arguments added by ArborFSM.SendTrigger as default arguments.



## [3.7.4] - 2020-09-29

### Fixed

#### Data flow

- Fixed a bug that an exception occurs during actual play with IL2CPP build when using a value type input slot.
- Fixed a bug that an exception occurs during actual play with IL2CPP build when using a list.  
  *When building with pre-compilation (AOT) such as IL2CPP, refer to [Ahead-of-Time (AOT) Restrictions](https://caitsithware.com/assets/arbor/docs/en/manual/dataflow/list.html#AOTRestrictions).

#### ArborEvent(InvokeMethod/GetValue)

- Fixed a bug that caused an exception when trying to call a member of an interface.

#### Built in StateBehaviour

- Fixed a bug that the output instance is not changed when NewArray is specified in List.AddElement.
- Fixed a bug that an exception occurs when trying to delete the last element by specifying NewArray in List.RemoveAtIndex.
- Fixed a bug that an exception occurs when trying to add an element at the end by specifying NewArray in List.InsertElement.



## [3.7.3] - 2020-09-22

### Changed

#### ArborEvent(InvokeMethod/GetValue)

- Changed not to exclude by argument in method selection window.

### Fixed

#### Arbor Editor

- Fixed a bug that an exception occurred when there were multiple files with the same name as the class and split in partial.
- [Unity2019 or later] Fixed a bug that dragging and dropping a GameObject other than the graph view in the Arbor Editor window would leave the graph in auto-scroll mode.

#### Unity support

- [Unity2020.2.0b2] Fixed Obsolete warning.

#### ArborEvent(InvokeMethod/GetValue)

- Fixed a bug that members of classes other than Unity object type cannot be called.
- Fixed a bug that the type of the output slot of the instance is not specified normally when the specified class type is changed to struct later.

#### Other

- Fixed a bug that caused an error message that ArborSettings.asset could not be loaded when using Arbor with multiple Unity versions.



## [3.7.2] - 2020-09-11

### Added

#### Arbor Editor

- When the behavior has the Obsolete attribute, it corresponds to display a message under the title bar.

#### Scripts

- Added RendererPropertyBlock class that can be managed for each Renderer to which MaterialPropertyBlock is assigned.

### Fixed

#### Arbor Editor

- Fixed a bug that the pop-up window is displayed at a distant position when opening the pop-up window from the menu.
- Fixed a bug that the layout of Editor was broken when a non-serializable type was specified for FlexibleField<T>.
- [Unity2020.1 or later] Fixed a bug that the warning "GetDataSlotField: DataSlotField was not found." Is displayed and the slot is not displayed when a generic class containing a data slot is defined in the field.

#### Built in StateBehaviour

- Fixed a bug that NullReferenceException occurs when the InputTargets field of ExistsGameObjectTransition is not used.
- Fixed a bug where the warning "Get DataSlotField: DataSlotField was not found." Was displayed when opening a graph using Parameter Transition in an older version of Arbor.
- Fixed a bug that the initial value of the material is not reflected in the script called later when using two or more StateBehaviour that change the property of the material of the same Renderer.
  - TweenColor
  - TweenColorSimple
  - TweenMaterialFloat
  - TweenMaterialVector
  - TweenTextureOffset
  - TweenTextureScale
- Fixed an issue where "Game scripts or other custom code contains OnMouse_ event handlers." Was displayed when building on some platforms.
  If you want to use StateBehaviour on the corresponding platform (Android, iOS, Windows Store Apps, Apple TV), please add ARBOR_ENABLE_ONMOUSE_EVENT to Scripting Define Symbols.
  - OnMouseDownTransition
  - OnMouseDragTransition
  - OnMouseEnterTransition
  - OnMouseExitTransition
  - OnMouseOverTransition
  - OnMouseUpAsButtonTransition
  - OnMouseUpTransition

#### Scripts

- [Compatible with Unity 2020.1] Fixed to cancel the abstract specification of OutputSlot<T> and InputSlot<T> so that it can be used directly in the field.



## [3.7.1]　- 2020-09-04

### Added

#### Welcome window

- Added a link to the review page.

#### Built in StateBehaviour

- AddForceRigidbody, AddVelocityRigidbody, SetVelocityRigidbody, AddForceRigidbody2D, AddVelocityRigidbody2D, SetVelocityRigidbody2D
  - Added "ExecuteMethodFlags" field to specify the execution method.
  - Added "DirectionType" field to specify direction type.
- SetParameter
  - Added "ExecuteMethodFlags" field to specify the timing of parameter setting.
- RigidbodyMoveOnWaypoint, Rigidbody2DMoveOnWaypoint, TransformMoveOnWaypoint
  - Added "Offset" field for position adjustment from waypoint.
- DistantTransform
  - Added "Transform" field to specify comparison Transform.
- ExistsGameObjectTransition
  - Added "InputTargets" field that receives an array of GameObjects.
  - Added "CheckActive" field that determines whether or not it is active.
  - Added "SomeExistsState" field that transitions if at least one object exists.

#### Built in Calculator

- Added Input/Input.GetAxis, Input/Input.GetAxisRaw.
- Added String/ToString.
- Added Scene/GetActiveSceneName.

#### Scripts

- Added TagSelectorAttribute that displays the tag selection popup when the type of FlexibleString is Constant.
- Added FlexibleField of Unity related.
  - FlexibleForceMode
  - FlexibleForceMode2D
  - FlexibleKeyCode
  - FlexibleSpace
  - FlexibleLoadSceneMode
  - FlexibleLayerMask
  - FlexibleInputButton
- Added FlexibleField of Arbor core related.
  - FlexibleExecuteMethodFlags
  - FlexibleTimeType
  - FlexibleSendTriggerFlags
  - FlexibleTransitionTiming
- Added FlexibleField of Builtin script related.
  - FlexibleDirectionType
  - FlexibleAgentUpdateType
  - FlexibleMoveWaypointType
  - FlexiblePatrolCenterType
  - FlexibleTweenMoveType
  - FlexibleUpdateMethodType
  - FlexibleCalcFunction
  - FlexibleCompareType
  - FlexibleInterpolateType
  - FlexiblePostureType
  - FlexibleTexcoordVector2Type
  - FlexibleWaypointTimeType

#### Examples

- Added Example 18(Roll a Ball).

### Changed

#### Built in scripts in general

- Changed to use FlexibleField for fields that do not use FlexibleField.

#### Built in StateBehaviour

- Changed UISetSliderFromParameter not to cache Slider (same processing as other UISet...FromParameter).

### Fixed

#### Arbor Editor

- Fixed a bug that the connection information of the copy source remains when you copy and paste the node to which the data slot is connected.

#### Built in scripts in general

- Fixed the disconnection process of slots that are hidden when switching types.

#### Built in StateBehaviour

- SetParameter
  - Fixed to cast to the correct type when setting the value.
  - Fixed a bug that if you copy and paste SetParameter that refers to a parameter in a graph to another graph, it looks like it refers to a parameter in the copy destination graph.

#### Built in ActionBehaviour

- SetParameter
  - Fixed to cast to the correct type when setting the value.
  - Fixed a bug that if you copy and paste SetParameter that refers to a parameter in a graph to another graph, it looks like it refers to a parameter in the copy destination graph.

#### Built in Calculator

- Fixed a bug that an exception occurs when opening another scene while displaying a graph with Random.SelectComponent.
- GetParameter
  - Fixed a bug that if you copy and paste GetParameter that refers to a parameter in a graph to another graph, it looks like it refers to a parameter in the copy destination graph.

#### Scripts

- Fixed a bug that InvalidCastException occurs when passing int type value in Parameter.value to Enum type parameter.
- Fixed a bug that an exception occurs when trying to cast to interface with DynamicUtility.Cast method.



## [3.7.0] - 2020-08-24

### Added

#### Arbor Editor

- Added the HelpURL attribute so that it can be used to specify the URL displayed by the behavior help button.
- Added the word "Deprecated" to the title bar when the behavior has the Obsolete attribute.
- Added to display the word (Missing) when the type referenced by ClassTypeReference cannot be found.
- Added so that the playback status of a graph is displayed below the graph label during play.
- Added the mode to always display the logo.
- Added help button for each type to the additional menu of NodeBehaviour.
- Added welcome window.
- [Unity 2018.1 or later] Supported behavior presets.
- [Unity 2018.1 or later] In the language setting of Arbor Editor, added a mode to refer to the language setting of Unity Editor.
- [Unity 2019.3 or later] Supported Reload Domain off in Enter Play Mode Settings (Reload Scene off is not supported).

#### ArborFSM

- Added the item to select the connection destination in the node list to the menu when dragging and dropping StateLink on the graph.
- Added the setting item of StateLink display to "View > StateLink" on the toolbar.
- Visualize pending transitions and destination states in purple if the graph becomes invalid during play.
- Visualize StateBehaviour that was not active in purple when the graph was disabled during play.

#### BehaviourTree

- Added mode to move child node when dragging parent node (Win: Alt+drag, Mac: option+drag)

#### ParameterContainer

- The following types have been added to the parameters.
    - AssetObject
	- IntList
	- LongList
	- FloatList
	- BoolList
	- StringList
	- EnumList
	- Vector2List
	- Vector3List
	- QuaternionList
	- RectList
	- BoundsList
	- ColorList
	- GameObjectList
	- ComponentList
	- AssetObjectList
	- VariableList

#### DataFlow

- Added "View > DataSlot > Show Flexibly" on the toolbar.
    - Displayed outside the node if the slot is connected.
	- If it is not connected, it will be displayed outside the node if the dragged slot can be connected.
- Added Parent Graph to Hierarchy Type of Flexible Component and Flexible Game Object.

#### ArborEvent(InvokeMethod)

- Added the ability to set values for fields and properties.
- Added so that types other than Component can be specified.
- Added so that static class can be specified.
- Add search filter to type selection popup.
- Add search filter to member selection popup.

#### Built-in StateBehaviour

- Added OnGraphStopTransition for transitioning when the graph is stopped to perform termination processing.
- Add behavior to change array.
    - List.AddElement
	- List.Clear
	- List.InsertElement
	- List.RemoveAtIndex
	- List.RemoveElement
	- List.SetElement
- Added SendTriggerFlags to SendTrigger.
    - SendTrigger
	- SendTriggerUpwards
	- SendTriggerGameObject
	- BroadcastTrigger
- Added a field that can directly set a value to SetParameter.
- Added a field to specify coordinates directly to InstantiateGameObject.
- Added a field to specify the timing to make a judgment in CalculatorTransition.
- Added a field to AnimatorCrossFade that does not cause transition if Animator is already in transition to the specified state.
- Added StateLink that transitions Arbor side when the state of Animator side is completed in AnimatorCrossFade.
- Added a flag to AnimatorCrossFade that uses Animator.CrossFadeInFixedTime.
- Added a flag to the MoveOnWaypoint related script to clear the target point when restarting.
    - AgentMoveOnWaypoint
	- RigidbodyMoveOnWaypoint
	- Rigidbody2DMoveOnWaypoint
	- TransfomMoveOnWaypoint

#### Built-in ActionBehaviour

- Added InvokeMethod.
- Added a field that can directly set a value to SetParameter.
- Added a type that can specify coordinates directly to InstantiateGameObject.
- Added a field to AnimatorCrossFade that does not cause transition if Animator is already in transition to the specified state.
- Added a field to AnimatorCrossFade that waits for the state of Animator to complete the transition.
- Added a field to AnimatorCrossFade to check whether the state on the Animator side has successfully transitioned.
- Added a flag to AnimatorCrossFade that uses Animator.CrossFadeInFixedTime.
- Added CalcParameter.
- Added a flag to AgentMoveOnWaypoint to clear the target point when restarting.

#### Built-in Calculator

- Added a calculation node GetValueCalculator that gets the values of member fields and properties and outputs them to the data flow.
- Added Calculator related to array.
    - NewArrayList
	- List.Contains
	- List.Count
	- List.GetElement
	- List.IndexOf
	- List.LastIndexOf
	- List.ToArrayList

#### Built-in VariableList

- Added GradientList
- Added AnmationCurtveList

#### Scripts

- Added TypeFilterAttribute so that search filter type can be set in ClassTypeReference type field.
- Add RenamedFromAttribute to set the rename so that references in ClassTypeReference and ArborEvent are not removed.
- Added ClassNotNodeBehaviourAttribute that excludes only NodeBehaviour
- Added ClassAssetObjectAttribute to restrict only AssetObject.
- Added AssetObjectParameterReference.
- Added FlexibleAssetObject.
- Added HideTypeAttrubute to hide the type in the type selection popup.
- Add SendTriggerFlags to prevent sending to resident state with ArborFSM.SendTrigger.
- [Unity 2019.3 or later]Support SerializeReference attribute.
- Added a version of callback method that passes node result to Decorator.OnRepeatCheck.
- Added targetGraph to FlexibleSceneObject.

### Changed

#### Arbor Editor

- Changed to display the icon set in the script when the type of the type selection popup is Unity object.
- Changed to display the basic type such as System.Int32 displayed in the type selection popup with an alias such as int.
- Changed to display connection direction when reroute node is not connected.
- When the logo display is set to FadeOut, the logo is changed to fade out every time the graph is switched.
- Changed to display the logo in the foreground when capturing a graph.

#### ArborFSM

- Changed the display style of StateLink.
- When the graph of oneself was invalidated, it changed so that the transition process might be suspended until it was re-enabled.

#### BehaviourTree

- Changed the display style of NodeLink.
- When the graph of oneself was invalidated, it changed so that the transition process might be suspended until it was re-enabled.

#### ParameterContainer

- Changed so that the popup of Parameter.Type can be easily organized in the parameter settings specified in subgraphs such as SubStateMachine.

#### DataFlow

- Changed the display style of DataSlot.
- Change the display position of the specified field of the type displayed in such InputSlotTypable.
- Changed to refer to the graph directly when RootGraph or ParentGraph is specified in FlexibleComponent which refers to NodeGraph.

#### Built-in StateBehaviour

- Moved InvokeMethod to Events folder.
- Renamed Normalized Time of AnimatorCrossFade to Time Offset.
- Changed Transition Duration and Time Offset of AnimatorCrossFade to FlexibleFloat type.

#### Built-in ActionBehaviour

- Renamed Normalized Time of AnimatorCrossFade to Time Offset.
- Changed Transition Duration and Time Offset of AnimatorCrossFade to FlexibleFloat type.

#### Scripts

- IInputSlot
    - Renamed SetInputBranch to SetBranch.
	- Renamed GetInputBranch to GetBranch.
	- Renamed RemoveInputBranch to RemoveBranch.
	- Renamed IsConnectedInput to IsConnected.
- IOutputSLot
	- Renamed AddOutputBranch to AddBranch.
	- Renamed GetOutputBranch to GetBranch.
	- Renamed RemoveOutputBranch to RemoveBranch.
	- Renamed IsConnectedOutput to IsConnected.
	- Changed GetOutputBranchCount method to branchCount property.
- Changed ShowEventAttribute and HideEventAttribute to be set on fields and properties.
- Changed InputSlotAny and InputSlotTypable from directly passing a boxed value to copy the value type.
- In GetValue of each InputSlot, if the acquired value was null, it was returned false, but changed to judge whether the value is stored (DataBranch.isUsed is true).
- Changed so that eachField can enumerate internface.
- Changed ArborFSMInternal.nextTransitionState to return null after OnStateBegin().
- Changed the namespace of built-in Calculator to Arbor.Calculators.
- Changed the internal class related to ArborEditor to internal.
- Changed a class that does not need to be inherited to sealed.

#### Other

- Changed the icon of each Arbor component script.

### Improved

#### Arbor Editor

- Speed up the type selection popup.

#### Built-in StateBehaviour

- Improved to use CompareTag to compare GameObject.tag.

#### Scripts

- Speed up by caching when enumerating fields.

#### Load reduction

- Load reduction when acquiring values from data flows.

### Fixed

#### Arbor Editor

- [Unity 2017.3 or later] Fixed a bug that the graph scrolled even if you tried to move only the group node with Alt+drag.
- [Unity 2017.3 or later] Fixed a bug that Arbor-related types were not listed in the type selection window.
- [Unity 2018.1 or later] It was corrected to exclude it from the preset because it does not operate properly even if the preset is used in the following components.
    - ArborFSM
	- BehaviourTree
	- ParameterContainer
	- SubStateMachine
	- SubBehaviourTree
- Fixed a bug that the slot frame outside the node frame did not become invalid when DataSlot was displayed as invalid.
- Fixed a bug that the display of other fields also changed when the display type in EulerAnglesAttribute was changed.
- Fixed a bug that the reading destination was not switched until the editor was restarted when Language Path was moved to another folder.

#### ArborEvent

- Fixed a bug that the reference is broken when the specified type is moved to another module in Unity.

#### Built-in StateBehaviour

- When performing MoveOnWaypoint, fixed a bug that the movement does not start if the initial position and Waypoint match.
- Fixed RigidbodyMoveOnWaypoint and Rigidbody2DMoveOnWaypoint to move with FixedUpdate.
- Fixed the bug that the Angular Speed field of AgentLookAtPosition was displayed twice.

#### Scripts

- Modified to catch the exception that occurred at the time of calling each callback method of Arbor.
- Fixed that the field was displayed even if HideInInspector was specified for the added field when the data slot was created by itself.


## [3.6.13] - 2020-06-12

### Fixed

#### Arbor Editor

- Unity2020.1
    - Fixed a bug that only part of the graph can be operated with the mouse.
	- Fixed a bug that when the image was captured, all parts were painted black.


## [3.6.12] - 2020-05-29

### Fixed

#### Arbor Editor

- Fixed a bug that the copy source connection is disconnected when a node with a connected data slot is duplicated on the same graph.
- Fixed a bug that the data slot is disconnected when "Redo" after "Undo" of the graph copy and paste.
- (Temporary measure) NullReferenceException occurred at the place where NodeBehaviour.GetDataSlotField (int) is called, so fix it to check for null.
  When it becomes null, the error log such as "Type name : GetDataSlotField(0) == null" will be output.


## [3.6.11] - 2020-05-17

### Improved

#### Arbor Editor

- Improved to cancel live tracking when the graph is switched intentionally during playback.
- Adjusted the GUI when Animator is not specified in AnimatorParameterReference.

#### Load reduction

- Reduced load of NodeGraph deserialization.

### Fixed

#### Arbor Editor

- Fixed a problem that the connection of data slot is broken when copy and paste is done by Copy Component and Paste NodeGraph As New from Inspector of NodeGraph.
- Fixed a bug that the indentation collapsed when the ParameterReference field was displayed.
- Fixed a bug that the indentation collapsed when the AnimatorParameterReference field was displayed.
- Fixed a bug that the display graph is switched at high speed when transitioning to SubStateMachine that has no state during live tracking.


## [3.6.10] - 2020-04-20

### Fixed

#### Arbor Editor

- Fixed a bug where the graph screenshot did not capture the entire graph when the display was scaled to anything other than 1x.


## [3.6.9] - 2020-02-27

### Improved

#### Load reduction

- Load reduction when acquiring values from data flows.

### Fixed

#### Arbor Editor

- Fixed the bug that NullReferenceException occurs when compiling a script while the type or method selection popup is displayed.
- Fixed a bug that the value to be obtained changes when "Delete (Keep Connection)" of data reroute node during execution.
- Unity2019.3
    - Fixed layout of AddBehaviour menu.
	- Fixed layout of type and method selection popup.

#### Built-in StateBehaviour

- [Unity2018.4 or later] Fixed a bug that error may occur in InvokeMethod editor extension.


## [3.6.8] - 2020-01-10

### Fixed

#### Arbor Editor

- Fixed a bug where copying and pasting a graph with parameters in the graph causes the reference to be shared.
    - **To apply this fix to the corresponding graph created before Arbor 3.6.8, you need to reopen the scene or prefab and resave it.**
- Fixed a bug that Undo / Redo of graph copy & paste was not performed normally.
    - To copy and paste a graph from the Inspector window, use "Copy Component", "Paste NodeGraph Values", and "Paste NodeGraph As New" in the gear icon menu.
- Fixed the problem that the focus remains when switching the display graph to a child graph etc. while the input field of the side panel has keyboard focus.
- Unity2018.1 or later: Fixed a bug that Canvas etc. was not displayed in the list of type selection popup window and could not be selected.
- Unity2019.3
    - Fixed a bug that the icon button on the title bar of the behavior was not displayed.
    - Fixed a bug that the layout collapsed.

#### Scripts

- Fixed a problem that the execution state of the child graph was not reset if the parent graph during the execution of the child graph was stopped after being paused.


## [3.6.7] - 2019-11-08

### Fixed

#### Arbor Editor

- Fixed a bug that caused a NullReferenceException when a field in a connected data slot was deleted without being properly disconnected.

#### Built-in StateBehaviour

- Fixed a bug so that the connected data slot is normally disconnected when deleting the graph argument of SubStateMachine or SubBehaviourTree.


## [3.6.6] - 2019-11-06

### Fixed

#### Scripts

- Fixed a bug that GameObject passed to the argument of FlexibleGameObject(GameObject gameObject) constructor was not reflected.
  [Related bugs]
  - When updating from an older version of Arbor to 3.6.x, the transition from GameObject field to FlexibleGameObject field is not handled correctly.

## [3.6.5] - 2019-10-24

### Fixed

#### Arbor Editor

- Fixed a bug that the automatic scrolling of the graph remained working when dragging and dropping a parameter out of range.
- Fixed a bug that DropParameter area of ParameterReference remained for a moment when dragging and dropping parameter out of range.


## [3.6.4] - 2019-10-08

### Added

#### Scripts

- Added SerializeVersion class.
- Added ISerializeVersionCallbackReceiver interface.

### Fixed

#### Arbor Editor

- Fixed a bug that the size of the whole graph is not updated even if the node size changes, and it cannot scroll to the end.
- Fixed a bug that scrolling stopped midway when StateLink was right-clicked and "Go to Next State" was selected.

#### Built-in StateBehaviour

- Fixed long argument of method specified by InvokeMethod to be specified by FlexibleLong.

#### Scripts

- Fixed a bug that upgrade process of some parameters is not performed correctly when updating from Arbor old version to 3.6.x.
  [Related bugs]
    - The color of the group node is strange
    - Part of ParameterTransition's ConditionList is strange
	- Part of CalculatorTransition's ConditionList is strange
- Fixed a bug that caused a NullReferenceException when the parameter of the reference destination was not specified while the type was set to Parameter in a FlexibleField that handles value types such as FlexibleRect.


## [3.6.3] - 2019-09-20

### Fixed

#### Arbor Editor

- Fixed a bug that editor freezes when copying and pasting SubStateMachine or SubBehaviourTree.
- Fixed a bug that text cannot be selected by mouse click while renaming a node.
- [Unity2019.3beta]Fixed a bug that caused an exception if there was a DLL created in old Unity.


## [3.6.2] - 2019-08-09

### Fixed

#### Arbor Editor

- Fixed the bug that an exception occurs at the start of play when restarting the UnityEditor by displaying another tab in the same pane with the prefab graph selected.


## [3.6.1] - 2019-07-26

### Fixed

#### Arbor Editor

- Fixed that the setting of Color and Auto Alignment is not copied by copy & paste of group node.
- Fixed that pressing ESC while dragging a group node does not return to the original position.


## [3.6.0] - 2019-07-19

### Added

#### Arbor Editor

- Added "DataSlot> Show Inside Node" to the "View" menu on the toolbar.
- Added drag and drop Parameter to ParameterReference.
- Added drag and drop Parameter to FlexibleField.
- Added drag and drop GameObject to FlexibleGameObject.
- Added drag and drop Component to FlexibleComponent.
- Added Hierarchy to reference types of in-scene objects such as FlexibleGameObject and FlexibleComponent.
    - Self : See the GameObject that owns the self graph
	- RootGraph : If you are hierarchizing the graph, refer to the GameObject that owns the root graph.

#### Built-in StateBehaviour

- Added TransformMoveOnWaypoint.
- Added RigidbodyMoveOnWaypoint.
- Added Rigidbody2DMoveOnWaypoint.
- Added method call by OnStateUpdate and OnStateLateUpdate to InvokeMethod.
- Add event add button and delete button to set only the event that you want to invoke with InvokeMethod.
- Added update type of destination coordinates to Agent related script.
    - Time : Update by time
	- Done : Update on completion
	- StartOnly : Update only at start
	- Always : Always updated
- Added to be able to set the time type to be used when updating time of Agent related script.

#### Buiilt-in ActionBehaviour

- Added AnimatorCrossFade.
- Added an item to set the move destination change interval to AgentPatrol.

#### Scripts

- Added OutputSlotComponent<T> class.

### Changed

#### Arbor Editor

- Changed to auto scroll in graph while dragging parameter or scene object.

#### Built-in StateBehaviour

- Change Interval property of Agent related script to FlexibleFloat.

#### Scripts

- Changed a class that refers to an in-scene object such as FlexibleGameObject or FlexibleComponent to be derived from FlexibleSceneObjectBase.
    - Changed reference type from FlexibleType to FlexibleSceneObjectType.
- Move DataFlow related scripts to DataFlow folder.
- Source file organization of DataSlot related classes.

### Fixed

#### Arbor Editor

- [Unity2018.3 or later]Fixed that the graph view is not clipped and the scroll bar is covered.
- [Unity2019.2beta]Fixed that mouse event did not respond in the graph view.

#### Built-in StateBehaviour

- Fixed the exception that occurs when you specify SkinnedMeshRenderer which is not set mesh in TweenBlendShapeWeight.

#### Other

- Fixed a typo in Arbor.BuiltInBehaviours.asmdef.


## [3.5.5] - 2019-05-29

### Fixed

#### Arbor Editor

- Fixed the event not being used after executing shortcut commands such as copy.


## [3.5.4] - 2019-05-27

### Fixed

#### Built-in Scripts

- Fixed the bug that setter of animator property of AgentController class does not work properly.


## [3.5.3] - 2019-05-20

### Changed

#### Built-in Scripts

- Changed to copy instance of value when casting AnimationCurve to FlexibleAnimationCurve.
  (Change due to bug fix of Tween system StateBehaviour)
- Changed to copy instance of value when casting Gradient to FlexibleGradient.
  (Change due to bug fix such as TweenColor)
- Unity2018.3 or later: Changed to output a warning to log when "Reuse Collision Callbacks" is enabled when outputting Collision or Collision2D data.
    - OnCollisionEnterTransition
	- OnCollisionExitTransition
	- OnCollisionStayTransition
	- OnCollisionEnter2DTransition
	- OnCollisionExit2DTransition
	- OnCollisionStay2DTransition

### Fixed

#### Arbor Editor

- Fixed the bug that ArgumentOutOfRangeException occurs when adding script and parameters directly to the state node by drag & drop.

#### Built-in Scripts

- Fixed a bug that the Curve field can not be edited immediately after adding Tween-based State Behavior such as TweenRotation to the state.
- Fixed a bug that Gradient field can not be edited immediately after adding TweenColor etc. to a state.


## [3.5.2] - 2019-05-03

### Fixed

#### Built-in Scripts

- Fixed an exception when changing the parameter type that ParameterConditionList refers to.


## [3.5.1] - 2019-04-29

### Added

#### Scripts

- Added access method to each value to Parameter class.
    - SetInt, GetInt, TryGetInt
	- SetLong, GetLong, TryGetLong
	- SetFloat, GetFloat, TryGetFloat
	- SetBool, GetBool, TryGetBool
	- SetString, GetString, TryGetString
	- SetEnumInt, GetEnumInt, TryGetEnumInt
	- SetEnum, GetEnum, TryGetEnum
	- SetEnum<TEnum>, GetEnum<TEnum>, TryGetEnum<TEnum>
	- SetVector2, GetVector2, TryGetVector2
	- SetVector3, GetVector3, TryGetVector3
	- SetQuaternion, GetQuaternion, TryGetQuaternion
	- SetRect, GetRect, TryGetRect
	- SetBounds, GetBounds, TryGetBounds
	- SetColor, GetColor, TryGetColor
	- SetGameObject, GetGameObject, TryGetGameObject
	- SetTransform, GetTransform, TryGetTransform
	- SetRectTransform, GetRectTransform, TryGetRectTransform
	- SetRigidbody, GetRigidbody, TryGetRigidbody
	- SetRigidbody2D, GetRigidbody2D, TryGetRigidbody2D
	- SetComponent, GetComponent, TryGetComponent
	- SetComponent<TComponent>, GetComponent<TComponent>, TryGetComponent<TComponent>
	- SetVariable, GetVariable, TryGetVariable
	- SetVariable<TVariable>, GetVariable<TVariable>, TryGetVariable<TVariable>
- Added access property to each value to Parameter class.
    - enumIntValue
	- componentValue
	- transformValue
	- rectTransformValue
	- rigidbodyValue
	- rigidbody2DValue
- Added to throw ParameterTypeMismatchException when accessing the wrong type property of Parameter class.
- Add reference property variableObject to the object storing Variable in Parameter class.
- Added access method to parameter value to ParameterContainerInternal class.
    - SetComponent, GetComponent, TryGetComponent
	- SetComponent<TComponent>, GetComponent<TComponent>, TryGetComponent<TComponent>
	- SetVariable, GetVariable, TryGetVariable
	- SetVariable<TVariable>, GetVariable<TVariable>, TryGetVariable<TVariable>
- Added access property to various parameter values to AnyParameterReference
    - intValue
	- longValue
	- floatValue
	- boolValue
	- enumIntValue
	- enumValue
	- vector2Value
	- vector3Value
	- rectValue
	- boundsValue
	- colorValue
	- gameObjectValue
	- componentValue
	- transformValue
	- rectTransformValue
	- rigidbodyValue
	- rigidbody2DValue
	- variableValue
- Add value property to access any parameter value to AnyParameterReference
- Added value property to access parameter value in BoolParameterReference.
- Added value property to access parameter value in BoundsParameterReference.
- Added value property to access parameter value in ColorParameterReference.
- Added value property to access parameter value in ComponentParameterReference.
- Added value property to access parameter value in FloatParameterReference.
- Added value property to access parameter value in GameObjectParameterReference.
- Added value property to access parameter value in IntParameterReference.
- Added value property to access parameter value in LongParameterReference.
- Added value property to access parameter value in QuaternionParameterReference.
- Added value property to access parameter value in RectParameterReference.
- Added value property to access parameter value in RectTransformParameterReference.
- Added value property to access parameter value in RigidbodyParameterReference.
- Added value property to access parameter value in Rigidbody2DParameterReference.
- Added value property to access parameter value in StringParameterReference.
- Added value property to access parameter value in TransformParameterReference.
- Added value property to access parameter value in Vector2ParameterReference.
- Added value property to access parameter value in Vector3ParameterReference.

### Improved

#### Arbor Editor

- Improved to start dragging the behavior title bar when moving 6 pixels or more from the mouse button pressed position.

### Deprecated

#### Scripts

- Changed Parameter.GetVariable<T>(ref T) to deprecated.

### Fixed

#### Arbor Editor

- Fixed an exception that occurred when deleting a script of behavior added to a node.



## [3.5.0] - 2019-04-12

### What's New

#### DataLink

By adding DataLink attribute to the field that you want to be able to input from the data flow, added the function to easily have an input slot.

### Added

#### Arbor Editor

- Added flag to make node comments affected by zoom in configuration.
- Added batch display flag of node comment to the toolbar display menu.

#### Behaviour Tree

- Add node breakpoints.

#### Parameter Container

- Added search box for parameters.

#### Data Flow

- By using the DataLink attribute, added the ability to easily accept input from the data flow.

#### Built-in Calculator

- Added Calculator to calculate remainder.
    - Int.ModCalcualtor
	- Long.ModCalculator
	- Float.ModCalculator
- Added Calculator to perform bit operation of Enum flag.
    - EnumFlags.Add
	- EnumFlags.Remove
	- EnumFlags.Contains
- Added NodeGraph related Calculator.
    - NodeGraph.GetRootGameObject
	- NodeGraph.GetRootGraph
	- NodeGraph.GetName

### Improved

#### Arbor Editor

- Improved to not edit in case of graph with HideFlags.NotEditable.
   (You need to use the prefab editor if you want to edit prefabs with Unity 2018.3 or later)
- Improved so that the pop-up area of the behavior insertion button does not overlap with other buttons.

#### Parameter Container

- Organized to make it easy to see the parameter addition menu.

#### Data Flow

- Improved the texture of data connection lines to a design with easy to understand connection direction.
- Improves the display of the current value displayed on the connection line when outputting Everyting to the Enum flag slot during play.

#### Editor

- Improved to edit in Default mode of EulerAnglesAttribute in Vector4Field.

#### Scripts

- Improved to call Service.OnUpdate method immediately after node becomes active.

### Changed

#### Arbor Editor

- Changed the layout to display the side panel toolbar next to the whole toolbar.

#### Behaviour Tree

- (Breakpoint related)Change the display position of the priority to the upper right of the node.

#### Data Flow

- (DataLink related)Changed to display the data input slot outside the frame of the node.
- (DataLink related)Changed data slot GUI style.
- (DataLink related)Changed to display the internal input slot of FlexibleField and ParameterReference outside the node frame.

#### Built-in StateBehaviour

- Added a field to specify the update timing to TransformSetPosition.
- Added a field to specify the update timing to TransformSetRotation.
- Added a field to specify the update timing to TransformSetScale.

#### Scripts

- Changed so that it is not displayed in the parameter addition menu when the null character is specified in AddVariableMenu.

### Fixed

#### Arbor Editor

- Fixed a bug that side panel width is not saved in Unity 2017.3 or later.
- Fixed Playmode tint not being reflected during playback. (Unity2017.2 ~ Unity2018.2 is not supported by the Unity of the specification)

#### Built-in StateBehaviour

- When EndStateMachine is used in the root state machine, it has been fixed to stop playback not stopping.
- Fixed that the child state machine continued to be played if the state transition did not occur when returning to the parent state machine with EndStateMachine.

### Other

#### Package Manager

- Added package.json for use with Package Manager.

#### Example

- Organize Example.
- Added examples of DataFlow, DataLink, and External Graph.
- Example readme added.


## [3.4.4] - 2019-03-01

### Fixed

#### Unity support

- Support for Unity 2019.1.0b3


## [3.4.3] - 2019-02-13

### Improved

#### Parameter Container

- Improved the problem that when GetParameter or SetParameter is created and ParameterContainer of the same graph is selected for Container, it becomes internal parameter mode and parameters can not be selected.

### Fixed

#### Arbor Editor

- Fixed a bug that graph refreshing by value updating was not done when the connection destination of data flow was other than Calculator or rerout node.

#### Behaviour Tree

- Fixed a bug where Decorator with AbortFlags.LowerPriority set was reevaluated during execution of upper node.

#### Parameter Container

- [Before Unity 2017.1.5f1] Fixed an exception occurs when parameters of Vector 2, Vector 3, Rect, and Bounds are added.

#### Runtime

- Fixed a bug that will always break when setting an exception breakpoint during runtime debugging (exception breakpoints on the Unity editor are not supported).


## [3.4.2] - 2019-01-02

### Fixed

#### Arbor Editor

- Fixed a bug that NullReferenceException occurred in StateBehaviour with ArborFSM array and List.


## [3.4.1] - 2018-12-17

### Fixed

#### ParameterContainer

- Fixed a bug that when Object related parameters were added before Arbor 3.3.2, errors occurred when loading.

#### Scripts

- Fixed that NullReferenceException occurs when ParameterCondition reads scenes at run time while keeping old format.
  [Related behavior]
    - [StateBehaviour] ParameterTransition
	- [Decorator] ParameterCheck
	- [Decorator] ParameterConditionalLoop
- Fixed that NullReferenceException occurs when CalculatorCondition reads scenes at runtime while keeping old format.
  [Related behavior]
    - [StateBehaviour] CalculatorTransition
	- [Decorator] CalculatorCheck
	- [Decorator] CalculatorConditionalLoop


## [3.4.0] - 2018-12-14

### What's New

#### Parameters in node graph

A parameter function directly related to ArborFSM and Behavior tree was added.
You can create it from the "Parameters" tab of the side panel.

Parameters can be accessed from the graph by dragging and dropping from the dragging area of the parameter to the graph view.

#### Data transfer to subgraph

Added access field to graph parameter in Subgraph related behavior such as SubStateMachine and SubBehaviourTree.

#### Resize node

We added a function that can change by dragging the width of various nodes (Except for some nodes such as the root node of Behavior tree)

#### Align within group nodes

Added a function to automatically adjust nodes so that they do not overlap when the position and size of nodes in group nodes are changed.

You can set "Auto Alignment" from the group node setting window.

### Added

#### Arbor Editor

- Added parameter tab to side panel.
- Added size change of various nodes.
- Added alignment function within group nodes.

#### ParameterContainer

Added drag area for creating parameter access node.

#### Built-in StateBehaviour

- Added method setting to make transition call to GoToTransition.
- Added PlayStateMachine to start playback of ArborFSM.
- Added StopStateMachine to stop playback of ArborFSM.
- Added PlayBehaviourTree to start playback BehaviourTree.
- Added StopBehaviourTree to stop playback BehaviourTree.
- SendMessageGameObject, SendMessageUpwardsGameObject, BroadcastMessageGameObject
    - Change MethodName field to FlexibleString type.
    - Added available types for arguments
		- Long
        - Enum
        - GameObject
        - Vector2
        - Vector3
        - Quaternion
        - Rect
        - Bounds
        - Color
        - Component
        - Slot

#### Built-in ActionBehaviour

- Added PlayStateMachine to start playback of ArborFSM.
- Added StopStateMachine to stop playback of ArborFSM.
- Added PlayBehaviourTree to start playback BehaviourTree.
- Added StopBehaviourTree to stop playback BehaviourTree.

#### Scripts

- Added access method to Enum type parameter to ParameterContainer.

### Improved

#### Arbor Editor

- Adjust the starting position of the StateLink connection line.
- Improved editor performance.
- Improved to cache the search word of the script selection window such as StateBehaviour for each type of script.
- Improved to display data connection lines where data is not stored during execution as dark.

#### ParameterContainer

- Improved to serialize each parameter only the necessary minimum data.

#### Scripts

- Improved to serialize each element of ParameterCondition only necessary minimum data.
  [Related Behaviours]
    - [StateBehaviour] ParameterTransition
	- [Decorator] ParameterCheck
	- [Decorator] ParameterConditionalLoop
- Improved to serialize each element of CalculatorCondition only necessary minimum data.
  [Related Behaviours]
    - [StateBehaviour] CalculatorTransition
	- [Decorator] CalculatorCheck
	- [Decorator] CalculatorConditionalLoop

### Fixed

#### Arbor Editor

- Fixed the connection position of the connection line of the data with the Behaviour collapsed.
- Fixed creation positions of nodes when creating the node by dragging the connection slots of each node BehaviourTree.
- Fixed that the help box displayed when one of the ArborEditor windows can not be used is displayed one line when there is no data slot label.
- Fixed output of log when deleting each element of WeightList used in Random.SelectComponent etc.
- Fixed an issue where NullReferenceException may occur when playing with live tracking on.
- FixibleField reference type DataSlot is displayed as Calculator.
- Fixed a bug that disconnected when you re-select the reference type of FlexibleField to DataSlot.

#### Scripts

- Fixed that NullReferenceException occurs when the input slot of data is connected only to the reroute node and it is not connected to the output slot.

#### Unity support

- Support for Unity 2018.3.0f1
- Support for Unity2019.1.0a10

#### "Odin - Inspector and Serializer" Support

- Temporarily deal with the problem that some PropertyDrawer were not working properly.

Annotation: 
  Cooperation with other assets is outside the guarantee of operation in principle.
  There is no guarantee that the problem is not generated by this deal.


## [3.3.2] - 2018-10-29

### Changed

#### Arbor Editor

- Changed to use the button style for the icon button of the node header.
- When StateLink is connected, change the background style so that the gear icon is easy to see.

### Fixed

#### Arbor Editor

- Support for Unity 2019.1.0a5.

#### Build

- Fixed an error when building to Universal Windows Platform.


## [3.3.1] - 2018-10-15

### Fixed

#### Arbor Editor

- Fixed a bug that an error box is displayed in the field of the data slot when canceling Maximize of docked ArborEditor window.
- Fixed a bug that separator line between side panel and graph view is not displayed in Unity 2017.3.0 or later.

#### ArborFSM

- Fixed a bug that transition count of StateLink which could not be reserved increases when transitioning with TransitionTiming.LateUpdateDontOverwrite.
- Fixed a bug that transition count does not increase when StateLink's TransitionTiming is Immediate.


## [3.3.0] - 2018-10-05

### New: InvokeMethod

Added built-in script which calls method of Component.
You can input arguments from the data flow and output return values and out arguments to the data flow.

#### Built-in StateBehaviour

- Added InvokeMethod.

#### Scripts

- Added ArborEvent class (Core classes to perform the method call)
- Added ShowEventAttribute class (Attributes that can be selected even for methods with arguments of type not supported)
- Added HideEventAttribute class (Attribute to be hidden so that it can not be selected by ArborEvent)

### New: ObjectPool

Added function to pool instantiated objects.

#### Built-in StateBehaviour

- Added AdvancedPooling which performs advance pooling.
- Add UsePool flag to instantiate from ObjectPool.
    - InstantiateGameObject
	- SubStateMachineReference
	- SubBehaviourTreeReference
- Changed to return to ObjectPool at Destroy.
  (Return to Pool only when Instantiating from ObjectPool)
	- DestroyGameObject
	- OnCollisionEnterDestroy
	- OnCollisionExitDestroy
	- OnTriggerEnterDestroy
	- OnTriggerExitDestroy
	- OnCollisionEnter2DDestroy
	- OnCollisionExit2DDestroy
	- OnTriggerEnter2DDestroy
	- OnTriggerExit2DDestroy

#### Built-in ActionBehaviour

- Added AdvancedPooling which performs advance pooling.
- Add UsePool flag to instantiate from ObjectPool.
    - InstantiateGameObject
	- SubStateMachineReference
	- SubBehaviourTreeReference
- Changed to return to ObjectPool at Destroy.
  (Return to Pool only when Instantiating from ObjectPool)
    - DestroyGameObject

#### Scripts

- Added ObjectPool class to ObjectPooling namespace.

### Added

#### Arbor Editor

- Added "Delete (Keep Connection)" in the right click menu of the data slot reroute node.
- Added "Delete (Keep Connection)" in the right click menu of the StateLink reroute node.
- Added "Disconnect" in the right-click menu of the data slot. ("Disconnect All" in case of output slot)
- Added "Edit Editor Script" in the menu of NodeBehaviour. (Only when there is an Editor extension script)
- Added "Show all data values always" check in the debug menu on the toolbar.

#### ArborFSM

- Added infinite loop debug setting.

#### BehaviourTree

- Added infinite loop debug setting.

#### ParameterContainer

- Added enum type.

#### Built-in StateBehaviour

- Added support for enum type of CalcParameter.
- Added support for enum type of ParameterTransition.
- Added "Save To Prefab" in SubStateMachine's menu.
- Added "Save To Prefab" in SubBehavioutTree's menu.

#### Built-in ActionBehaviour

- Added "Save To Prefab" in SubStateMachine's menu.
- Added "Save To Prefab" in SubBehavioutTree's menu.

#### Built-in Decorator

- Added support for enum type of ParameterCheck.
- Added support for enum type of ParameterConditionLoop.

#### Editor extension

- Added LanguagePath asset which specifies installation directory path of self-created script language file.

#### Scripts

- Added OutputSlotTypable class.
- Added InputSlotTypable class.
- Added FlexibleEnumAny class (FlexibleField system class that can handle enum)
- ArborFSMInternal class
    - Added prevTransitionState property which can refer to the state before transition.
    - Added nextTransitionState property which can refer to state after transition.
- StateBehaviour class
    - Added prevTransitionState property which can refer to the state before transition.
    - Added nextTransitionState property which can refer to state after transition.
    - Added stateLinkCount property to return the number of StateLink.
    - Added GetStateLink method to return StateLink.
    - Added RebuildStateLinkCache method to rebuild StateLink cache.
- Added Disconnect method to DataSlot class.
- Added AddBehaviourMenu multilingual support.
- Added BehaviourTitle multilingual support.
- Added BehaviourMenuItem multilingual support.

### Changed

#### Arbor Editor

- Changed to prevent nodes from being selected when clicking the node's main content GUI.
- Change GUI style of data slot.
- Changed to display the type name on tooltip when mouse over the reroute node of data slot.
- Integrate StateLink reroute node pin menu into node's right click menu.
- Changed to not change the connection destination when StateLink is dragged and it is in the stateLink frame.
- Changed GUI style of NodeLinkSlot of BehaviourTree.
- Changed to display the current condition of the decorator.
- Adjust the display position of the behaviour selection popup when pressing the insert behaviour button.
- Changed to display on the front when mouse over the connection line.
- Changed to be able to select None in the type selection popup.
- Supports selection of type selection popup window with key input.
- Changed ParameterContainer referenced by ParameterReference so that it can also be set from data slot.
- Adjust the color of connecting lines of other data types.

#### BehaviourTree

- When Decorator returns failure on node active, change ActionBehaviour and Service OnStart () not to call.

#### Built-in StateBehaviour

- Changed "Prefab" field of InstantiateGameObject to FlexibleComponent.
- Changed "External FSM" field of SubStateMachineReference to FlexibleComponent.
- Changed "External BT" field of SubBehaviourTreeReference to FlexibleComponent.

#### Built-in ActionBehaviour

- Changed "Prefab" field of InstantiateGameObject to FlexibleComponent.
- Changed "External FSM" field of SubStateMachineReference to FlexibleComponent.
- Changed "External BT" field of SubBehaviourTreeReference to FlexibleComponent.

### Improved

#### Arbor Editor

- Optimize by changing Reflection to delegate.

#### ArborFSM

- Changed StateLink to cache beforehand.

#### Scripts

- Changed EachField class to cache the field.
- Optimized by changing Reflection use of EachField class to delegate. (No change in AOT or IL2CPP environment)

### Deprecated

#### Scripts

- Change the nextState property of ArborFSMInternal to obsolete.
  (Added reserverdState instead)
- Changed the TidyAssemblyTypeName method of ClassTypeReference to obsolete.
　(Added TypeUtility.TidyAssemblyTypeName instead)
- Changed the GetAssemblyType method of ClassTypeReference to Obsolete.
  (Added TypeUtility.GetAssemblyType instead)

### Fixed

#### Arbor Editor

- Fix to switch Arbor Editor selection when Hierarchy selected graph instantiated by SubStateMachineReference or SubBehaviourTreeReference.
- Fixed an exception occurred in the editor when there was a class named StateLink different from Arbor.StateLink.
- Fixed that connection was broken when Undo insertion of reroute node of data slot.
- Fixed that rendering of dragging connection line was done even in other than Repaint event.

#### ArborFSM

- A bug when calling Stop() on the same graph in OnStateBegin()
	- Fixed an exception occurred.
    - Fixed that OnStateEnd() and OnStateBegin() on next execution will not be called.
	- Fixed that OnStateBegin () after this state was called.
- Fix to hide fields not wanted to be edited from Inspector of ArborFSM instantiated by SubStateMachineReference.
- Fixed that extra copies of NodeBehaviours used in child graphs by SubStateMachine etc. when "Copy Component" was done in Inspector.

#### BehaviourTree

- Fix to hide fields not wanted to be edited from Inspector of BehaviourTree instantiated by SubBehaviourTreeReference.
- Fixed that extra copies of NodeBehaviours used in child graphs by SubStateMachine etc. when "Copy Component" was done in Inspector.

#### Waypoint

- Fixed deleting the next element when deleting an element for which Points' Transform is not set.

#### Scripts

- Fixed bug that can not be found when passing target type array to EachField.Find.
- Fixed not to scan the contents when passing an instance of target type to EachField.Find.
- Fixed deleting the next element when WeightList<T> element type is Unity object and trying to delete an element not setting object.

#### Others

- Support for Unity 2018.3.0b3.


## [3.2.4] - 2018-08-22

### Fixed

#### Arbor Editor

- Fixed a bug that caused an exception when trying to copy a character string of node comment in Unity 2017.3 or later.
- Fixed a bug that copying with the right-click menu of intra-node TextField does not work in Unity 2018.1 or later.
- Fixed a bug that automatic scrolling keeps working when dragging and dropping behaviors in MacOS Unity 2017.3 or later.
- Fixed a bug that a useless separator is displayed in the right click menu of the reroute node.
- Fixed that the display position of behavior insertion button on zoom out was slightly misaligned.
- Fix wording of behavior collapse.


## [3.2.3] - 2018-08-08

### Fixed

#### Arbor Editor

- Fixed using method which became Obsolete with Unity 2018.1.

#### Built-in Behaviour

- Fixed that AngularSpeed property of AgentLookAtPosition was not displayed.
- Fixed that AngularSpeed property of AgentLookAtTransform was not displayed.


## [3.2.2] - 2018-07-26

### Fixed

#### Arbor Editor

- Fixed that it is not immediately reflected on the connection line when undoing the direction change of the reroute node of data.
- Fixed that if you undo the state move when the state is outside the screen, it will not be immediately reflected on the connection line.

#### Other

- Fixed that Arbor group may not be displayed on Hierarchy's Create button when importing other asset etc


## [3.2.1] - 2018-07-24

### Fixed

#### Arbor Editor

- Fixed that inserting position becomes one before if behavior is added from insert button after dragging behavior once.

#### ArborFSM

- Fixed that the state of the transition destination state is processed with LateUpdate() of the same frame after transition by TransitionTiming.Immediate in OnStateUpdate().

#### BehaviourTree

- Fixed that common menu items of composite node and action node were missing.


## [3.2.0] - 2018-07-18

### Added

#### Arbor Editor

- Added "Edit Script" to the right-click menu of the Calculator.
- Added color change of group node.
- Added "Settings" in the right-click menu of the transition line.
- Added drag-and-drop behavior to the Inspector.
- Added drag-and-drop behavior to another node.
	- Copy with Ctrl + Drop (Option + Drop on Mac).
- Added insert behavior button.
- Added expansion and collapse of behavior within the node.
	- Node right click menu
	- Graph right click menu (Selection nodes)
	- Toolbar (All nodes)
- Added active node tracking during play.
	- Switch with the "Live Tracking" toggle on the toolbar.
- Added setting items of ArborEditor
	- Docking Open : Set whether to dock with SceneView when the ArborEditor window is opened
	- Mouse Wheel Mode : Set whether to zoom or scroll (Unity 2017.3 or later)
	- Live Tracking Hierarchy : When "Live Tracking" is done, it is set whether to switch automatically to a child graph
- Added display of the area to be automatically scrolled when mouse over occurs during dragging.

#### Behaviour Tree

- Added "Replace Composite" to the right-click menu of the composite node.
- Added "Replace Action" to the right-click menu of the action node.

#### Parameter Container

- Added constructor and type conversion of FlexibleField to Variable script template.

#### Built-in Behaviour

- Added a movement mode from the current value to the value specified for Tween behavior.
  (Along with this, change Relative field to TweenMoveType field)
	- TweenPosition
	- TweenRotation
	- TweenScale
	- TweenRigidbodyPosition
	- TweenRigidbodyRotation
	- TweenRigidbody2DPosition
	- TweenRigidbody2DRotation
	- TweenTextureOffset
	- TweenCanvasGroupAlpha
	- UITweenPosition
	- UITweenSize
- Added TweenColorSimple.
- Added UITweenColorSimple.
- Added TweenTextureScale.
- Added TweenMaterialFloat.
- Added TweenMaterialVector2.
- Added TweenTimeScale.
- Added TweenBlendShapeWeight.

#### Built-in Calculator

- Added Random.Value.
- Added Random.InsideUnitCircle.
- Added Random.InsideUnitSphere.
- Added Random.OnUnitSphere.
- Added Random.Rotation.
- Added Random.RotationUniform.
- Added Random.RangeInt.
- Added Random.RangeFloat.
- Added Random.Bool.
- Added Random.RangeVector2.
- Added Random.RangeVector3.
- Added Random.RangeQuaternion.
- Added Random.RangeColor.
- Added Random.RangeColorSimple.
- Added Random.SelectString.
- Added Random.SelectGameObject.
- Added Random.SelectComponent.

#### Built-in CompositeBehaviour

- Added RandomExecutor.
- Added RandomSelector.
- Added RandomSequencer.

#### Built-in Variable

- Added Gradient.
- Added AnimationCurve.

#### Scripts

- Added classes that can use ClassTypeConstraint attribute.
	- AnyParameterReference
	- ComponentParameterReference
	- InputSlotComponent
	- InputSlotUnityObject
	- InputSlotAny
	- FlexibleComponent
- Added ClassGenericArgumentAttribute.
- Added TryGetInt method etc. to ParameterContainer.
- Added a GetInt method etc. that can specify the default value when there is no parameter to ParameterContainer.
- Added EulerAnglesAtribute to enable Quaternion to be edited at Euler angles.
	- Added to Quaternion of Parameter.
	- Added to FlexibleQuaternion.
- Added AddVariableMenu attribute to specify additional menu name of Variable.
- Added State.IndexOfBehaviour method.
- Added NodeBehaviourList.IndexOf method.
- Added FlexiblePrimitiveBase class as the base class of the class that uses FlexiblePrimitiveType.
- Added ConstantRangeAttribute (FlexibleField version of Range attribute)
	- FlexibleInt, FlexibleFloat.
- Added HideSlotFields attribute to hide each DataSlot specific field
	- It is mainly used to hide the Type field of OutputSlotComponent and OutputSlotUnitObject.
- Added property to get / set access to each field on AgentController.
	- agent
	- animator
	- movingParameter
	- movingSpeedThreshold
	- speedParameter
	- isDivAgentSpeed
	- speedDampTime
	- movementType
	- movementDivValue
	- movementXParameter
	- movementXDampTime
	- movementYParameter
	- movementYDampTime
	- movementZParameter
	- movementZDampTime
	- turnParameter
	- turnType
	- turnDampTime
- Added Methods to CompositeBehaviouor
	- GetBeginIndex
	- GetNextIndex
	- GetInterruptIndex
- Added isRevaluation property to Decorator.

### Changed

#### Arbor Editor

- When creating a state rerouting node, it changed to inherit the color of the line.
- Calculator using FlexiblePrimitiveType.Random has been changed to always recalculate.
- Changed to auto-scroll behavior while dragging.

#### Parameter Container

- Changed to show value label.

#### AgentController

- When Animator is none, change the parameter name so that it can be edited with TextField.
	- (AnimatorParameterReference is changed as well)

#### Built-in Behaviour

- Changed some built-in behavior fields to FlexibleField.
	- TweenBase : Duration, Curve, UseRealtime, RepeatUntilTransition
	- TweenColor : Gradient
	- TweenTextureOffset : PropertyName
	- UITweenColor : Gradient
	- LoadLevel : LevelName
	- BroadcastTrigger : Message
	- SendTrigger : Target, Message
	- SendTriggerGameObject : Message
	- SendTriggerUpwards : Message
	- TriggerTransition : Message
- Material changed behavior to use MaterialPropertyBlock.
	- TweenColor
	- TweenTextureOffset
- Changed each field of Tween behavior to cache at state start.

#### Built-in Calculator

- Changed the title name to match AddBehaviorMenu.

#### Built-in Decorator

- Changed to show the time progress bar only when reevaluating.
	- Cooldown
	- TimeLimit

#### Scripts

- Changed ComponentParameterReference so that it can specify component parameters other than Parameter.Type.Component.
- Rename CalculatorSlot to DataSlot.
- Rename CalculatorBranch to DataBranch.
- Rename CalculatorBranchRerouteNode to DataBranchRerouteNode.
- Rename FlexibleType.Calculator to DataSlot.
- Rename FlexiblePrimitiveType.Calculator to DataSlot.
- Rename DataBranch.isVisible to showDataValue.

### Deprecated

#### Scripts

- Obsolete InputSlotAny(System.Type) constructor.
- Obsolete OutputSlotAny(System.Type) constructor.
- Obsolete AnyParameterReference.parameterType.
- Obsolete AnyParameterReference(System.Type).
- Obsolete ParameterContainer's old GetInt method etc.

### Removed

#### Scripts

- Obsolete the creation of scripts by boo and javascript.

### Fixed

#### Arbor Editor

- When Behavior Tree was selected, a separator was added at the bottom of the debug menu on the tool bar.
- Fixed wording of switching menu of show data value.
- Fixed that abstract class of behavior is enumerated in behavior addition window.
- Fix to update rectangle when automatic scrolling during rectangle selection drag.


## [3.1.3] - 2018-06-26

### Fixed

#### Editor

- Fixed an exception occurs when displaying Inspector when declaring CalculatorSlot in MonoBehaviour script.
- Fixed an issue where label of field name disappeared when declaring FlexibleField<T> with non-serializable type.

#### Scripts

- Fixed that InputSlot<T> is not displayed in the script reference.
	- Renamed InputSlot class to InputSlotBase.
- Fixed that OutputSlot<T> is not displayed in the script reference.
	- Renamed OutputSlot class to OutputSlotBase.
- Fixed that FlexibleField<T> could only use class with Serializable attribute.
- Fixed that Variable<T> could only use class with Serializable attribute.
- Fixed that it was "not serializable" even if specifying a serializable type for Variable<T>.
- Fixed the Arbor script template so that it is not displayed in the AddComponent menu.
- Fixed the path of the AddComponent menu of Example script to "Arbor / Example".
- Fix to ignore when a type other than a subclass is specified in the field of CalculatorSlot with SlotTypeAttribute.
- Fix to constrain CalculatorSlot which can use SlotTypeAttribute to the following class.
	InputSlotComponent, InputSlotUnityObject, InputSlotAny, OutputSlotAny


## [3.1.2] - 2018-06-19

### Fixed

#### Arbor Editor

- Fixed unlimited recursion when the Unity object derived class with the System.Serializable attribute has a field that refers to itself.


## [3.1.1] - 2018-06-12

### Added

#### Built-in StateBehaviour

- Added AgentWarpToPosition
- Added AgentWarpToTransform
- Added TransformSetPosition
- Added TransformSetRotation
- Added TransformSetScale
- Added TransformTranslate
- Added TransformRotate

#### Built-in ActionBehaviour

- Added AgentWarpToPosition
- Added AgentWarpToTransform

#### Built-in Calculator

- Added StringConcatCalculator
- Added StringJoinCalculator

#### Scripts

- Added Warp method to AgentController.
- Added OnGraphPause, OnGraphResume, OnGraphStop callback to NodeBehaviour.

### Fixed

#### ArborFSM

- Fixed a problem that transition number does not increase if TransitionTiming.Immediate transition is made except State callback method.
- Fixed that the OnStateEnd method will not be called back when ArborFSM.Stop() is called.

#### BehaviourTree

- Fixed that the OnEnd method will not be called back when BehaviourTree.Stop() is called.

#### Built-in StateBehaviour

- Fixed not transitioning on completion when Tween's Duration is set to 0.


## [3.1.0] - 2018-05-31

### Added

#### ArborEditor

- When the graph is not selected, a graph creation button and a button for opening a manual page are displayed.
- Added zoom of graph (Unity 2017.3.0f3 or later)
- Added capture of graph.
- Added graph creation button on toolbar.
- Added display of Arbor logo when opening graph.
	You can toggle in the setting window from the gear icon.
- Added update notification of AssetStore.

#### ArborEditor extension

- Added underlayGUI callback which can customize the back side to ArborEditorWindow class.
- Added overlayGUI callback which can customize front side to ArborEditorWindow class.
- Added toolbarGUI callback which can customize toolbar to ArborEditorWindow class.

#### ParameterContainer

- Add Variable to add user-defined type.
- Added VariableGeneratorWindow to create Variable definition script from template.

#### AgentController

- Added MovementType field.
- Added MovementDivValue field.
- Added TurnType field.
- Changed to transfer movement value and rotation amount to Animator according to MovementType and TurnType.

#### Built-in StateBehaviour

- Added AgentLookAtPosition to rotate AgentController in the direction of specified position.
- Added AgentLookAtTransform to rotate AgentController int the direction of specified Transform.
- Added SubStateMachineReference which executes prefabricated ArborFSM as a child graph.
- Added SubBehaviourTreeReference which executes prefabricated BehaviourTree as a child graph.
- Added SetActiveScene to activate the scene.
- Add IsActiveScene field to LoadLevel.
- Added Done transition to LoadLevel.

#### Built-in ActionBehaviour

- Added AgentLookAtPosition to rotate AgentController in the direction of specified position.
- Added AgentLookAtTransform to rotate AgentController int the direction of specified Transform.
- Added SubStateMachineReference which executes prefabricated ArborFSM as a child graph.
- Added SubBehaviourTreeReference which executes prefabricated BehaviourTree as a child graph.
- Added display of elapsed time to Wait action.

#### Built-in Decorator

- Added display of elapsed time to TimeLimit decorator.
- Added display of elapsed time to Cooldown decorator.

#### Scripts

- Supports Assembly Definition (Unity 2017.3.0f3 or later)
	Following this support, the folder structure has also changed.
- Added rootGraph property to NodeGraph class.
- Added ToString method to NodeGraph class.
- Added GetName method to Node class.
- Added ToString method to Node class.
- Added InputSlotAny class which can specify input type.
- Added OutputSlotAny class witch can specify output type.
- Added AnyParameterReference class witch can specify the type of parameter to refer.
- Added FlexibleField<T> generic class version of Flexible type class.
- Added variableValue property to Parameter class.
- Added SetVariable method to Parameter class.
- Added GetVariable method to Parameter class.

### Changed

#### ArborEditor

- Change Grid button to gear icon, rename popup window displayed by pressing button to GraphSettings window to set other than grid.
- Move the toolbar language popup to the GraphSettings window.
- Changed to display the menu for opening asset store and manual page from the help button of the toolbar.
- Changed the highlight display to the easy-to-see design when mouse over the connection line.
- Change resizing of group nodes so that they can be dragged on each side.
- Change it so that multiple selection can be made by holding Ctrl or Shift in the side panel node list.
- When dragging the direction of the reroute node, press Esc key to change it so that it can be canceled.

#### Built-in StateBehaviour

- Change Additive field of LoadLevel to LoadSceneMode field.
- Change the Seconds field of TimeTransition to FlexibleFloat type.

#### Scripts

- Changed to use reference types commonly used in FlexibleField related class.
- Property such as Parameter.intValue etc.
	Changed to call onChanged when set.
- Added set to Parameter.value property.

### Deprecated  

#### Scripts

- Changed Parameter.OnChanged method to Obsolete.
	OnChanged is now internally called back when changing Parameter.intValue etc, so calling is no longer necessary.
- Change AddCalculatorMenu attribute to Obsolete.
	Changed to use AddBehaviourMenu attribute in common.
- Change BuiltInCalculator attribute to Obsolete.
	Changed to use BuiltInBehaviour attribute in common.
	Also, since the BuiltInBehaviour attribute is an attribute for built-in behavior, there is no problem if you do not use it otherwise.
- Change CalculatorHelp attribute to Obsolete.
	Changed to use BehaviourHelp attribute in common.
- Change CalculatorTitle attribute to Obsolete.
	Changed to use BehaviourTitle attribute in common.

### Fixed

#### ArborEditor

- Fixed that active display was kept on ArborEditor even when BehaviourTree ended.
- Fixed height of Calculator type field of FlexibleGameObject.
- Fixed that the graph name input field in the side panel was kept in focus even if you clicked anything other than the input field.
- Fixed that pressing the Esc key while dragging a connection line such as StateLink or CalculatorSlot left line display during dragging.
- Fixed that the appearance of the header style of the side panel was changed depending on the version of Unity.
- Fixed that editing such as cutting and pasting of text could not be done with FlexibleString with ConstantMultilineAttribute.
- Fixed occurrence of NullReferenceException when starting playing or switching scenes with docked ArborEditor window hidden.
- Fixed incorrect display position of connection line to node outside screen when exiting play mode.
- Fixed that node name was not copied when copying Action node.
- Fixed that node name was not copied when copying Composite node.
- Fixed that grid snap was not working when pasting or duplicating node.
- Fixed that the connection line did not disappear even if State was deleted when State transition source is a reroute node.
- Fixed that the connection line will not be redrawn immediately if you redo the state connecting StateLink after deleting it.
- Fixed Undo / Redo of node selection.
- Fixed that memory leak occurred when repeating Undo / Redo of node creation and deletion.
- Fixed bug when performing graph selection Undo / Redo.
- Fixed to prevent Frame Selected when node was not selected.

#### ArborFSM

- Fixed that NullReferenceException occurs when doing RemoveComponent of ArborFSM in Unity 2018.1 or later.
- When dragging and dropping the prefabricated ArborFSM onto the scene window, the component used inside the graph is displayed in the inspector.

#### BehaviourTree

- Fixed occurrence of NullReferenceException if interruption judgment is made at the timing when the current node becomes Root.
- Fixed that NullReferenceException occurs when doing RemoveComponent of BehaviourTree in Unity 2018.1 or later.
- When dragging and dropping the prefabricated BehaviourTree onto the scene window, the component used inside the graph is displayed in the inspector.

#### AgentController

- Fixed a bug that referenced Transform of AgentController itself.
- Fix to initialize AgentController with Awake.

#### Built-in StateBehaviour

- Fix SubStateMachine UpdateType to Manual
	Changed to handle at the appropriate timing by UpdateType of route graph.
- Fixed that ArborFSM no longer appears in the inspector when adding SubBehaviourTree.

#### Built-in ActionBehaviour

- Fixed Wait's Seconds recalculating every frame.

#### Built-in Decorator

- Fixed TimeLimit's Seconds recalculating every frame.
- Fixed Cooldown's Seconds recalculating every frame.

#### Scripts

- Fix State.transitionCount to uint.
- Fix to prevent State.transitionCount from exceeding uint.MaxValue.
- Fix StateLink.transitionCount to uint.
- Fixed StateLink.transitionCount not to exceed uint.MaxValue.

#### Others

- Fixed that it took time to start when play started immediately after script compilation.
- Fixed error in Unity 2018.2.0 Beta version.


## [3.0.2] - 2018-03-20

### Added

#### Built-in StateBehaviour

- Add ForceMode and Space fields to AddForceRigidbody.
- Add Space field to AddVelocityRigidbody.
- Add Space field to SetVelocityRigidbody.
- Add ForceMode and Space fields to AddForceRigidbody2D.
- Add Space field to AddVelocityRigidbody2D.
- Add Space field to SetVelocityRigidbody2D.
- Added StoppingDistance field to AgentMoveOnWaypoint.

#### Built-in ActionBehaviour

- Added StoppingDistance field to AgentMoveOnWaypoint.

### Fixed

#### ArborEditor

- Fixed that connection slots were not highlighted in inheritance relationship while dragging the calculator slots.

#### Component

- Fixed that AgentControllor 's isDone might not become true due to calculation error of float.

#### Built-in StateBehaviour

- Fix SubStateMachine's UpdateType to Manual.


## [3.0.1] - 2018-03-10

### Fixed

#### Arbor Editor

- Fixed an exception occurs when deleting the node graph after closing the Arbor Editor window.
- Fixed that the rename frame was not displayed when creating an action node or a composite node without docking the Arbor Editor window.
- Fixed an error occurred in ArborEditor if a DLL that failed to load existed.

#### Built-in ActionBehaviour

- Fixed that there was no reference of DestroyGameObject.


## [3.0.0] - 2018-03-09

### New: Behavior Tree

#### Overview

As a new function, Behaviour Tree which can combine behaviors while visualizing priority by tree structure has been added.

There are RootNode which becomes active first, CompositeNode which decides execution order of child nodes, and ActionNode which specifies action.

In ActionNode, you can set a script ActionBehavior which describes behavior.
ActionBehaviour can be customized as well as ArborFSM's StateBehaviour.

For CompositeNode and ActionNode you can also add a script Decorator which checks the condition to execute and repeats it.
Customization is also possible here.

Besides, flexible AI can be created by Service script executed while the node is active.

Also, as with ArborFSM, data can be exchanged between nodes by using calculator nodes and calculator slots.

#### Component

- Added BehaviourTree component.

#### Built-in CompositeBehaviour

- Added Selector.
- Added Sequencer.

#### Built-in ActionBehaviour

- Added Wait.
- Added PlaySound.
- Added PlaySoundAtPoint.
- Added PlaySoundAtTransform.
- Added StopSound.
- Added SubStateMachine.
- Added SubBehaviourTree.
- Added InstantiateGameObject.
- Added DestroyGameObject.
- Added ActivateGameObject.
- Added AgentPatrol.
- Added AgentMoveToPosition.
- Added AgentMoveToTransform.
- Added AgentMoveOnWaypoint.
- Added AgentEscapeFromPosition.
- Added AgentEspaceFromTransform.
- Added AgentStop.

#### Built-in Decorator

- Added Loop.
- Added SetResult.
- Added InvertResult.
- Added ParameterCheck.
- Added CalculatorCheck.
- Added ParameterConditionalLoop.
- Added CalculatorConditionLoop.
- Added TimeLimit.
- Added Cooldown.

#### Script

- Added CompositeBehaviour to control execution of child nodes.
- Added ActionBehaviour to execute action.
- Added Decorator to decorate nodes.
- Added Service to run while the node is active.

### Added

#### Arbor Editor

- Implement hierarchy of graphs.
- Added Graph item to the side panel.
- Added menus when dropping on nothing when StateLink is dragging.
- Added a reroute node of the StateLink connection line.
- Added a reroute node of CalculatorBranch connection line.
- Added to log to Console by clicking the value of CalculatorBranch.
- Adding a function that does not move the nodes in the group when moving the group node while holding down the Alt(Option on Mac) key.

#### ArborFSM

- Flag to play at start Added Play On Start field.
- Added Update Settings field to set update interval etc.

#### ParameterContainer

- Added type specification of Component parameter.
- Added Color parameter.

#### Built-in StateBehaviour

- Added SubStateMachine.
- Added EndStateMachine.
- Added SubBehaviourTree.
- Added Center Type field to AgentPatrol.
- Added Center Transform field to AgentPatrol.
- Added Center Position field to AgentPatrol.
- Added Is Check Tag field to RaycastTransition.
- Added Tag field to RaycastTransition.

#### Script

- Added base class NodeGraph for handling graphs.
- Added interface INodeBehaviourContainer to be used when Node stores NodeBehaviour.
- Added interface INodeGraphContainer to be used when NodeBehaviors store child graphs.
- Added OnCreated method called when creating NodeBehaviour.
- Added OnPreDestroy method called before NodeBehaviour discard.
- Added Play and Stop methods to ArborFSMInternal.
- Added Pause and Resume method to ArborFSMInternal.
- Added playState property to ArborFSMInternal.
- Added TimeUtility.CurrentTime method to return current time from TimeType.
- Added GetClosestParam method to Bezier2D.
- Added GetClosestPoint method to Bezier2D.
- Added SetStartPoint method to Bezier 2D.
- Added SetEndPoint method to Bezier 2D.
- Added ComponentParameterReference so that the type of Component referenced by SlotTypeAttribute can be specified.

### Changed

#### Arbor Editor

- Renamed state list of toolbar to side panel.
- Renamed state list of side panel to node list.
- When copying and pasting the state, when copying source and copying destination were the same ArborFSM, changed to paste with StateLink connected.
- When copying and pasting the state, change StateLink so that StateLink will stay connected if the state to which StateLink is connected is also pasted node.
- Change color when mouse over of StateLink's line.
- Changed to display the menu even when right-clicking the title bar of StateBehaviour.
- Change the way to display the value of CalculatorBranch during execution by mouse over the line or checking "Always display value" from the right click menu.
- Change the design of the current state being executed.
- Adjust scrolling to move to the selected node.

#### Built-in StateBehaviour

- Agent MoveOnWaypoint Changed to move to the current target point at the start and move the target point to the next after the movement is completed.

#### Script

- ArborFSMInternal changed to inherit from NodeGraph.
- Remove the fsmName of ArborFSMInternal (the use of graphName to NodeGraph).
- Changed FindFSM and FindFSMs of ArborFSM to Obsolete (use FindGraph and FindGraphs of NodeGraph).
- Changed stateID of StateBehaviour to Obsolete (use nodeID).
- Changed the graph referenced from Node and NodeBehaviour from ArborFSMInternal to NodeGraph.
- Changed to manage CalculatorNode by NodeGraph.
- Changed to manage GroupNode with NodeGraph.
- Changed to manage CommentNode with NodeGraph.
- Changed to manage ClaculatorBranch by NodeGraph.
- Change the stateMachine field of CalculatorSlot to the nodeGraph field.
- Move the stateMachine property of NodeBehaviour to StateBehaviour.
- Changed lineEnable of StateLink to NonSerialized.
- Delete StateLink's lineStart and add a bezier field.
- Move the TimeType in the TimeTransition class to the Arbor namespace.
- Changed the built-in StateBehaviour namespace to Arbor.StateMachine.StateBehaviours.
- Changed to specify Parameter to be referenced by SlotTypeAttribute specified by FlexibleComponent.
- Changed so that you can use the RequiresContentRepaint method of the Editor class when redrawing is required.

#### Other

- Dragged from NodeGraph from Inspector to Hierarchy so that it can not be moved.
- Move built-in NodeBehaviour related scripts to the BuiltInBehaviours folder.

### Fixed

#### Arbor Editor

- Fixed Copy node disappearing when ArborFSM Copy Component is executed after copying node.
- Fixed that StackOverflowException occurred when repeating transition by TransitionTiming.Immediate.
- Fixed that it did not stop if you wanted to transition by TransitionTiming.Immediate even if a breakpoint was set in the state.
- Fixed an exception occurred when entering search word during hierarchical animation in AddBehaviourMenu window and AddCalculatorMenu window.
- Fixed that Arbor Editor reference does not return when Unode of Remove Component of NodeGraph.
- Fixed so that menu of graph is not displayed when right clicking in node.
- Fix to call OnStateAwake and OnStateBegin when behaviourEnabled is switched during execution.
- Fix to call OnStateAwake and OnStateBegin when StateBehaviour was added during execution.
- Fixed a log of "Removed unparented EditorWindow while reading window layout" appearing in TypePopupWindow when starting Unity.
- Fix to always do redrawing during execution only when necessary.
- Fix to grid snap when creating nodes.
- Fixed display of node blurred when size of graph area was changed by node creation etc.

#### Script

- Fixed that EachField enumerated duplicate public and protected fields of base class.

#### Other

- Fixed that NodeBehaviour remains internally when Reset from Inspector.
- Fixed that Calculator remained internally when deleting ArborFSM component.


## [2.2.3] - 2018-02-21

### Fixed

#### Arbor Editor

- Fixed that node shortcuts could be used while renaming nodes.
- When changing the order of ParameterContainer 's parameters, an exception that occurred when referring to that parameter was corrected.


## [2.2.2] - 2017-11-22

### Added

#### Arbor Editor

- Added so that nodes and objects using deleted StateBehaviour and Calculator scripts can be deleted.

### Changed

#### Other

- Raised Unity's minimum action version to 5.4.0f3.

### Fixed

#### Arbor Editor

- Fixed an exception (ArgumentNullException) occurred in the Arbor Editor window when deleting StateBehaviour and Calculator scripts used by ArborFSM.


## [2.2.1] - 2017-11-14

### Added

#### Component

- Long added to the parameters that can be used in ParameterContainer.

#### Built in Calculator

- Added LongAddCalculator
- Added LongSubCalculator
- Added LongMulCalculator
- Added LongDivCalculator
- Added LongNegativeCalculator
- Added LongCompareCalculator
- Added LongToFloatCalculator
- Added FloatToLongCalculator

#### Script

- Added ConstantMultilineAttribute which makes multiple lines of FlexibleString Constant display.
- Added FlexibleLong, LongParameterReference, InputSlotLong, OutputSlotLong along with the addition of long type parameter.

### Changed

#### Arbor Editor

- FlexibleString's Constant indication back.

#### Component

- Changed so that parameters can be rearranged by ParameterContainer.
- Display parameter type in ParameterContainer.

### Fixed

#### Arbor Editor

- Fixed occurrence of an exception (NullReferenceException: SerializedObject of SerializedProperty has been Disposed.) When playing on the Unity editor is open with Arbor Editor open.
- Fixed that Label of Graph does not change even when switching languages.

#### Script

- Fixed load was being applied by type enumeration processing at the beginning of play on Unity editor.


## [2.2.0] - 2017-10-25

### Added

#### Arbor Editor

- Added group node.
- Added comments for each node.
- Added node cutout.
- Added items for cutting, copying, duplicating, deleting nodes in node context menu.
- Transition Timing icon display on StateLink button.
- Copy Component of ArborFSM Inspector supports copying including internal components.

#### Built in Behaviour

- Added AgentStop.
- Added AnimatorCrossFade.
- Added AnimatorSetLayerWeight.
- Added BackToStartState.
- Added ActivateBehaviour.
- Added ActivateRenderer.
- Added ActivateCollider.
- Added ChangeTimingUpdate parameter to UISetSlider, UISetText, UISetToggle.

#### Built in Calculator

- Added GameObjectGetComponentCalculator.

#### Component

- Added various parameters for Animator setting to AgentController.
- Added Waypoint component.
- Added Component to parameters available in ParameterContainer.

#### Script

- Add parameter reference class for each Animator type.
- Addition of OnStateUpdate callback called during Update in order of StateBehaviour.
- Addition of OnStateLateUpdate callback called during LateUpdate in order of StateBehaviour.
- InputSlotUnityObject, OutputSlotUnityObject added.
- Added FlexibleComponent.
- Added SlotTyeAttribute which can specify type with CalculatorSlot or FlexibleCompoment.
- ComponentParameterReference added.
- Create a NodeBehaviour class that summarizes the intersection of StateBehaviour and Calculator.
- Added interface, INodeBehaviourSerializationCallbackReceiver, to describe the processing at serialization of NodeBehaviour.
- Added type and parameter property to various Flexible classes.

### Changed

#### Arbor Editor

- Editor design change.
- Changed to display state name input field with double click.
- Changed to sort the state list in order of type (start state -> normal state -> resident state).
- Change Immediate Transition of StateLinkSettingWindow to Transition Timing.
- Added type specification to OutputSlotComponent.
- Change the display when Constant of FlexibleString to TextArea.
- Corresponds to scroll at fixed time intervals while scrolling with dragging.
- Adjust the maximum amount of movement during drag scrolling.

#### Built in Behaviour

- Added flag to whether LookAtGameObject uses each coordinate component of Target transform.
- Added flag to determine whether to stop Agent when leaving state to Agent system.
- Added reference URL for format specification to the format document of UITextFromParameter.
- Built-in behavior referring to Component corresponds to FlexibleComponent.
- Move UISetSliderFromParameter and UISetToggleFromParameter to Lagacy by FlexibleComponent correspondence of UISetSlider and UISetToggle.

#### Component

- Change Animator specification of AgentController so that it is unified for each parameter.

#### Script

- The ID of each node is unified to Node.nodeID.
- Added ToString method to Parameter class.
- Added updatedTime to CalculatorBranch.
- Addition of isUsed and updatedTime to InputSlot.
- Change transition timing specification in Transition method to TransitionTiming.
- Change CalculatorSlot.position to NonSerialized.
- Add using System.Collections.Generic; and AddComponentMenu ("") to the template.

### Fixed

#### Arbor Editor

- Fixed that node position shifted when scrolling while dragging a node
- Correct that vertex number error occurs when CalculatorBranch line is long.
- Fixed that vertical margin is displayed when StateLinkSettingWindow is displayed at the screen edge.
- Fixed that lines are hidden when StateLink is reconnected to the same state.
- Fixed that StateBehaviour appears in the inspector when Apply to the Prefab of ArborFSM object during execution.
- Fixed that StateBehaviour of the current state is saved in Prefab while it is in the effective state when Apply to the Prefab of ArborFSM object during execution.
- Corrected the connection when changing Size in Behavior which has CalculatorSlot as an array.
- Fixed that other nodes are not displayed for a moment when node is deleted.

#### Built in Behaviour

- Fixed that exceptions are generated after transition of scenes with behavior being processed when Parameter is changed via GlobalParameterContainer (UISetTextFromParameter, UISetToggleFromParameter, UISetSliderFromParameter, ParameterTransition).
- Fixed that there was no UISetImage reference.

#### Built in Calculator

- Fixed an exception occurred after the scene transition in the Calculator processing when Parameter was changed via GlobalParameterContainer.

#### Script

- Fixed it because we did not reference the field of base class in EachField.

### Other

#### Arbor Editor

- Optimize display of dot line of CalculatorBranch.


## [2.1.8] - 2017-10-10

### Fixed

#### Arbor Editor

- Fixed that the drag process will not stop when the node is not displayed by scrolling while dragging StateLink or CalculatorSlot.


## [2.1.7] - 2017-09-15

### Changed

#### Script

- When disabled ArborFSM's Transition method is called, transition is delayed until it becomes enable even if immediateTransition is set to true.


## [2.1.6] - 2017-07-25

### Fixed

#### Arbor Editor

- Fixed a bug that exception occurs with the start of play when you add a StateBehaviour to Prefab instance of ArborFSM.


## [2.1.5] - 2017-07-20

### Fixed

#### Arbor Editor

- Speed up the Arbor Editor window.

#### Built in Behaviour

- Fix not to display RandomTransition on Inspector's Add Component menu.


## [2.1.4] - 2017-07-14

### Added

#### Script

- Added method to obtain index of various nodes in ArborFSMInternal: GetStateIndex(), GetCommentIndex(), GetCalculatorIndex()

### Fixed

#### Arbor Editor

- Fixed that a warning is displayed at the beginning of playing by copying a node.
- Fixed a bug that the node will be restored at the start of playing if deleting a node only from Prefabed instance.


## [2.1.3] - 2017-07-13

### Fixed

#### Arbor Editor

- Fixed exception which appears when pressing Apply button of Prefab instance attaching ArborFSM.


## [2.1.2] - 2017-07-07

### Added

#### Arbor Editor

- Added a function that can transition to arbitrary state on ArborEditor window during play.

#### Built in Behaviour

- Add OnStateAwake event and OnStateEnd event to SendEventGameObject.

#### Script

- Added Get / Set method of all types handled by ParameterContainer.
- Added GetParamID method to ParameterContainer.
- Added a method that can also perform Get / Set method of ParameterContainer from ID.

### Changed

#### Built in Behaviour

- Added OnTweenBegin method of callback for initialization at the start of Tween.
- Rename the Event parameter of SendEventGameObject to OnStateBegin.

### Fixed

#### Arbor Editor

- Fixed that ArborEditor window does not display properly when an exception occurs in StateBehaviour and Calculator Editor.
- Fixed exception that occurred when StateBehaviour and Calculator script could not be loaded.
- Fixed an exception "NullReferenceException: SerializedObject of SerializedProperty has been disposed." When ArborEditor is displayed.
- Fixed that the width of the state list becomes too narrow when the width of the ArborEditor window becomes narrower.

#### Script

- When SendTrigger was used during transition processing, it was modified to send Trigger after completion of transition processing.


## [2.1.1] - 2017-05-31

### Changed

#### Arbor Editor

- Move breakpoint and state count display out of frame

### Fixed

#### Arbor Editor

- Fixed that Copy & Paste could not be done with String of ParameterContainer.
- Fix to delete CalculatorBranch when data slot is deleted from script


## [2.1.0] - 2017-05-17

### Added

#### Arbor Editor

- Added a toggle to lock so as not to switch even when GameObject is selected.
- Implemented to be able to rearrange by dragging the title bar of StateBehaviour.
- Implemented so that StateBehaviour can be inserted at arbitrary position by drag & drop.
- Implemented so that you can set breakpoints in State.
- Implemented to display the number of times State and StateLink passed during execution.
- Implemented to highlight StateLink that passed immediately during execution.
- Implemented to display the value of CalculaterBranch during execution.
- Opens the help page from the built-in component's help button.
- Opens the help page from the help button of the built-in Calculator.
- Change the line color according to the type of CalculatorBranch.
- Add icon to ArborEditor window

#### Built in Behaviour

- Added RandomTransition.

### Changed

#### Built in Behaviour

- Correspond to output GameObject found by FindGameObject, FindWithTagGameObject to the operation node.
- TimeType specification added to TimeTransition.

#### Script

- Change OnStateTrigger to virtual function of StateBehaviour.

#### Other

- Updated reference site.
- Raise Unity's lowest action version to 5.3.0f4.

### Fixed

#### Arbor Editor

- ArborFSM Fixed no longer be able to access the data from the input-output slot when you move to another GameObject.
- The graph display area of Arbor Editor is fixed.
- Fixed that Arbor Editor's graphic display blurred when automatically scrolling to the selected state, such as when selecting from the state list.

#### Built in Behaviour

- Fix caching with reference to Flexible component.
- Fixed an error when decreasing the size of the array on the editor with Behavior which has StateLink in the array.

#### Script

- Fix to prevent errors when null is passed to AgentController.Follow and Escape.


## [2.0.10] - 2017-04-11

### Changed

#### Arbor Editor

- When selecting GameObject, Arbor Editor also works in conjunction so that display is switched.

### Fixed

#### Arbor Editor

- Fixed an error when creating a new comment.
- Fixed an error when starting play when copying node.
- Fixed that you can not paste after copying the node and starting playing once.
- Fix copy of Calculator node.
- Fixed processing when copying & pasting or duplicating StateBehaviour or Calculator with CalculatorSlot.


## [2.0.9] - 2017-04-04

### Changed

#### Built in Behaviour

- Change Bool of CalculatorTransition to compare two Bool values.

#### Scripts

- Add State.behaviourCount and GetBehaviourFromIndex. Deprecated State.behaviours.
- ArborFSMInternal.stateCount and GetStateFromIndex added. Deprecated ArborFSMInternal.states.
- Added ArborFSMInternal.commentCount and GetCommentFromIndex. Deprecated ArborFSMInternal.comments.
- ArborFSMInternal.calculatorCount and GetCalculatorFromIndex added. Deprecated ArborFSMInternal.calculators.
- ArborFSMInternal.calculatorBranchCount and GetCalculatorBranchFromIndex added. Deprecated ArborFSMInternal.calculatorBranchies.

### Fixed

#### Arbor Editor

- Fixed an error when opening ArborEditor with Unity 5.6.
- Speed up the Arbor Editor window.


## [2.0.8] - 2016-12-29

### Changed

#### Arbor Editor

- Change Script Execution Order so that ArborFSM is executed first.

### Fixed

#### Arbor Editor

- Fixed an error displayed when copying Node or StateBehaviour in Arbor Editor after Unity 5.3.4 or later.


## [2.0.7] - 2016-11-16

### Added

#### Arbor Editor

- Added OutputSlotString and InputSlotString
- Added FlexibleString
- Added string to ParameterContainer
- Implement processing with string parameter in CalcParameter
- Implement transitions with string parameters in ParameterTransition
- Implement text setting from string to UISetTextFromParameter.

### Fixed

#### Arbor Editor

- Fixed that slots do not display correctly in Arbor Editor when creating customized class of OutputSlot / InputSlot


## [2.0.6] - 2016-11-07

### Fixed

#### Arbor Editor

- Fixes the Undo / Redo when deleting a node.
- Fixes the Undo / Redo when deleting a StateBehaviour.


## [2.0.5] - 2016-10-13

### Changed

#### Built in Behaviour

- change the parameters of the Tween series to be able to receive from the calculator node.

### Fixed

#### Arbor Editor

- Fix to repaint the ArborEditor window when you add a calculator node and state behaviour.
- Fixed the ArborEditor namespace RectUtility.
- Fix a template for Boo.



## [2.0.4] - 2016-09-24

### Added

#### Arbor Editor

- Adding to call the OnStateAwake() when you first entered the State.

### Fixed

#### Arbor Editor

- Fix a menu which can be moved to the transition destination when you right-click a transition arrow to display in control + click on the Mac.
- Fix the error in the Unity5.5.0Beta.


## [2.0.3] - 2016-07-16

### Fixed

#### Arbor Editor

- Fixed a warning that exits at Unity5.4.0Beta
- Modify the state is not generated in the mouse position at the time of the state of the paste or duplication.


## [2.0.2] - 2016-07-13

### Added

#### Built in Behaviour

- corresponding to be able to set the Additive property to Scene / LoadLevel.
- Scene / UnloadLevel add (Unity5.2 or later).

### Fixed

#### Arbor Editor

- Unity5.3.0 Fixed When you play start still selected ArborFSM object StateBehaviour from being removed on the later of the Unity editor.

#### Built in Behaviour

- Fixed a Application.LoadLevel warning exiting at Unity5.3.0 later Scene / LoadLevel.


## [2.0.1] - 2016-07-01

### Added

#### Built in Behaviour

- Adds the specified AudioMixerGroup and SpatialBlend in Audio / PlaySoundAtTransform.
- add an Audio / PlaySoundAtPoint of new coordinates specified.

### Changed

#### Arbor Editor

- reduce the amount of heap memory.

#### Built in Behaviour

- Renamed the Audio / PlaySoundAtPoint in Audio / PlaySoundAtTransform.

### Fixed

#### Arbor Editor

- Fixed editor management for the object every time you compile had been increasing.


## [2.0.0] - 2015-10-16


### Added

#### Arbor Editor

- Add calculaltor node.
- corresponding to be able to hold the Vector2 in ParameterContainer.
- corresponding to be able to hold the Vector3 in ParameterContainer.
- corresponding to be able to hold a Quaternion in ParameterContainer.
- corresponding to be able to hold the Rect in ParameterContainer.
- corresponding to be able to hold the Bounds in ParameterContainer.
- corresponding to be able to hold the Transform in ParameterContainer.
- corresponding to be able to hold the RectTransform in ParameterContainer.
- corresponding to be able to hold the Rigidbody in ParameterContainer.
- corresponding to be able to hold the Rigidbody2D in ParameterContainer.

#### Built in Behaviour

- Transition/Physics/RaycastTransition
- Transition/Physics2D/Raycast2DTransition
- Transition/CalculatorTransition
- Add the output of GameObject generated in InstantiateGameObject
- Add the output of the other Collision hitting the OnCollisionEnterTransition
- Add the output of the other Collision hitting the OnCollisionExitTransition
- Add the output of the other Collision hitting the OnCollisionStayTransition
- Add the output of the other Collider, which hit the OnTriggerEnterTransition
- Add the output of the other Collider, which hit the OnTriggerExitTransition
- Add the output of the other Collider, which hit the OnTriggerStayTransition
- Add the output of the other Collision2D hitting the OnCollisionEnter2DTransition
- Add the output of the other Collision2D hitting the OnCollisionExit2DTransition
- Add the output of the other Collision2D hitting the OnCollisionStayT2Dransition
- Add the output of the other Collider2D hitting the OnTriggerEnter2DTransition
- Add the output of the other Collider2D hitting the OnTriggerExit2DTransition
- Add the output of the other Collider2D hitting the OnTriggerStayT2Dransition

#### Built in Calculator

- Calculator additional Bool
- Calculator additional Bounds
- Calculator additional Collider
- Calculator additional Collider2D
- Calculator additional Collision
- Calculator additional Collision2D
- Calculator additional Component
- Calculator additional Float
- Calculator additional Int
- Calculator additional Mathf
- Calculator additional Quaternion
- Calculator additional RaycastHit
- Calculator additional RaycastHit2D
- Calculator additional Rect
- Calculator additional RectTransform
- Calculator additional Rigidbody
- Calculator additional Rigidbody2D
- Calculator additional Transform
- Calculator additional Vector2
- Calculator additional Vector3

#### Scripts

- FlexibleBounds implementation
- FlexibleQuaternion implementation
- FlexibleRect implementation
- FlexibleRectTransform implementation
- FlexibleRigidbody implementation
- FlexibleRigidbody2D implementation
- FlexibleTransform implementation
- FlexibleVector2 implementation
- FlexibleVector3 implementation

### Changed

#### Built in Behaviour

- AgentEscape the corresponding to FlexibleTransform.
- AgentFllow the corresponding to FlexibleTransform.
- PlaySoundAtPoint the corresponding to FlexibleTransform.
- InstantiateGameObject the corresponding to FlexibleTransform.
- LookAtGameObject the corresponding to FlexibleTransform.
- AddForceRigidbody the corresponding to FlexibleRigidbody.
- AddVelocityRigidbody the corresponding to FlexibleRigidbody.
- SetVelocityRigidbody the corresponding to FlexibleRigidbody.
- AddForceRigidbody2D the corresponding to FlexibleRigidbody2D.
- AddVelocityRigidbody2D the corresponding to FlexibleRigidbody2D.
- SetVelocityRigidbody2D the corresponding to FlexibleRigidbody2D.


## [1.7.7p2] - 2015-09-30

### Fixed

#### Arbor Editor

- Fixed an error that exits at Unity5.2.1 later.


## [1.7.7p1] - 2015-09-29

### Fixed

#### Arbor Editor

- Fix for creation and deletion of the state and the comment could not be Undo.


## [1.7.7] - 2015-09-19

### Added

#### Arbor Editor

- Corresponding to be able to hold a GameObject in ParameterContainer.

#### Built in Behaviour

- Collision/OnCollisionEnterStore
- Collision/OnCollisionExitStore
- Collision/OnControllerColliderHitStore
- Collision/OnTriggerEnterStore
- Collision/OnTriggerExitStore
- Collision2D/OnCollisionEnter2DStore
- Collision2D/OnCollisionExit2DStore
- Collision2D/OnTriggerEnter2DStore
- Collision2D/OnTriggerExit2DStore
- GameObject/FindGameObject
- GameObject/FindWithTagGameObject
- Added to allow relative specified in UITweenPosition.
- Added to allow relative specified in UITweenSize.

#### Script

- FlexibleInt implementation
- FlexibleFloat implementation
- FlexibleBool implementation
- FlexibleGameObject implementation
- Corresponding to use the ContextMenu.

### Changed

#### Arbor Editor

- Change to be able to transition to their own state.
- Change the background of behavior.
- Change the background of ListGUI.
- Change the comment node to resize depending on the contents.
- Corresponding to save the settings, such as the grid for each major version of Unity instead of every project.

#### Built in Behaviour

- Corresponding value of BroadcastMessageGameObject to use such FlexibleInt.
- Corresponding value of CalcAnimatorParameter to use such FlexibleInt.
- Corresponding value of CalcParameter to use such FlexibleInt.
- Corresponding value of ParameterTransition to use such FlexibleInt.
- Corresponding value of SendMessageGameObject to use such FlexibleInt.
- Corresponding value of SendMessageUpwardsGameObject to use such FlexibleInt.
- The corresponding AgentEscape to ArborGameObject.
- The corresponding AgentFllow to ArborGameObject.
- The corresponding ActivateGameObject to FlexibleGameObject.
- The corresponding DestroyGameObject to FlexibleGameObject.
- The corresponding LookatGameObject to FlexibleGameObject.
- The corresponding BroadcastTrigger to FlexibleGameObject.
- The corresponding SendTriggerGameObject to FlexibleGameObject.
- The corresponding SendTriggerUpwards to FlexibleGameObject.
- Corresponding to be able to store the object that was generated by the InstantiateGameObject the parameter.

#### Other

- Parameter related to move to Core folder and the Internal folder.
- Component to the icon set.

### Fixed

#### Arbor Editor

- Bug fixes around Undo
- Fixed resident state could be set to the start state.


## [1.7.6] - 2015-09-17

### Added

#### Arbor Editor

- The name setting adds to StateLink.
- Add immediate transition flag to StateLink.

#### Component

- GlobalParameterContainer

#### Built int Behaviour

- Audio/PlaySound
- Audio/StopSound
- Collision/OnCollisionEnterDestroy
- Collision/OnCollisionExitDestroy
- Collision/OnControllerColliderHitDestroy
- Collision2D/OnCollisionEnter2DDestroy
- Collision2D/OnCollisionExit2DDestroy
- GameObject/BroadcastMessageGameObject
- GameObject/SendMessageUpwardsGameObject
- Physics/AddForceRigidbody
- Physics/AddVelocityRigidbody
- Physics2D/AddForceRigidbody2D
- Physics2D/AddVelocityRigidbody2D
- Renderer/SetSprite
- Transition/Collision/OnCollisionEnterTransition
- Transition/Collision/OnCollisionExitTransition
- Transition/Collision/OnCollisionStayTransition
- Transition/Collision/OnControllerColliderHitTransition
- Transition/Collision2D/OnCollisionEnter2DTransition
- Transition/Collision2D/OnCollisionExit2DTransition
- Transition/Collision2D/OnCollisionStay2DTransition
- Transition/Input/ButtonTransition
- Transition/Input/KeyTransition
- Transition/Input/MouseButtonTransition
- Transition/ExistsGameObjectTransition
- Trigger/BroadcastTrigger
- Trigger/SendTriggerGameObject
- Trigger/SendTriggerUpwards
- Tween/TweenRigidbody2DPosition
- Tween/TweenRigidbody2DRotation
- Tween/TweenTextureOffset
- UI/UISetSlider
- UI/UISetSliderFromParameter
- UI/UISetToggle
- UI/UISetToggleFromParameter
- Add to display a progress bar the current time to TimeTransition.
- It added to allow transition at the time of Tween end.
- Added to allow relative specified in TweenPosition.
- Added to allow relative specified in TweenRotation.
- Added to allow relative specified in TweenScale.
- Added to allow relative specified in TweenRigidbodyPosition.
- Added to allow relative specified in TweenRigidbodyRotation.

#### Script

- Corresponding to prevent modification of the immediate transition flag in FixedImmediateTransition attribute.

#### Example

- Sample additional GlobalParameterContainer as Example9.

### Changed

#### Built int Behaviour

- Renamed SetRigidbodyVelocity to SetVelocityRigidbody.
- Renamed SetRigidbody2DVelocity to SetVelocityRigidbody2D.

### Improved

#### Arbor Editor

- When you open the behavior added, corresponding as focus moves to the search bar.
- In order of at Add Behaviour, adjusted so that the group comes first.

### Fixed

#### Arbor Editor

- Fixed search string in the behavior added was not able to save.

#### Built int Behaviour

- Fixed OnTriggerExit2DDestroy of was in Collision.
- Fixed floatValue of had become int of CalcAnimatorParameter.
- Fixed floatValue of had become int of CalcParameter.
- Fixed floatValue of had become int of ParameterTransition.

#### Example

- Modify because Coin has been added to the Tags.


## [1.7.5] - 2015-09-03

### Added

#### Built in Behaviour

- Collision/OnTriggerEnterDestroy
- Collision/OnTriggerExitDestroy
- Collision2D/OnTriggerEnter2DDestroy
- Collision2D/OnTriggerExit2DDestroy
- GameObject/LookAtGameObject
- Parameter/SetBoolParameterFromUIToggle
- Parameter/SetFloatParameterFromUISlider
- Physics/SetRigidbodyVelocity
- Physics2D/SetRigidbody2DVelocity
- Transition/EventSystems/OnPointerClickTransition
- Transition/EventSystems/OnPointerDownTransition
- Transition/EventSystems/OnPointerEnterTransition
- Transition/EventSystems/OnPointerExitTransition
- Transition/EventSystems/OnPointerUpTransition
- Tween/TweenCanvasGroupAlpha
- Tween/TweenRigidbodyPosition
- Tween/TweenRigidbodyRotation
- UI/UISetImage
- UI/UISetTextFromParameter
- Add in a way that allows you to specify the initial Transform at the time of generation in InstantiateGameObject.

#### Script

- The value property added to the Parameter.
- IntParameterReference added.
- FloatParameterReference added.
- BoolParameterReference added.

#### Example

- coin pusher game add as Example7.
- sample additional EventSystem as Example8.

#### Other

- add from the Hierarchy of the Create button to make the ArborFSM with GameObject.
- add from the Hierarchy of the Create button to make the ParameterContainer with GameObject.
- add from the Hierarchy of the Create button to make the AgentController with GameObject.

### Changed

#### Other

- folder organization.

### Improved

#### Arbor Editor

- support to be able to resize the width of the state list.

### Fixed

#### Arbor Editor

- Fixed there are times when the grid is not displayed correctly.

#### Built in Behaviour

- Fixed did not work properly in the case of type Bool in CalcParameter.
- modified to not bother to specify the person to call in SendEventGameObject.


## [1.7.4] - 2015-08-25

### Added

#### Built in Behaviour

- Agent system Behaviour added.
- uGUI system Behaviour added.
- uGUI system Tween added.
- SendEventGameObject added.
- The pass-by-value function added to the SendMessageGameObject.

### Changed

#### Other

- Pull up on the Unity minimum operating version to 4.6.7f1 due to uGUI correspondence.

### Fixed

#### Arbor Editor

- AnimatorParameterReference of reference is to Fixed get an error when that did not refer to a AnimatorController.


## [1.7.3] - 2015-08-21

### Added

- OnMouse system Transition add

### Changed

- modified to sort the state list by name.
- Arbor change to be able to place the infinite state also to the upper left of the Editor.
- to renew the manual site.

### Fixed

- move when the scroll position correction to the selected state


## [1.7.2] - 2015-08-18

### Added

- Add a comment node in ArborEditor.
- corresponding to be able to search at the time of behavior added.
- CalcAnimatorParameter added.
- AnimatorStateTransition added.
- add to be able to move to a transition source and a transition destination in the right-click on the transition line.

### Changed

- Renamed the ForceTransition to GoToTransition.
- change so as not to omit the name of the built-in Behaviour that is displayed in the behavior added.
- change so as not to display the built-in Behaviour to Add Component.

### Fixed

- Prefab source to the Fixed not correctly added to the Prefab destination and behavior added.


## [1.7.1] - 2015-08-11

### Added

- Add the state list.
- Add PropertyDrawer of ParamneterReference.
- Add GUI, the ListGUI for the list that can be deleted elements.

### Fixed

- Fixed boolValue of had become int of CalcParameter.


## [1.7.0] - 2015-08-11

### Added

- parameter container.

### Fixed

- OnStateBegin () If you have state transitions, fix than it so as not to run the Behaviour under.


## [1.6.3f1] - 2015-07-29

### Chaned

- Unity5 RC3 by the corresponding Renamed OnStateEnter / OnStateExit to OnStateBegin / OnStateEnd.

### Fixed

- Fixed an error that exits at Unity5 RC3.


## [1.6.3] - 2015-02-20

### Added

- Add the force flag in Transition. you can do to transition on the spot at the time of the call to be to true.
- Embedded documentation comments to the source code.
- Place the script reference to Assets / Arbor / Docs.
  Please open the index.html Unzip.


## [1.6.2] - 2014-08-15

### Fixed

- The Fixed a state can not transition in OnStateEnter.


## [1.6.1] - 2014-08-07

### Fixed

- Error is displayed if you press Grid button in the Mac environment.


## [1.6] - 2014-07-08

### Added

- Resident state.
- Multilingual.
- Correspondence to be named to ArborFSM.

### Fixed

- Are not reflected in the snap interval when you change the grid size.
- Deal of the problems StateBehaviour is lost when you copy and paste a component of ArborFSM.
- Modified to send only to the state currently in effect the SendTrigger.
- StateBehaviour continues to move If you disable ArborFSM.


## [1.5] -  2014-06-25

### Added

- Support for multiple selection of the state. 
- Support for shortcut key. 
- grid display support. 

### Fixed

- it placed in a state in which it is spread by default when adding Behaviour. 
- I react mouse over to the state is shifted while dragging StateLink.


## [1.4] - 2014-06-21

### Added

- Tween-based Behaviour added. 
  - Tween / Color 
  - Tween / Position 
  - Tween / Rotation 
  - Tween / Scale 
- The HideBehaviour to add attributes that do not appear in the Add Behaviour. 
- online help of the built-in display Behaviour from the Help button on the Behaviour.


## [1.3] - 2014-06-18

### Added

- Add built-in Behaviour. 
  - Audio / PlaySoundAtPoint 
  - GameObject / SendMessage 
  - Scene / LoadLevel 
  - Transition / Force 
- Copy and paste across the scene. 

### Fixed

- memory leak warning is displayed when you save the scene after which you copied the State. 
- arrow will remain when you scroll the screen to drag the connection of the StateLink.


## [1.2] - 2014-06-07

### Added

- Enabled check box of StateBehaviour. 

### Fixed

- Errors Occur When you release the maximization of Arbor Editor. 
- Warning of the new line of code can when you edit a C# script that generated.

## [1.1] - 2014-05-30

### Added

- script generation of Boo and JavaScript. 
- Copy and paste the State. 
- Copy and paste the StateBehaviour. 

### Fixed

- support when the script becomes Missing. 
- Fixed array of StateLink is not displayed.


## [1.0.1] - 2014-05-27

### Fixed

- Error in Unity4.5. 
- Arbor Editor is not repaint when running in the editor. 
- class name of the Inspector extension of ArborFSM.


## [1.0] - 2014-05-21

- First release