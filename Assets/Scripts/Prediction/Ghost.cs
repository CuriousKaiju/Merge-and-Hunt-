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
    public GameObject _magicBuff;
    public Camera _camera;
    public GameObject _arrow;


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
        _arrow.transform.LookAt(_arrow.transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
    }
    private void Move()
    {
        _distanceTreveled += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTreveled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTreveled);
    }
    private void ChangeStatusOfGhost(bool status)
    {
        if (!status)
        {
            _speed = 0;
            _animator.SetTrigger("Stop");
        }
    }

    public void ActivateParticles()
    {
        _magicBuff.SetActive(true);
        _magicBuff.GetComponent<ParticleSystem>().Play();
    }
    public void CloseGhost()
    {
        
        Destroy(gameObject);
        
        /*
        _visual.SetActive(false);
        _magicBuff.SetActive(true);
        _magicBuff.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<Collider>().enabled = false;
        */
    }
}
