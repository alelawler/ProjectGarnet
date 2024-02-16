using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    /*private GameObject _camera;
    private Transform _cameraTransform;
    private GameObject _player;
    private PlayerCharacterController _playerCC;

    private void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCC = _player.GetComponent<PlayerCharacterController>();
        _cameraTransform = _camera.GetComponent<Transform>();
    }

    public void OnShootRecoil(Vector3 upRecoil)
    {
        Vector3 _originalRotation = transform.localEulerAngles;

        if (Input.GetButton("Fire1"))
        {
            _playerCC.isRecoiling = true;
            AddRecoil(upRecoil);
            _playerCC.isRecoiling = false;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _playerCC.isRecoiling = false;
            //StopRecoil(_originalRotation);
        }
    }

    private void AddRecoil(Vector3 upRecoil)
    {
        //Vector3 newVector = Vector3.Lerp(_cameraTransform.localEulerAngles, upRecoil, 1f);

       // _cameraTransform.localEulerAngles += upRecoil;
    }

    private void StopRecoil(Vector3 originalRotation)
    {
        //_cameraTransform.localEulerAngles = originalRotation;
    }*/
}
