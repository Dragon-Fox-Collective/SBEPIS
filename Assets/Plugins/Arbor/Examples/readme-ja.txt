# Exampleリスト

## ExampleSelector

サンプルシーンを一覧から選択するシーン。
(ビルドする場合は各シーンをBuild SettingsのScenes In Buildへ追加する必要があります)

## 01(Basic FSM)

基本的なステートマシンの使用例。
MainMenuオブジェクトのArborFSMではOnGUIを使用したメニューの遷移を行っている。

### 内容

* Input
    * Any Key
	  何かしらの入力による遷移の例
	  (AnyKeyDownTransition, AnyKeyTransition)

	* Key
	  キーボード入力による遷移の例
	  (KeyDownTransition, KeyUpTransition)

	* Mouse Button
	  マウスボタン入力による遷移の例
	  (MouseButtonDownTransition, MouseButtonUpTransition)

* Time
  時間経過による遷移の例
  (TimeTransition)

* Collision
  OnTriggerEnterイベント、OnTriggerExitイベントによる遷移の例
  (InstantiateGameObject, OnTriggerEnterTransition, OnTriggerExitTransition, OnTriggerEnterDestroy)

* Trigger
  FSM間のトリガーメッセージ送受信による遷移の例
  (SendTrigger, TriggerTransition)



## 02(OnMouse)

OnMouseイベントを使用するステートマシンの例。

### 内容

* MouseEnter/Exit
  OnMouseEnterイベント、OnMouseExitイベントによる遷移の例
  (OnMouseEnterTransition, OnMouseExitTransition)

* MouseDown/Up
  OnMouseDownイベント、OnMouseUpイベントによる遷移の例
  (OnMouseDownTransition, OnMouseUpTransition)



## 03(EventSystems)

EventSystemsを使用するステートマシンの例。

### 内容

* Left Cube (PointerDown/Up)
  OnPointerDown、OnPointerUpによる遷移の例
  (OnPointerDownTransition, OnPointerUpTransition)

* Center Cube (PointerEnter/Exit)
  OnPointerEnter、OnPointerExitによる遷移の例
  (OnPointerEnterTransition, OnPointerExitTransition)

* Right Cube (PointerClick)
　OnPointerClickによる遷移の例
  (OnPointerClickTransition)



## 04(UI)

UnityUIを使用するステートマシンの例。

### 内容

* Button
  Buttonクリックによる遷移やTextへの文字列の設定を行う。
  また、UITweenを使用して出現アニメやクリックアニメも行う。
  (UIButtonTransition, UISetText, UITweenSize, UITweenColor, UITweenPosition)

* Toggle
  Toggleの切り替えによる遷移を行う。
  (UIToggleTransition, UISetText)

* Slider
  Sliderの値を判定して遷移を行う。
  (UISliderTransition, UISetText)



## 05(ParameterContainer)

ParameterContainerを使用してパラメータによる遷移条件付けを行う例。

### 内容

* ParameterContainer
  int型のCounterパラメータを所持。

* ArborFSM
  Counterパラメータを0から1ずつ増やして10以上になったら完了する。
  (CalcParameter, ParameterTransition, UISetTextFromParameter)



## 06(GlobalParameterContainer)

GlobalParameterContainerを使用してシーンを変更しても破棄されないパラメータを扱う例。

### 内容

* GlobalParameterContainer
  共有するParameterContainerプレハブへの参照を保持。

* ArborFSM
  CalcParameterのContainer参照にGlobalParameterContainerを介す例。
  (CalcParameter, UISetTextFromParameter, TweenCancasGroupAlpha)



## 07(DataFlow)

データフローによるデータの入出力を行う例。

### 内容

* DataFlowExampleData.cs
  自作の構造体DataFlowExampleDataを定義するスクリプト。
  自作の構造体をデータフローで扱うためのInputSlot、OutputSlotも併せて定義。

* DataFlowExampleBehaviour.cs
  入力スロットから受け取ったDataFlowExampleDataの値をTextに表示するStateBehaviourスクリプト。

* DataFlowExampleNewDataCalculator.cs
  DataFlowExampleDataをnewして出力するCalculatorスクリプト。
  データの各フィールドはFlexibeStringとFlexibleIntを使用し、固定値やデータフローからの入力値を設定する例。

* ArborFSMオブジェクト
  自作の構造体を作成するCalculatorと、InputSlotから受けとったデータを表示する例。
  (DataFlowExampleBehaviour, DataFlowExampleNewDataCalculator, Random.RangeInt)



