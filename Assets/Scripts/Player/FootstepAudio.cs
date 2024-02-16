using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] PlayerCharacterController _myPlayerCharacterController;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.transform.tag)
        {
            case "Grass":
                _myPlayerCharacterController.GetSurfaceStepped("Grass");
                break;
            case "Metal":
                _myPlayerCharacterController.GetSurfaceStepped("Metal");
                break;
            case "Stone":
                _myPlayerCharacterController.GetSurfaceStepped("Stone");
                break;
        }
    }
}
