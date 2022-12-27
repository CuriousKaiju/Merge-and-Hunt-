using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPI : MonoBehaviour
{

    [SerializeField] private GameObject[] _caveMen;
    [SerializeField] private ParticleSystem _particles;

    public void SetStatus(int id)
    {
        for (int i = 0; i < _caveMen.Length; i++)
        {
            if (i == id)
            {
                _caveMen[i].SetActive(true);
            }
            else
            {
                _caveMen[i].SetActive(false);
            }
        }

        _particles.Play();
    }
}
