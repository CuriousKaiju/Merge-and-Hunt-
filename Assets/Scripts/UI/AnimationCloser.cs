using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCloser : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    public void CloseObject()
    {
        gameObject.SetActive(false);
    }
    public void StartHunt()
    {
        _sceneController.LoadLevel();
    }
}
