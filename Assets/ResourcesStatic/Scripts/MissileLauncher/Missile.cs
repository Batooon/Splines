using System;
using UnityEditor;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _attackDuration;
    [SerializeField] private Vector3 _firstTangentOffset = new (4.36f, 5.43f, 0.67f);
    [SerializeField] private Vector3 _secondTangentOffset = new (16.59f, -3.40f, 3.50f);
    private IMissileChase _target;
    private Vector3 _finishHitPosition;
    private Vector3 _startingPosition;
    private Vector3 _firstTangent;
    private Vector3 _secondTangent;
    private float _timeElapsed;

    private bool _chasing;
    
    public void Chase(IMissileChase target)
    {
        _target = target;
        _finishHitPosition = target.GetWorldPosition();
        _startingPosition = transform.position;
        _firstTangent = _startingPosition + _firstTangentOffset;
        _secondTangent = _finishHitPosition - _secondTangentOffset;
        _timeElapsed = 0f;
        _chasing = true;
    }

    private void Update()
    {
        if (_chasing == false)
            return;

        if (_timeElapsed < _attackDuration)
        {
            var t = _timeElapsed / _attackDuration;
            transform.position = GetPosition(t);
            transform.rotation = Quaternion.LookRotation(GetVelocity(t).normalized);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.position = GetPosition(1f);
            transform.rotation = Quaternion.LookRotation(GetVelocity(1).normalized);
            _chasing = false;
            Destroy(gameObject);
        }
    }

    private Vector3 GetPosition(float t)
    {
        var position = _startingPosition * (-t * t * t + 3 * t * t - 3 * t + 1) +
                       _firstTangent * (3 * t * t * t - 6 * t * t + 3 * t) +
                       _secondTangent * (-3 * t * t * t + 3 * t * t) +
                       _finishHitPosition * (t * t * t);
        return position;
    }

    private Vector3 GetVelocity(float t)
    {
        var velocity = _startingPosition * (-3 * t * t + 6 * t - 3) +
                       _firstTangent * (9 * t * t - 12 * t + 3) +
                       _secondTangent * (-9 * t * t + 6 * t) +
                       _finishHitPosition * (3 * t * t);
        return velocity;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawBezier(_startingPosition, _finishHitPosition, _firstTangent, _secondTangent, Color.red,
            Texture2D.whiteTexture, 1f);
    }
}