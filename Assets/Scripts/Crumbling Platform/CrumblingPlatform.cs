using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField]
    float _timerUntilDown;

    [SerializeField]
    float _timerUntilDestroy;
    [SerializeField]
    AudioClip _crumblingAudio;
    [SerializeField]
    SoundHelper _mySoundHelper;


    float _timer;
    bool _isCrumbling;
    bool _goingRight;
    // Start is called before the first frame update
    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isCrumbling == false && other.CompareTag("Player"))
        {

            _isCrumbling = true;
            StartCoroutine(StartCrumbling());
           _mySoundHelper.PlaySound(_crumblingAudio);
        }

    }

    private void Update()
    {
        if (_isCrumbling)
            _timer += Time.deltaTime;

    }
    IEnumerator StartCrumbling()
    {
        while (_timer <= _timerUntilDown)
        {
            if (_goingRight)
            {
                transform.position += new Vector3(0.05f, 0f, 0f);
                _goingRight = false;
            }
            else
            {
                transform.position += new Vector3(-0.05f, 0f, 0f);
                _goingRight = true;
            }
            yield return new WaitForSeconds(.05f);
        }
        _timer = 0;
        StartCoroutine(GoDown());

    }

    IEnumerator GoDown()
    {

        while (_timer <= _timerUntilDestroy)
        {
            transform.position -= new Vector3(0f, 0.1f, 0f);
            yield return new WaitForSeconds(.02f);
        }

        Destroy(this.gameObject);


    }

}
