using UnityEngine;

public class Dummy : MonoBehaviour, IMissileChase
{
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}