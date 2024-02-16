using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerBehaviour : MonoBehaviour
{
    [SerializeField] private string[] RestartLvl;
    [SerializeField] private Transform RespawnPoint;
    [SerializeField] private int fallDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = RespawnPoint.position;
            PlayerStats.myInstance.Damage(fallDamage);
            //Debug.Log("me cai y me saque de hp " + fallDamage);
        }
        /*
        else
        {
            SceneLoader._myInstance.RestartGame(RestartLvl);
        }*/

    }
}
