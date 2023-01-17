using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] private GameObject _waterParticles;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PredatorMain"))
        {
            var newWaterParticles = Instantiate(_waterParticles);
            newWaterParticles.transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
        }
    }
}
