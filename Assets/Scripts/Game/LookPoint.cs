using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPoint : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private Transform _target;
    private bool _rotationPhase;

    void Update()
    {
        if (_rotationPhase)
        {
            var lookPos = _target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }

    }
    public void StartFollowToPrey(Transform position)
    {
        _target = position;
        _rotationPhase = true;
    }

    public void StopFollow()
    {
        _rotationPhase = false;
    }

}
