using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour
{
    public float maxIntensity;
    public float pulseSpeed;
    private Light _myLight;

    private void Start()
    {
        _myLight = GetComponent<Light>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Light>().intensity = Mathf.PingPong(Time.time * pulseSpeed, maxIntensity);
    }
}
