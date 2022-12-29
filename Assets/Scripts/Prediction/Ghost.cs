using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
using PathCreation;

public class Ghost : MonoBehaviour
{
    public Animator _animator;
    public PathCreator _pathCreator;
    public float _distanceTreveled;
    public float _speed;
    public GameObject _visual;

    private void Awake()
    {
        GameEvents.OnSetGhostStatus += ChangeStatusOfGhost;
    }
    private void OnDestroy()
    {
        GameEvents.OnSetGhostStatus -= ChangeStatusOfGhost;
    }
    void Update()
    {
        Move();
    }
    private void Move()
    {
        _distanceTreveled += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTreveled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTreveled);
    }
    private void ChangeStatusOfGhost(bool status)
    { 
        if(!status)
        {
            _speed = 0;
            _animator.SetTrigger("Stop");
        }
    }

}
