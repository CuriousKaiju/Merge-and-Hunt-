using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lofelt.NiceVibrations;

public class Prey : MonoBehaviour
{
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _reward;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _ragDoll;
    [SerializeField] private Rigidbody _preyRagDoll;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Vector3 _punchSkale;
    [SerializeField] private GameObject _bloodParticles;
    [SerializeField] private Transform _animalTransform;
    [SerializeField] private Color _impactColor;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private float _impactTime;
    [SerializeField] private AnimalMovement _animalMovement;
    [SerializeField] private HapticSource _haptic;
    [SerializeField] private GameObject _warningParticles;
    [SerializeField] private Collider _magnitCollider;
    public Transform _huntPoint;
    private bool _isAnimalAlive = true;
    public Rigidbody _pointForConnection;

    private void Awake()
    {
        _pointForConnection = _ragDoll[0];
        _healthBar.SetStartHp(_totalHealth);
        RagDollStatus(true);
        GameEvents.OnStartHunt += StartMovementHunt;
    }
    private void OnDestroy()
    {
        GameEvents.OnStartHunt -= StartMovementHunt;
    }
    public void GetDamage(int damage, out bool isItDie, out int reward)
    {
        bool creatureStatus = false;

        if (_isAnimalAlive)
        {
            _bloodParticles.SetActive(true);
            _bloodParticles.GetComponent<ParticleSystem>().Play();

            transform.DOPunchScale(_punchSkale, _impactTime);
            _animalTransform.DOPunchScale(_punchSkale, _impactTime);
            _meshRenderer.material.DOColor(_impactColor, _impactTime / 2).OnComplete(() =>
            {
                _meshRenderer.material.DOColor(Color.white, _impactTime / 2);
            });

            _totalHealth -= damage;

            if (_totalHealth > 0)
            {
                _healthBar.MinusHealth(_totalHealth);
            }
            else
            {
                _magnitCollider.enabled = false;
                creatureStatus = true;
                GameEvents.CallOnPreyDied(gameObject, _reward);
                _healthBar.gameObject.SetActive(false);
                _animalMovement.enabled = false;
                RagDollStatus(false);
                _isAnimalAlive = false;
            }

            _haptic.Play();
        }

        reward = _reward;
        isItDie = creatureStatus;
    }

    private IEnumerator SetVarningStatus(float levelSpeed)
    {
        _animalMovement.StartGhost(levelSpeed);
        _warningParticles.SetActive(true);
        _animalMovement._speed = levelSpeed;
        _animator.speed = levelSpeed * 10;
        _animator.SetTrigger("Run");
        yield return new WaitForSeconds(1f);
        _warningParticles.SetActive(false);
    }

    private void StartMovementHunt(float levelSpeed)
    {
        StartCoroutine(SetVarningStatus(levelSpeed));
    }
        
    private void RagDollStatus(bool status)
    {
        _animator.enabled = status;

        foreach (Rigidbody rb in _ragDoll)
        {
            //rb.GetComponent<Collider>().enabled = status;
            rb.isKinematic = status;
        }
    }
}
