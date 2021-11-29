using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Bg.WaypointSystemForTimeline {
    [Serializable]
    public class WaypointTimelineClip :  PlayableAsset, ITimelineClipAsset {
        public WaypointTimelinePlayableBehaviour template = new WaypointTimelinePlayableBehaviour();
    
        public ClipCaps clipCaps => ClipCaps.SpeedMultiplier;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<WaypointTimelinePlayableBehaviour>.Create(graph, template);
            return playable;
        }
    }
}