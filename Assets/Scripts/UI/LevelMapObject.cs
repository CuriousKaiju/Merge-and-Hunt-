using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelMapObject : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _alreadyFinishColor;
    [SerializeField] private Color _selectedColor;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _levelNumber;


    public void SetBaseColor()
    {
        _image.color = _baseColor;
    }
    public void SetSelectedColor()
    {
        _image.color = _selectedColor;
    }
    public void SetFinishedColor()
    {
        _image.color = _alreadyFinishColor;
    }
    public void SetLevelNumber(int number)
    {
        _levelNumber.text = number.ToString();
    }    
}
