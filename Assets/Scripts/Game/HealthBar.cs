using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _currentHealth;
    [SerializeField] private Image _whiteHealth;
    [SerializeField] private float _whiteHealthDeley;
    [SerializeField] private float _filAmountTime;
    private int _hp;
    
    public void SetStartHp(int hp)
    {
        _hp = hp;
    }
    public void MinusHealth(int newHealth)
    {
        gameObject.SetActive(true);
        _whiteHealth.DOKill();
        StopCoroutine(StartWhiteHealth());
        _text.text = newHealth.ToString();
        _currentHealth.fillAmount = (float)newHealth / (float)_hp;
        StartCoroutine(StartWhiteHealth());
    }

    private IEnumerator StartWhiteHealth()
    {
        yield return new WaitForSeconds(_whiteHealthDeley);
        _whiteHealth.DOFillAmount(_currentHealth.fillAmount, _filAmountTime);
    }

    void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
    }
}
