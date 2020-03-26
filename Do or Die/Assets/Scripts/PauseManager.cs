using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{


    //Bool telling whether the game is currently paused
    private bool paused = false;


    //Stuff for the selector
    [SerializeField] protected RectTransform selector;
    [SerializeField] protected List<RectTransform> options;
    private int optionIndex = 0;

    //To handle fast inputs
    float inputCooldown = 20;
    float cooldownCounter = 0;


    [SerializeField] protected GameObject pauseScreen;


    //Singleton
    public static PauseManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPause();
        CheckForJoystickMovement();
        CheckForSubmitButton();
    }


    private void CheckForPause ()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))
        {
            TogglePause();

        }
    }

    private void CheckForJoystickMovement ()
    {
        
        if (!paused) { return; }
        if (cooldownCounter < inputCooldown)
        {
            cooldownCounter += 1;
            return;
        }


        //Check for joystick input
        Vector2 input = GetLeftJoystickInputs();
        if (input.y < 0)
        {
            MoveSelector(1);
            cooldownCounter = 0;
        }
        else if (input.y > 0)
        {
            MoveSelector(-1);
            cooldownCounter = 0;
        }


    }
    private void CheckForSubmitButton ()
    {

        if (!paused) { return; }

        //Check for submit input
        if (GetSubmitButton())
        {
            OnSubmitButton();
            cooldownCounter = 0;
        }

    }


    private void TogglePause ()
    {

        if (!paused)
        {
            Debug.Log("pause!");

            //Toggle the pause screen on
            pauseScreen.SetActive(true);
            //Freeze inputs?
            InputManager.instance.ToggleInputsOff();
            //Freeze the game
            Time.timeScale = 0.0f;
            //Toggle pause on
            paused = true;
            //Set up the UI
            ResetSelector();

            //Pause Sound except music and ambience
            //SpeedRTPCControl.instance.PauseSound();
        }
        else if (paused)
        {
            Debug.Log("unpause!");

            //Toggle the pause screen off
            pauseScreen.SetActive(false);
            //Unfreeze inputs?
            InputManager.instance.ToggleInputsOn();
            //Unfreeze the game
            Time.timeScale = 1.0f;
            //Toggle pause off
            paused = false;

            //Resume Sound except music and ambience
            //SpeedRTPCControl.instance.ResumeSound();

        }

    }


    public void QuitGame ()
    {
        LevelManager.instance.EndGame();
    }

    public void RestartGame ()
    {
        LevelManager.instance.RestartLevel();
    }


    private void OnSubmitButton ()
    {

        options[optionIndex].GetComponent<Button>().onClick.Invoke();

    }


    private void ResetSelector ()
    {

        optionIndex = 0;
        selector.localPosition = new Vector3(options[optionIndex].localPosition.x - 100, options[optionIndex].localPosition.y, options[optionIndex].localPosition.z);

    }
    private void MoveSelector (int direction)
    {

        optionIndex += direction;
        if (optionIndex < 0) { optionIndex = options.Count - 1; }
        else if (optionIndex >= options.Count) { optionIndex = 0; }

        selector.localPosition = new Vector3( options[optionIndex].localPosition.x - 100, options[optionIndex].localPosition.y, options[optionIndex].localPosition.z);

    }



    public Vector2 GetLeftJoystickInputs()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public bool GetSubmitButton()
    {
        return Input.GetButtonDown("A");
    }

}
