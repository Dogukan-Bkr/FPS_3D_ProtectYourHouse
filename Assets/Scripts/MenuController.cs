using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public GameObject LoadingPanel;
    public GameObject exitPanel;
    public Slider loadingSlider;

   public void startGame()
    {
        StartCoroutine(sceneLodaing());
    }

    IEnumerator sceneLodaing()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        LoadingPanel.SetActive(true);
        while (!operation.isDone) {
            float line = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = line;
            yield return null;
        }
    }
    public void exitGame()
    {
        exitPanel.SetActive(true);
        
    }
    public void prefer(string buttonValue)
    {
        if(buttonValue == "yes")
            Application.Quit();
        else
            exitPanel.SetActive(false);
    }
}
