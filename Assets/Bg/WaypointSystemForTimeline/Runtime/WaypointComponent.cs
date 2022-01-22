using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bg.WaypointSystemForTimeline {
    public class WaypointComponent : MonoBehaviour {
        [Serializable]
        public class Point {
            public Vector3 position;
            public Vector3 backTangent;
            public Vector3 nextTangent;
            [Range(0f, 1f)] public float t = 0f;
        }

        [SerializeField] public GameObject target;
        [SerializeField] private bool _isLookTangent = false;
        [SerializeField, HideInInspector] private List<Point> _wayPoints = new List<Point>();
        
        public IReadOnlyList<Point> wayPoints => _wayPoints;

        public void SetPosition(float t) {
            if (target == null) {
                return;
            }
            Vector3 p = GetPoint(t);
            target.transform.position = p;
            if (_isLookTangent) {
                Vector3 tangent = CalcBezierTangent(t);
                target.transform.LookAt(p + tangent);
            }
        }

        public void CalcTInWayPoints() {
            if (wayPoints.Count < 2) {
                return;
            }

            // トータル距離を計算
            List<float> distanceList = new List<float>();
            float totalDistance = 0f;
            for (int i = 0; i < wayPoints.Count - 1; i++) {
                var point = wayPoints[i];
                var nextPoint = wayPoints[i + 1];
                float distance = CalcDistanceBetweenPoints(point, nextPoint);
                distanceList.Add(distance);
                totalDistance += distance;
            }

            // 一つ目のtは0
            _wayPoints[0].t = 0f;

            // tを計算して設定
            for (int i = 1; i < wayPoints.Count - 1; i++) {
                var point = _wayPoints[i];
                var backPoint = _wayPoints[i - 1];
                float t = distanceList[i - 1] / totalDistance;
                t = Mathf.Min(t, 1.0f);
                point.t = backPoint.t + t;
            }

            // 最後のtは1
            _wayPoints[_wayPoints.Count - 1].t = 1f;
        }

        private float CalcDistanceBetweenPoints(Point a, Point b, float delta = 0.005f) {
            float distance = 0f;

            Vector3 start = transform.position + a.position;
            Vector3 end = transform.position + b.position;
            Vector3 startTan = start + a.nextTangent;
            Vector3 endTan = end + b.backTangent;

            float t = 0f;
            while (t < 1f) {
                Vector3 p1 = CalcBezierPoint(start, end, startTan, endTan, t);
                t += delta;
                Vector3 p2 = CalcBezierPoint(start, end, startTan, endTan, t);
                distance += Vector3.Distance(p1, p2);
            }

            return distance;
        }

        public Vector3 GetPoint(float t) {
            if (wayPoints.Count <= 0) {
                return transform.position;
            }

            if (wayPoints.Count < 2) {
                return wayPoints[0].position;
            }

            Point startPoint = null;
            Point endPoint = null;
            for (int i = 0; i < wayPoints.Count - 1; i++) {
                var point = wayPoints[i];
                var nextPoint = wayPoints[i + 1];
                if (point.t <= t && t <= nextPoint.t) {
                    startPoint = point;
                    endPoint = nextPoint;
                    break;
                }
            }

            if (startPoint == null) {
                return transform.position;
            }

            float tNorm = (t - startPoint.t) / (endPoint.t - startPoint.t);

            Vector3 start = transform.position + startPoint.position;
            Vector3 end = transform.position + endPoint.position;
            Vector3 startTan = start + startPoint.nextTangent;
            Vector3 endTan = end + endPoint.backTangent;
            return CalcBezierPoint(start, end, startTan, endTan, tNorm);
        }

        public Vector3 CalcBezierTangent(float t, float delta = 0.005f) {
            var point = GetPoint(t);
            var deltaPoint = GetPoint(Mathf.Clamp(t + delta, 0f, 1f));
            var tangent = deltaPoint - point;
            return tangent.normalized;
        }

        public static Vector3 CalcBezierPoint(Vector3 start, Vector3 end, Vector3 startTan, Vector3 endTan, float t) {
            Vector3 m = Vector3.Lerp(startTan, endTan, t);
            if (start == startTan && end == endTan) {
                // Liner
                return m;
            }
            
            Vector3 s = start;
            if (start != startTan) {
                s = Vector3.Lerp(start, startTan, t);
            }
            Vector3 e = end;
            if (e != endTan) {
                e = Vector3.Lerp(endTan, end, t);
            }
            
            if (start == startTan) {
                m = Vector3.Lerp(endTan, end, t);
            } 
            else if (end == endTan) {
                m = Vector3.Lerp(start, startTan, t);
            }
            
            Vector3 p1 = Vector3.Lerp(s, m, t);
            Vector3 p2 = Vector3.Lerp(m, e, t);

            return Vector3.Lerp(p1, p2, t);
        }
    }
}
