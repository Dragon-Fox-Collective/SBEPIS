# Changelog

このパッケージへのすべての注目すべき変更はこのファイルに文書化されます。

フォーマットは[Keep a Changelog](http://keepachangelog.com/)に基づいています。

## [3.9.5] - 2023-04-10

### 改善

#### Arbor Editor

- Ctrl + マウスホイール(MacではCommand + マウスホイール)で「マウスホイールの挙動」設定の逆の動作をするように対応。
- WindowsでもShift + マウスホイールでスクロールすると横スクロールするように対応。
- グラフのスクロール位置とズーム値をUnityが閉じられるまで保持し続けるように対応。
- ノードコメントの表示幅をコメントのテキストに合わせて自動調整するように変更。
- グラフ表示中にプレイした場合、Profilerの計測にエディタ側の処理がなるべく影響しないように改善。

#### Welcomeウィンドウ

- 自動的に開く設定を変更([ウェルカムウィンドウ](https://arbor-docs.caitsithware.com/ja/manual/window/welcomewindow.html))

#### スクリプト

- Calculator: パラメータ変更イベントをOnCalculate呼び出し時に実際に使用されたパラメータのみ購読するように改善

### 修正

#### Arbor Editor

- サブグラフとして参照している外部グラフが切り替わったときにグラフビューに表示されるグラフが切り替わらない不具合を修正。
- ズームを変更している状態でドラッグ中に自動スクロールするとドラッグ先がズレる不具合を修正。



## [3.9.4] - 2023-03-03

### 改善

#### Arbor Editor

- ソースコードの改善

#### スクリプト

- Calculator: パラメータ変更イベントをOnCalculate呼び出し後に購読するように改善

### 修正

#### Arbor Editor

- サブグラフとして参照している外部グラフが削除された際にグラフタブのツリーが更新されない不具合を修正。
- グラフをはじめて表示した際にスクロール位置が中央になるように修正。
- 実行中にArborEditorウィンドウを閉じてもウィンドウ更新用の処理が呼ばれてしまう不具合を修正。
- ノード移動中にグループノードの自動整列機能により位置調整が行われた場合にマウスカーソルの位置と移動中のノードの位置がズレる不具合を修正。

#### 挙動選択ウィンドウ

- [Unity2022.2以降] ハイライトされているアイテムをクリックしたときに選択されない不具合を修正。

#### 組み込みStateBehaviour

- [SubStateMachineReference](https://arbor-docs.caitsithware.com/ja/inspector/behaviours/StateMachine/substatemachinereference.html)のExternal FSMの参照タイプをHierarchyにすると例外が発生する不具合を修正。
- [SubBehaviourTreeReference](https://arbor-docs.caitsithware.com/ja/inspector/behaviours/BehaviourTree/subbehaviourtreereference.html)のExternal BTの参照タイプをHierarchyにすると例外が発生する不具合を修正。

#### 組み込みActionBehaviour

- [SubStateMachineReference](https://arbor-docs.caitsithware.com/ja/inspector/behaviourtree/actions/StateMachine/substatemachinereference.html)のExternal FSMの参照タイプをHierarchyにすると例外が発生する不具合を修正。
- [SubBehaviourTreeReference](https://arbor-docs.caitsithware.com/ja/inspector/behaviourtree/actions/BehaviourTree/subbehaviourtreereference.html)のExternal BTの参照タイプをHierarchyにすると例外が発生する不具合を修正。

#### Unity対応

- Unity2023.1.0b5でのコンパイル警告を修正。



## [3.9.3] - 2023-01-24

### 修正

#### Arbor Editor

- 非アクティブなシーンのグラフをArbor Editorで開いたままプレイ終了するとFailed to unpersistエラーが発生する不具合を修正。
- ノードのブレークポイントで停止した際にArbor Editorが開かれなくても不要に発生していたGC Allocを修正。

#### スクリプト

- NodeGraph.ownerBehaviourObjectのsetアクセサを呼び出した際に不要に発生していたGC Allocを修正。



## [3.9.2] - 2022-12-12

### 修正

#### Arbor Editor

- サイドパネルのノードリストタブにノードのアイコンが変更が反映されない不具合を修正。

### その他

#### Examples

- Unityビルトインマテリアルを使用しないように変更。
  (URPやHDRPのプロジェクトでは、Examplesフォルダ以下のマテリアルを変換すれば正常に動作するようになります)



## [3.9.1] - 2022-09-26

### 改善

#### Arbor Editor

- 挙動のタイトルバーの横幅が狭い時にタイトル名が途切れた場合、省略記号を表示するように改善。

#### ArborFSM

- OnGraphStopTransitionの遷移先でさらに即時遷移できるように改善。

#### データフロー

- Ray, Ray2D, RaycastHit, RaycastHit2Dを受け渡した時のGC Alloc削減。

### 修正

#### ArborFSM

- OnGraphStopTransitionの遷移先で先頭のStateBehaviourしか実行されない不具合を修正。
- ステートマシンを停止した時に遷移された接続線が強調表示したままだった不具合を修正。

#### Behaviour Tree

- 言語設定を切り替えてもノード接続線のメニューの「切断」テキストに反映されない不具合を修正。

#### Parameter Container

- AddVariableName属性で設定した同じグループ名が一つにまとまらない不具合を修正。

#### 組み込みCalculator

- Ray.Decomposeのメニュー名を修正。

#### スクリプト

- IValueSetterの名前空間をArbor.ValueFlowに修正。



## [3.9.0] - 2022-07-29

### 追加

#### Arbor Editor

- Arbor EditorウィンドウをUIElementsに対応。  
  (ノード用スクリプトのカスタムエディタにおいて、CreateInspectorGUI()メソッドを実装するとUIElements対応エディタを実装できます)
- ノードのブレークポイントで停止した際に、対象のノードをArbor Editorで開く機能を追加。  
  (ArborEditorウィンドウの設定メニューを開き、「ブレークポイント時にノードを開く」をオンにすることで有効化)
- 挙動のタイトルバーの設定メニューに「スクリプトを強調表示」を追加。
- 挙動のタイトルバーの設定メニューに「Editorスクリプトを強調表示」を追加。

#### Hierarchy

- HierarchyのGameObjectにArbor関連コンポーネントアイコンを表示する機能を追加。  
  (ArborEditorウィンドウの設定メニューを開き、「Hierarchyにアイコン表示」をオンにすることで有効化)

#### ObjectPool

- プールに格納されたオブジェクトのライフタイム設定を追加。

#### Behaviour Tree

- 複数のDecoratorの判定結果を論理演算する機能を追加。

#### 組み込みComponent

- [AgentController](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/agentcontroller.html)に挙動を追加  
  詳細は[エージェントの挙動制御](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/agentcontroller/behaviours.html)を参照してください。
  - 障害物に隠れる位置に移動するHide
  - 対象の移動速度を考慮して予測した位置に移動するPursuit
  - ２つの対象の移動速度を考慮して間に割り込む位置に移動するInterpose
  - 現在の進行方向に対してランダムに方向転換する位置に移動するWander
  - 対象の移動速度を考慮して予測した位置から離れるように移動するEvade
- AgentControllerにOffMeshLink通過時の挙動制御追加。  
  詳細は[AgentController#通過設定](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/agentcontroller.html#TraverseData)および[セットアップ例](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/agentcontroller/example.html)を参照してください。
- [AgentController.Stop](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor/Types/AgentController/M-Stop.html)にclearVelocity引数追加。
- [AgentController.MoveToRandomPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor/Types/AgentController/M-MoveToRandomPosition.html)にcheckRaycast引数追加。
- 移動速度を考慮した移動先予測を行うためのコンポーネント[MovingEntity](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/movingentity.html)追加
  - [MovingEntityCharacterController](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/movingentity/movingentitycharactercontroller.html)追加
  - [MovingEntityNavMeshAgent](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/movingentity/movingentitynavmeshagent.html)追加
  - [MovingEntityRigidbody](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/movingentity/movingentityrigidbody.html)追加
  - [MovingEntityTransform](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/movingentity/movingentitytransform.html)追加
- [OffMeshLinkSettings](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/offmeshlinksettings.html)追加
- [AnimationTriggerEventReceiver](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/builtin/animationtriggereventreceiver.html)追加

#### 組み込みStateBehaviour

- [SetRendererColor](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderercolor.html)追加
- [SetRendererFloat](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrendererfloat.html)追加
- [SetRendererMaterial](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderermaterial.html)追加
- [SetRendererTexture](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderertexture.html)追加
- [SetRendererTextureOffset](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderertextureoffset.html)追加
- [SetRendererTextureScale](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderertexturescale.html)追加
- [SetRendererVector](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Renderer/setrenderervector.html)追加
- [AnimatorPlay](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Animator/animatorplay.html)追加
- [AddComponent](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Component/addcomponent.html)追加
- [DestroyComponent](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Component/destroycomponent.html)追加
- [RestartScene](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Scene/restartscene.html)追加
- [PauseNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/NodeGraph/pausenodegraph.html)追加
- [PlayNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/NodeGraph/playnodegraph.html)追加
- [ResumeNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/NodeGraph/resumenodegraph.html)追加
- [StopNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/NodeGraph/stopnodegraph.html)追加
- [AgentEscapeFromPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentescapefromposition.html)追加
- [AgentEvade](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentevade.html)追加
- [AgentHideFromPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agenthidefromposition.html)追加
- [AgentHideFromTransform](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agenthidefromtransform.html)追加
- [AgentInterpose](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentinterpose.html)追加
- [AgentPursuit](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentpursuit.html)追加
- [AgentWander](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentwander.html)追加
- [InstantiateGameObject](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/GameObject/instantiategameobject.html)にプールのライフタイム設定(Life Time FlagsフィールドとLife Durationフィールド)追加。
- MouseButton**TransitionにIgnore UIフィールド追加。  
  関連するスクリプトは以下の通り:
  - [MouseButtonDownTransition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Transition/Input/mousebuttondowntransition.html)
  - [MouseButtonTransition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Transition/Input/mousebuttontransition.html)
  - [MouseButtonUpTransition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Transition/Input/mousebuttonuptransition.html)
- [AgentStop](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentstop.html)にClear Velocityフィールド追加。
- Agent系スクリプトにClear Velocity On Stopフィールド追加。
- Agent系スクリプトにCantMoveのStateLink追加。
- [AgentMoveToRandomPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentmovetorandomposition.html)にCheck Raycastフィールド追加。
- [AgentMoveOnWaypoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentmoveonwaypoint.html)にRadiusフィールド追加。
- Renderer関連のTweenスクリプトにMaterialIndexフィールドを追加  
  関連するスクリプトは以下の通り:
  - [TweenColor](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweencolor.html)
  - [TweenColorSimple](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweencolorsimple.html)
  - [TweenMaterialFloat](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweenmaterialfloat.html)
  - [TweenMaterialVector2](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweenmaterialvector2.html)
  - [TweenTextureOffset](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweentextureoffset.html)
  - [TweenTextureScale](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Tween/tweentexturescale.html)

#### 組み込みActionBehaviour

- [Idle](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Classes/idle.html)追加
- [PauseNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/NodeGraph/pausenodegraph.html)追加
- [PlayNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/NodeGraph/playnodegraph.html)追加
- [ResumeNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/NodeGraph/resumenodegraph.html)追加
- [StopNodeGraph](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/NodeGraph/stopnodegraph.html)追加
- [AgentEvade](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentevade.html)追加
- [AgentHideFromPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agenthidefromposition.html)追加
- [AgentHideFromTransform](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agenthidefromtransform.html)追加
- [AgentInterpose](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentinterpose.html)追加
- [AgentPursuit](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentpursuit.html)追加
- [AgentWander](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentwander.html)追加
- [AgentStop](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentstop.html)にClear Velocityフィールド追加。
- AgentMove系スクリプトにClear Velocity On Stopフィールド追加。
- AgentLook系スクリプトにClear Velocity On Stopフィールド追加。
- [AgentMoveToRandomPosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentmovetorandomposition.html)にCheck Raycastフィールド追加。
- [AgentMoveOnWaypoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/actions/Agent/agentmoveonwaypoint.html)にRadiusフィールド追加。

#### 組み込みDecorator

- [DistanceCheck](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/decorators/Classes/distancecheck.html)追加

#### 組み込みCalculator

- [Enum.Equals](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Enum/enumequalscalculator.html)追加
- [Enum.NotEquals](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Enum/enumnotequalscalculator.html)追加
- [Enum.ToInt](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Enum/enumtointcalculator.html)追加
- [Enum.ToString](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Enum/enumtostringcalculator.html)追加
- [Enum.TryParse](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Enum/enumtryparsecalculator.html)追加
- [Int.ToEnum](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Int/inttoenumcalculator.html)追加
- [GetRendererColor](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderercolor.html)追加
- [GetRendererFloat](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrendererfloat.html)追加
- [GetRendererMaterial](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderermaterial.html)追加
- [GetRendererTexture](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderertexture.html)追加
- [GetRendererTextureOffset](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderertextureoffset.html)追加
- [GetRendererTextureScale](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderertexturescale.html)追加
- [GetRendererVector](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getrenderervector.html)追加
- [GetSprite](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Renderer/getsprite.html)追加
- [Camera.ScreenPointToRay](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/camerascreenpointtoraycalculator.html)追加
- [Camera.ScreenToViewportPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/camerascreentoviewportpointcalculator.html)追加
- [Camera.ScreenToWorldPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/camerascreentoworldpointcalculator.html)追加
- [Camera.ViewportPointToRay](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/cameraviewportpointtoraycalculator.html)追加
- [Camera.ViewportToScreenPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/cameraviewporttoscreenpointcalculator.html)追加
- [Camera.ViewportToWorldPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/cameraviewporttoworldpointcalculator.html)追加
- [Camera.WorldToScreenPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/cameraworldtoscreenpointcalculator.html)追加
- [Camera.WorldToViewportPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Camera/cameraworldtoviewportpointcalculator.html)追加
- [Input.GetMousePosition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Input/inputgetmousepositioncalculator.html)追加
- [Ray.Compose](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raycomposecalculator.html)追加
- [Ray.Decompose](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raydecomposecalculator.html)追加
- [Ray.GetDirection](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raygetdirectioncalculator.html)追加
- [Ray.GetOrigin](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raygetorigincalculator.html)追加
- [Ray.GetPoint](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raygetpointcalculator.html)追加
- [Ray.SetDirection](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raysetdirectioncalculator.html)追加
- [Ray.SetOrigin](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raysetorigincalculator.html)追加
- [Ray.ToString](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/calculators/Ray/raytostringcalculator.html)追加

#### 組み込みスクリプト

- CalculatorConditionに複数の要素の判定結果を論理演算する機能を追加。  
  関連するスクリプトは以下の通り:
  - [(StateBehaviour)CalculatorTransition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Transition/calculatortransition.html)
  - [(Decorator)CalculatorCheck](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/decorators/Classes/calculatorcheck.html)
  - [(Decorator)CalculatorConditionalLoop](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/decorators/Classes/calculatorconditionalloop.html)
- ParameterConditionに複数の要素の判定結果を論理演算する機能を追加。  
  関連するスクリプトは以下の通り:
  - [(StateBehaviour)ParameterTransition](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Transition/parametertransition.html)
  - [(Decorator)ParameterCheck](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/decorators/Classes/parametercheck.html)
  - [(Decorator)ParameterConditionalLoop](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviourtree/decorators/Classes/parameterconditionalloop.html)

#### スクリプト

- async/awaitに対応。  
  詳細は[async/await](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/scripting/async_await.html)を参照してください。
  - [PlayableBehaviour.Yield](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.Playables/Types/PlayableBehaviour/M-Yield.html)メソッドで次のOnUpdate呼び出しまで待機。
  - [ActionBehaviour.WaitForExecute](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.BehaviourTree/Types/ActionBehaviour/M-WaitForExecute.html)メソッドで次のOnExecute呼び出しまで待機。  
    (WaitForExecuteメソッドで待機した場合は実質OnExecute内部での呼び出しと同等であるため、FinishExecuteメソッドが使用可能になります)
  - [PlayableBehaviour.CancellationTokenOnEnd](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.Playables/Types/PlayableBehaviour/P-CancellationTokenOnEnd.html)プロパティ追加。
  - UniTask対応。  
    UniTaskを導入している場合、Yield().ToUniTask()でUniTask化できます。  
    詳細は[UniTaskの利用](https://arbor-docs.caitsithware.com/ja/3.9.0/manual/extra/unitask.html)を参照してください。
- [AnimatorParameterReference.nameHash](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor/Types/AnimatorParameterReference/P-nameHash.html)プロパティ追加。
- [Decotator.OnRevaluationEnter](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.BehaviourTree/Types/Decorator/M-OnRevaluationEnter.html)メソッド追加。
- [Decorator.OnRevaluationExit](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.BehaviourTree/Types/Decorator/M-OnRevaluationExit.html)メソッド追加。

#### Examples

- AgentControllerのOffMeshLink通過設定の例として、19(OffMeshLink)追加。
- ObjectPoolの例として、20(ObjectPool)追加。

### 変更

#### Arbor Editor

- 全ノードタイプのノード名変更に対応。
- 演算ノードに挙動のタイトルバーを表示するように変更。

#### Behaviour Tree

- Decoratorのカスタムエディタを実装していても、AbortFlagsフィールドや論理演算フィールドを自動表示するように変更。
- ActionBehaviour.OnExecuteのデフォルト挙動を「失敗を返す」から「なにもしない」に変更。

#### ObjectPool

- グラフがプールされたときに停止するように変更。
- グラフがプールから復帰したときにplayOnStartがtrueなら再生開始するように変更。
- RigidbodyがプールされたときにSleepするように変更。
- Rigidbodyがプールから復帰したときにWakeUpするように変更。
- Rigidbody2DがプールされたときにSleepするように変更。
- Rigidbody2Dがプールから復帰したときにWakeUpするように変更。

#### 組み込みComponent

- AgentControllerのAddComponentメニューを「Arbor > Navigation > AgentController」に変更。
- WaypointのAddComponentメニューを「Arbor > Navigation > Waypoint」に変更。
- AgentControllerをMovingEntityクラスから派生するように変更。
- AgentController.Followを[MoveTo](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor/Types/AgentController/M-MoveTo.html)に改名

#### 組み込みStateBehaviour

- AgentEscapeを[AgentEscapeFromTransform](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Agent/agentescapefromtransform.html)に改名
- LoadLevelを[LoadScene](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Scene/loadscene.html)に改名
- UnloadLevelを[UnloadScene](https://arbor-docs.caitsithware.com/ja/3.9.0/inspector/behaviours/Scene/unloadscene.html)に改名

#### 組み込みDecorator

- TimeLimitにAbortFlags.LowerPriorityを設定しても割り込まないように変更。

#### Parameter Container

- パラメータの追加メニューをAddComponentメニュー形式に変更。

#### Unity対応

- Unity最低動作バージョンをUnity 2019.4.0f1に引き上げ。
- Unity2022.2.0b2対応。

#### Examples

- 名前空間を「Arbor.Examples」に統一。
- AddComponentメニューのグループを「Arbor > Examples」に統一。
- 挙動追加メニューのグループを「Examples」に統一。

### 改善

#### データフロー

- enum型のボックス化を改善
  (enum型のボックス化を最小限に抑えるには、[ValueMediator.RegisterEnum<T>()](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor.ValueFlow/Types/ValueMediator/MS-RegisterEnum.html)で対象の型を登録してください)

#### 組み込みComponent

- [AgentController.Escape](https://arbor-docs.caitsithware.com/ja/3.9.0/scriptreference/Arbor/Types/AgentController/M-Escape.html)で壁際に追い詰められた場合に極力嵌らないよう挙動を調整。

#### 組み込みスクリプト

- SetParameterのボックス化を改善
- Unity UIパッケージを削除した場合に、関連するスクリプトを無効にするように対応。
- Active Input Handlingを「Input System Package (New)」にした場合に、関連するスクリプトを無効にするように対応。



## [3.8.11] - 2022-06-27

### 修正

#### Arbor Editor

- グラフ未選択中にグラフ設定ウィンドウを開くと、グラフを選択したときにスクロール位置が不正な値になる不具合を修正。

#### Behaviour Tree

- Loopデコレータを使用した場合、子ノードが再実行される際にデコレータが再評価されずに実行されてしまう不具合を修正。



## [3.8.10] - 2022-03-14

### 修正

#### 組み込みスクリプト

- AnimatorにAnimatorOverrideControllerを指定すると関連スクリプトで例外が発生する不具合を修正。
  - AgentController
  - AnimatorCrossFade
  - AnimatorSetLayerWeight
  - AnimatorStateTransition
  - CalcAnimatorParameter



## [3.8.9] - 2021-12-27

### 追加

#### Parameter Container

- グラフに紐づいたParameterContainerの削除を追加。

### 改善

#### 型選択ウィンドウ

- フィルタを設定した場合に選択可能な要素がないグループは表示しないように改善。

#### メンバー選択ウィンドウ

- フィルタを設定した場合に選択可能な要素がないグループは表示しないように改善。

#### 組み込みスクリプト

- 型制約の設定をランタイム時に行うように改善。

### 修正

#### Arbor Editor

- [Unity2022.1.0b2] Arbor Editorに何も表示されない不具合を修正。

#### Parameter Container

- VariableListタイプのパラメーターを削除したときに内部オブジェクトが削除されない不具合を修正。

#### 組み込みスクリプト

- [データスロットへ接続可能な型が変更できる組み込みスクリプト] データスロットが切断される操作をUndoしても元に戻らない不具合を修正。



## [3.8.8] - 2021-12-02

### 改善

#### Arbor Editor

- ノードの矩形選択中にShiftキーで追加選択モード、Ctrlキーで除外モードに切り替わるように改善。

### 修正

#### Arbor Editor

- ロゴを常に表示している際、ロゴがグラフのズームの影響を受けてサイズが変わってしまう不具合を修正。
- 参照しているグラフが破棄された際にグラフ未選択に切り替わらない不具合を修正。
- ノードコメント表示中のノードを削除し、すぐに別のノードを作成するとノードコメントが表示された状態になる不具合を修正。
- ノードコメントの表示切替後にUndo/Redoしても表示が切り替わらない不具合を修正。
- ノード名変更中にウィンドウからフォーカスが外れた際に名前変更モードを終了するように修正。

#### 組み込みスクリプト

- Tween系でOnceを指定している際、時間経過後に遷移しないとTween開始時の状態に戻ってしまう不具合を修正。



## [3.8.7] - 2021-11-08

### 修正

#### Arbor Editor

- [Unity2021.2以降]グラフのズームを行うとグラフの表示範囲がずれる不具合を修正。
- [Unity2021.2以降]遷移先をノードリストから選択した後にグラフ内をドラッグすると「Should not be capturing when there is a hotcontrol」という警告が表示され左ボタンを離してもドラッグ状態が解除されなくなる不具合を修正。
- [Unity2021.2以降]コンパイル中にグラフ内をクリックするとコンパイル終了後にNullReferenceExceptionが発生する不具合を修正。
- グラフのズームを行うとドラッグ自動スクロールの領域も変更されてしまう不具合を修正。
- グラフのドラッグ自動スクロール領域の一部に反応しない箇所があった不具合を修正。

#### 組み込みスクリプト

- SubStateMachineの引数にList型を指定して実行するとParameterTypeMismatchExceptionが発生する不具合を修正。



## [3.8.6] - 2021-10-01

### 修正

#### Arbor Editor

- ParameterContainerからパラメータをドラッグ中にグラフエリア以外で離すとドラッグスクロールモードのままになる不具合を修正。
- ParameterContainerからパラメータをドラッグ中にグラフエリア以外で離すとStateの挙動挿入ボタンが表示されなくなる不具合を修正。

#### Parameter Container

- [Unity2021.1以降]ParameterContainerをスクロールするとパラメータが表示されなくなる不具合を修正。

#### 組み込みスクリプト

- [Unity2021.1以降]ParameterTransitionで参照するパラメータを変更したときに例外が発生する不具合を修正。



## [3.8.5] - 2021-09-02

### 修正

#### データフロー

- 実行時に表示されるデータ値が型に関係なくColor型になっていた不具合を修正。

#### Parameter Container

- SubStateMachineやSubBehaviourTreeを削除した際に内部パラメータのVariableデータが残ってしまう不具合を修正。
- ComponentListパラメータにComponent以外の型を指定すると要素追加時に例外が発生する不具合を修正。
- [Unity2020.2以降]パラメータを検索すると例外が発生する不具合を修正。



## [3.8.4] - 2021-08-13

### 変更

#### スクリプト

- InputSlotBaseクラスの「bool GetValue<T>(ref T)」を「bool TryGetValue<T>(out T)」に変更。

### 改善

#### 組み込みスクリプト

- ListAddElementなどList関連スクリプトのボックス化を削減。
- SubStateMachineReferenceやSubBehaviourTreeReferenceの"Use Directory In Scene"で同じグラフを使いまわせるように改善。

#### スクリプト

- 非sealedクラスのUnityコールバックメソッドにvirtualを設定するように改善。

### 修正

#### ArborFSM

- 自FSMへSendTriggerし、常駐ステートのTriggerTransitionからTrantisionTiming.Immediateで遷移しようとすると無限ループになる不具合を修正。

#### スクリプト

- Scripting Define SymbolsへARBOR_DISABLE_DEFAULT_EDITORを設定するとコンパイルエラーが発生する不具合を修正。



## [3.8.3] - 2021-07-16

### 修正

#### Arbor Editor

- ノードコメントを表示していると例外が発生する不具合を修正。



## [3.8.2] - 2021-07-01

### 修正

#### 型選択ウィンドウ

- ParameterContainerのComponentパラメータの型選択などをクリックするとTypeLoadExceptionが発生するのを修正。

#### Welcomeウィンドウ

- UnityEditorをbatchmodeで起動した場合にWelcomeウィンドウが表示されてしまう不具合を修正。

#### Unity対応

- [Unity2021.2.0b1] 例外が発生してArborの各種ウィンドウが正常動作しなくなっていた不具合を修正。



## [3.8.1] - 2021-06-18

### 追加

#### 組み込みスクリプト

- TransformMoveOnWaypointで使用するWaypointTimeTypeにFixedUnscaledTime追加。

#### スクリプト

- 経過時間を計測するTimerクラス追加。
- TimeTypeにFixedTimeとFixedUnscaledTimeを追加。

### 変更

#### BehaviourTree

- 再評価対象のデコレータのOnGraphPause/OnGraphResumeが呼び出されるように変更。

#### スクリプト

- NodeBehaviourのOnDrawGizmos/OnDrawGizmosSelectedが呼び出されるように変更。

### 改善

#### グラフ

- グラフが一時停止中の時間経過を無効にするように改善
  - UpdateSettingsのSpecifySeconds
  - TimeTransition, Tween系, Agent系, MoveOnWaypoint系
  - Waitアクション, Agent系アクション, TimeLimitデコレータ, Cooldownデコレータ

### 修正

#### ドキュメント

- 一部プロパティがドキュメント化されていなかったのを修正。



## [3.8.0] - 2021-05-20

### 追加

#### Arbor Editor

- サイドパネルにミニマップ追加。

#### ArborFSM

- TransitionTimingに以下のタイプを追加。
  - NextUpdateOverwrite
  - NextUpdateDontOverwrite(初期値をこれに変更)

#### ParameterContainer

- パラメータに以下のタイプを追加。
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

#### データフロー

- 演算ノードに再演算モードの設定を追加。

#### 組み込み演算ノード

- 以下の型の各メンバーを使用する演算ノードを追加
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

#### スクリプト

- StateBehaviourにOnStateFixedUpdateメソッド追加。
- TreeNodeBehaviourにOnFixedUpdateメソッド追加。

#### Welcomeウィンドウ

- ドキュメントのzipダウンロードボタン追加。

### 変更

#### Arbor Editor

- サイドパネルのグラフタブをグラフツリーに変更。
- サイドパネルのノードリストをタブに分離。
- グラフの作成ボタンから表示するメニューを選択ウィンドウに変更。
- 未接続のStateリルートノードとデータリルートノードを見分けられるように変更。

#### ArborFSM

- StateLinkを右クリックで設定ウィンドウを表示するように変更。

#### 組み込みスクリプト

- Update()、LateUpdate()、FixedUpdate()を使用している箇所をArbor用コールバックメソッドに置き換え。
- AgentController
  - PatrolメソッドをMoveToRandomPositionにリネーム。
  - IsDivAgentSpeedパラメータをSpeedType列挙型のパラメータに変更。
  - SpeedDivValueパラメータ追加。
  - isDivAgentSpeedプロパティを廃止し、speedTypeプロパティに変更。

#### 組み込みStateBehaviour

- SubStateMachineReference, SubBehaviourTreeReferenceでシーン内の他グラフをサブグラフ扱いできるUseDirectlyInSceneフラグ追加。
- AgentPatrolをAgentMoveToRandomPositionにリネーム。

#### 組み込みActionBehaviour

- SubStateMachineReference, SubBehaviourTreeReferenceでシーン内の他グラフをサブグラフ扱いできるUseDirectlyInSceneフラグ追加。
- AgentPatrolをAgentMoveToRnadomPositionにリネーム。

#### スクリプト

- AnimatorParameterReferenceのanimatorフィールドをFlexibleComponentで指定するように変更。

#### Examples

- エージェント用モデル追加。

#### Unity対応

- Unity最低動作バージョンをUnity 2018.4.0f1に引き上げ。

### 改善

#### データフロー

- データフローに値型を出力する場合のボックス化を削減。

#### 組み込み演算ノード

- List系演算ノードのボックス化を削減。

#### スクリプト

- 内部で使用しているforeachを削減
- 内部で使用しているラムダ式を削減
- GetComponent, GetComponents, GetComponentsInChildrenなどによるGC Allocを削減
- 内部で行っている文字列判定にStringComparison.Ordinalを指定するように改善。

### 修正

#### Arbor Editor

- コンパイル中に挙動追加すると例外が発生する不具合を修正。
- 種類の異なるグラフのノードをコピーペーストすると例外が発生する不具合を修正。
- Arbor Editorに選択している状態でグラフコンポーネントをRemove Componentし、それをUndoしてもグラフの選択状態が戻らない不具合を修正。
- ノード作成時に名前を指定すると、Undoしたときに名前だけ戻ってしまう不具合を修正。

#### ArborFSM

- 2つのFSMを同時に実行開始する場合、実行開始直後にSendTriggerしてもTriggerTransitionが反応しないのを修正。

### 廃止

#### スクリプト

- DataSlotFieldクラスをObsoleteに変更。

#### ドキュメント

- ドキュメントzipの同梱を廃止。



## [3.7.9] - 2021-03-24

### 修正

#### Arbor Editor

- [Unity2017.2 - 2018.4] グラフのキャプチャ後にグラフ内のスクロールが効かなくなる不具合を修正。
- ProjectSettingsのColorSpaceをLinearにしている場合にグラフキャプチャした画像の色味が変わってしまう不具合を修正。
- SubStateMachine付きステートを他ステートのSubStateMachineへドラッグ&ドロップすると例外が発生する不具合を修正。

#### ParameterContainer

- EnumList, ComponentList, AssetObjectListのパラメータを削除した場合にゴミデータが残ってしまう不具合を修正。

#### Unity対応

- [Unity2021.1.0f1以降] WaypointやWeightListなどでの要素の削除を対応



## [3.7.8] - 2020-12-21

### 修正

#### ArborFSM

- StateLinkをノード上部に表示していると例外が発生する不具合を修正。

#### 型選択ウィンドウ

- 一部のAssemblyの型が表示されない不具合を修正。
- 階層を左右の矢印キーで開閉した場合に切り替わらない不具合を修正。

#### Package Manager

- パッケージとしてインポートできない不具合を修正。



## [3.7.7] - 2020-12-12

### 修正

#### データフロー

- ClassTypeReferenceで参照している型が別のアセンブリへ移動した場合に参照切れしてしまうのを修正。
- RenamedFromAttributeをUWP(.NET)ビルドでも使用できるように修正。



## [3.7.6] - 2020-12-08

### 修正

#### BehaviourTree

- DecoratorやServiceの挿入メニューの「貼り付け」の判定を修正。



## [3.7.5] - 2020-10-27

### 修正

#### Arbor Editor

- ドラッグ中に右クリックした際にドラッグが不正に中断されてしまう不具合を修正。
- グループノードの色を変更するとヘッダ上のボタンの色まで変更されてしまう不具合を修正。

#### スクリプト

- ArborFSM.SendTriggerの追加した引数をデフォルト引数にせず、メソッドを分けるように修正。



## [3.7.4] - 2020-09-29

### 修正

#### データフロー

- 値型の入力スロットを使用しているとIL2CPPビルドでの実機プレイ中に例外が発生する不具合を修正。
- リストを使用しているとIL2CPPビルドでの実機プレイ中に例外が発生する不具合を修正。  
  ※IL2CPPなどの事前コンパイル(AOT)でビルドする場合は [事前コンパイル(AOT)での制限](https://caitsithware.com/assets/arbor/docs/ja/manual/dataflow/list.html#AOTRestrictions) を参照してください。

#### ArborEvent(InvokeMethod/GetValue)

- インターフェイスのメンバーを呼び出そうとすると例外が発生する不具合を修正。

#### 組み込みStateBehaviour

- List.AddElementでNewArrayを指定すると、出力するインスタンスが変更されていない不具合を修正。
- List.RemoveAtIndexでNewArrayを指定し末尾の要素を削除しようとすると例外が発生する不具合を修正。
- List.InsertElementでNewArrayを指定し末尾に要素を追加しようとすると例外が発生する不具合を修正。



## [3.7.3] - 2020-09-22

### 変更

#### ArborEvent(InvokeMethod/GetValue)

- メソッド選択ウィンドウで引数により除外しないように変更。

### 修正

#### Arbor Editor

- partialで分割かつクラスと同名ファイルが複数ある場合に例外が発生していた不具合を修正。
- [Unity2019以降] GameObjectをArbor Editorウィンドウ内のグラフビュー以外にドラッグ＆ドロップすると、グラフの自動スクロールモードになったままになってしまう不具合を修正。

#### Unity対応

- [Unity2020.2.0b2] Obsolete警告を修正。

#### ArborEvent(InvokeMethod/GetValue)

- Unityオブジェクト型以外のクラスのメンバーが呼び出せない不具合を修正。
- 指定しているクラス型が後からstructに変更された場合にインスタンスの出力スロットの型が正常に指定されない不具合を修正。

#### その他

- 複数のUnityバージョンでArborを使用しているとArborSettings.assetが読み込めないエラーメッセージが表示される不具合を修正。



## [3.7.2] - 2020-09-11

### 追加

#### Arbor Editor

- 挙動にObsolete属性がついている場合、タイトルバー下にメッセージを表示するように対応。

#### スクリプト

- MaterialPropertyBlockを割り当てているRenderer別に管理できるRendererPropertyBlockクラスを追加。

### 修正

#### Arbor Editor

- メニューからポップアップウィンドウを開く際にポップアップウィンドウが離れた位置に表示されてしまう不具合を修正。
- FlexibleField<T>にシリアライズ不可の型を指定した場合に、Editorのレイアウトが崩れていた不具合を修正。
- [Unity2020.1以降]データスロットが含まれるジェネリッククラスをフィールドに定義すると「GetDataSlotField: DataSlotField was not found.」という警告が表示されスロットが表示されない不具合を修正。

#### 組み込みStateBehaviour

- ExistsGameObjectTransitionのInputTargetsフィールドを使用しないとNullReferenceExceptionが発生する不具合を修正。
- 古いバージョンのArborでParameterTransitionを使用したグラフを新しいバージョンで開くと「GetDataSlotField: DataSlotField was not found.」という警告が表示される不具合を修正。
- 同じRendererのマテリアルのプロパティを変更するStateBehaviourを２つ以上使用すると、後から呼び出したスクリプトの際にマテリアルの初期値が反映されない不具合を修正。
  - TweenColor
  - TweenColorSimple
  - TweenMaterialFloat
  - TweenMaterialVector
  - TweenTextureOffset
  - TweenTextureScale
- 一部プラットフォームでのビルド中に「Game scripts or other custom code contains OnMouse_ event handlers.」と表示されている問題を修正。
  以下StateBehaviourを該当プラットフォーム(Android, iOS, Windows Store Apps, Apple TV)でも使用したい場合は、Scripting Define SymbolsへARBOR_ENABLE_ONMOUSE_EVENTを追加してください。
  - OnMouseDownTransition
  - OnMouseDragTransition
  - OnMouseEnterTransition
  - OnMouseExitTransition
  - OnMouseOverTransition
  - OnMouseUpAsButtonTransition
  - OnMouseUpTransition

#### スクリプト

- [Unity2020.1対応]OutputSlot<T>とInputSlot<T>のabstract指定を解除しフィールドに直接使用できるように修正。



## [3.7.1]　- 2020-09-04

### 追加

#### ウェルカムウィンドウ

- レビューページへのリンク追加。

#### 組み込みStateBehaviour

- AddForceRigidbody, AddVelocityRigidbody, SetVelocityRigidbody, AddForceRigidbody2D, AddVelocityRigidbody2D, SetVelocityRigidbody2D
  - 実行メソッドを指定する「ExecuteMethodFlags」フィールド追加。
  - 方向タイプを指定する「DirectionType」フィールド追加。
- SetParameter
  - パラメータの設定タイミングを指定する「ExecuteMethodFlags」フィールド追加。
- RigidbodyMoveOnWaypoint, Rigidbody2DMoveOnWaypoint, TransformMoveOnWaypoint
  - Waypoint上からの位置調整用の「Offset」フィールド追加。
- DistantTransform
  - 比較元Transformを指定する「Transform」フィールド追加。
- ExistsGameObjectTransition
  - GameObjectの配列を受け取る「InputTargets」フィールド追加。
  - アクティブかどうかも判定する「CheckActive」フィールド追加。
  - 1つでもオブジェクトが存在していたら遷移する「SomeExistsState」フィールド追加。

#### 組み込みCalculator

- Input/Input.GetAxis, Input/Input.GetAxisRaw追加。
- String/ToString追加。
- Scene/GetActiveSceneName追加。

#### スクリプト

- FlexibleStringのタイプがConstantの時に、タグ選択ポップアップを表示するTagSelectorAttribute追加。
- Unity関連のFlexibleField追加。
  - FlexibleForceMode
  - FlexibleForceMode2D
  - FlexibleKeyCode
  - FlexibleSpace
  - FlexibleLoadSceneMode
  - FlexibleLayerMask
  - FlexibleInputButton
- Arborコア関連のFlexibleField追加。
  - FlexibleExecuteMethodFlags
  - FlexibleTimeType
  - FlexibleSendTriggerFlags
  - FlexibleTransitionTiming
- 組み込みスクリプト関連のFlexibleField追加。
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

- Example 18(Roll a Ball)追加。

### 変更

#### 組み込みスクリプト全般

- FlexibleFieldを使用していないフィールドをなるべくFlexibleFieldを使用するように変更。

#### 組み込みStateBehaviour

- UISetSliderFromParameterでSliderをキャッシュしないように変更(他のUISet...FromParameterと同じ処理に)。

### 修正

#### Arbor Editor

- データスロットが接続されているノードをコピー＆ペーストするとコピー元の接続情報が残ったままになってしまう不具合を修正。

#### 組み込みスクリプト全般

- タイプ切り替えにより非表示になるスロットの切断処理を修正。

#### 組み込みStateBehaviour

- SetParameter
  - 値を設定する際に、正確な型へキャストするように修正。
  - グラフ内パラメータを参照しているSetParameterを他グラフへコピー＆ペーストすると、コピー先グラフ内のパラメータを参照している見た目になってしまう不具合を修正。

#### 組み込みActionBehaviour

- SetParameter
  - 値を設定する際に、正確な型へキャストするように修正。
  - グラフ内パラメータを参照しているSetParameterを他グラフへコピー＆ペーストすると、コピー先グラフ内のパラメータを参照している見た目になってしまう不具合を修正。

#### 組み込みCalculator

- Random.SelectComponentを持つグラフを表示した状態で別のシーンを開くと例外が発生する不具合を修正。
- GetParameter
  - グラフ内パラメータを参照しているGetParameterを他グラフへコピー＆ペーストすると、コピー先グラフ内パラメータを参照している見た目になってしまう不具合を修正。

#### スクリプト

- Enum型のパラメータにParameter.valueでint型の値を受け渡すとInvalidCastExceptionが発生する不具合を修正。
- DynamicUtility.Castメソッドでinterfaceへキャストしようとすると例外が発生する不具合を修正。



## [3.7.0] - 2020-08-24

### 追加

#### Arbor Editor

- 挙動のヘルプボタンで表示するURL指定にHelpURL属性も使えるように対応。
- 挙動にObsolete属性がついている場合、タイトルバーにDeprecatedの文言を追加するように対応。
- ClassTypeReferenceで参照した型が見つからなくなった場合に(Missing)の文言を表示するように対応。
- プレイ中にグラフの再生状態をグラフラベル下に表示するように対応。
- ロゴを常に表示するモード追加。
- NodeBehaviourの追加メニューに各型のヘルプボタンを追加。
- ウェルカムウィンドウを追加。
- [Unity2018.1以降]挙動のプリセット対応。
- [Unity2018.1以降]Arbor Editorの言語設定で、UnityEditor側の言語設定を参照するモードを追加。
- [Unity2019.3以降]Enter Play Mode SettingsのReload Domainオフに対応(Reload Sceneオフは未対応)。

#### ArborFSM

- StateLinkをグラフ上にドラッグ＆ドロップした際のメニューに、接続先をノードリストで選択する項目追加。
- ツールバーの「表示 > StateLink」にStateLinkの表示の設定項目追加。
- プレイ中にグラフが無効になった場合に、保留中の遷移と遷移先ステートを紫色で視覚化。
- プレイ中にグラフが無効になった場合に、アクティブにならなかったStateBehaviourは紫色で視覚化。

#### BehaviourTree

- 親ノードをドラッグした際に子ノードも移動するモード追加（Win: Alt+ドラッグ、Mac: option+ドラッグ)

#### ParameterContainer

- パラメータに以下のタイプを追加。
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

- ツールバーに「表示 > データスロット > フレキシブルに表示」追加。
    - スロットが接続中であればノード外に表示。
	- 接続中でない場合、ドラッグしているスロットが接続可能ならノード外に表示。
- FlexibleComponentやFlexibleGameObjectのHierarchyTypeにParentGraph追加。

#### ArborEvent(InvokeMethod)

- フィールドやプロパティに値を設定できるように追加。
- Component以外の型も指定できるように追加。
- staticクラスも指定できるように追加。
- 型選択ポップアップに検索フィルター追加。
- メンバー選択ポップアップに検索フィルター追加。

#### 組み込みStateBehaviour

- 終了処理を行うためにグラフ停止時に遷移するためのOnGraphStopTransition追加。
- 配列を変更する挙動を追加。
    - List.AddElement
	- List.Clear
	- List.InsertElement
	- List.RemoveAtIndex
	- List.RemoveElement
	- List.SetElement
- SendTrigger関連にSendTriggerFlags追加。
    - SendTrigger
	- SendTriggerUpwards
	- SendTriggerGameObject
	- BroadcastTrigger
- SetParameterに値を直接設定できるフィールド追加。
- InstantiateGameObjectに座標を直接指定するフィールドを追加。
- CalculatorTransitionに判定を行うタイミングを指定するフィールド追加。
- AnimatorCrossFadeにAnimatorが既に指定ステートに遷移処理中であれば遷移させないように設定するフィールド追加。
- AnimatorCrossFadeにAnimator側のステートが遷移完了した際に、Arbor側も遷移するStateLink追加。
- AnimatorCrossFadeにAnimator.CrossFadeInFixedTimeを使用するフラグを追加。
- MoveOnWaypoint関連スクリプトへ再開時に目標ポイントをクリアするフラグを追加。
    - AgentMoveOnWaypoint
	- RigidbodyMoveOnWaypoint
	- Rigidbody2DMoveOnWaypoint
	- TransfomMoveOnWaypoint

#### 組み込みActionBehaviour

- InvokeMethod追加。
- SetParameterに値を直接設定できるフィールド追加。
- InstantiateGameObjectに座標を直接指定できるタイプを追加。
- AnimatorCrossFadeにAnimatorが既に指定ステートに遷移処理中であれば遷移させないように設定するフィールド追加。
- AnimatorCrossFadeにAnimator側のステートが遷移完了まで待つフィールド追加。
- AnimatorCrossFadeにAnimator側のステートが遷移成功したかどうかをチェックするフィールド追加。
- AnimatorCrossFadeにAnimator.CrossFadeInFixedTimeを使用するフラグを追加。
- CalcParameter追加。
- AgentMoveOnWaypointへ再開時に目標ポイントをクリアするフラグを追加。

#### 組み込みCalculator

- メンバーフィールドやプロパティの値を取得しデータフローに出力する演算ノードGetValueCalculator追加。
- 配列に関連するCalculator追加
    - NewArrayList
	- List.Contains
	- List.Count
	- List.GetElement
	- List.IndexOf
	- List.LastIndexOf
	- List.ToArrayList

#### 組み込みVariableList

- GradientList追加
- AnmationCurtveList追加

#### スクリプト

- ClassTypeReference型フィールドに検索フィルタータイプを設定できるようにTypeFilterAttribute追加。
- ClassTypeReferenceやArborEventでの参照が外れないようにリネームしたことを設定するRenamedFromAttribute追加。
- NodeBehaviourのみを除外するClassNotNodeBehaviourAttribute追加
- AssetObjectのみに制約するClassAssetObjectAttribute追加。
- AssetObjectParameter追加。
- FlexibleAssetObject追加。
- 型選択ポップアップに型を表示しないようにするHideTypeAttrubute追加。
- BehaviorTitleにuseNicifyNameフィールド追加。falseにすると指定したtitleNameがそのまま挙動のタイトルとして表示されるようになる。
- ArborFSM.SendTriggerで常駐ステートに送らないようにするためにSendTriggerFlags追加。
- [Unity2019.3以降]SerializeReference属性に対応。
- Decorator.OnRepeatCheckにノードの結果を受け渡すバージョンのコールバックメソッドを追加。
- FlexibleSceneObjectにtargetGraph追加。

### 変更

#### Arbor Editor

- 型選択ポップアップの型がUnityオブジェクトの場合にスクリプトに設定しているアイコンを表示するように変更。
- 型選択ポップアップに表示されるSystem.Int32などの基本型を、intなどのエイリアスで表示するように変更。
- リルートノードが接続されていない時は、接続方向を表示するように変更。
- ロゴ表示をFadeOutにしている場合、グラフが切り替わるたびにロゴがフェードアウトするように変更。
- グラフキャプチャを行った際にロゴが前面に表示されるように変更。

#### ArborFSM

- StateLinkの表示スタイルを変更。
- 自分自身のグラフを無効にした場合に、再度有効にするまで遷移処理を保留するように変更。

#### BehaviourTree

- NodeLinkの表示スタイルを変更。
- 自分自身のグラフを無効にした場合に、再度有効になるまで遷移処理を保留するように変更。

#### ParameterContainer

- SubStateMachineなどのサブグラフで指定するパラメータ設定で、Parameter.Typeのポップアップを見やすく整理するように変更。

#### DataFlow

- データスロットの表示スタイルを変更。
- InputSlotTypableなどで表示される型の指定フィールドの表示位置を変更。
- NodeGraphを参照するFlexibleComponentでRootGraphやParentGraphを指定した場合に、直接グラフを参照するように変更。

#### 組み込みStateBehaviour

- InvokeMethodをEventsフォルダに移動。
- AnimatorCrossFadeのNormalized TimeをTime Offsetにリネーム。
- AnimatorCrossFadeのTransition DurationとTime OffsetをFlexibleFloat型に変更。

#### 組み込みActionBehaviour

- AnimatorCrossFadeのNormalized TimeをTime Offsetにリネーム。
- AnimatorCrossFadeのTransition DurationとTime OffsetをFlexibleFloat型に変更。

#### スクリプト

- IInputSlot
    - SetInputBranchをSetBranchにリネーム。
	- GetInputBranchをGetBranchにリネーム。
	- RemoveInputBranchをRemoveBranchにリネーム。
	- IsConnectedInputをIsConnectedにリネーム。
- IOutputSLot
	- AddOutputBranchをAddBranchにリネーム。
	- GetOutputBranchをGetBranchにリネーム。
	- RemoveOutputBranchをRemoveBranchにリネーム。
	- IsConnectedOutputをIsConnectedにリネーム。
	- GetOutputBranchCountメソッドをbranchCountプロパティに変更。
- ShowEventAttributeとHideEventAttributeをフィールドとプロパティにも設定できるように変更。
- InputSlotAny、InputSlotTypableがボックス化された値を直接介していたのを値型のコピーを行うように変更。
- 各InputSlotのGetValueで、取得した値がnullだった場合にfalseを返していたのを、値が格納されているか(DataBranch.isUsedがtrue)で判定するように変更。
- EachFieldでinternfaceの列挙ができるように変更。
- OnStateBegin()以降、ArborFSMInternal.nextTransitionStateがnullを返すように変更。
- 組み込みCalculatorの名前空間をArbor.Calculatorsに変更。
- ArborEditor関連の内部クラスをinternalに変更。
- 継承が不要なクラスをsealedに変更。

#### その他

- 各Arbor用コンポーネントスクリプトのアイコン変更。

### 改善

#### Arbor Editor

- 型選択ポップアップを高速化。

#### 組み込みStateBehaviour

- GameObject.tagの比較にCompareTagを使うように改善。

#### スクリプト

- フィールドを列挙する際にキャッシュすることで高速化。

#### 負荷軽減

- データフローから値を取得する際の負荷軽減。

### 修正

#### Arbor Editor

- [Unity2017.3以降]グループノードのみをAlt+ドラッグで移動しようとしてもグラフのスクロールになってしまっていた不具合を修正。
- [Unity2017.3以降]型選択ウィンドウにArbor関連の型が列挙されていなかった不具合を修正。
- [Unity2018.1以降]以下コンポーネントでプリセットを使用しても正常に動作しないためプリセットから除外するように修正。
    - ArborFSM
	- BehaviourTree
	- ParameterContainer
	- SubStateMachine
	- SubBehaviourTree
- DataSlotが無効表示の際に、ノード枠外のスロット枠が無効にならなかった不具合を修正。
- EulerAnglesAttributeでの表示タイプを変更した際に他のフィールドの表示も変わっていた不具合を修正。
- LanguagePathを別フォルダに移動した場合にエディタ再起動するまで読込先が切り替わらなくなっていた不具合を修正。

#### ArborEvent

- 指定した型がUnity内の別モジュールに移動された場合に参照が切れてしまう不具合を修正。

#### 組み込みStateBehaviour

- MoveOnWaypointを行う際、初期位置とWaypointが一致していると移動開始されない不具合を修正。
- RigidbodyMoveOnWaypoint、Rigidbody2DMoveOnWaypointをFixedUpdateで移動処理するように修正。
- AgentLookAtPositionのAngular Speedフィールドが2重に表示されていた不具合を修正。

#### スクリプト

- Arborの各コールバックメソッドを呼び出し時に発生した例外をcatchするように修正。
- データスロットを自作した場合に追加したフィールドにHideInInspectorを指定してもフィールドが表示されていたのを修正。


## [3.6.13] - 2020-06-12

### 修正

#### Arbor Editor

- Unity2020.1
    - グラフの一部しかマウス操作できない不具合を修正。
	- キャプチャした際に一部以外が黒塗りになる不具合を修正。


## [3.6.12] - 2020-05-29

### 修正

#### Arbor Editor

- 接続中のデータスロットのあるノードを同じグラフ上に複製すると、コピー元の接続が解除される不具合を修正。
- グラフのコピー＆ペーストをUndoした後にRedoするとデータスロットの接続が解除されている不具合を修正。
- (暫定対処)NodeBehaviour.GetDataSlotField(int)を呼び出している箇所でNullReferenceExceptionが発生していたためnullチェックをするように対処。
  nullになった場合に"タイプ名 : GetDataSlotField(0) == null"というようなエラーログが出力されるようになります。


## [3.6.11] - 2020-05-17

### 改善

#### Arbor Editor

- 再生中に故意にグラフを切り替えた場合に、ライブ追跡を解除するように改善。
- AnimatorParameterReferenceでAnimatorを指定していない時のGUIを調整。

#### 負荷軽減

- NodeGraphのデシリアライズ処理の負荷軽減。

### 修正

#### Arbor Editor

- NodeGraphのInspectorからCopy ComponentとPaste NodeGraph As Newでコピー＆ペーストを行うと、データスロットの接続が壊れる不具合を修正。
- ParameterReferenceのフィールドを表示した際、インデントが崩れる不具合を修正。
- AnimatorParameterReferenceのフィールドを表示した際、インデントが崩れる不具合を修正。
- ライブ追跡中にステートが一切ないSubStateMachineに遷移すると、高速で表示グラフが切り替わってしまう不具合を修正。


## [3.6.10] - 2020-04-20

### 修正

#### Arbor Editor

- ディスプレイのスケーリングを等倍以外にしている場合に、グラフのスクリーンショットでグラフ全体がキャプチャされない不具合を修正。


## [3.6.9] - 2020-02-27

### 改善

#### 負荷軽減

- データフローから値を取得する際の負荷軽減。

### 修正

#### Arbor Editor

- 型やメソッドの選択ポップアップ表示中にスクリプトコンパイルするとNullReferenceExceptionが発生する不具合を修正。
- 実行中にデータリルートノードの「削除（接続を保持）」を行った場合に、取得する値が変わってしまう不具合を修正。
- Unity2019.3
    - AddBehaviourメニューのレイアウトを修正。
	- 型やメソッドの選択ポップアップのレイアウトを修正。

#### 組み込みStateBehaviour

- [Unity2018.4以降]InvokeMethodのエディタ拡張でエラーが発生することがある不具合を修正。


## [3.6.8] - 2020-01-10

### 修正

#### Arbor Editor

- グラフ内パラメータを持つグラフをコピー＆ペーストすると参照を共有してしまう不具合を修正。
    - **この修正をArbor 3.6.8以前に作成した該当グラフに適用するには一度シーンやプレハブを開き直し再保存が必要です。**
- グラフのコピー＆ペーストのUndo/Redoが正常に行われていなかった不具合を修正。
    - Inspectorウィンドウからグラフをコピー＆ペーストする場合は、歯車アイコンのメニューにある「Copy Component」「Paste NodeGraph Values」「Paste NodeGraph As New」を使用して下さい。
- サイドパネルの入力フィールドにキーボードフォーカスがある状態で、表示グラフを子グラフなどに切り替えるとフォーカスが残ったままになる不具合を修正。
- Unity2018.1以降: 型選択ポップアップウィンドウのリストにCanvasなどが表示されず選択できない不具合を修正。
- Unity2019.3
    - 挙動のタイトルバー上のアイコンボタンが表示されない不具合を修正。
    - レイアウトが崩れる不具合を修正。

#### スクリプト

- 子グラフ実行中の親グラフをポーズした後に停止させると子グラフの実行状態がリセットされない不具合を修正。


## [3.6.7] - 2019-11-08

### 修正

#### Arbor Editor

- 接続中データスロットのフィールドが正常に切断処理されずに削除された場合にNullReferenceExceptionが発生していた不具合を修正。

#### 組み込みStateBehaviour

- SubStateMachineやSubBehaviourTreeのグラフ引数を削除する際に接続中データスロットを正常に切断処理するように修正。


## [3.6.6] - 2019-11-06

### 修正

#### スクリプト

- FlexibleGameObject(GameObject gameObject)コンストラクタの引数に渡したGameObjectが反映されなかった不具合を修正。
  [関連不具合]
  - Arborの古いバージョンから3.6.xにアップデートした際に、GameObjectフィールドからFlexibleGameObjectフィールドへの移行が正常処理されない。

## [3.6.5] - 2019-10-24

### 修正

#### Arbor Editor

- Parameterを範囲外にドラッグ＆ドロップするとグラフの自動スクロールが動作したままになる不具合を修正。
- Parameterを範囲外にドラッグ＆ドロップするとParameterReferenceのDrop Parameterエリアが一瞬残る不具合を修正。


## [3.6.4] - 2019-10-08

### 追加

#### スクリプト

- SerializeVersionクラス追加。
- ISerializeVersionCallbackReceiverインターフェイス追加。

### 修正

#### Arbor Editor

- ノードのサイズが変わってもグラフ全体のサイズが更新されず、端までスクロールできない不具合を修正。
- StateLinkを右クリックして「遷移先へ移動」を選択した際、スクロールが途中で止まってしまう不具合を修正。

#### 組み込みStateBehaviour

- InvokeMethodで指定したメソッドのlong型引数をFlexibleLongで指定ができるように修正。

#### スクリプト

- Arborの古いバージョンから3.6.xにアップデートした際に、一部パラメータのアップグレード処理が正しく行われない不具合を修正。 
  [関連不具合]
    - グループノードの色がおかしい
    - ParameterTransitionのConditionListが一部おかしくなる
	- CalculatorTransitionのConditionListが一部おかしくなる
- FlexibleRectなどの値型を扱うFlexibleFieldでタイプをParameterにしつつ、参照先のParameterを指定しなかった場合にNullReferenceExceptionが発生していた不具合を修正。


## [3.6.3] - 2019-09-20

### 修正

#### Arbor Editor

- SubStateMachineやSubBehaviourTreeをコピー＆ペーストするとエディタがフリーズする不具合を修正。
- ノードのリネーム中にマウスクリックで文字列選択ができない不具合を修正。
- [Unity2019.3beta]古いUnityで作成されたDLLがあると例外が発生していた不具合を修正。


## [3.6.2] - 2019-08-09

### 修正

#### Arbor Editor

- プレハブのグラフを選択したまま同一ペインにある別のタブを表示し、UnityEditorを再起動するとプレイ開始時に例外が発生する不具合を修正。


## [3.6.1] - 2019-07-26

### 修正

#### Arbor Editor

- グループノードのコピー＆ペーストで、ColorとAuto Alignmentの設定がコピーされないのを修正。
- グループノードをドラッグ中にEscキーを押してキャンセルしても元の位置に戻らないのを修正。


## [3.6.0] - 2019-07-19

### 追加

#### Arbor Editor

- ツールバーの「表示」メニューに「データスロット > ノード内に表示」を追加。
- ParameterをParameterReferenceへドラッグ＆ドロップ追加。
- ParameterをFlexibleFieldへドラッグ＆ドロップ追加。
- GameObjectをFlexibleGameObjectへドラッグ&ドロップ追加。
- ComponentをFlexibleComponentなどへドラッグ&ドロップ追加。
- FlexibleGameObjectやFlexibleComponentなどのシーン内オブジェクトの参照タイプにHierarchyを追加。
    - Self : 自グラフを所有しているGameObjectを参照
	- RootGraph : グラフの階層化をしている場合に、ルートグラフを所有しているGameObjectを参照。

#### 組み込みStateBehaviour

- TransformMoveOnWaypoint追加。
- RigidbodyMoveOnWaypoint追加。
- Rigidbody2DMoveOnWaypoint追加。
- InvokeMethodにOnStateUpdateとOnStateLateUpdateによるメソッド呼び出しを追加。
- InvokeMethodで呼び出したいイベントのみ設定するように、イベントの追加ボタンと削除ボタンを追加。
- Agent関連スクリプトに移動先座標の更新タイプを追加。
    - Time : 時間指定で更新
	- Done : 完了したら更新
	- StartOnly : 開始時のみ更新
	- Always : 常に更新
- Agent関連スクリプトのTime更新の際に使用する時間タイプを設定できるように追加。

#### 組み込みActionBehaviour

- AnimatorCrossFade追加。
- AgentPatrolに移動先変更インターバルを設定する項目追加。

#### スクリプト

- OutputSlotComponent<T>クラス追加。

### 変更

#### Arbor Editor

- パラメータやシーンオブジェクトのドラッグ中にグラフ内を自動スクロールするように変更。

#### 組み込みStateBehaviour

- Agent関連スクリプトのInterval指定方法をFlexibleFloatに変更。

#### スクリプト

- FlexibleGameObjectやFlexibleComponentなどのシーン内オブジェクトを参照するクラスをFlexibleSceneObjectBaseから派生するように変更。
    - 参照タイプの型がFlexibleTypeからFlexibleSceneObjectTypeに変更。
- DataFlow関連スクリプトをDataFlowフォルダに移動。
- DataSlot関連クラスのソースファイル整理。

### 修正

#### Arbor Editor

- [Unity2018.3以降]グラフビューがクリッピングされずスクロールバーに表示が被ってしまうのを修正。
- [Unity2019.2beta]グラフビュー内にマウスイベントが反応しない箇所があったのを暫定修正。

#### 組み込みStateBehaviour

- TweenBlendShapeWeightにMeshが設定されていないSkinnedMeshRendererを指定した場合に例外が発生するのを修正。

#### その他

- Arbor.BuiltInBehaviours.asmdefの誤植を修正。


## [3.5.5] - 2019-05-29

### 修正

#### Arbor Editor

- コピーなどのショートカットコマンドを実行した後にイベントを使用済みにしていなかったのを修正。


## [3.5.4] - 2019-05-27

### 修正

#### 組み込みスクリプト

- AgentControllerのanimatorプロパティのsetterが正常に動作しない不具合を修正。


## [3.5.3] - 2019-05-20

### 変更

#### 組み込みスクリプト

- AnimationCurveをFlexibleAnimationCurveへキャストした時に、値のインスタンスをコピーするように変更。
  (Tween系StateBehaviourの不具合修正に伴う変更)
- GradientをFlexibleGradientへキャストした時に、値のインスタンスをコピーするように変更。
  (TweenColorなどの不具合修正に伴う変更)
- Unity2018.3以降 : CollisionやCollision2Dをデータ出力する際に「Reuse Collision Callbacks」が有効になっていると警告をログに出力するように変更。
    - OnCollisionEnterTransition
	- OnCollisionExitTransition
	- OnCollisionStayTransition
	- OnCollisionEnter2DTransition
	- OnCollisionExit2DTransition
	- OnCollisionStay2DTransition

### 修正

#### Arbor Editor

- ステートノードにスクリプトやパラメータを直接ドラッグ＆ドロップして追加した時に、ArgumentOutOfRangeExceptionが発生する不具合を修正。

#### 組み込みスクリプト

- TweenRotationなどのTween系StateBehaviourをステートに追加した直後にCurveフィールドが編集できない不具合を修正。
- TweenColorなどをステートに追加した直後にGradientフィールドが編集できない不具合を修正。


## [3.5.2] - 2019-05-03

### 修正

#### 組み込みスクリプト

- ParameterConditionListが参照するパラメータタイプが変更されると例外が発生するのを修正。


## [3.5.1] - 2019-04-29

### 追加

#### スクリプト

- Parameterクラスに各値へのアクセスメソッドを追加。
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
- Parameterクラスに各値へのアクセスプロパティを追加。
    - enumIntValue
	- componentValue
	- transformValue
	- rectTransformValue
	- rigidbodyValue
	- rigidbody2DValue
- Parameterクラスの間違ったタイプのプロパティにアクセスした場合にParameterTypeMismatchExceptionをスローするように追加。
- ParameterクラスにVariableを格納しているオブジェクトへの参照プロパティvariableObjectを追加。
- ParameterContainerInternalクラスにパラメータの値へのアクセスメソッドを追加。
    - SetComponent, GetComponent, TryGetComponent
	- SetComponent<TComponent>, GetComponent<TComponent>, TryGetComponent<TComponent>
	- SetVariable, GetVariable, TryGetVariable
	- SetVariable<TVariable>, GetVariable<TVariable>, TryGetVariable<TVariable>
- AnyParameterReferenceに各種パラメータの値へのアクセスプロパティを追加
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
- AnyParameterReferenceにパラメータの値への汎用的にアクセスするvalueプロパティを追加
- BoolParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- BoundsParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- ColorParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- ComponentParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- FloatParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- GameObjectParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- IntParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- LongParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- QuaternionParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- RectParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- RectTransformParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- RigidbodyParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- Rigidbody2DParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- StringParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- TransformParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- Vector2ParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。
- Vector3ParameterReferenceにパラメータの値へアクセスするvalueプロパティ追加。

### 改善

#### Arbor Editor

- 挙動のタイトルバーのドラッグ開始をマウスボタン押下位置から6ピクセル以上移動した時に行うように改善。

### 非推奨

#### スクリプト

- Parameter.GetVariable<T>(ref T)を非推奨に変更。

### 修正

#### Arbor Editor

- ノードに追加されている挙動のスクリプトを削除すると例外が発生していたのを修正。


## [3.5.0] - 2019-04-12

### 新機能

#### DataLink

データフローから入力できるようにしたいフィールドにDataLink属性をつけることで、簡単に入力スロットを持たせられる機能を追加しました。

### 追加

#### Arbor Editor

- ノードコメントがズームの影響を受けるようにするフラグを設定に追加。
- ツールバーの表示メニューに、ノードコメントの一括表示フラグを追加。

#### Behaviour Tree

- ノードのブレークポイント追加。

#### Parameter Container

- パラメータの検索ボックスを追加。

#### Data Flow

- DataLink属性を使用することで、簡単にデータフローからの入力が受け付けられる機能を追加。

#### 組み込みCalculator

- 剰余演算するCalculator追加。
    - Int.ModCalcualtor
	- Long.ModCalculator
	- Float.ModCalculator
- Enumフラグのビット演算をするCalculator追加。
    - EnumFlags.Add
	- EnumFlags.Remove
	- EnumFlags.Contains
- NodeGraph関連Calculator追加。
    - NodeGraph.GetRootGameObject
	- NodeGraph.GetRootGraph
	- NodeGraph.GetName

### 改善

#### Arbor Editor

- HideFlags.NotEditableのついたグラフの場合に編集できないように改善。  
  (Unity2018.3以降でプレハブを編集する場合はプレハブエディターを使う必要があります)
- 挙動挿入ボタンのポップアップ領域が他ボタンと被らないように改善。

#### Parameter Container

- パラメータ追加メニューを見やすいように整理。

#### Data Flow

- データの接続線のテクスチャを接続方向が分かりやすいデザインに改善。
- プレイ中にEnumフラグにEverytingを出力していると、接続線に表示される現在値が"-1"となってしまうのを改善。

#### Editor

- EulerAnglesAttributeのDefaultモードによる編集をVector4Fieldで行うように改善。

#### スクリプト

- Service.OnUpdate()をノードがアクティブになった直後にも呼ぶように改善。

### 変更

#### Arbor Editor

- サイドパネルのツールバーが全体ツールバーの横に表示されるレイアウトに変更。

#### Behaviour Tree

- (ブレークポイント関連)プライオリティの表示位置をノードの右上に変更。

#### Data Flow

- (DataLink関連)データの入力スロットをノードの枠外に表示するように変更。
- (DataLink関連)データスロットのGUIスタイルを変更。
- (DataLink関連)FlexibleField, ParameterReferenceの内部入力スロットをノード枠外に表示するように変更。

#### 組み込みStateBehaviour

- TransformSetPositionに更新タイミングを指定するフィールドを追加。
- TransformSetRotationに更新タイミングを指定するフィールドを追加。
- TransformSetScaleに更新タイミングを指定するフィールドを追加。

#### スクリプト

- AddVariableMenuに空文字を指定した場合はパラメータ追加メニューに表示しないように変更。

### 修正

#### Arbor Editor

- Unity 2017.3以降でサイドパネルの横幅が保存されない不具合を修正。
- 再生中にPlaymode tintが反映されなかったのを修正。(Unity2017.2～Unity2018.2はUnityの仕様により非対応)

#### 組み込みStateBehaviour

- ルートのステートマシンでEndStateMachineを使用した場合に、再生停止しなかったのを停止するように修正。
- EndStateMachineで親ステートマシンに戻った際に状態遷移していないと子ステートマシンが再生され続けていたのを修正。

### その他

#### Package Manager

- Package Managerで使用できるようにpackage.json追加。

#### Example

- Exampleを整理。
- DataFlow, DataLink, External Graphの例を追加。
- Exampleのreadme追加。


## [3.4.4] - 2019-03-01

### 修正

#### Unity対応

- Unity2019.1.0b3対応


## [3.4.3] - 2019-02-13

### 改善

#### Parameter Container

- GetParameterやSetParameterをグラフに追加し、Containerに同じグラフのParameterContainerを選択すると内部パラメータモードになってしまいパラメータが選択できなくなる問題を改善。

### 修正

#### Arbor Editor

- データフローの接続先がCalculatorかリルートノード以外の時に、値更新によるグラフ再描画がされない不具合を修正。

#### Behaviour Tree

- AbortFlags.LowerPriorityが設定されたDecoratorが上位ノードの実行中に再評価されてしまっていた不具合を修正。

#### Parameter Container

- [Unity2017.1.5f1以前] Vector2、Vector3、Rect、Boundsのパラメータを追加すると例外が発生する不具合を修正。

#### ランタイム

- ランタイムによるデバッグ実行中に例外ブレークポイントを設定すると必ずブレークしてしまう不具合を修正（Unityエディタ上での例外ブレークポイントは未対応）。


## [3.4.2] - 2019-01-02

### 修正

#### Arbor Editor

- ArborFSMの配列やListを持つStateBehaviourでNullReferenceExceptionが発生する不具合を修正。


## [3.4.1] - 2018-12-17

### 修正

#### ParameterContainer

- Arbor3.3.2以前にObject関連パラメータを追加していた場合、ロード時にエラーが発生してしまう不具合を修正。

#### スクリプト

- ParameterConditionが旧フォーマットのまま実行時にシーン読み込みするとNullReferenceExceptionが発生するのを修正。
  [関連する挙動]
    - [StateBehaviour] ParameterTransition
	- [Decorator] ParameterCheck
	- [Decorator] ParameterConditionalLoop
- CalculatorConditionが旧フォーマットのまま実行時にシーン読み込みするとNullReferenceExceptionが発生するのを修正。
  [関連する挙動]
    - [StateBehaviour] CalculatorTransition
	- [Decorator] CalculatorCheck
	- [Decorator] CalculatorConditionalLoop


## [3.4.0] - 2018-12-14

### 新機能

#### ノードグラフ内パラメータ

ArborFSMやBehaviourTreeに直接紐づけられるパラメータ機能が追加されました。
サイドパネルの「パラメータ」タブより作成できます。

パラメータのドラッグエリアからグラフビューへドラッグ＆ドロップすることでグラフからパラメータにアクセスできます。

#### サブグラフへのデータ受け渡し

SubStateMachineやSubBehaviourTreeなどサブグラフ関連の挙動にグラフ内パラメータへのアクセスフィールドを追加しました。

#### ノードのサイズ変更

各種ノードの横幅をドラッグして変更できる機能を追加しました（BehaviourTreeのルートノードなど一部ノードを除く）

#### グループノード内の整列

グループノードの中にあるノードの位置やサイズが変更された際に、ノードが重ならないように自動調整する機能を追加しました。

グループノードの設定ウィンドウから「Auto Alignment」を変更して設定できます。

### 追加

#### Arbor Editor

- サイドパネルにパラメータタブ追加。
- 各種ノードのサイズ変更追加。
- グループノード内の整列機能追加。

#### ParameterContainer

- パラメータのアクセスノードを作成するためのドラッグエリア追加。

#### 組み込みStateBehaviour

- GoToTransitionに遷移呼び出しをするメソッド設定を追加。
- ArborFSMを再生開始するPlayStateMachine追加。
- ArborFSMの再生停止するStopStateMachine追加。
- BehaviourTreeを再生開始するPlayBehaviourTree追加。
- BehaviourTreeの再生停止するStopBehaviourTree追加。
- SendMessageGameObject, SendMessageUpwardsGameObject, BroadcastMessageGameObject
    - MethodNameフィールドをFlexibleString型に変更。
    - 引数に使用できる型を追加
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

#### 組み込みActionBehaviour

- ArborFSMを再生開始するPlayStateMachine追加。
- ArborFSMの再生停止するStopStateMachine追加。
- BehaviourTreeを再生開始するPlayBehaviourTree追加。
- BehaviourTreeの再生停止するStopBehaviourTree追加。

#### スクリプト

- ParameterContainerにEnum型パラメータへのアクセスメソッド追加。

### 改善

#### Arbor Editor

- StateLinkの接続線の開始位置を調整。
- エディタのパフォーマンスを改善。
- StateBehaviourなどのスクリプト選択ウィンドウの検索ワードをスクリプトの種類ごとにキャッシュするように改善。
- 実行中にデータが格納されていないデータ接続線は暗く表示するように改善。

#### ParameterContainer

- 各パラメータに全種のデータフィールドを持っていたのを、必要最低限のデータのみ扱うように改善。

#### スクリプト

- ParameterConditionの各要素に全種のデータフィールドを持っていたのを、必要最低限のデータのみ扱うように改善。
  [関連する挙動]
    - [StateBehaviour] ParameterTransition
	- [Decorator] ParameterCheck
	- [Decorator] ParameterConditionalLoop
- CalculatorConditionの各要素に全種のデータフィールドを持っていたのを、必要最低限のデータのみ扱うように改善。
  [関連する挙動]
    - [StateBehaviour] CalculatorTransition
	- [Decorator] CalculatorCheck
	- [Decorator] CalculatorConditionalLoop

### 修正

#### Arbor Editor

- 挙動を折りたたんだ状態でのデータの接続線の接続位置を修正。
- BehaviourTreeの各ノードの接続スロットをドラッグしてノードを作成した時のノードの作成位置を修正。
- データスロットのラベルがない場合に、ArborEditorウィンドウ以外で使用できない際のヘルプボックスが一段下に表示されていたのを修正。
- Random.SelectComponentなどで使用しているWeightListの各要素を削除するとログが出力されるのを修正。
- ライブ追跡オンの状態でプレイ開始するとNullReferenceExceptionが発生することがある問題を修正。
- FlexibleFieldの参照タイプのDataSlotがCalculatorと表示されてしまうのを修正。
- FlexibleFieldの参照タイプをDataSlotに選択し直すと接続が切れてしまうのを修正。

#### スクリプト

- データの入力スロットがリルートノードにのみ接続されているだけで、出力スロットとつながっていない場合にNullReferenceExceptionが発生するのを修正。

#### Unity対応

- Unity2018.3.0f1対応
- Unity2019.1.0a10対応

#### "Odin - Inspector and Serializer" 対応

- 一部PropertyDrawerが正常に動作していなかったのを対処。

注釈: 
  他アセットとの連携は原則動作保証外です。
  この対処によって問題が発生しなくなるのを保証するものではありません。


## [3.3.2] - 2018-10-29

### 変更

#### Arbor Editor

- ノードヘッダーのアイコンボタンにボタンスタイルを使用するように変更。
- StateLinkが接続されている時、歯車アイコンが見えやすいように背景スタイルを変更。

### 修正

#### Arbor Editor

- Unity 2019.1.0a5に対応。

#### Build

- Universal Windows Platformへビルドするときにエラーが発生するのを修正。


## [3.3.1] - 2018-10-15

### 修正

#### Arbor Editor

- ドッキングしたArborEditorウィンドウの最大化を解除すると、データスロットのフィールドにエラーボックスが表示されてしまう不具合を修正。
- Unity2017.3.0以降でサイドパネルとグラフビューの境界線が表示されない時がある不具合を修正。

#### ArborFSM

- TransitionTiming.LateUpdateDontOverwriteによる遷移で予約上書きできなかったStateLinkの遷移カウントが増加してしまう不具合を修正。
- TransitionTiming.Immediateで遷移したStateLinkの遷移カウントが増えない不具合を修正。


## [3.3.0] - 2018-10-05

### 新機能 : InvokeMethod

Componentのメソッドを呼び出す組み込みスクリプトを追加。
引数をデータフローから入力したり、戻り値やout引数をデータフローに出力できる。

#### 組み込みStateBehaviour

- InvokeMethod追加。

#### スクリプト

- ArborEventクラス追加（メソッド呼び出しを行うコアクラス）
- ShowEventAttributeクラス追加（引数がFlexibleに対応していないメソッドの場合でも選択可能する属性）
- HideEventAttributeクラス追加（ArborEventで選択できないように隠す属性）

### 新機能 : ObjectPool

Arbor内でInstantiateしたオブジェクトを使いまわす機能を追加。

#### 組み込みStateBehaviour

- 事前Poolingを行うAdvancedPooling追加。
- ObjectPoolからインスタンス化するUsePoolフラグ追加。
    - InstantiateGameObject
	- SubStateMachineReference
	- SubBehaviourTreeReference
- Destroy時にObjectPoolへ返却するように変更。
  (ObjectPoolからInstantiateしている場合のみPoolに戻す)
	- DestroyGameObject
	- OnCollisionEnterDestroy
	- OnCollisionExitDestroy
	- OnTriggerEnterDestroy
	- OnTriggerExitDestroy
	- OnCollisionEnter2DDestroy
	- OnCollisionExit2DDestroy
	- OnTriggerEnter2DDestroy
	- OnTriggerExit2DDestroy

#### 組み込みActionBehaviour

- 事前Poolingを行うAdvancedPooling追加。
- 組み込みスクリプトによるInstantiateをObjectPoolに対応。
    - InstantiateGameObject
	- SubStateMachineReference
	- SubBehaviourTreeReference
- 組み込みスクリプトによるDestroyをObjectPoolに対応。
  (ObjectPoolからInstantiateしている場合はPoolに戻す)
    - DestroyGameObject

#### スクリプト

- ObjectPooling名前空間にObjectPoolクラス追加。

### 追加

#### Arbor Editor

- データスロットのリルートノードの右クリックメニューに「削除（接続を保持）」を追加。
- StateLinkリルートノードの右クリックメニューに「削除（接続を保持）」を追加。
- データスロットの右クリックメニューに「切断」を追加。（出力スロットの場合は「全て切断」）
- NodeBehaviourのInspector拡張スクリプトがある場合、メニューに「Editorスクリプト編集」を追加。
- ツールバーのデバッグメニューに「常にすべてのデータ値を表示」チェックを追加。

#### ArborFSM

- 無限ループのデバッグ設定追加。

#### BehaviourTree

- 無限ループのデバッグ設定追加。

#### ParameterContainer

- enum型追加。

#### 組み込みStateBehaviour

- CalcParameterのenum型対応追加。
- ParameterTransitionのenum型対応追加。
- SubStateMachineのメニューに「プレハブに保存」を追加。
- SubBehavioutTreeのメニューに「プレハブに保存」を追加。

#### 組み込みActionBehaviour

- SubStateMachineのメニューに「プレハブに保存」を追加。
- SubBehavioutTreeのメニューに「プレハブに保存」を追加。

#### 組み込みDecorator

- ParameterCheckのenum型対応追加。
- ParameterConditionLoopのenum型対応追加。

#### エディタ拡張

- 自作スクリプト用言語ファイルの設置場所を指定するLanguagePathアセット追加。

#### スクリプト

- OutputSlotTypableクラス追加。
- InputSlotTypableクラス追加。
- FlexibleEnumAnyクラス追加(enumを扱えるFlexibleField系クラス）
- ArborFSMInternalクラス
    - 遷移前ステートを参照できるprevTransitionStateプロパティ追加。
    - 遷移後ステートを参照できるnextTransitionStateプロパティ追加。
- StateBehaviourクラス
    - 遷移前ステートを参照できるprevTransitionStateプロパティ追加。
    - 遷移後ステートを参照できるnextTransitionStateプロパティ追加。
    - StateLinkの数を返すstateLinkCountプロパティ追加。
    - StateLinkを返すGetStateLinkメソッド追加。
    - StateLinkのキャッシュを再構築するRebuildStateLinkCacheメソッド追加。
- DataSlotクラスにDisconnectメソッド追加。
- AddBehaviourMenuの多言語対応追加。
- BehaviourTitleの多言語対応追加。
- BehaviourMenuItemの多言語対応追加。

### 変更

#### Arbor Editor

- ノードのメインコンテンツGUIをクリックしたときにノードが選択されないように変更。
- データスロットのGUIスタイル変更。
- データスロットのリルートノードをマウスオーバーした時に型名をツールチップに表示するように変更。
- StateLinkリルートノードの右クリックがスロット枠とノード枠で別扱いだったのを統合。
- StateLinkをドラッグ中、そのStateLinkにマウスオーバーしている場合は自ステートに接続しないように変更。
- BehaviourTreeのNodeLinkSlotのGUIスタイル変更。
- デコレータの現在のコンディションを表示するように変更。
- 挙動挿入ボタンを押した時の挙動選択ポップアップの表示位置を調整
- 接続線をマウスオーバーした時に前面表示するように変更。
- 型指定ポップアップでNoneを指定できるように変更。
- 型指定ポップアップウィンドウを方向キーで選択変更できるように対応。
- ParameterReferenceで参照するParameterContainerをデータスロットからも指定できるように変更。
- その他の型のデータの接続線の色を調整。

#### BehaviourTree

- ノードアクティブ時にDecoratorが失敗を返した場合はActionBehaviourやServiceのOnStart()は呼び出さないように変更。

#### 組み込みStateBehaviour

- InstantiateGameObjectのPrefabをFlexibleComponentに変更。
- SubStateMachineReferenceのExternal FSMをFlexibleComponentに変更。
- SubBehaviourTreeReferenceのExternal BTをFlexibleComponentに変更。

#### 組み込みActionBehaviour

- InstantiateGameObjectのPrefabをFlexibleComponentに変更。
- SubStateMachineReferenceのExternal FSMをFlexibleComponentに変更。
- SubBehaviourTreeReferenceのExternal BTをFlexibleComponentに変更。

### 改善

#### Arbor Editor

- Reflectionを使用していたところをdelegateを介すようにして高速化。

#### ArborFSM

- StateLinkを事前にキャッシュするように変更。

#### スクリプト

- EachFieldクラスを使用した際にフィールドをキャッシュして高速化。
- EachFieldクラスでReflectionを使用していたところをdelegateを介すようにして高速化(AOTやIL2CPP環境では変更なし)。

### 非推奨

#### スクリプト

- ArborFSMInternalのnextStateプロパティを廃止。
  (代わりにreserverdStateを追加)
- ClassTypeReferenceのTidyAssemblyTypeNameメソッドを廃止。
　(代わりにTypeUtility.TidyAssemblyTypeNameを追加)
- ClassTypeReferenceのGetAssemblyTypeメソッドを配置。
  (代わりにTypeUtility.GetAssemblyTypeを追加)

### 修正

#### Arbor Editor

- SubStateMachineReferenceやSubBehaviourTreeReferenceでインスタンス化したグラフをHierarchyで選択した場合にArbor Editorの選択も切り替わるように修正。
- Arbor.StateLinkとは別のStateLinkという名前のクラスがあった場合、エディタ表示で例外が発生していたのを修正。
- データスロットのリルートノード挿入をUndoすると接続が切れてしまっていたのを修正。
- ドラッグ中の接続線の描画がRepaintイベント以外でも行われていたのを修正。

#### ArborFSM

- OnStateBegin()内で同グラフのStop()を呼び出した場合の不具合
	- 例外が発生していたのを修正。
    - OnStateEnd()と次回実行時のOnStateBegin()が呼び出されなくなるのを修正。
	- 同ステートの次以降のOnStateBegin()が呼び出されていたのを修正。
- SubStateMachineReferenceでインスタンス化されている場合に、編集すると問題のある項目をInspectorに表示しないように修正。
- Inspectorにて「Copy Component」を行った際、SubStateMachineなどによる子グラフで使用しているNodeBehaviourが余分にコピーされてしまうのを修正。

#### BehaviourTree

- SubBehaviourTreeReferenceでインスタンス化されている場合に、編集すると問題のある項目をInspectorに表示しないように修正。
- Inspectorにて「Copy Component」を行った際、SubStateMachineなどによる子グラフで使用しているNodeBehaviourが余分にコピーされてしまうのを修正。

#### Waypoint

- Transformを指定していないPointsの要素を削除すると、次の要素まで削除されてしまうのを修正。

#### スクリプト

- EachField.Findにターゲットの配列を渡した時に取得できないのを修正。
- EachField.Findにターゲットのインスタンスを渡した時は中身を走査しないように修正。
- WeightList<T>の要素の型をUnityオブジェクトにした場合、オブジェクトを指定しない要素を削除しようとすると次の要素まで削除されてしまうのを修正。

#### その他

- Unity2018.3.0b3に対応。


## [3.2.4] - 2018-08-22

### 修正

#### Arbor Editor

- Unity2017.3以降で、ノードコメントの文字列をコピーしようとすると例外が発生するのを修正。
- Unity2018.1以降で、ノード内TextFieldの右クリックメニューによるコピーなどが動かないのを修正。
- MacOSのUnity2017.3以降で、挙動のドラッグ＆ドロップをすると自動スクロール判定が残り続けるのを修正。
- ズームアウト時の挙動挿入ボタンの表示位置が若干ずれていたのを修正。 
- リルートノードの右クリックメニューに無駄なセパレータが表示されているのを修正。


## [3.2.3] - 2018-08-08

### 修正

#### Arbor Editor

- Unity2018.1でObsoleteになったメソッドを使用していたのを修正。

#### 組み込み挙動

- AgentLookAtPositionのAngularSpeedプロパティが表示されていなかったのを修正。
- AgentLookAtTransformのAngularSpeedプロパティが表示されていなかったのを修正。


## [3.2.2] - 2018-07-26

### 修正

#### Arbor Editor

- データのリルートノードの方向変更をアンドゥしても即座に接続線に反映されないのを修正。
- 遷移元ステートが画面外にあるときにステートの移動をアンドゥすると即座に接続線に反映されないのを修正。

#### その他

- 他のアセットなどをインポートしていると、HierarchyのCreateボタンにArborグループが表示されない場合があるのを修正。


## [3.2.1] - 2018-07-24

### 修正

#### Arbor Editor

- 一度挙動をドラッグした後に、挿入ボタンから挙動追加すると、挿入位置が一つ前になるのを修正。

#### ArborFSM

- OnStateUpdate()内でTransitionTiming.Immediateにより遷移した後、同じフレームのLateUpdate()で遷移先ステートの処理がされてしまうのを修正。

#### BehaviourTree

- コンポジットノードとアクションノードの共通メニュー項目が抜け落ちていたのを修正。


## [3.2.0] - 2018-07-18

### 追加

#### Arbor Editor

- Calculatorの右クリックメニューに「スクリプト編集」追加。
- グループノードの色変更追加。
- 遷移ラインの右クリックメニューに「設定」追加。
- 挙動をInspectorにドラッグ＆ドロップできるように対応。
- 挙動を別ノードにドラッグ＆ドロップできるように対応。
	- Ctrl+ドロップ(Macではoption+ドロップ)でコピー。
- 挙動挿入ボタン追加。
- ノード内の挙動の展開と折りたたみ追加。
	- ノードの右クリックメニュー
	- グラフの右クリックメニュー（選択中ノードが対象）
	- ツールバー(全てのノードが対象)
- プレイ中のアクティブノード追跡
	- ツールバーの「ライブ追跡」トグルで切り替え
- ArborEditorの設定項目追加
	- ドッキングして開く : ArborEditorウィンドウを開いたときにSceneViewとドッキングするか設定
	- マウスホイールの挙動 : ズームするかスクロールするかを設定(Unity 2017.3以降)
	- 階層のライブ追跡 : ライブ追跡をする際、子グラフに自動的に切り替わるか設定
- ドラッグ中にマウスオーバーすると自動スクロールするエリアの表示を追加。

#### Behaviour Tree

- コンポジットノードの右クリックメニューに「コンポジット置き換え」追加。
- アクションノードの右クリックメニューに「アクション置き換え」追加。

#### Parameter Container

- VariableスクリプトのテンプレートにFlexibleFieldのコンストラクタや型変換追加。

#### 組み込み挙動

- Tween系挙動に現在値から指定した値までの変化モードを追加。
  (これに伴い、RelativeフィールドをTweenMoveTypeフィールドに変更)
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
- TweenColorSimple追加。
- UITweenColorSimple追加。
- TweenTextureScale追加。
- TweenMaterialFloat追加。
- TweenMaterialVector2追加。
- TweenTimeScale追加。
- TweenBlendShapeWeight追加。

#### 組み込み演算ノード

- Random.Value追加。
- Random.InsideUnitCircle追加。
- Random.InsideUnitSphere追加。
- Random.OnUnitSphere追加。
- Random.Rotation追加。
- Random.RotationUniform追加。
- Random.RangeInt追加。
- Random.RangeFloat追加。
- Random.Bool追加。
- Random.RangeVector2追加。
- Random.RangeVector3追加。
- Random.RangeQuaternion追加。
- Random.RangeColor追加。
- Random.RangeColorSimple追加。
- Random.SelectString追加。
- Random.SelectGameObject追加。
- Random.SelectComponent追加。

#### 組み込みコンポジット

- RandomExecutor追加。
- RandomSelector追加。
- RandomSequencer追加。

#### 組み込みVariable

- Gradient追加。
- AnimationCurve追加。

#### スクリプト

- ClassTypeConstraint属性を使用できるクラスを追加。
	- AnyParameterReference
	- ComponentParameterReference
	- InputSlotComponent
	- InputSlotUnityObject
	- InputSlotAny
	- FlexibleComponent
- ClassGenericArgumentAttribute追加。
- ParameterContainerにTryGetIntメソッドなどを追加。
- ParameterContainerにパラメータがない場合のデフォルト値を指定できるGetIntメソッドなどを追加。
- Quaternionをオイラー角で編集できるようにするEulerAnglesAtribute追加。
	- ParameterのQuaternionをEulerAnglesに対応。
	- FlexibleQuaternionをEulerAnglesに対応。
- Variableの追加メニュー名を指定するAddVariableMenu属性追加。
- State.IndexOfBehaviourメソッド追加。
- NodeBehaviourListにIndexOfメソッド追加。
- FlexiblePrimitiveTypeを使うクラスの基本クラスFlexiblePrimitiveBase追加。
- ConstantRangeAttribute追加(Range属性のFlexibleField版)
	- FlexibleInt, FlexibleFloatに対応。
- 各DataSlot固有のフィールドを隠すHideSlotFields属性追加
	- 主にOutputSlotComponent、OutputSlotUnitObjectのTypeフィールドを隠すのに使用する。
- AgentControllerに各フィールドにget/setアクセスするプロパティ追加。
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
- CompositeBehaviouorに拡張用メソッド追加
	- GetBeginIndex
	- GetNextIndex
	- GetInterruptIndex
- DecoratorにisRevaluationプロパティ追加。

### 変更

#### Arbor Editor

- ステートのリルートノードを作成した時、ラインの色を引き継ぐように変更。
- FlexiblePrimitiveType.Randomを使用しているCalculatorは常に再計算するように変更。
- 挙動をドラッグ中に自動スクロールするように変更。

#### Parameter Container

- 値のラベルを表示するように変更。

#### AgentController

- Animatorを指定しない場合、パラメータ名をTextFieldで編集できるように変更。
	- (AnimatorParameterReferenceも同様に対応)

#### 組み込み挙動

- いくつかの組み込み挙動のフィールドをFlexibleFieldに変更。
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
- Material変更する挙動をMaterialPropertyBlockを使用する様に変更。
	- TweenColor
	- TweenTextureOffset
- Tween系の各フィールドをステート開始時にキャッシュするように変更。

#### 組み込み演算ノード

- AddBehaviourMenuとタイトル名が一致するように変更。

#### 組み込みデコレータ

- 時間経過プログレスバーを再評価対象の時のみ表示するように変更。
	- Cooldown
	- TimeLimit

#### スクリプト

- ComponentParameterReferenceにParameter.Type.Component以外のコンポーネントパラメータを指定できるように変更。
- CalculatorSlotをDataSlotにリネーム。
- CalculatorBranchをDataBranchにリネーム。
- CalculatorBranchRerouteNodeをDataBranchRerouteNodeにリネーム。
- FlexibleType.CalculatorをDataSlotにリネーム。
- FlexiblePrimitiveType.CalculatorをDataSlotにリネーム。
- DataBranch.isVisibleをshowDataValueにリネーム。

### 非推奨

#### スクリプト

- InputSlotAny(System.Type)コンストラクタを廃止。
- OutputSlotAny(System.Type)コンストラクタを廃止。
- AnyParameterReference.parameterTypeを廃止。
- AnyParameterReference(System.Type)コンストラクタを廃止。
- ParameterContainerの古いGetIntメソッドなどを廃止。

### 廃止

#### スクリプト

- booとjavascriptのスクリプト作成を廃止。

### 修正

#### Arbor Editor

- BehaviourTreeを選択している時、ツールバーのデバッグメニューの一番下にセパレータが表示されていたのを修正。
- データ値表示切替メニューの文言修正。
- abstractの挙動クラスが挙動追加ウィンドウに表示されてしまうのを修正。
- 矩形選択ドラッグ中に自動スクロールした時に、矩形を更新するように修正。


## [3.1.3] - 2018-06-26

### 修正

####Editor

- MonoBehaviourスクリプトにCalculatorSlotを宣言した場合にInspectorを表示すると例外が発生するのを修正。
- シリアライズできない型でFlexibleField<T>を宣言した場合に、フィールド名のラベルが表示されなくなっていたのを修正。

#### スクリプト

- InputSlot<T>がスクリプトリファレンスに表示されないのを修正。
	- InputSlotクラスをInputSlotBaseに改名。
- OutputSlot<T>がスクリプトリファレンスに表示されないのを修正。
	- OutputSlotクラスをOutputSlotBaseに改名。
- FlexibleField<T>にSerializable属性のついたクラスしか使用できなかったのを修正。
- Variable<T>にSerializable属性のついたクラスしか使用できなかったのを修正。
- Variable<T>にシリアライズ可能な型を指定しても"not serializable"になっていたのを修正。
- AddComponentメニューに表示されないようにArborスクリプトテンプレートを修正。
- Exmaple用スクリプトのAddComponentメニューの表示位置を"Arbor/Example"に修正。
- CalculatorSlotのフィールドにSlotTypeAttributeでサブクラス以外の型を指定した場合に無視するように修正。
- SlotTypeAttributeを使用できるCalculatorSlotを以下のクラスに制限するように修正。
	InputSlotComponent、InputSlotUnityObject、InputSlotAny、OutputSlotAny


## [3.1.2] - 2018-06-19

### 修正

#### Arbor Editor

- System.Serializable属性をつけたUnityオブジェクト派生クラスが自身を参照するフィールドを持っている時に無限再帰してしまうのを修正。


## [3.1.1] - 2018-06-12

### 追加

#### 組み込みStateBehaviour

- AgentWarpToPosition追加
- AgentWarpToTransform追加
- TransformSetPosition追加
- TransformSetRotation追加
- TransformSetScale追加
- TransformTranslate追加
- TransformRotate追加

#### 組み込みActionBehaviour

- AgentWarpToPosition追加
- AgentWarpToTransform追加

#### 組み込みCalculator

- StringConcatCalculator追加
- StringJoinCalculator追加

#### スクリプト

- AgentControllerにWarpメソッド追加。
- NodeBehaviourにOnGraphPause,OnGraphResume,OnGraphStopコールバック追加。

### 修正

#### ArborFSM

- Stateコールバックメソッド以外でTransitionTiming.Immediateの遷移を行うと遷移回数が増えない問題を修正。
- ArborFSM.Stop()を呼び出した時に、OnStateEndメソッドがコールバックされないのを修正。

#### BehaviourTree

- BehaviourTree.Stop()を呼び出した時に、OnEndメソッドがコールバックされないのを修正。

#### 組み込みStateBehaviour

- Tween系のDurationを0にすると完了時の遷移が行われないのを修正。


## [3.1.0] - 2018-05-31

### 追加

#### ArborEditor

- グラフ未選択のときにグラフ作成ボタンやマニュアルページを開くボタンなどを表示。
- グラフのズーム機能を追加(Unity 2017.3.0f3以降のみ有効)
- グラフのキャプチャ機能を追加。
- ツールバーにグラフ作成ボタン追加。
- ArborEditorでグラフを開いた時にArborロゴの表示を追加。
	歯車アイコンをクリックして表示される設定ウィンドウでトグルできます。
- AssetStoreの更新通知を追加。

#### ArborEditor拡張

- ArborEditorWindowクラスに、背面をカスタマイズできるunderlayGUIコールバック追加。
- ArborEditorWindowクラスに、前面をカスタマイズできるoverlayGUIコールバック追加。
- ArborEditorWindowクラスに、ツールバーをカスタマイズできるtoolbarGUIコールバック追加。

#### ParameterContainer

- ユーザー定義型を追加できるVariable追加。
- Variable定義のスクリプトをテンプレートから作成するVariableGeneratorWindow追加。

#### AgentController

- MovementTypeフィールド追加。
- MovementDivValueフィールド追加。
- TurnTypeフィールド追加。
- 移動値や回転量をMovementTypeとTurnTypeに従ってAnimatorへ受け渡すように変更。

#### 組み込みStateBehaviour

- AgentControllerを指定位置方向に向き直すAgentLookAtPosition追加。
- AgentControllerを指定Transform方向に向き直すAgentLookAtTransform追加。
- プレハブのArborFSMを子グラフとして実行するSubStateMachineReference追加。
- プレハブのBehaviourTreeを子グラフとして実行するSubBehaviourTreeReference追加。
- シーンをアクティブにするSetActiveScene追加。
- LoadLevelにIsActiveSceneフィールドを追加。
- LoadLevelにDone遷移追加。

#### 組み込みActionBehaviour

- AgentControllerを指定位置方向に向き直すAgentLookAtPosition追加。
- AgentControllerを指定Transform方向に向き直すAgentLookAtTransform追加。
- プレハブのArborFSMを子グラフとして実行するSubStateMachineReference追加。
- プレハブのBehaviourTreeを子グラフとして実行するSubBehaviourTreeReference追加。
- Waitに経過時間の表示を追加。

#### 組み込みDecorator

- TimeLimitに経過時間の表示を追加。
- Cooldownに経過時間の表示を追加。

#### スクリプト

- Assembly Definitionに対応（Unity2017.3.0f3以降のみ有効）
	この対応に伴い、フォルダ構成も変更。
- NodeGraphクラスにrootGraphプロパティ追加。
- NodeGraphクラスにToStringメソッド追加。
- NodeクラスにGetNameメソッド追加。
- NodeクラスにToStringメソッド追加。
- 入力する型を指定できるInputSlotAnyクラス追加。
- 出力する型を指定できるOutputSlotAnyクラス追加。
- 参照するパラメータの型を指定できるAnyParameterReferenceクラス追加。
- Flexibleな型クラスのジェネリッククラス版FlexibleField<T>追加。
- ParameterにvariableValueプロパティ追加。
- ParameterにSetVariableメソッド追加。
- ParameterにGetVariableメソッド追加。

### 変更

#### ArborEditor

- Gridボタンを歯車アイコンに変更し、ボタンを押して表示されるポップアップウィンドウをグリッド以外も設定するGraphSettingsウィンドウに改名。
- ツールバーの言語ポップアップをGraphSettingsウィンドウへ移動。
- ツールバーのヘルプボタンからアセットストアやマニュアルページを開くメニューを表示するように変更。
- 接続線をマウスオーバーした時のハイライト表示を見やすいデザインに変更。
- グループノードのリサイズを各辺のドラッグでできるように変更。
- サイドパネルのノードリストでCtrlやShiftを押しながら複数選択できるように変更。
- リルートノードの向きをドラッグ中にEscキーを押すとキャンセルできるように変更。

#### 組み込みStateBehaviour

- LoadLevelのAdditiveフィールドをLoadSceneModeフィールドに変更。
- TimeTransitionのSecondsフィールドをFlexibleFloat型に変更。

#### スクリプト

- FlexibleField関係のクラスで使用する参照タイプを共通で使用するように変更。
- Parameter.intValueなどをプロパティ化。
	setした際にonChangedを呼ぶように変更。
- Parameter.valueプロパティにset追加。

### 非推奨

#### スクリプト

- Parameter.OnChangedをObsoleteに変更。
	Parameter.intValueなどを変更した際に内部でonChangedがコールバックされるようになったため、呼び出しは不要になりました。
- AddCalculatorMenu属性をObsoleteに変更。
	AddBehaviourMenu属性を共通で使用するように変更しました。
- BuiltInCalculator属性をObsoleteに変更。
	BuiltInBehaviour属性を共通で使用するように変更しました。
	また、BuiltInBehaviour属性は組み込み挙動用の属性のため、それ以外では使用しなくても問題ありません。
- CalculatorHelp属性をObsoleteに変更。
	BehaviourHelp属性を共通で使用するように変更しました。
- CalculatorTitle属性をObsoleteに変更。
	BehaviourTitle属性を共通で使用するように変更しました。

### 修正

#### ArborEditor

- BehaviourTreeが実行終了してもArborEditor上ではアクティブ表示されたままとなり、まるで実行が継続されているように見えていたのを修正。
- FlexibleGameObjectのCalculatorタイプフィールドの高さを修正。
- サイドパネルのグラフ名入力欄が入力欄以外をクリックしてもフォーカスしたままになっているのを修正。
- StateLinkやCalculatorSlotなどの接続線のドラッグ中にEscキーを押すとドラッグ中のライン表示が残ってしまうのを修正。
- サイドパネルのヘッダスタイルがUnityのバージョンにより見た目が変わっていたのを修正。
- ConstantMultilineAttributeをつけたFlexibleStringでテキストの切り取りや貼り付けなどの編集ができなかったのを修正。
- ドッキングされているArborEditorウィンドウが非表示のままプレイ開始やシーン切り替えなどをするとNullReferenceExceptionが発生するのを修正。
- プレイモード終了時に画面外のノードへの接続線の表示位置が正しくない不具合を修正。
- Actionノードをコピーするときノード名がコピーされていなかったのを修正。
- Compositeノードをコピーするときノード名がコピーされていなかったのを修正。
- ノードを貼り付けや複製した時にグリッドスナップが効いていなかったのを修正。
- Stateの遷移元がリルートノードの場合に、Stateを削除しても接続ラインが消えなかったのを修正。
- StateLinkを接続しているStateを削除後にリドゥすると、接続ラインがすぐに再描画されないのを修正。
- ノード選択のUndo/Redoを修正。
- ノード作成や削除のUndo/Redoを繰り返し行うとメモリリークしていたのを修正。
- グラフ選択のUndo/Redoをしたときの不具合を修正。
- ノード選択していない場合はFrame Selectedできないように修正。

#### ArborFSM

- Unity2018.1以降でArborFSMのRemoveComponentを行うとNullReferenceExceptionが発生するのを修正。
- プレハブのArborFSMをシーンウィンドウにドラッグ＆ドロップすると、グラフ内部で使用しているコンポーネントがインスペクタに表示されてしまうのを修正。

####BehaviourTree

- 現在ノードがRootになったタイミングで割り込み判定が行われるとNullReferenceExceptionが発生するのを修正。
- Unity2018.1以降でBehaviourTreeのRemoveComponentを行うとNullReferenceExceptionが発生するのを修正。
- プレハブのBehaviourTreeをシーンウィンドウにドラッグ＆ドロップすると、グラフ内部で使用しているコンポーネントがインスペクタに表示されてしまうのを修正。

#### AgentController

- AgentController自身のTransformを参照していた不具合を修正。
- AgentControllerの初期化をAwakeで行うように修正。

#### 組み込みStateBehaviour

- SubStateMachineのUpdateTypeをManualに修正
	ルートグラフのUpdateTypeにより適切なタイミングで処理されるように変更。
- SubBehaviourTreeを追加するとArborFSMがインスペクタに表示されなくなるのを修正。

#### 組み込みActionBehaviour

- WaitのSecondsが毎フレーム再計算していたのを修正。

#### 組み込みDecorator

- TimeLimitのSecondsが毎フレーム再計算していたのを修正。
- CooldownのSecondsが毎フレーム再計算していたのを修正。

#### スクリプト

- State.transitionCountをuintに修正。
- State.transitionCountがuint.MaxValueを超えないように修正。
- StateLink.transitionCountをuintに修正。
- StateLink.transitionCountがuint.MaxValueを超えないように修正。

#### その他

- スクリプトのコンパイル直後にプレイ開始すると、開始までに時間がかかるようになっていたのを修正。
- Unity2018.2.0 ベータ版でのエラー修正。


## [3.0.2] - 2018-03-20

### 追加

#### 組み込みStateBehaviour

- AddForceRigidbodyにForceModeとSpaceのフィールド追加。
- AddVelocityRigidbodyにSpaceのフィールド追加。
- SetVelocityRigidbodyにSpaceのフィールド追加。
- AddForceRigidbody2DにForceModeとSpaceのフィールド追加。
- AddVelocityRigidbody2DにSpaceのフィールド追加。
- SetVelocityRigidbody2DにSpaceのフィールド追加。
- AgentMoveOnWaypointにStoppingDistance追加。

#### 組み込みActionBehaviour

- AgentMoveOnWaypointにStoppingDistance追加。

### 修正

#### ArborEditor

- 演算スロットのドラッグ中に、継承の関係にあり接続可能なスロットがハイライト表示されていなかったのを修正。

#### コンポーネント

- AgentControllorのisDoneがfloatの計算誤差によりtrueにならないことがあったのを修正。

#### 組み込みStateBehaviour

- SubStateMachineのUpdateTypeをManualに修正。


## [3.0.1] - 2018-03-10

### 修正

#### Arbor Editor

- Arbor Editorウィンドウを閉じた後にノードグラフを削除すると例外が発生するのを修正。
- Arbor Editorウィンドウをドッキングしていない状態でアクションノードやコンポジットノードを作成するとリネーム枠が表示されなかったのを修正。
- 読み込みに失敗したDLLが存在しているとArborEditorでもエラーが発生してしまうのを修正。

#### 組み込みActionBehaviour

- DestroyGameObjectのリファレンスがなかったのを修正。


## [3.0.0] - 2018-03-09

### 新機能：Behaviour Tree

#### 概要

新機能として、木構造により優先順位を見える化しながら挙動を組めるBehaviour Treeを追加しました。

最初にアクティブになるRootNodeや、子ノードの実行順などを決めるCompositeNodeと行動を指定するActionNodeがあります。

ActionNodeには挙動を記述するためのスクリプトActionBehaviourを設定できます。
ActionBehaviourも、ArborFSMのStateBehaviourと同様にカスタマイズ可能です。

CompositeNodeとActionNodeには実行する条件チェックや繰り返しなどを行うスクリプトDecoratorも追加できます。
こちらもカスタマイズが可能です。

他にも、ノードがアクティブな間実行されるServiceスクリプトにより柔軟なAIが作成可能となっております。

また、ArborFSMと同様に演算ノードと演算スロットを活用することでノード間のデータの受け渡しもできます。

#### コンポーネント

- BehaviourTreeコンポーネント追加。

#### 組み込みCompositeBehaviour

- Selector追加。
- Sequencer追加。

#### 組み込みActionBehaviour

- Wait追加。
- PlaySound追加。
- PlaySoundAtPoint追加。
- PlaySoundAtTransform追加。
- StopSound追加。
- SubStateMachine追加。
- SubBehaviourTree追加。
- InstantiateGameObject追加。
- DestroyGameObject追加。
- ActivateGameObject追加。
- AgentPatrol追加。
- AgentMoveToPosition追加。
- AgentMoveToTransform追加。
- AgentMoveOnWaypoint追加。
- AgentEscapeFromPosition追加。
- AgentEspaceFromTransform追加。
- AgentStop追加。

#### 組み込みDecorator

- Loop追加。
- SetResult追加。
- InvertResult追加。
- ParameterCheck追加。
- CalculatorCheck追加。
- ParameterConditionalLoop追加。
- CalculatorConditionLoop追加。
- TimeLimit追加。
- Cooldown追加。

#### Script

- 子ノードの実行を制御するCompositeBehaviour追加。
- アクションを実行するActionBehaviour追加。	
- ノードを装飾するDecorator追加。
- ノードがアクティブの間実行するService追加。

### 追加

#### Arbor Editor

- グラフの階層化に対応。
- サイドパネルにGraph項目追加。
- StateLinkのドラッグ中に何もないところでドロップした場合のメニュー追加。
- StateLink接続ラインのリルートノード追加。
- CalculatorBranch接続ラインのリルートノード追加。
- CalculatorBranchの値をクリックするとConsoleにログ出力するように追加。
- Alt(MacではOption)キーを押しながらグループノードを移動すると、グループ内のノードは移動しない機能を追加。

#### ArborFSM

- 開始時に再生するフラグPlay On Startフィールド追加。
- 更新間隔などの設定を行うUpdate Settingsフィールド追加。

#### ParameterContainer

- Componentパラメータの型指定を追加。
- Colorパラメータ追加。

#### 組み込みStateBehaviour

- SubStateMachine追加。
- EndStateMachine追加。
- SubBehaviourTree追加。
- AgentPatrolにCenter Typeフィールド追加。
- AgentPatrolにCenter Transformフィールド追加。
- AgentPatrolにCenter Positionフィールド追加。
- RaycastTransitionにIs Check Tagフィールド追加。
- RaycastTransitionにTagフィールド追加。

#### Script

- グラフを扱うための基本クラスNodeGraph追加。
- NodeがNodeBehaviourを格納する場合に使用するインターフェイスINodeBehaviourContainer追加。
- NodeBehaviourが子グラフを格納する場合に使用するインターフェイスINodeGraphContainer追加。
- NodeBehaviour作成時に呼ばれるOnCreatedメソッド追加。
- NodeBehaviour破棄前に呼ばれるOnPreDestroyメソッド追加。
- ArborFSMInternalにPlayとStopメソッド追加。
- ArborFSMInternalにPauseとResumeメソッド追加。
- ArborFSMInternalにplayStateプロパティ追加。
- TimeTypeから現在時間を返すTimeUtility.CurrentTimeメソッド追加。
- Bezier2DにGetClosestParamメソッド追加。
- Bezier2DにGetClosestPointメソッド追加。
- Bezier2DにSetStartPointメソッド追加。
- Bezier2DにSetEndPointメソッド追加。
- ComponentParameterReferenceで参照するComponentの型をSlotTypeAttributeで指定できるように追加。

### 変更

#### Arbor Editor

- ツールバーのステートリストをサイドパネルに改名。
- サイドパネルのステートリストをノードリストに改名。
- ステートをコピー＆ペーストした際に、コピー元とコピー先が同じArborFSMだった場合にStateLinkを接続したままペーストするように変更。
- ステートをコピー＆ペーストした際に、StateLinkの接続先ステートもペーストされたノードならStateLinkを接続したままにするように変更。
- StateLinkのラインのマウスオーバー時に色を変更。
- StateBehaviourのタイトルバーを右クリックしたときにもメニューが表示されるように変更。
- 実行中にCalculatorBranchの値を表示する方法を、ラインをマウスオーバーするか右クリックメニューから「値を常に表示する」にチェックする方法に変更。
- 実行中の現在ステートのデザインを変更。
- 選択中ノードへの移動スクロールを調整。

#### 組み込みStateBehaviour

- AgentMoveOnWaypoint開始時に現在目標地点への移動を行い、移動完了後に目標地点を次に移すように変更。

#### Script

- ArborFSMInternalをNodeGraphから継承するように変更。
- ArborFSMInternalのfsmNameを削除（NodeGraphにgraphNameを使用すること）。
- ArborFSMのFindFSMとFindFSMsをObsoleteに変更（NodeGraphのFindGraphとFindGraphsを使用すること)。
- StateBehaviourのstateIDをObsoleteに変更（nodeIDを使用すること）。
- NodeとNodeBehaviourから参照するグラフをArborFSMInternalからNodeGraphに変更。
- CalculatorNodeをNodeGraphで管理するように変更。
- GroupNodeをNodeGraphで管理するように変更。
- CommentNodeをNodeGraphで管理するように変更。
- ClaculatorBranchをNodeGraphで管理するように変更。
- CalculatorSlotのstateMachineフィールドをnodeGraphフィールドに変更。
- NodeBehaviourのstateMachineプロパティをStateBehaviourに移動。
- StateLinkのlineEnableをNonSerializedに変更。
- StateLinkのlineStartなどを削除しbezierフィールドを追加。
- TimeTransitionクラス内のTimeTypeをArbor名前空間に移動。
- 組み込みStateBehaviourの名前空間をArbor.StateMachine.StateBehavioursに変更。
- FlexibleComponentで指定したSlotTypeAttributeで参照するParameterを指定するように変更。
- 再描画が必要な時にEditorクラスのRequiresContentRepaintメソッドを使用できるように変更。

#### その他

- NodeGraphをInspectorからHierarchyにドラッグして移動できないように変更。
- 組み込みNodeBehaviour関連スクリプトをBuiltInBehavioursフォルダに移動。

### 修正

#### Arbor Editor

- ノードをコピーした後にArborFSMのCopy Componentを行うとノードのコピーが消えるのを修正。
- TransitionTiming.Immediateによる遷移を繰り返すとStackOverflowExceptionが発生していたのを修正。
- ステートにブレークポイントが設定されていてもTransitionTiming.Immediateによって遷移したい場合に停止しなかったのを修正。
- AddBehaviourMenuウィンドウとAddCalculatorMenuウィンドウ内の階層アニメーション中に検索ワードを入力すると例外が発生するのを修正。
- NodeGraphのRemove ComponentをUndoした場合にArbor Editorの参照が戻らないのを修正。
- ノード内を右クリックしたときにグラフのメニューが表示されないように修正。
- 実行中にStateBehaviourのbehaviourEnabledを切り替えた場合にOnStateAwakeとOnStateBeginが呼ばれるように修正。
- 実行中にStateBehaviourを追加した場合にOnStateAwakeとOnStateBeginが呼ばれるように修正。
- Unityを起動した際にTypePopupWindowで"Removed unparented EditorWindow while reading window layout"のログが出るのを修正。
- 実行中に常に再描画していたのを必要な時のみ行うように修正。
- ノード作成時にグリッドスナップするように修正。
- ノード作成などでグラフ領域のサイズが変更されたときにノードの表示がボケるのを修正。

#### Script

- EachFieldで基本クラスのpublicとprotectedフィールドを重複して列挙していたのを修正。

#### その他

- InspectorからResetを行うとNodeBehaviourが内部的に残ったままになるのを修正。
- ArborFSMコンポーネントを削除した時にCalculatorが内部的に残ったままになるのを修正。


## [2.2.3] - 2018-02-21

### 修正

#### Arbor Editor

- ノードのリネーム中にノードのショートカットが使えていたのを修正。
- ParameterContainerのパラメータの順番を変更するとそのパラメータを参照していると発生する例外を修正。


## [2.2.2] - 2017-11-22

### 追加

#### Arbor Editor

- 削除したStateBehaviourやCalculatorのスクリプトを使用しているノードやオブジェクトを削除できるように追加。

### 変更

#### その他

- Unity最低動作バージョンを5.4.0f3に引き上げ。

### 修正

#### Arbor Editor

- ArborFSMで使用しているStateBehaviourやCalculatorのスクリプトを削除するとArbor Editorウィンドウで例外(ArgumentNullException)が発生するのを修正。


## [2.2.1] - 2017-11-14

### 追加

#### コンポーネント

- ParameterContainerで使用できるパラメータにLong追加。

#### 組み込みCalculator

- LongAddCalculator追加
- LongSubCalculator追加
- LongMulCalculator追加
- LongDivCalculator追加
- LongNegativeCalculator追加
- LongCompareCalculator追加
- LongToFloatCalculator追加
- FloatToLongCalculator追加

#### Script

- FlexibleStringのConstant時の表示を複数行にするConstantMultilineAttribute追加。
- long型パラメータの追加に伴い、FlexibleLong、LongParameterReference、InputSlotLong、OutputSlotLong追加。

### 変更

#### Arbor Editor

- FlexibleStringのConstant時の表示を差し戻し。

#### コンポーネント

- ParameterContainerでパラメータを並び替えできるように変更。
- ParameterContainerにパラメータの型を表示。

### 修正

#### Arbor Editor

- Arbor Editorを開いた状態でUnityエディタ上でのプレイ終了すると例外(NullReferenceException: SerializedObject of SerializedProperty has been Disposed.)が発生するのを修正。
- 言語を切り替えてもGraphのラベルが変わらないのを修正。

#### Script

- Unityエディタ上でのプレイ開始時に型列挙処理が行われることで負荷がかかっていたのを修正。


## [2.2.0] - 2017-10-25

### 追加

#### Arbor Editor

- グループノード追加。
- 各種ノードごとのコメント追加。
- ノードの切り取りに対応。
- ノードのコンテキストメニューにノードの切り取り、コピー、複製、削除の項目追加。
- StateLinkボタンにTransition Timingアイコン表示。
- ArborFSMインスペクタのCopy Componentで内部コンポーネント含めてコピーできるように対応。

#### 組み込み挙動

- AgentStop追加。
- AgentMoveOnWaypoint追加。
- AnimatorCrossFade追加。
- AnimatorSetLayerWeight追加。
- BackToStartState追加。
- ActivateBehaviour追加。
- ActivateRenderer追加。
- ActivateCollider追加。
- UISetSlider、UISetText、UISetToggleにChangeTimingUpdateパラメータ追加。

#### 組み込みCalculator

- GameObjectGetComponentCalculator追加。

#### コンポーネント

- AgentControllerにAnimator設定用の各種パラメータ追加。
- Waypointコンポーネント追加。
- ParameterContainerで使用できるパラメータにComponent追加。

#### Script

- Animatorの型ごとのパラメータ参照用クラス追加。
- StateBehaviourの順でUpdate時に呼ばれるOnStateUpdateコールバック追加。
- StateBehaviourの順でLateUpdate時に呼ばれるOnStateLateUpdateコールバック追加。
- InputSlotUnityObject、OutputSlotUnityObject追加。
- FlexibleComponent追加。
- CalculatorSlotやFlexibleCompomentで型を指定できるSlotTyeAttribute追加。
- ComponentParameterReference追加。
- StateBehaviourとCalculatorの共通部分をまとめたNodeBehaviourクラス作成。
- NodeBehaviourのシリアライズ時の処理を記述するためのinterface、INodeBehaviourSerializationCallbackReceiverを追加。
- 各種Flexibleクラスにtypeとparameterプロパティ追加。

### 変更

#### Arbor Editor

- Editorのデザイン変更。
- ステートの名前入力欄をダブルクリックで表示するように変更。
- ステートリストを種類順(開始ステート -> 通常ステート -> 常駐ステート)にソートするように変更。
- StateLinkSettingWindowのImmediate TransitionをTransition Timingに変更。
- OutputSlotComponentに型指定を追加。
- FlexibleStringのConstant時の表示をTextAreaに変更。
- ドラッグでのスクロール中は一定時間間隔でスクロールされるように対応。
- ドラッグでのスクロール中の最大移動量を調整。

#### 組み込み挙動

- LookAtGameObjectにTarget transformの各座標成分を使用するかどうかのフラグ追加。
- Agent系にステートを抜けるときにAgentを止めるかどうかのフラグ追加。
- ドキュメントのUITextFromParameterのformatに形式指定の参考URL追加。
- Componentを参照している組み込み挙動をFlexibleComponentに対応。
- UISetSliderとUISetToggleのFlexibleComponent対応により、UISetSliderFromParameterとUISetToggleFromParameterをLagacyに移動。

#### コンポーネント

- AgentControllerのAnimator指定をパラメータごとではなく統一するように変更。

#### Script

- 各種ノードのIDをNode.nodeIDに統一。
- ParameterクラスにToStringメソッド追加。
- CalculatorBranchにupdatedTime追加。
- InputSlotにisUsedとupdatedTime追加。
- Transitionメソッドでの遷移タイミングの指定をTransitionTimingに変更。
- CalculatorSlot.positionをNonSerializedに変更。
- テンプレートにusing System.Collections.Generic;とAddComponentMenu("")を追加。

### 改善

#### Arbor Editor

- CalculatorBranchのドット線の表示を最適化。

### 修正

#### Arbor Editor

- ノードをドラッグ中にスクロールするとノードの位置がずれるのを修正
- CalculatorBranchの線が長い場合に頂点数エラーが出るのを修正
- StateLinkSettingWindowを画面端で表示すると縦に余白が表示されるのを修正。
- StateLinkを同じステートに接続し直した時にラインが非表示になってしまうのを修正。
- 実行中にArborFSMオブジェクトのPrefabにApplyした際にStateBehaviourがインスペクタに表示されてしまうのを修正。
- 実行中にArborFSMオブジェクトのPrefabにApplyした際に現在ステートのStateBehaviourが有効状態のままPrefabに保存されてしまうのを修正。
- CalculatorSlotを配列で持っているBehaviourでSizeを変更した際の接続を修正。
- ノードを削除した時に他のノードが一瞬表示されなくなるのを修正。

#### 組み込み挙動

- GlobalParameterContainerを介してParameter変更時に処理している挙動で、シーン遷移後に例外が発生してしまうのを修正(UISetTextFromParameter、UISetToggleFromParameter、UISetSliderFromParameter、ParameterTransition)。
- UISetImageのリファレンスがなかったのを修正。

#### 組み込みCalculator

- GlobalParameterContainerを介してParameter変更時に処理しているCalculatorで、シーン遷移後に例外が発生してしまうのを修正。

#### Script

- EachFieldで基本クラスのフィールドを参照していなかったので修正。


## [2.1.8] - 2017-10-10

### 修正

#### Arbor Editor

- StateLinkやCalculatorSlotをドラッグ中にスクロールしてノードが表示されなくなるとドラッグ処理が行われなくなるのを修正。


## [2.1.7] - 2017-09-15

### 変更

#### Script

- 無効なArborFSMのTransitionメソッドを呼び出した場合、immediateTransitionをtrueにしていても有効になるまで遷移を遅延させるように変更。


## [2.1.6] - 2017-07-25

### 修正

#### Arbor Editor

- ArborFSMのPrefabインスタンスにStateBehaviourを追加するとプレイ開始とともに例外が発生するのを修正。


## [2.1.5] - 2017-07-20

### 修正

####組み込み挙動

- RandomTransitionをInspectorのAdd Componentメニューに表示しないように修正。

### 改善

#### Arbor Editor

- Arbor Editorウィンドウの高速化。


## [2.1.4] - 2017-07-14

### 追加

#### Script

- ArborFSMInternalに各種ノードのインデックスを取得するメソッドを追加 : GetStateIndex() , GetCommentIndex() , GetCalculatorIndex()

### 修正

#### Arbor Editor

- ノードをコピーするとプレイ開始時などに警告が表示されるのを修正。
- Prefab化しているインスタンスからのみノードを削除しているとプレイ開始時にノードが復活してしまう不具合を修正。


## [2.1.3] - 2017-07-13

### 修正

#### Arbor Editor

- ArborFSMをアタッチしているPrefabインスタンスのApplyボタンを押すと出る例外を修正。


## [2.1.2] - 2017-07-07

### 追加

#### Arbor Editor

- ArborEditorウィンドウ上で実行中に任意のステートに遷移できる機能を追加。

#### 組み込み挙動

- SendEventGameObjectにOnStateAwakeイベントとOnStateEndイベント追加。

#### Script

- ParameterContainerで扱うすべての型のGet/Setメソッド追加。
- ParameterContainerにGetParamIDメソッド追加。
- ParameterContainerのGet/SetメソッドをIDからも行えるメソッド追加。

### 変更

#### 組み込み挙動

- Tween開始時の初期化用コールバックOnTweenBeginメソッド追加。
- SendEventGameObjectのEventパラメータをOnStateBeginにリネーム。

### 修正

#### Arbor Editor

- StateBehaviourとCalculatorのEditorで例外が発生した場合にArborEditorウィンドウが正常に表示されなくなるのを修正。
- StateBehaviourとCalculatorのスクリプトが読み込めない場合に出ていた例外を修正。
- ArborEditorを表示していると"NullReferenceException: SerializedObject of SerializedProperty has been Disposed."という例外がでることがあるのを修正。
- ArborEditorウィンドウの幅が狭くなった際にステートリストの幅が狭くなりすぎるのを修正。

#### Script

- 遷移処理中にSendTriggerを使用した場合、遷移処理が完了した後にTriggerを送るように修正。


## [2.1.1] - 2017-05-31

### 変更

#### Arbor Editor

- ブレークポイントとステートカウントの表示を枠外に移動

### 修正

#### Arbor Editor

- ParameterContainerのStringでコピー&ペーストできなかったのを修正。
- データスロットがスクリプトから削除された場合にCalculatorBranchも削除するように修正


## [2.1.0] - 2017-05-17

### 追加

#### Arbor Editor

- GameObjectを選択しても切り替わらないようにロックするトグル追加。
- StateBehaviourのタイトルバーをドラッグして並び替えできるように対応。
- StateBehaviourをドラッグ&ドロップで任意の位置に挿入できるように対応。
- Stateにブレークポイントを設定できるように対応。
- 実行中にStateとStateLinkが通った回数を表示するように対応。
- 実行中に直前に通ったStateLinkを強調表示するように対応。
- 実行中にCalculaterBranchの値を表示するように対応。
- 組み込みコンポーネントのヘルプボタンからヘルプページを開く。
- 組み込みCalculatorのヘルプボタンからヘルプページを開く。
- CalculatorBranchの型によって線の色を変更。
- ArborEditorウィンドウにアイコン追加

#### 組み込み挙動

- RandomTransition追加。

### 変更

#### 組み込み挙動

- FindGameObject、FindWithTagGameObjectで見つけたGameObjectを演算ノードへ出力するように対応。
- TimeTransitionにTimeTypeの指定を追加。

#### Script

- OnStateTriggerをStateBehaviourの仮想関数に変更。

#### その他

- リファレンスサイトを更新。
- Unity最低動作バージョンを5.3.0f4に引き上げ。

### 修正

#### Arbor Editor

- ArborFSMを別のGameObjectに移動したときに入出力スロットからデータにアクセスできなくなっていたのを修正。
- Arbor Editorのグラフ表示エリアがずれるのを修正。
- ステートリストから選択する場合など、選択したステートまで自動的にスクロールした時にArbor Editorのグラフ表示が滲むのを修正。

#### 組み込み挙動

- Flexibleなコンポーネントの参照でのキャッシュ処理を修正。
- 配列にStateLinkがあるBehaviourでエディタ上で配列のサイズを減らすとエラーが出るのを修正。

#### Script

- AgentController.FollowとEscapeにnullが渡されたときにエラーが出ないように修正。


## [2.0.10] - 2017-04-11

### 変更

#### Arbor Editor

- GameObjectを選択した際、Arbor Editorも連動して表示が切り替わるように対応。

### 修正

#### Arbor Editor

- コメントを新規作成するとエラーが出るのを修正。
- ノードのコピーを行うとプレイ開始時にエラーが出るのを修正。
- ノードをコピーし一度プレイ開始したあとペーストできなくなるのを修正。
- Calculatorノードのコピーを修正。
- CalculatorSlotを持ったStateBehaviourやCalculatorをコピー&ペーストや複製した時の処理を修正。


## [2.0.9] - 2017-04-04

### 変更

#### 組み込み挙動

- CalculatorTransitionのBoolを２つのBool値を比較するように変更。

#### スクリプト

- State.behaviourCountとGetBehaviourFromIndex追加。State.behavioursを非推奨に。
- ArborFSMInternal.stateCountとGetStateFromIndex追加。ArborFSMInternal.statesを非推奨に。
- ArborFSMInternal.commentCountとGetCommentFromIndex追加。ArborFSMInternal.commentsを非推奨に。
- ArborFSMInternal.calculatorCountとGetCalculatorFromIndex追加。ArborFSMInternal.calculatorsを非推奨に。
- ArborFSMInternal.calculatorBranchCountとGetCalculatorBranchFromIndex追加。ArborFSMInternal.calculatorBranchiesを非推奨に。

### 修正

#### Arbor Editor

- Unity5.6でArborEditorを開くとエラーが出るのを修正。
- Arbor Editorウィンドウの高速化。


## [2.0.8] - 2016-12-29

### 変更

#### Arbor Editor

- ArborFSMが先に実行されるようにScript Execution Orderを変更。

### 修正

#### Arbor Editor

- Unity5.3.4以降のArbor EditorでNodeやStateBehaviourをコピーするとエラーが表示されるのを修正。


## [2.0.7] - 2016-11-16

### 追加

#### Arbor Editor

- OutputSlotStringとInputSlotString追加
- FlexibleString追加
- ParameterContainerにstring追加
- CalcParameterにstringの処理追加
- ParameterTransitionにstringによる遷移追加
- UISetTextFromParameterにstringパラメータからのテキスト設定に対応

#### Arbor Editor

- OutputSlot/InputSlotをカスタマイズしたクラスを作成した際にArbor Editorにスロットが正常に表示されないのを修正


## [2.0.6] - 2016-11-07

### 修正

#### Arbor Editor

- ノードを削除したときのUndo/Redoを修正。
- StateBehaviourを削除した時のUndo/Redoを修正。


## [2.0.5] - 2016-10-13

### 変更

#### 組み込み挙動

- Tween系のパラメータを演算ノードから受け取れるように変更。

### 修正

#### Arbor Editor

- 演算ノードやステート挙動を追加した時に、ArborEditorウィンドウを再描画するように修正。
- RectUtilityをArborEditor名前空間に修正。
- Boo用テンプレート修正。


## [2.0.4] - 2016-09-24

### 追加

#### Arbor Editor

- Stateに初めて入った際にOnStateAwake()を呼ぶように追加。

### 修正

#### Arbor Editor

- 遷移矢印を右クリックすると遷移先に移動できるメニューをMacでのcontrol+クリックでも表示するように修正。
- Unity5.5.0Betaでの警告とエラー修正。


## [2.0.3] - 2016-07-16

### 修正

#### Arbor Editor

- Unity5.4.0Betaで警告が出るのを修正
- ステートのペーストや複製時にマウスの位置にステートが生成されないのを修正


## [2.0.2] - 2016-07-13

### 追加

#### 組み込み挙動

- Scene/LoadLevelにAdditiveプロパティを設定できるように対応。
- Scene/UnloadLevel追加(Unity5.2以降対応)。

### 修正

#### Arbor Editor

- Unity5.3.0以降のUnityエディタ上でArborFSMオブジェクトを選択したままプレイ開始するとStateBehaviourが削除されてしまうのを修正。

#### 組み込み挙動

- Unity5.3.0以降のScene/LoadLevelにてApplication.LoadLevelの警告が出るのを修正。


## [2.0.1] - 2016-07-01

### 追加

#### 組み込み挙動

- Audio/PlaySoundAtTransformにAudioMixerGroupとSpatialBlendの指定を追加。
- 新たに座標指定のAudio/PlaySoundAtPointを追加。

### 変更

#### Arbor Editor

- ヒープメモリの使用量を削減。

#### 組み込み挙動

- Audio/PlaySoundAtPointをAudio/PlaySoundAtTransformに改名。

### 修正

#### Arbor Editor

- コンパイルするたびにエディタ管理用オブジェクトが増えていたのを修正。


## [2.0.0] - 2015-10-16

### 追加

#### Arbor Editor

- 演算ノード追加。
- ParameterContainerでVector2を保持できるように対応。
- ParameterContainerでVector3を保持できるように対応。
- ParameterContainerでQuaternionを保持できるように対応。
- ParameterContainerでRectを保持できるように対応。
- ParameterContainerでBoundsを保持できるように対応。
- ParameterContainerでTransformを保持できるように対応。
- ParameterContainerでRectTransformを保持できるように対応。
- ParameterContainerでRigidbodyを保持できるように対応。
- ParameterContainerでRigidbody2Dを保持できるように対応。

#### 組み込み挙動

- Transition/Physics/RaycastTransition
- Transition/Physics2D/Raycast2DTransition
- Transition/CalculatorTransition
- InstantiateGameObjectに生成したGameObjectの出力を追加
- OnCollisionEnterTransitionに当たった相手のCollisionの出力を追加
- OnCollisionExitTransitionに当たった相手のCollisionの出力を追加
- OnCollisionStayTransitionに当たった相手のCollisionの出力を追加
- OnTriggerEnterTransitionに当たった相手のColliderの出力を追加
- OnTriggerExitTransitionに当たった相手のColliderの出力を追加
- OnTriggerStayTransitionに当たった相手のColliderの出力を追加
- OnCollisionEnter2DTransitionに当たった相手のCollision2Dの出力を追加
- OnCollisionExit2DTransitionに当たった相手のCollision2Dの出力を追加
- OnCollisionStayT2Dransitionに当たった相手のCollision2Dの出力を追加
- OnTriggerEnter2DTransitionに当たった相手のCollider2Dの出力を追加
- OnTriggerExit2DTransitionに当たった相手のCollider2Dの出力を追加
- OnTriggerStayT2Dransitionに当たった相手のCollider2Dの出力を追加

#### 組み込み演算

- BoolのCalculator追加
- BoundsのCalculator追加
- ColliderのCalculator追加
- Collider2DのCalculator追加
- CollisionのCalculator追加
- Collision2DのCalculator追加
- ComponentのCalculator追加
- FloatのCalculator追加
- IntのCalculator追加
- MathfのCalculator追加
- QuaternionのCalculator追加
- RaycastHitのCalculator追加
- RaycastHit2DのCalculator追加
- RectのCalculator追加
- RectTransformのCalculator追加
- RigidbodyのCalculator追加
- Rigidbody2DのCalculator追加
- TransformのCalculator追加
- Vector2のCalculator追加
- Vector3のCalculator追加

#### スクリプト

- FlexibleBounds実装
- FlexibleQuaternion実装
- FlexibleRect実装
- FlexibleRectTransform実装
- FlexibleRigidbody実装
- FlexibleRigidbody2D実装
- FlexibleTransform実装
- FlexibleVector2実装
- FlexibleVector3実装

### 変更

#### 組み込み挙動

- AgentEscapeをFlexibleTransformに対応。
- AgentFllowをFlexibleTransformに対応。
- PlaySoundAtPointをFlexibleTransformに対応。
- InstantiateGameObjectをFlexibleTransformに対応。
- LookAtGameObjectをFlexibleTransformに対応。
- AddForceRigidbodyをFlexibleRigidbodyに対応。
- AddVelocityRigidbodyをFlexibleRigidbodyに対応。
- SetVelocityRigidbodyをFlexibleRigidbodyに対応。
- AddForceRigidbody2DをFlexibleRigidbody2Dに対応。
- AddVelocityRigidbody2DをFlexibleRigidbody2Dに対応。
- SetVelocityRigidbody2DをFlexibleRigidbody2Dに対応。


## [1.7.7p2] - 2015-09-30

### 修正

#### Arbor Editor

- Unity5.2.1以降でエラーが出るのを修正。


## [1.7.7p1] - 2015-09-29

### 修正

#### Arbor Editor

- ステートとコメントの作成と削除がUndoできなかったのを修正。


## [1.7.7] - 2015-09-19

### 追加

#### Arbor Editor

- ParameterContainerでGameObjectを保持できるように対応。

#### 組み込み挙動

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
- UITweenPositionに相対指定できるように追加。
- UITweenSizeに相対指定できるように追加。

#### スクリプト

- FlexibleInt実装
- FlexibleFloat実装
- FlexibleBool実装
- FlexibleGameObject実装
- ContextMenuを使えるように対応。

### 変更

#### Arbor Editor

- 自分自身のステートへ遷移できるように変更。
- 挙動の背景を変更。
- ListGUIの背景を変更。
- コメントノードを内容によってリサイズするように変更。
- グリッドなどの設定をプロジェクトごとではなくUnityのメジャーバージョンごとに保存するように変更。

#### 組み込み挙動

- BroadcastMessageGameObjectの値をFlexibleIntなどを使用するように対応。
- CalcAnimatorParameterの値をFlexibleIntなどを使用するように対応。
- CalcParameterの値をFlexibleIntなどを使用するように対応。
- ParameterTransitionの値をFlexibleIntなどを使用するように対応。
- SendMessageGameObjectの値をFlexibleIntなどを使用するように対応。
- SendMessageUpwardsGameObjectの値をFlexibleIntなどを使用するように対応。
- AgentEscapeをArborGameObjectに対応。
- AgentFllowをArborGameObjectに対応。
- ActivateGameObjectをFlexibleGameObjectに対応。
- BroadastMessageGameObjectをFlexibleGameObjectに対応。
- DestroyGameObjectをFlexibleGameObjectに対応。
- LookatGameObjectをFlexibleGameObjectに対応。
- SendMessageGameObjectをFlexibleGameObjectに対応。
- SendMessageUpwardsGameObjectをFlexibleGameObjectに対応。
- BroadcastTriggerをFlexibleGameObjectに対応。
- SendTriggerGameObjectをFlexibleGameObjectに対応。
- SendTriggerUpwardsをFlexibleGameObjectに対応。
- InstantiateGameObjectで生成したオブジェクトをパラメータに格納できるように対応。

#### その他

- Parameter関連をCoreフォルダとInternalフォルダに移動。
- コンポーネントにアイコン設定。

### 修正

#### Arbor Editor

- Undo周りのバグ修正
- 常駐ステートが開始ステートに設定できたのを修正。


## [1.7.6] - 2015-09-17

### 追加

#### Arbor Editor

- StateLinkに名前設定追加。
- StateLinkに即時遷移フラグ追加。

#### コンポーネント

- GlobalParameterContainer

#### 組み込み挙動

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
- TimeTransitionに現在時間をプログレスバーで表示するように追加。
- Tween終了時に遷移できるように追加。
- TweenPositionに相対指定できるように追加。
- TweenRotationに相対指定できるように追加。
- TweenScaleに相対指定できるように追加。
- TweenRigidbodyPositionに相対指定できるように追加。
- TweenRigidbodyRotationに相対指定できるように追加。

#### スクリプト

- FixedImmediateTransition属性で即時遷移フラグを変更できないように対応。

#### Example

- Example9としてGlobalParameterContainerのサンプル追加。

### 変更

#### 組み込み挙動

- SetRigidbodyVelocityをSetVelocityRigidbodyに改名。
- SetRigidbody2DVelocityをSetVelocityRigidbody2Dに改名。

### 改善

#### Arbor Editor

- 挙動追加を開いた際、検索バーにフォーカスが移るように改善。
- 挙動追加での並び順で、グループが先に来るように調整。

### 修正

#### Arbor Editor

- 挙動追加での検索文字列が保存できていなかったのを修正。

#### 組み込み挙動

- OnTriggerExit2DDestroyがCollisionにあったのを修正。
- CalcAnimatorParameterのfloatValueがintになっていたのを修正。
- CalcParameterのfloatValueがintになっていたのを修正。
- ParameterTransitionのfloatValueがintになっていたのを修正。

#### Example

- TagsにCoinが追加されていたので修正。


## [1.7.5] - 2015-09-03

### 追加

#### 組み込み挙動

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
- InstantiateGameObjectで生成時の初期Transformを指定できるように追加。

#### スクリプト

- Parameterにvalueプロパティ追加。
- IntParameterReference追加。
- FloatParameterReference追加。
- BoolParameterReference追加。

#### Example

- Example7としてコインプッシャーゲーム追加。
- Example8としてEventSystemのサンプル追加。

#### その他

- HierarchyのCreateボタンからArborFSM付きGameObjectを作れるように追加。
- HierarchyのCreateボタンからParameterContainer付きGameObjectを作れるように追加。
- HierarchyのCreateボタンからAgentController付きGameObjectを作れるように追加。

### 変更

#### その他

- フォルダ整理。

### 改善

#### Arbor Editor

- ステートリストの横幅をリサイズできるように対応。

### 修正

#### Arbor Editor

- グリッドが正しく表示されない時があるのを修正。

#### 組み込み挙動

- CalcParameterでBool型の場合に正しく動作しなかったのを修正。
- SendEventGameObjectで呼び出す方をわざわざ指定しないように修正。


## [1.7.4] - 2015-08-25

### 追加

#### 組み込み挙動

- Agent系Behaviour追加。
- uGUI系Behaviour追加。
- uGUI系Tween追加。
- SendEventGameObject追加。
- SendMessageGameObjectに値渡し機能追加。

### 変更

#### その他

- uGUI対応に伴いUnity最低動作バージョンを4.6.7f1に引き上げ。

### 修正

#### Arbor Editor

- AnimatorParameterReferenceの参照先がAnimatorControllerを参照していなかったときにエラーが出るのを修正。


## [1.7.3] - 2015-08-21

### 追加

- OnMouse系Transition追加

### 変更

- ステートリストを名前順でソートするように変更。
- Arbor Editorの左上方向へも無限にステートを配置できるように変更。
- マニュアルサイトを一新。

### 修正

- 選択ステートへの移動時のスクロール位置修正


## [1.7.2] - 2015-08-18

### 追加

- ArborEditorにコメントノードを追加。
- 挙動追加時に検索できるように対応。
- CalcAnimatorParameter追加。
- AnimatorStateTransition追加。
- 遷移線を右クリックで遷移元と遷移先へ移動できるように追加。

### 変更

- ForceTransitionをGoToTransitionに改名。
- 挙動追加で表示される組み込みBehaviourの名前を省略しないように変更。
- 組み込みBehaviourをAdd Componentに表示しないように変更。

### 修正

- Prefab元に挙動追加するとPrefab先に正しく追加されないのを修正。


## [1.7.1] - 2015-08-11

### 追加

- ステートリストを追加。
- ParamneterReferenceのPropertyDrawerを追加。
- 要素の削除ができるリスト用のGUI、ListGUIを追加。

### 修正

- CalcParameterのboolValueがintになっていたのを修正。


## [1.7.0] - 2015-08-11

### 追加

- パラメータコンテナ。

### 修正

- OnStateBegin()で状態遷移した場合、それより下のBehaviourを実行しないように修正。


## [1.6.3f1] - 2015-07-29

### 変更

- Unity5 RC3対応により、OnStateEnter/OnStateExitをOnStateBegin/OnStateEndに改名。

### 修正

- Unity5 RC3でエラーが出るのを修正。


## [1.6.3] - 2015-02-20

### 追加

- Transitionにforceフラグを追加。trueにすると呼び出し時にその場で遷移するようにできる。
- ソースコードへドキュメントコメント埋め込み。
  Player Settings の Scripting Define Symbols に ARBOR_DOC_JA を追加すると日本語でドキュメントコメントが見れるようになります。
- スクリプトリファレンスをAssets/Arbor/Docsに配置。
  解凍してindex.htmlを開いてください。


## [1.6.2] - 2014-08-15

### 修正

- OnStateEnterでステート遷移できないのを修正。


## [1.6.1] - 2014-08-07

### 修正

- Mac環境でGridボタン押すとエラーが表示される。


## [1.6] - 2014-07-08

### 追加

- 常駐ステート。
- 多言語対応。
- ArborFSMに名前を付けられるように対応。

### 修正

- グリッドサイズを変更してもスナップ間隔に反映されない。
- ArborFSMのコンポーネントをコピー＆ペーストした際にStateBehaviourが消失する問題の対処。
- SendTriggerを現在有効なステートにのみ送るように変更。
- ArborFSMを無効にしてもStateBehaviourが動き続ける。


## [1.5] -  2014-06-25

### 追加

- ステートの複数選択に対応。
- ショートカットキーに対応。
- グリッド表示対応。


### 修正

- Behaviour追加時にデフォルトで広げた状態にする。
- StateLinkのドラッグ中にステートへのマウスオーバーがずれて反応する。
 

## [1.4] - 2014-06-21

### 追加

- Tween系Behaviour追加。
 - Tween / Color
 - Tween / Position
 - Tween / Rotation
 - Tween / Scale
- Add Behaviourに表示されないようにするHideBehaviour属性追加。
- Behaviourのヘルプボタンから組み込みBehaviourのオンラインヘルプ表示。


## [1.3] - 2014-06-18

### 追加

- 組み込みBehaviour追加。
 - Audio / PlaySoundAtPoint
 - GameObject / SendMessage
 - Scene / LoadLevel
 - Transition / Force
- シーンを跨いだコピー&ペースト。

### 修正

- Stateをコピーしたあとシーンを保存するとメモリリークの警告が表示される。
- StateLinkの接続ドラッグ中に画面スクロールすると矢印が残る。


## [1.2] - 2014-06-07

### 追加

- StateBehaviourの有効チェックボックス。

### 修正

- Arbor Editorの最大化を解除するとエラーが出る。
- 生成したC#スクリプトを編集すると改行コードの警告が出る。


## [1.1] - 2014-05-30

### 追加

- JavaScriptとBooのスクリプト生成。
- Stateのコピー＆ペースト。
- StateBehaviourのコピー＆ペースト。

### 修正

- スクリプトがMissingになったときの対応。
- StateLinkの配列が表示されないのを修正。


## [1.0.1] - 2014-05-27

### 修正

- Unity4.5でのエラー。
- エディタ上での実行時にArbor Editorが再描画されない。
- ArborFSMのInspector拡張のクラス名。


## [1.0] - 2014-05-21

- 初公開