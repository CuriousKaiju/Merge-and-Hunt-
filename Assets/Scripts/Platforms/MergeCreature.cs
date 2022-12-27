using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeCreature : MonoBehaviour
{
    [Header("MATERIALS")]
    [SerializeField] private SkinnedMeshRenderer _meshrenderer;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _highlightMaterial;

    public Vector3 _platformOffset;

    public void SetHighlightStatus()
    {
        _meshrenderer.material = _highlightMaterial;
    }
    public void SetBaseStatus()
    {
        _meshrenderer.material = _baseMaterial;
    }
    public void SetPosition()
    {
        transform.localPosition = _platformOffset;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
