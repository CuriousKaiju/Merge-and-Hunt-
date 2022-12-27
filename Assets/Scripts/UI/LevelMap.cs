using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    [SerializeField] private LevelMapObject[] _objects;


    public void SetLevelMapStatus(int currentLevel)
    {
        
        int number = currentLevel;
        int firstNumberInArray = 0;

        if (currentLevel > 10)
        {
            number = currentLevel % 10;
            firstNumberInArray = currentLevel - number;
        }

        for(int i = 0; i < _objects.Length; i++)
        {
            if (i + 1 == number)
            {
                _objects[i].SetSelectedColor();
                _objects[i].SetLevelNumber(i + 1 + firstNumberInArray);
            }
            else if (i + 1 < number)
            {
                _objects[i].SetFinishedColor();
                _objects[i].SetLevelNumber(firstNumberInArray + i + 1);
            }
            else if(i + 1 > number)
            {
                _objects[i].SetBaseColor();
                _objects[i].SetLevelNumber(firstNumberInArray + i + 1);
            }
        }
    }
}
