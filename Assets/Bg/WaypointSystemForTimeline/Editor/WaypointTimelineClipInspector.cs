using UnityEditor;
using UnityEngine.Timeline;

namespace Bg.WaypointSystemForTimeline.Editor {
    [CustomEditor(typeof(WaypointTimelineClip))]
    public class WayPointTimelineClipInspector : UnityEditor.Editor {
        protected SerializedProperty _templateProp = null;

        private TimelineClip _timelineClip;
    
        public void OnEnable() {
            _templateProp = serializedObject.FindProperty("template");
            _timelineClip = FindTimelineClip(target as WaypointTimelineClip);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_templateProp);
            serializedObject.ApplyModifiedProperties();
        }

        protected TimelineClip FindTimelineClip(WaypointTimelineClip targetClip) {
            string[] guids = AssetDatabase.FindAssets("t:TimelineAsset");
            foreach (string guid in guids) {
                TimelineAsset timeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(TimelineAsset));
                foreach (var track in timeline.GetOutputTracks()) {
                    foreach (var clip in track.GetClips()) {
                        if (clip.asset.GetType() == typeof(WaypointTimelineClip) && object.ReferenceEquals(clip.asset, targetClip)) {
                            return clip;
                        }
                    }
                }
            }
            return null;
        }
    }
}