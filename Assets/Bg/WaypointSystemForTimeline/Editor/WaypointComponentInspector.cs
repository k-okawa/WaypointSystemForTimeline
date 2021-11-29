using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bg.WaypointSystemForTimeline.Editor {
    [CustomEditor(typeof(WaypointComponent))]
    public class WaypointComponentInspector : UnityEditor.Editor {
        public enum PointEditMode {
            POSITION,
            BACK_TANGENT,
            NEXT_TANGENT
        }
        
        private int _currentEditIndex = -1;
        private PointEditMode _currentPointEditMode = PointEditMode.POSITION;
        private readonly Vector3 _boxSize = new Vector3(0.1f, 0.1f, 0.1f);
        
        private SerializedObject _serializedObject;
        private SerializedProperty _wayPointsProperty;
    
        public void OnEnable() {
            var component = target as WaypointComponent;
            _serializedObject = new SerializedObject(component);
            _wayPointsProperty = _serializedObject.FindProperty("_wayPoints");
    
            SceneView.duringSceneGui += OnSceneGUI;
        }
    
        public void OnDisable() {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    
        void OnSceneGUI(SceneView sceneView) {
            _serializedObject.Update();
            
            var component = target as WaypointComponent;
            var transform = component.transform;
            
            // ベジェ曲線描画
            if (component.wayPoints.Count > 1) {
                for (int i = 0; i < component.wayPoints.Count - 1; i++) {
                    var point = component.wayPoints[i];
                    var nextPoint = component.wayPoints[i + 1];
                    var start = transform.position + point.position;
                    var end = transform.position + nextPoint.position;
                    var startTan = start + point.nextTangent;
                    var endTan = end + nextPoint.backTangent;
                    Handles.DrawBezier(start, end, startTan, endTan, Color.red, null, 2f);
                }
            }
            
            // Cube描画・ハンドル
            for (int i = 0; i < component.wayPoints.Count; i++) {
                var point = component.wayPoints[i];
                var start = transform.position + point.position;
                GUIStyle textStyle = new GUIStyle();
                textStyle.normal.textColor = Color.cyan;
                Handles.Label(start, $"Index:{i}", textStyle);
                Handles.DrawWireCube(start, _boxSize);
                if (i == _currentEditIndex) {
                    switch (_currentPointEditMode) {
                        case PointEditMode.POSITION: {
                            var offset = transform.position;
                            var p = offset + point.position;
                            p = Handles.PositionHandle(p, Quaternion.identity);
                            _wayPointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector3Value = p - offset;
                            break;
                        }
                        case PointEditMode.BACK_TANGENT: {
                            var offset = transform.position + point.position;
                            var p = offset + point.backTangent;
                            Handles.DrawBezier(offset, p, offset, p, Color.yellow, null, 2f);
                            p = Handles.PositionHandle(p, Quaternion.identity);
                            _wayPointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("backTangent").vector3Value = p - offset;
                            break;
                        }
                        case PointEditMode.NEXT_TANGENT: {
                            var offset = transform.position + point.position;
                            var p = offset + point.nextTangent;
                            Handles.DrawBezier(offset, p, offset, p, Color.yellow, null, 2f);
                            p = Handles.PositionHandle(p, Quaternion.identity);
                            _wayPointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("nextTangent").vector3Value = p - offset;
                            break;
                        }
                    }
                    _serializedObject.ApplyModifiedProperties();
                }
            }
    
            // クリックイベント
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && (MouseButton) currentEvent.button == MouseButton.LeftMouse) {
                for (int i = 0; i < component.wayPoints.Count; i++) {
                    var point = component.wayPoints[i];
                    var bounds = new Bounds(point.position, _boxSize);
                    Ray sceneRay = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
                    if (bounds.IntersectRay(sceneRay)) {
                        _currentEditIndex = i;
                        break;
                    }
                }
            }
        }
    
        public override void OnInspectorGUI() {
            var component = target as WaypointComponent;
            _serializedObject.Update();
            
            base.OnInspectorGUI();
            
            DrawWayPointsList();
    
            if (GUILayout.Button("Tを自動計算")) {
                component.CalcTInWayPoints();
                serializedObject.ApplyModifiedProperties();
            }
        }
    
        private ReorderableList _wayPointsReorderableList;
        private void DrawWayPointsList() {
            if (_wayPointsReorderableList == null) {
                _wayPointsReorderableList = new ReorderableList(_serializedObject, _wayPointsProperty);
    
                _wayPointsReorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "WayPoints");
                _wayPointsReorderableList.elementHeightCallback = index => 140f;
                _wayPointsReorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
                    var elementProp = _wayPointsProperty.GetArrayElementAtIndex(index);
                    var positionProp = elementProp.FindPropertyRelative("position");
                    var backTangentProp = elementProp.FindPropertyRelative("backTangent");
                    var nextTangentProp = elementProp.FindPropertyRelative("nextTangent");
                    var tProp = elementProp.FindPropertyRelative("t");
                    EditorGUI.LabelField(GetRowRect(rect, 0), $"Index{index}", EditorStyles.boldLabel);
                    EditorGUI.PropertyField(GetRowRect(rect, 1), positionProp, new GUIContent("    Position", "GameObjectの位置からのオフセット"));
                    EditorGUI.PropertyField(GetRowRect(rect, 2), backTangentProp, new GUIContent("    Back Tangent", "Positionからのオフセット"));
                    EditorGUI.PropertyField(GetRowRect(rect, 3), nextTangentProp, new GUIContent("    NextTangent", "Positionからのオフセット"));
                    EditorGUI.PropertyField(GetRowRect(rect, 4), tProp, new GUIContent("    T", "補間の目安になる値"));
                    EditorGUI.LabelField(GetRowRect(rect, 5), $"編集", EditorStyles.boldLabel);
                    var buttonRect = GetRowRect(rect, 6);
                    float buttonWidth = rect.width / 3f;
                    var leftRect = new Rect(buttonRect.x, buttonRect.y, buttonWidth, buttonRect.height);
                    var centerRect = new Rect(buttonRect.x + buttonWidth, buttonRect.y, buttonWidth, buttonRect.height);
                    var rightRect = new Rect(buttonRect.x + buttonWidth * 2f, buttonRect.y, buttonWidth, buttonRect.height);
                    if (GUI.Button(leftRect, "<-")) {
                        _currentEditIndex = index;
                        _currentPointEditMode = PointEditMode.BACK_TANGENT;
                        SceneView.RepaintAll();
                    }
                    if (GUI.Button(centerRect, "Position")) {
                        _currentEditIndex = index;
                        _currentPointEditMode = PointEditMode.POSITION;
                        SceneView.RepaintAll();
                    }
                    if (GUI.Button(rightRect, "->")) {
                        _currentEditIndex = index;
                        _currentPointEditMode = PointEditMode.NEXT_TANGENT;
                        SceneView.RepaintAll();
                    }
                };
            }
            _wayPointsReorderableList.DoLayoutList();
    
            _serializedObject.ApplyModifiedProperties();
        }
    
        private Rect GetRowRect(Rect rect, int index, int height = 20) {
            Rect ret = new Rect(rect);
            ret.yMin += height * index;
            ret.height = height;
            return ret;
        }
    }
}
