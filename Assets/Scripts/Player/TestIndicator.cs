using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicator : MonoBehaviour
{
    [SerializeField] float destroyTimer = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Register", Random.Range(0, 8));
    }

    // Update is called once per frame
    void Register()
    {
        if (!DI_System.CheckIfObjectInSight(this.transform))
        {
            DI_System.CreateIndicator(this.transform);

        }
        //Destroy(this.gameObject, destroyTimer);
    }
}