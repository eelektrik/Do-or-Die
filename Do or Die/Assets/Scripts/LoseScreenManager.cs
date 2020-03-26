using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreenManager : MonoBehaviour
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



    //Singleton
    public static LoseScreenManager instance;
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

        CheckForJoystickMovement();
        CheckForSubmitButton();

    }


    public void StartLoseScreen()
    {

        //Toggle pause on
        paused = true;
        //Set up the UI
        ResetSelector();

    }


    private void CheckForJoystickMovement()
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
            //PickUpAudio.instance.PlayClicking();
        }
        else if (input.y > 0)
        {
            MoveSelector(-1);
            cooldownCounter = 0;
            //PickUpAudio.instance.PlayClicking();

        }


    }
    private void CheckForSubmitButton()
    {

        if (!paused) { return; }

        //Check for submit input
        if (GetSubmitButton())
        {
            OnSubmitButton();
            cooldownCounter = 0;
            //PickUpAudio.instance.PlayConfirmButton();

        }

    }

    private void OnSubmitButton()
    {

        //Debug.Log(options[optionIndex].GetComponent<Button>().onClick.ToString());
        //options[optionIndex].GetComponent<Button>().onClick.Invoke();

        if (optionIndex == 0) { RestartGame(); }
        else if (optionIndex == 1) { QuitGame(); }

    }


    private void ResetSelector()
    {

        optionIndex = 0;
        selector.localPosition = new Vector3(options[optionIndex].localPosition.x - 100, options[optionIndex].localPosition.y, options[optionIndex].localPosition.z);

    }
    private void MoveSelector(int direction)
    {

        optionIndex += direction;
        if (optionIndex < 0) { optionIndex = options.Count - 1; }
        else if (optionIndex >= options.Count) { optionIndex = 0; }

        selector.localPosition = new Vector3(options[optionIndex].localPosition.x - 100, options[optionIndex].localPosition.y, options[optionIndex].localPosition.z);

    }



    public Vector2 GetLeftJoystickInputs()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public bool GetSubmitButton()
    {
        return Input.GetButtonDown("A");
    }



    public void QuitGame()
    {
        LevelManager.instance.EndGame();
    }

    public void RestartGame()
    {
        LevelManager.instance.RestartLevel();
    }
}
