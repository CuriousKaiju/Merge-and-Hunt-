using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPartOfTheBody : MonoBehaviour
{
    [SerializeField] private Predator _animal;
    [SerializeField] private string _enemyTag;
    public bool _canCollision = false;

    private void Start()
    {

    }

    public void SetStartParams(Predator animal, string enemyTag)
    {
        _animal = animal;
        _enemyTag = enemyTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_canCollision)
        {
            if (other.gameObject.CompareTag(_enemyTag))
            {
<<<<<<< HEAD
                DOTween.KillAll();
                //_animal.ChangeTriggersToColliders(other.transform.root.GetComponent<Prey>());
                _animal.ChangeTriggersToColliders(other.transform.GetComponent<PreyPart>()._rootTransform.GetComponent<Prey>());
=======
                _animal.ChangeTriggersToColliders(other.transform.root.GetComponent<Prey>());
>>>>>>> parent of fcc2af8f (Finish)
                var newJoint = gameObject.AddComponent<HingeJoint>();
                newJoint.connectedBody = other.GetComponent<Rigidbody>();
                newJoint.enableCollision = true;
                Debug.Log("Collision");
            }
        }
    }

}
