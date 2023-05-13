-----------------------------------------------------
            Arbor 3: FSM & BT Graph Editor
          Copyright (c) 2014-2020 caitsithware
          https://caitsithware.com/wordpress/
          support@caitsithware.com
-----------------------------------------------------

Arborを購入いただきありがとうございます！

# 概要

Arborは、有限ステートマシン(以下FSM)とビヘイビアツリー(以下BT)に対応したグラフエディタアセットです。
FSMとBTの各ノードはカスタムスクリプトに対応しており、MonoBehaviourに似たスタイルでゲームロジックを記述できます。

## 有限ステートマシン(FSM)とは？

有限ステートマシンとは、ある状態での挙動と、その状態から別の状態へ遷移する仕組みです。
たとえば、スイッチと電灯を考えてみましょう。

* スイッチと電灯にはONとOFFという状態があり、スイッチをONにすれば電灯もONになります。
* スイッチは押せばONに切り替わり、再度押すとOFFに切り替わる挙動です。
* 電灯はONであれば明かりを灯す挙動になります。

このように、状態(電灯が点いているかどうか)と遷移条件（スイッチを切り替えたか）を明確化させるのに役立ちます。

## ビヘイビアツリー(BT)とは？

行動の優先度と行動を行う条件をセットで扱えるようにした挙動の木構造です。
たとえば、敵AIを考えてみましょう。

* プレイヤーに近づく挙動はプレイヤーとの距離が近ければ行う。
* それ以外の時は決まった経路を巡回する。
* プレイヤーに近づく挙動は決まった経路を移動する挙動に比べて優先度が高いと言えます。

このように、どの行動(巡回するかプレイヤーに近づくか)がある条件（プレイヤーとの距離）によって優先付けされているため、行動を整理するのに役立ちます。



# 主な利用方法

1. ArborFSMが付いたGameObjectの作成

	作成には以下の方法があります。
	* HierarchyのCreateボタンからArbor/ArborFSMを選択してGameObjectを作成。
	* 既に作成済みのGameObjectがある場合は、InspectorのAdd ComponentボタンからArbor/ArborFSMを選択。

2. Arbor Editorウィンドウを開く

	* ArborFSMのインスペクタにあるOpen Editorボタンをクリック。

3. ステート作成

	* Arbor Editorのグラフ内を右クリックし「ステート作成」を選択。

4. ステートの挙動を追加

	* 作成したステートのヘッダ部を右クリックもしくは歯車アイコンをクリックし、「挙動追加」を選択。
	  表示されたAddBehaviourMenuウィンドウで追加したい挙動を選択。

	  組み込みで追加されている挙動については、以下リファレンスページを参照してください。
	  https://arbor-docs.caitsithware.com/ja/

5. 挙動からの遷移を接続

	遷移接続のためのStateLinkクラスのフィールドを持つ挙動の場合は、ほかステートへの接続できます。
	* StateLinkフィールドをドラッグしほかステート上でドロップして接続。



# ドキュメント

## 同梱しているドキュメント

* Assets/Plugins/Arbor/readme-ja.txt
  このファイル
  基本的な概要や注意点など
* Assets/Plugins/Arbor/CHANGELOG-ja
  更新ログ

## 詳細ドキュメント

使用方法などに関する詳細なドキュメントは同梱しておりません。
オンラインドキュメントかダウンロードしたドキュメントを参照してください。

[オンラインドキュメント](https://arbor-docs.caitsithware.com/ja/)

### ドキュメントのダウンロード

#### Welcomeウィンドウからダウンロードする

* メニューから「Window > Arbor > Welcome」を選択。
* 「ドキュメント」欄の「Download Zip」ボタンをクリック。
* ダウンロード先のファイル名を指定して保存。
* ダウンロードしたファイルを解凍。
* 解凍したフォルダのindex.htmlをブラウザで開く。

#### ダウンロードページからダウンロードする

* [ダウンロードページ](https://arbor.caitsithware.com/download_reference/)をブラウザで開く。
* ダウンロードしたいドキュメントバージョンの「ダウンロード」リンクをクリックしてダウンロード。
* ダウンロードしたファイルを解凍。
* 解凍したフォルダのindex.htmlをブラウザで開く。

## その他のリンク

* 公式サイト : https://arbor.caitsithware.com/
  概要や新情報を記載。
* チュートリアル : https://arbor.caitsithware.com/tutorial/
  Arborの基本的な使い方について実際に手順を追いながら学習できます。



# サンプルシーン 

サンプルシーンはプロジェクト内の以下のフォルダにあります。
Assets/Plugins/Arbor/Examples/

各サンプルの詳細については、Examplesフォルダ下のreadme-ja.txtを参照してください。



# サポート

## フォーラム

質問や要望、不具合報告などはこちら:
https://forum-arbor.caitsithware.com/?language=ja

## メール

個別対応が必要なお問い合わせはこちら:
support@caitsithware.com



# アップデートガイド

Arborを更新する際は必ずお読みください。

## 更新手順

1. 更新前に必ずプロジェクトのバックアップを取ってください。
2. 既存のシーンを開いている場合は、メニューの「File / New Scene」からシーンを新規作成しておきます。
3. Arbor　Editorウィンドウを開いている場合は一旦閉じておきます。
4. 既にインポートされているArborフォルダを削除。
5. Arborの新バージョンをインポート。

## 各バージョンの更新ガイド

Arborの各バージョン別の更新ガイドは以下ページを参照してください。

https://caitsithware.com/assets/arbor/docs/ja/manual/updateguide.html



# MadeWithArbor3ロゴアセット

Arbor3利用作品に掲載する用途のロゴアセットをご用意いたしました。

掲載は任意ですが、もしご厚意で掲載していただける場合は、以下ページよりダウンロードしてご利用ください。

https://arbor.caitsithware.com/madewitharbo3-logo-assets/
