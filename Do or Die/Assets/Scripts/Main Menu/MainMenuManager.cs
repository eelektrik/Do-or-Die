using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


    //Current canvas we're looking at
    [SerializeField] protected Canvas currentCanvas;


    [SerializeField] protected Text highScore;
    [SerializeField] protected Text mostWaves;
    [SerializeField] protected Text runs;


    // Start is called before the first frame update
    void Start()
    {

        //Update player stats on entering the main menu
        UpdatePlayerStats();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame ()
    {
        StartCoroutine(StartGameSequence());
    }
    IEnumerator StartGameSequence ()
    {

        Darkness.instance.FadeIn();

        while (Darkness.instance.fading)
        {
            yield return null;
        }

        SceneManager.LoadSceneAsync(1);

    }


    private void UpdatePlayerStats ()
    {

        highScore.text = ProgressManager.instance.GetHighScore().ToString();
        mostWaves.text = ProgressManager.instance.GetHighestWavesCleared().ToString();
        runs.text = ProgressManager.instance.GetRuns().ToString();

    }


    public void UpdatePlayerClass (int classIndex)
    {
        ClassManager.instance.ChooseClass(classIndex);
    }


    public void SwitchCanvas (Canvas canvas)
    {
        StartCoroutine(SwitchCanvasSequence(canvas));
    }
    IEnumerator SwitchCanvasSequence (Canvas canvas)
    {

        //Don't accept inputs until fade is over
        canvas.GetComponent<ButtonManager>().enabled = false;
        currentCanvas.GetComponent<ButtonManager>().enabled = false;

        Darkness.instance.FadeIn();

        while (Darkness.instance.fading)
        {
            yield return null;
        }


        //Switch canvas here
        currentCanvas.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);
        currentCanvas = canvas;

        canvas.GetComponent<ButtonManager>().Recalibrate();

        Darkness.instance.FadeOut();

        while (Darkness.instance.fading)
        {
            yield return null;
        }

        //Don't accept inputs until fade is over
        canvas.GetComponent<ButtonManager>().enabled = true;

    }


    public void QuitGame ()
    {
        Application.Quit();
    }

}
