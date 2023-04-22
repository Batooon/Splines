using UnityEditor;
using UnityEngine;

namespace Curves
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField] private Transform _p0;
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _p3;
        [SerializeField] private Transform _projectile;
        [SerializeField, Range(0, 1)] private float _t;

        private void OnDrawGizmos()
        {
            if (_p0 == null || _p1 == null || _p2 == null || _p3 == null || _projectile == null)
                return;
            // Debug.Log($"P1 offset from P0: {_p1.position - _p0.position}");
            // Debug.Log($"P2 offset from P3: {_p3.position - _p2.position}");
            Handles.DrawAAPolyLine(1f, _p0.position, _p1.position);
            Handles.DrawAAPolyLine(1f, _p1.position, _p2.position);
            Handles.DrawAAPolyLine(1f, _p2.position, _p3.position);
            var A = Vector3.Lerp(_p0.position, _p1.position, _t);
            var B = Vector3.Lerp(_p1.position, _p2.position, _t);
            var C = Vector3.Lerp(_p2.position, _p3.position, _t);
            Handles.DrawAAPolyLine(1f, A, B);
            Handles.DrawAAPolyLine(1f, B, C);
            var D = Vector3.Lerp(A, B, _t);
            var E = Vector3.Lerp(B, C, _t);
            Handles.DrawAAPolyLine(1f, D, E);
            var P = Vector3.Lerp(D, E, _t);
            Gizmos.DrawSphere(P, .3f);
            Handles.DrawBezier(_p0.position, _p3.position, _p1.position, _p2.position, Color.blue,
                Texture2D.whiteTexture, 1f);
            _projectile.localPosition = P;
            _projectile.localRotation = Quaternion.LookRotation(GetVelocity().normalized);
        }

        private Vector3 GetVelocity()
        {
            var velocity = _p0.position * (-3 * _t * _t + 6 * _t - 3) +
                           _p1.position * (9 * _t * _t - 12 * _t + 3) +
                           _p2.position * (-9 * _t * _t + 6 * _t) +
                           _p3.position * (3 * _t * _t);
            return velocity;
        }
    }
}
