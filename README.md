# WaypointSystemForTimeline

Timeline上で軌跡(ベジェ曲線)の補完を行うことが可能です。

![Screen Shot 2021-11-29 at 11 28 30](https://user-images.githubusercontent.com/49301086/143799354-433e3214-bd28-4a22-a105-77f4bce7dc58.png)

## インストール
### PackageManager
Window/Package Managerを開き、add package from git URL...で以下を入力して追加してください。

```
https://github.com/k-okawa/WaypointSystemForTimeline.git?path=Assets/Bg/WaypointSystemForTimeline
```
## 使い方

![Screen Shot 2021-11-29 at 11 31 40](https://user-images.githubusercontent.com/49301086/143799565-1bacbc80-30b5-4d54-a6bf-811585a45231.png)

### 1.WaypointComponentを空オブジェクトにAddComponentする

### 2.Targetに移動させたいGameObjectを設定
IsLookTangentにチェックを入れることで、GameObjectの向きがベジェ曲線の接線方向になります。

### 3.WayPointsを編集
+,-で追加と削除を行うことができます。

|プロパティ名|説明|
|----------|---|
|Position|Pointの位置|
|BackTangent|前のポイントに対してのベジェ曲線制御点|
|NextTangent|次のポイントに対してのベジェ曲線制御点|
|T(0.0~1.0)|補完する際の目安になる点。例えば上の画像の場合0.4で補間の計算をする場合Index1,Index2の間で補間の計算がされる|

プロパティの値を直接入力して編集することができますが、以下のボタンを押下することでそれぞれのプロパティがScene上で編集可能になります。

※位置編集ツールを選択している状態で押下すると親オブジェクトの位置編集ツールと被る場合があります。

|ボタン名|説明|
|----------|---|
|<-|BackTangentの位置を編集|
|Position|Positionの位置を編集|
|->|NextTangentの位置を編集|

またInspectorをロックしている状態で、Scene上のPointの位置をクリックするとPositionの編集が可能です。

### 4.補間値の自動計算
手動でTの値を入力しても大丈夫ですが、Tを自動計算を押下すると曲線の長さに応じて均等に割り振られます。
