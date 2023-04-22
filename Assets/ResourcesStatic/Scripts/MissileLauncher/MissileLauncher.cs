using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public interface IMissileChase
{
    Vector3 GetWorldPosition();
}

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private float _triggerRadius;
    [SerializeField] private float _initialAttackDelay;
    [SerializeField] private float _fireCooldown;
    [SerializeField] private Missile _missilePrefab;

    private IMissileChase _target;
    private bool _targetFound;

    private void Update()
    {
        if (_target == null)
        {
            var results = new Collider[15];
            var size = Physics.OverlapSphereNonAlloc(transform.localPosition, _triggerRadius, results);
            for (var i = 0; i < size; i++)
            {
                if (results[i].TryGetComponent(out IMissileChase chase))
                {
                    _target = chase;
                    StartCoroutine(TryFireMissile());
                    break;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _target.GetWorldPosition()) > _triggerRadius)
            {
                _target = null;
            }
        }
    }

    private IEnumerator TryFireMissile()
    {
        yield return new WaitForSeconds(_initialAttackDelay);
        while (_target != null)
        {
            var missile = Instantiate(_missilePrefab, transform.position, transform.rotation);
            missile.Chase(_target);
            yield return new WaitForSeconds(_fireCooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, _triggerRadius);
    }
}
