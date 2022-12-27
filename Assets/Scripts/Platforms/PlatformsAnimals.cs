using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsAnimals : MonoBehaviour
{
    public int _animalLevel;
    [SerializeField] private Animator _animator;
    [SerializeField] private MergeCreature _mergeCreature;

    public void ActivateBounce()
    {
        _animator.SetTrigger("Set");
    }
    public void ActivateCloseBounce()
    {
        _animator.SetTrigger("ToIdle");
    }

    public void SetHighlightStatusOfCreature()
    {
        _mergeCreature.SetHighlightStatus();
    }

    public void SetBaseStatusOfCreature()
    {
        _mergeCreature.SetBaseStatus();
    }

    public void RemoveAnimal()
    {
        Destroy(_mergeCreature.gameObject);
    }
    public void SetMergeCreatuture(MergeCreature mergeCreature, int level)
    {
        ActivateBounce();
        _mergeCreature = mergeCreature;
        _animalLevel = level;
    }

    public void SetMergeCreatutureAfterSpawn(MergeCreature mergeCreature, int level)
    {
        _mergeCreature = mergeCreature;
        _animalLevel = level;
        mergeCreature.SetPosition();
    }
}
