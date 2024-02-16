using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class videoManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject skipButton;

    [SerializeField]
    GameObject videoPlayer;

    float timerIntro = 8f;
    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerIntro)
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel") && !skipButton.activeInHierarchy)
        {
            skipButton.SetActive(true);
        }
        else if (Input.GetButtonDown("Cancel") && skipButton.activeInHierarchy)
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            videoPlayer.SetActive(false);
        }
            
    }
}