## 08(Variable)

ParameterContainerへ自作のデータ型を追加する例。

### 内容

* VariableExampleDataVariable.cs
  Variable GeneratorによるParameterContainer用Variableの生成例。

* VariableExampleBehaviour.cs
  生成したFlexibleVariableExampleData型を使用して値を表示するStateBehaviourスクリプト例。

* VariableExampleSetNameCalculator.cs
  生成したInputSlotVariableExampleData型とOutputSlotVariableExampleData型を使用して、自作Variableをデータフローで扱うCalculatorスクリプト例。

* ParameterContainerオブジェクト
　生成したVariableExampleDataをパラメータに使用する例。

* ArborFSMオブジェクト
  自作のVariableを使用するステートマシンの例。
  (VariableExampleBehaviour, VariableExampleSetNameCalculator, GetParameter)



## 09(DataLink)

DataLink属性を使用して新しいデータフローからの入力を行う例。

### 内容

* DataLinkExampleData.cs
  自作の構造体DataLinkExampleDataを定義するスクリプト。
  自作の構造体をデータフローで扱うためのOutputSlotも併せて定義。

* DataLinkExampleBehaviour.cs
  DataLink属性を使用して設定されたDataLinkExampleDataの値をTextに表示するStateBehaviourスクリプト。

* DataLinkExampleNewDataCalculator.cs
  DataLinkExampleDataをnewして出力するCalculatorスクリプト。
  データの各フィールドはDataLink属性を使用し、固定値やデータフローからの入力値を設定する例。

* ArborFSMオブジェクト
  自作の構造体を作成するCalculatorと、データフローから受けとったデータを表示する例。
  (DataLinkExampleBehaviour, DataLinkExampleNewDataCalculator, Random.RangeInt)



## 10(Animator)

AnimatorのステートマシンとArborFSMを連動する例。

### 内容

* Animator Parameter Controller
  Triggerパラメータにより遷移するだけのシンプルなAnimatorController

* Animatorオブジェクト
  AnimatorコンポーネントとArborFSMを連動する例。
  (CalcAnimatorParamerter, AnimatorStateTransition)



## 11(Agent)

NavMeshAgentを使用してキャラクターを動かす例。

### 内容

* PatrolAgentオブジェクト
  初期位置を中心にうろつくキャラクター。
  (AgentMoveToRandomPosition)

* FollowAgentオブジェクト
  プレイヤーを追従するキャラクター。
  (AgentFollow)

* EscapeAgentオブジェクト
  プレイヤーから逃げるキャラクター。
  (AgentEscape)

* Patrol-FollowAgentオブジェクト
  プレイヤーとの距離が5メートル以内であればプレイヤーを追従する。そうでなければ初期位置を中心にうろつく。
  (AgentMoveToRandomPosition, AgentFollow, DistanceTransition)

* Patrol-EscapeAgentオブジェクト
  プレイヤーとの距離が5メートル以内であればプレイヤーから逃避する。そうでなければ初期位置を中心にうろつく。
  (AgentMoveToRandomPosition, AgentEscape, DistanceTransition)

* Waypoint-FollowAgentオブジェクト
  プレイヤーとの距離が5メートル以内であればプレイヤーを追従する。そうでなければWaypoint上を移動する。
  (AgentMoveOnWaypoint, AgentFollow, DistanceTransition)



## 12(RaycastFollow)

NavMeshAgentが追従するオブジェクトをRaycastを使用して決定する例。

### 内容

* Patrol-EscapeAgentオブジェクト
  RaycastFollowAgentとの距離が3メートル以内であればRaycastFollowAgentから逃避する。そうでなければ初期位置を中心にうろつく。
  (AgentMoveToRandomPosition, AgentEscape, DistanceTransition)

* RaycastFollowAgentオブジェクト
  Waypoint上を移動しながら自身の位置からレイキャストし、ヒットした相手がいれば追従する。
  (AgentMoveOnWaypoint, RaycastTransition, RaycastHitTransformCalculator, AgentFollow, AgentMoveToPosition)



## 13(BT Agent)

BehaviourTreeを使用してNavMeshAgentを移動させる例。

### 内容

* EnemyAngetオブジェクト
　プレイヤーとの距離が5メートル以内であればプレイヤーを追従する。そうでなければWaypoint上を移動する。
  (Selector, Sequencer, CalculatorCheck, AgentMoveToTransition, AgentMoveOnWaypoint)



