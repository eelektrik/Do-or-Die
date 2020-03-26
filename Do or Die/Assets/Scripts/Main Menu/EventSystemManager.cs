using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour
{


    //Simple UI image to show which button is being selected
    //All buttons should be the same size
    [SerializeField] protected RectTransform selector;

    private GameObject currentButton;

    //Whether or not the UI can receive inputs atm
    private bool active = false;


    //Singleton
    public static EventSystemManager instance;
    private void Awake()
    {
        instance = this;
    }


    private void LateUpdate ()
    {

        //For now, we'll set the position of the selector via update
        UpdateSelector();

    }


    private void MoveSelector (Vector3 pos)
    {
        selector.anchoredPosition = pos;
    }

    public void SetCurrentButton (GameObject button)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button);
        UpdateSelector();
    }


    private void UpdateSelector ()
    {

        if (currentButton != UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject) {

            currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            //Debug.Log(currentButton.GetComponent<RectTransform>().transform.position);

            selector.localPosition = new Vector3(currentButton.GetComponent<RectTransform>().localPosition.x - 100, currentButton.GetComponent<RectTransform>().localPosition.y, currentButton.GetComponent<RectTransform>().localPosition.z);
            //Debug.Log(currentButton.GetComponent<RectTransform>().localPosition);

        }


    }


}
