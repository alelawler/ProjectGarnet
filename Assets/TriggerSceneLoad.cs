using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSceneLoad : MonoBehaviour
{
    [SerializeField]
    public string[] scenesToLoad;
    [SerializeField]
    public string[] scenesToUnload;

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            SceneLoader._myInstance.LoadSceneAsyncSystem(scenesToLoad, scenesToUnload);
        }
    }
}
