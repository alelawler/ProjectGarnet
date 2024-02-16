using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private Animator transition;     //para hacerle una animacion en algun momento
    [SerializeField]
    private AudioSource audioSource; //para hacer ruidito a los botones
    [SerializeField]
    private SceneLoader sceneLoader;
    [SerializeField]
    private string[] sceneToLoad; 

    
    public void PlayGame()
    {
        //GameManagerController.instance.LoadGame((int)SceneIndexes.LEVEL_1);
        sceneLoader.StartGame(sceneToLoad);
    }

    public void QuitGame()
    {
        sceneLoader.ExitGame();
    }
}
