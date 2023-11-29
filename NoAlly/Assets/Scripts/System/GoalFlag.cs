//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    [SerializeField]
    SceneController _sceneController = null;
    [SerializeField]
    string _sceneName = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _sceneController.Load(_sceneName);
        }
    }
}
