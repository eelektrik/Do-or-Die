using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkness : MonoBehaviour {


    public Image black;
    public float fadeSpeed;
    public bool fading;


    //Darkness.instance.FadeIn();
    //while (Darkness.instance.fading) { yield return null; }

    //DO STUFF HERE

    //Darkness.instance.FadeOut();
    //while (Darkness.instance.fading) { yield return null; }


    //Singleton
    public static Darkness instance;
    private void Awake()
    {
        instance = this;
    }

    public void FadeIn ()
    {
        StartCoroutine(Dark());
    }

    public void FadeOut ()
    {
        StartCoroutine(Light());
    }

    IEnumerator Dark ()
    {
        black.gameObject.SetActive(true);
        black.color = new Color(black.color.r, black.color.g, black.color.b, 0);

        fading = true;
        while (black.color.a < 1.0f)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + (Time.deltaTime * fadeSpeed));
            yield return null;
        }
        fading = false;
    }

    IEnumerator Light()
    {
        fading = true;
        while (black.color.a > 0)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }
        fading = false;

        black.gameObject.SetActive(false);
    }    

    public void SetToBlack ()
    {
        black.gameObject.SetActive(true);
        black.color = new Color(black.color.r, black.color.g, black.color.b, 1);
    }

}
