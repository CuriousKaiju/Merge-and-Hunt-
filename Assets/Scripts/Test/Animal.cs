using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public Rigidbody[] _ragDoll;
    public List<Collider> _colliders = new List<Collider>();
    public List<AnimalPartOfTheBody> _animalParts = new List<AnimalPartOfTheBody>();
    public Animator _animator;
    public string _tagForColliders;
    public string _enemyTag;

   

    public virtual void SetRigidbodyStatus(bool status)
    {
        foreach (Rigidbody rb in _ragDoll)
        {
            rb.isKinematic = status;
        }
    }

    public virtual void SetAnimationStatus(bool status)
    {
        _animator.enabled = false;
    }

    public virtual void EnableAnimalsParts()
    {
        foreach (AnimalPartOfTheBody ap in _animalParts)
        {
            ap.enabled = false;
        }
    }

    public virtual void ChangeTriggersToColliders()
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
}
