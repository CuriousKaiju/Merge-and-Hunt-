using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Combo : MonoBehaviour
{
    [SerializeField] private Color[] _collorsOfCombo;
    [SerializeField] private TextMeshProUGUI _comboCount;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _comboObject;
    [SerializeField] private GameObject _plusMeatPopUp;

    [SerializeField] private GameObject _multiplier;
    [SerializeField] private Multiplier _multiplierScript;

    private int _comboLevel = 0;
    private int _comboValue = 0;
    private bool _isComboActive;

    private void Awake()
    {
        GameEvents.OnIncreaseCombo += IncreaseCombo;
        GameEvents.OnPreyDied += SpawnPlusMeatPopUp;
    }
    private void OnDestroy()
    {
        GameEvents.OnIncreaseCombo -= IncreaseCombo;
        GameEvents.OnPreyDied -= SpawnPlusMeatPopUp;
    }

    private void Start()
    {
        _comboCount.text = "+" + _comboValue.ToString() + "%";
        _comboCount.color = _collorsOfCombo[_comboLevel];
    }
    private void IncreaseCombo(bool status)
    {
        if (status)
        {
            if (_isComboActive)
            {
                _comboValue += 10;
                _comboCount.text = "+" + _comboValue.ToString() + "%";
                _comboCount.color = _collorsOfCombo[_comboLevel];
                transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f);
                _comboLevel += 1;
            }
            else
            {
                _isComboActive = true;
                _comboObject.SetActive(true);
                _comboValue = 10;
                _comboCount.text = "+" + _comboValue.ToString() + "%";
                _comboCount.color = _collorsOfCombo[_comboLevel];
                _comboLevel += 1;
            }
        }
        else
        {
            if (_isComboActive)
            {
                _comboValue = 0;
                _comboLevel = 0;
                _isComboActive = false;
                _animator.SetTrigger("Close");
            }
        }
    }

    private void SpawnPlusMeatPopUp(GameObject prey, int reward)
    {
        if (!_isComboActive)
        {
            _plusMeatPopUp.SetActive(true);
            _plusMeatPopUp.GetComponent<UIHalper>().SetText("+" + reward.ToString());
        }
        else
        {
            _plusMeatPopUp.SetActive(true);
            _plusMeatPopUp.GetComponent<UIHalper>().SetText("+" + reward.ToString());
            _multiplier.SetActive(true);

            float totalWin = reward + ((float)reward / 100 * (float)_comboValue) ;

            _multiplierScript.SetMultiplierParams(_comboValue, _collorsOfCombo[_comboLevel - 1], (int)totalWin);
            GameEvents.CallOnUpdateMeatStatus((int)totalWin);

        }
    }
}
