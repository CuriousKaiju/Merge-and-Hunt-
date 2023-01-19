using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Predator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _ragDoll;

    [SerializeField] private List<Collider> _colliders = new List<Collider>();
    [SerializeField] private List<AnimalPartOfTheBody> _animalParts = new List<AnimalPartOfTheBody>();

    [SerializeField] private Rigidbody _animalHead;
    [SerializeField] private Rigidbody _predatorRb;
    [SerializeField] private DragAndShoot _dragAndShoot;
    [SerializeField] private AnimalMovement _animalMovement;
    [SerializeField] private Transform _creature;
    [SerializeField] private GameObject _trueHead;

    [SerializeField] private int _damage;
    [SerializeField] private GameObject _waterParticles;

    private GameObject _lastCreature;
    private Transform _closestHuntPoint;
    private bool _canMagnitude;
    public bool _readyForAttack;
    private bool _collisionWas;
    private bool _initMoveToPreyRadius;


    private void Awake()
    {
        SetRigidbodyStatus(true, true);
        GameEvents.OnStartHunt += SetRunStatus;
    }

    private void Update()
    {
        /*
        if(_canMagnitude && Input.GetMouseButtonDown(0))
        {
            Magnitude();
        }
        */
    }

    public void MoveToPrey()
    {
        if(_canMagnitude)
        {
            transform.position = Vector3.MoveTowards(transform.position, _closestHuntPoint.position, 2f);
        }
    }
    public void SetRigidbodyStatus(bool status, bool doOverrideColliders)
    {
        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = status;
            var collider = rb.GetComponent<Collider>();
            collider.isTrigger = true;
            _colliders.Add(collider);
            collider.gameObject.tag = "Predator";
            var newAnimlPart = collider.gameObject.AddComponent<AnimalPartOfTheBody>();
            newAnimlPart.SetStartParams(this, "Prey");
            _animalParts.Add(newAnimlPart);
        }
    }

    public void SetRigidbodyStatus(bool status)
    {
        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = status;
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnStartHunt -= SetRunStatus;
    }
    public void SetJumpAnim()
    {
        _animator.SetTrigger("Jump");
        foreach(AnimalPartOfTheBody pb in _animalParts)
        {
            pb._canCollision = true;
        }
        GameEvents.CallWindParticles(true);
    }

    public void SetRunStatus(float speed)
    {
        _animator.SetTrigger("Run");
        _animalMovement._speed = speed;

    }
    public void SetRagDoll()
    {
        _animator.enabled = false;

        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = false;
        }
    }
    private void ShiftToPrey()
    {
        _predatorRb.isKinematic = true;
        //transform.DOLookAt(_closestHuntPoint.position, 0.45f);
        transform.DOMove(_closestHuntPoint.position, 0.45f).OnComplete(() =>
        {
            if (!_collisionWas)
            {
                var prey = _lastCreature.GetComponent<Prey>();
                ChangeTriggersToColliders(prey);
                var newJoint = _animalHead.gameObject.AddComponent<HingeJoint>();
                newJoint.connectedBody = prey._pointForConnection.GetComponent<Rigidbody>();
                newJoint.enableCollision = true;
            }
        });

    }
    private void OnTriggerEnter(Collider other)
    {
        if (_readyForAttack)
        {

            if (other.gameObject.CompareTag("Ground"))
            {
                _collisionWas = true;
                StopCollisions();
                transform.DOKill();
                GameEvents.CallOnUpdateGhostStatus();
                GameEvents.CallIncreaseCombo(false);
                GameEvents.CallOnFindNewPredator(other.transform.root);
                gameObject.GetComponent<Collider>().enabled = false;
                SetRagDoll();
                _animalHead.AddForce(Vector3.up * 90, ForceMode.Impulse);
                transform.SetParent(other.transform);
                _dragAndShoot.OffLandingPoint();
                _animalMovement.InitMovement();
                GameEvents.CallWindParticles(false);
                _creature.SetParent(null);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("PreyRadius"))
            {
                if(!_initMoveToPreyRadius)
                {
                    Debug.Log("Shift");
                    _initMoveToPreyRadius = true;
                    _collisionWas = false;
                    _closestHuntPoint = other.gameObject.GetComponent<Prey>()._huntPoint;
                    _lastCreature = other.gameObject;
                    ShiftToPrey();
                }
            } 
        }
    }


    private void StopCollisions()
    {
        _animator.enabled = false;

        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = false;
        }

        foreach (Collider collider in _colliders)
        {
            collider.isTrigger = false;
        }

        foreach (AnimalPartOfTheBody ap in _animalParts)
        {
            ap._canCollision = false;
        }
    }

    public virtual void ChangeTriggersToColliders(Prey prey)
    {
        _animator.enabled = false;

        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = false;
        }

        foreach (Collider collider in _colliders)
        {
            collider.isTrigger = false;
        }

        foreach (AnimalPartOfTheBody ap in _animalParts)
        {
            ap._canCollision = false;
        }

        _collisionWas = true;
        bool isItKill;
        int rewardForKill;
        GameEvents.CallIncreaseCombo(true);
        prey.GetDamage(_damage, out isItKill, out rewardForKill);
        GameEvents.CallOnFindNewPredator(prey.transform);
        gameObject.GetComponent<Collider>().enabled = false;
        SetRagDoll();
        _predatorRb.isKinematic = true;
        _dragAndShoot.OffLandingPoint();
        _animalMovement.InitMovement();
        GameEvents.CallWindParticles(false);
        _creature.SetParent(null);
        
        if (isItKill)
        {
            _animalHead.AddForce(Vector3.up * 300, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
