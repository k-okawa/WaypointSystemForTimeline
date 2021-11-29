using System;
using UnityEngine.Timeline;

namespace Bg.WaypointSystemForTimeline {
    [Serializable]
    [TrackClipType(typeof(WaypointTimelineClip))]
    [TrackBindingType(typeof(WaypointComponent))]
    public class WaypointTimelineTrack : PlayableTrack {
        
    }
}