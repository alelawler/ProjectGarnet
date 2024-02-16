using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WinController : MonoBehaviour
{

    // PONERLAS EN EL MISMO ORDEN. SI EL ENEMIGO 1 ESTA EN INDEX 0, PONER LA LUZ 1 EN INDEX 0
    [SerializeField] Enemy[] _finalEnemies;
    [SerializeField] GameObject[] _finalLights;
    [SerializeField] GameObject _myShotgunSpawnPoint;

    List<GameObject> _myFinalLights = new List<GameObject>();
    bool _ending;
    float scale = 1;
    Vector3 tempScale;

    private void Start()
    {
        foreach (GameObject fl in _finalLights)
            _myFinalLights.Add((GameObject)fl.transform.GetChild(0).gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        //kill all win condition enemies
        if (Input.GetKeyDown(KeyCode.B))
            foreach (var eenmy in _finalEnemies)
                eenmy.enemyState = Enemy.EnemyState.Dead;
        */
        for (int i = 0; i < _finalEnemies.Length; i++)
        {
            if (_finalEnemies[i].enemyState == Enemy.EnemyState.Dead)
                _finalLights[i].SetActive(true);
        }
        //remuevo a los muertos del array porque ya no existen
        _finalEnemies = _finalEnemies.Where(finalEnemy => finalEnemy.enemyState != Enemy.EnemyState.Dead).ToArray();
        _finalLights = _finalLights.Where(finalLight => finalLight.activeInHierarchy == false).ToArray();

        if (!_ending && _finalEnemies.Length == 0)
        {
            if (scale >= 0)
            {
                foreach (GameObject fLight in _myFinalLights)
                {
                    tempScale = fLight.transform.localScale;
                    tempScale.y -= 0.005f;
                    fLight.transform.localScale = tempScale;
                }
                scale -= 0.005f;
            }
            else
            {
                foreach (GameObject fLight in _myFinalLights)
                    fLight.SetActive(false);
                _ending = true;
                StartCoroutine(DropShotgun());
            }
        }

    }
    IEnumerator DropShotgun()
    {
        _myShotgunSpawnPoint.SetActive(true);

        while (_myShotgunSpawnPoint.transform.localPosition.y > 0f)
        {
            _myShotgunSpawnPoint.transform.position -= new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
