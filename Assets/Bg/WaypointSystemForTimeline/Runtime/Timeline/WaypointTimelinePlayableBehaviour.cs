using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Bg.WaypointSystemForTimeline {
    [Serializable]
    public class WaypointTimelinePlayableBehaviour : PlayableBehaviour {
        // is stop animator controller when clip in
        [Tooltip("If GameObject has animator component enabled, waypoint system dose not work.")]
        [SerializeField] private bool _isStopAnimator = true;

        private WaypointComponent _wayPointComponent;
        private Animator _animator;
        
        public override void OnBehaviourPause (Playable playable, FrameData info) {
            EnableAnimator(true);
        }

        public override void OnGraphStop (Playable playable) {
            EnableAnimator(true);
        }
        
        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            EnableAnimator(false);
        }
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            if (playerData == null) {
                return;
            }
        
            _wayPointComponent = playerData as WaypointComponent;
            _animator = _wayPointComponent.GetComponent<Animator>();
            EnableAnimator(false);

            double t = playable.GetTime() / playable.GetDuration();
            _wayPointComponent.SetPosition((float)t);
        }

        private void EnableAnimator(bool isEnable) {
            if (!_isStopAnimator || _animator == null) return;
            _animator.enabled = isEnable;
        }
    }
}