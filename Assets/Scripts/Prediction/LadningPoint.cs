using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadningPoint : MonoBehaviour
{
    [SerializeField] private Collider _landingCollider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Gradient _green;
    [SerializeField] private Gradient _red;

    [SerializeField] private ParticleSystem _wind;
    [SerializeField] private ParticleSystem _groundAim;
    [SerializeField] private ParticleSystem _additionalParticles;
    public void SetGreenStatus()
    {
        _sprite.color = Color.green;
        _lineRenderer.colorGradient = _green;
        _wind.startColor = Color.green;
        _groundAim.startColor = Color.green;
        _additionalParticles.startColor = Color.green;
    }
    public void SetRedStatus()
    {
        _sprite.color = Color.red;
        _lineRenderer.colorGradient = _red;
        _wind.startColor = Color.red;
        _groundAim.startColor = Color.red;
        _additionalParticles.startColor = Color.red;

    }
    public void TurnOnFeature()
    {
        _landingCollider.enabled = true;
        SetRedStatus();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PreyGhost"))
        {
            SetGreenStatus();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PreyGhost"))
        {
            SetRedStatus();
        }
    }
}
