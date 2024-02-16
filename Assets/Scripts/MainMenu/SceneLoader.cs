using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject menu;
    public Slider slider;
    public TextMeshProUGUI text;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public static SceneLoader _myInstance;

    private void Awake()
    {
        if (_myInstance == null)
        {
            _myInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void StartGame(string[] sceneIndex)
    {
        HideMenu();
        ShowLoadingScreen();

        SceneManager.LoadScene(sceneIndex[0]);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneIndex[1], LoadSceneMode.Additive));

        //StartCoroutine(LoadAsync(sceneIndex));
    }

    public void RestartGame(string[] sceneIndex)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");//lo destruimos por que es un singleton
        Destroy(player);

        SceneManager.LoadScene(sceneIndex[0]);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneIndex[1], LoadSceneMode.Additive));
    }
    public void ReturnToTitle(string sceneMainMenu)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //lo destruimos por que es un singleton
        Destroy(player);

        SceneManager.LoadScene(sceneMainMenu);
    }

    public void LoadSceneAsyncSystem(string[] sceneToAdd, string[] sceneToUnload)
    {
        for (int i = 0; i < sceneToAdd.Length; i++)
        {
            if (!SceneManager.GetSceneByName(sceneToAdd[i]).isLoaded) //si esta activa no la carga de nuevo
                StartCoroutine(LoadSceneCoroutine(sceneToAdd[i]));

        }

        for (int i = 0; i < sceneToUnload.Length; i++)
        {
            if (SceneManager.GetSceneByName(sceneToUnload[i]).isLoaded) //si esta activa la remueve
                StartCoroutine(UnloadSceneCuroutine(sceneToUnload[i]));
        }
    }


    IEnumerator LoadSceneCoroutine(string sceneToAdd)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToAdd, LoadSceneMode.Additive);
        scenesToLoad.Add(asyncLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    IEnumerator UnloadSceneCuroutine(string sceneToUnload)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

    }

    /*
     * //Metodo que usa una lista de las escenas siendo lodeadas para mostrar alguna barra de progreso o algo
    IEnumerator LoadAsync(string[] sceneIndex)
    {
        //carga la escena en segundo plano, mientras se pueden acceder a la info de la misma
        for (int i = 0; i < scenesToLoad.Count; i++)
        {

            while (!scenesToLoad[i].isDone)
            {
                float progress = Mathf.Clamp01(scenesToLoad[i].progress / .9f);
                //slider.value = progress;
                text.text = progress * 100f + "%";
                yield return null;
            }
        }

    }
    */
    public void ShowMenu()
    {
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
    public void UnloadMainMenuScene()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
