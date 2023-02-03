using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeLocationHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _locations;
    GameObject _currenLocation;

    public void SetLocation(int locID)
    {
        int id = locID;

        if (locID > 10)
        {
            id = locID % 10;
        }


        _locations[id - 1].SetActive(true);

        if (_currenLocation)
        {
            _currenLocation.SetActive(false);
        }
        _currenLocation = _locations[id - 1];
    }
}
