using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Multiplier : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _meatPopUp;
    [SerializeField] private Transform _meatPanel;
    private int _totalWin;
    public void SetMultiplierParams(int number, Color color, int finishNumber)
    {
        _text.text = "+" + number.ToString() + "%";
        _text.color = color;
        _totalWin = finishNumber;
    }

    public void PlusMeat()
    {
        _meatPopUp.text = "+" + _totalWin;
        _meatPopUp.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.9f);
        gameObject.SetActive(false);
        _meatPanel.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.9f);
    }
}