## 14(Graph Hierarcy)

グラフの階層化の例。

### 内容

* RootFSMオブジェクト
  このArborFSMが子BehaviourTreeと子ArborFSMを持っている。
  (SubBehaviourTree, SubStateMachine, EndStateMachine)



## 15(External Graph)

プレハブ化したグラフを子グラフとして実行する例。

### 内容

* RootFSMオブジェクト
  このArborFSMが外部のBehaviourTreeやArborFSMをインスタンス化して実行する。
  (SubBehaviourTreeReference, SubStateMachineReference)

* ExternalFSMプレハブ
  RootFSMからSubStateMachineReferenceでインスタンス化されるFSMオブジェクト。

* ExternalBTプレハブ
  RootFSMからSubBehaviourTreeReferenceでインスタンス化されるBTオブジェクト。
  ここからさらにSubStateMachineReferenceでExternalFSMも実行する。



## 16(RandomTransition)

重みづけしたStateLinkによるランダム遷移の例。

### 内容

* ArborFSMオブジェクト
  RandomTransitionを使用してランダムに遷移する。
  (RandomTransition)



## 17(Coin Pusher)

ArborFSMを使用した簡単なゲーム（コインプッシャー）の作成例。

### 内容

* Stageオブジェクト
  ArborFSMを使用してゲームの進行管理を行う。
  Startボタンを押したら開始し、手持ちスコアを判定して結果画面を表示する。

* ParameterContainerオブジェクト
  スコアパラメータを所持。

* Pusherオブジェクト
　前後の移動処理を行う。
  (TweenRigidbodyPosition)

* Spawnerオブジェクト
  スペースキーが入力された場合、手持ちスコアがあるならコインをインスタンス化する。

* Saucerオブジェクト
  コインが落下してきたらスコアを加算する。



## 18(Roll a Ball)

玉転がしゲームの作成例。

### 内容

* Playerオブジェクト
  プレイヤーとなる玉オブジェクト。

* PickUpオブジェクト
  拾うアイテム。全て拾うと勝ち。

* Obstacleオブジェクト
  障害物。当たると負け。

## 19(OffMeshLink)

AgentControllerのOffMeshLink通過設定の使用例。

### 内容

* Levelオブジェクト
  OffMeshLinkが設定された舞台。

* ClickMoveAgentオブジェクト
  クリックした位置に移動するエージェント。

* WaypointAgentオブジェクト
  Waypointに沿って移動するエージェント。

## 20(ObjectPool)

ObjectPoolの使用例。

### 内容

* MenuFSMオブジェクト
  メニューを制御するオブジェクト。

* BulletSpawnFSMオブジェクト
  クリックした位置に弾を飛ばすオブジェクト。
  弾に当たったオブジェクトがプールに格納される。

* DontDestroyOnLoad内のObjectPoolオブジェクト
  オブジェクトプールの本体。
  プールに格納されたオブジェクトはこのオブジェクトの子になる。
  ライフタイムが設定されている場合は時間経過やシーン切替によってプール済みオブジェクトは完全に削除される。
  プールからオブジェクトを生成する場合、すでにプールに存在するなら使いまわす。

* Bulletプレハブ
  弾のプレハブ。
  TagがFinishに設定されており、当たったオブジェクトを消す(プールに格納させる)。

* Cube_SceneUnloadedFSMプレハブ
  LifeTimeFlagsをSceneUnloadedに設定したCube。
  プール格納時にシーンがアンロードされると完全に削除される。
  メニューの「FSM : SceneUnloaded」で実体化する。

* Cube_TimeElapsedFSMプレハブ
  LifeTimeFlagsをTimeElapsedに設定したCube。
  プール格納時に5秒経過すると完全に削除される。
  メニューの「FSM : TimeElapsed」で実体化する。

* Cylinder_SceneUnloadedBTプレハブ
  LifeTimeFlagsをSceneUnloadedに設定したCylinder。
  プール格納時にシーンがアンロードされると完全に削除される。
  メニューの「BT : SceneUnloaded」で実体化する。

* Cylinder_TimeElapsedBTプレハブ
  LifeTimeFlagsをTimeElapsedに設定したCylinder。
  プール格納時に5秒経過すると完全に削除される。
  メニューの「BT : TimeElapsed」で実体化する。
