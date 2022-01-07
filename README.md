# WaypointSystemForTimeline
[[日本語](https://github.com/k-okawa/WaypointSystemForTimeline/blob/master/README.ja.md)]

[![openupm](https://img.shields.io/npm/v/com.littlebigfun.addressable-importer?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.littlebigfun.addressable-importer/)


This package is possible to complement the track (Bezier curve) on the Timeline.

![Screen Shot 2021-11-29 at 11 28 30](https://user-images.githubusercontent.com/49301086/143799354-433e3214-bd28-4a22-a105-77f4bce7dc58.png)

## Installation
### PackageManager
#### Install via git url
Open Window/Package Manager, and add package from git URL...

```
https://github.com/k-okawa/WaypointSystemForTimeline.git?path=Assets/Bg/WaypointSystemForTimeline
```

#### Install via OpenUPM

```
openupm add com.bg.waypointystemfortimeline
```

## How to use

![Screen Shot 2021-11-29 at 12 27 13](https://user-images.githubusercontent.com/49301086/143804109-548ffdcb-884b-422c-8b63-23f8c888840a.png)
### 1.Create empty GameObject and add WaypointComponent

### 2.Set GameObject that need to move to target
If GameObject need to look tangent direction, check IsLookTangent.

### 3.Edit Waypoints
"+" and "-" button enables adding and removing point.

|PropertyName|Description|
|----------|---|
|Position|Position of point|
|BackTangent|Bezier curve control point for back point|
|NextTangent|Bezier curve control point for next point|
|T(0.0~1.0)|value for interpolation. For example with above screen shot, when t is 0.4, interpolate between index1 and index2.|

You can edit property directly or on the scene using the buttons below.

If you press edit button(<-, Position, ->) while the position editing tool is selected, position handle may overlap with the position editing tool of the parent object.

|ButtonName|Description|
|----------|---|
|<-|Edit BackTangent position|
|Position|Edit Position|
|->|Edit NextTangent position|


Also, with the Inspector locked, you can edit the Position by clicking the position of the Point on the Scene.


### 4.Automatic calculation of interpolated value
You can enter the value of T manually, but if you press "Calculate T Automatically", it will be evenly distributed according to the length of the curve.

### 5.Timeline Settings
#### 5-1.Create Track

<img width="807" alt="Screen Shot 2021-11-29 at 12 32 24" src="https://user-images.githubusercontent.com/49301086/143804555-aa6f5f37-30d6-4bfe-8c38-1aa2c9e11f41.png">

#### 5-2.Set WaypointComponent to track reference

<img width="1791" alt="Screen Shot 2021-11-29 at 12 35 08" src="https://user-images.githubusercontent.com/49301086/143804802-d5c08b4f-b204-4f83-b94e-14048bc4d90c.png">

### 5-3.Create clip

<img width="751" alt="Screen Shot 2021-11-29 at 12 36 28" src="https://user-images.githubusercontent.com/49301086/143804856-1d341945-95ed-4044-874d-7ccdb5ec2a9e.png">
