using System;
using UnityEngine.Playables;

namespace Bg.WaypointSystemForTimeline {
    [Serializable]
    public class WaypointTimelinePlayableBehaviour : PlayableBehaviour {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            if (playerData == null) {
                return;
            }
        
            var wayPointsComponent = playerData as WaypointComponent;

            double t = playable.GetTime() / playable.GetDuration();
            wayPointsComponent.SetPosition((float)t);
        }
    }
}