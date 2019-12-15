using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public TextMeshProUGUI backstoryText;
    public Image splash;
    private Color transparent;
    public float fadeTime;
    public bool displayDesc;

    void Start()
    {
        backstoryText = GameObject.Find("Backstory").GetComponent<TextMeshProUGUI>();
        splash = GameObject.Find("Splash").GetComponent<Image>();
        transparent = new Color(1f, 1f, 1f, 0.5f);
    }

    void Update()
    {
        FadeSplash();
    }

    void OnMouseOver()
    {
        displayDesc = true;
    }

    void OnMouseExit()
    {
        displayDesc = false;
    }

    void FadeSplash()
    {
        Debug.Log("Entering FadeSplash");
        if (displayDesc)
        {
            Debug.Log("True!");
            backstoryText.color = Color.Lerp(backstoryText.color, Color.white, fadeTime * Time.deltaTime);
            splash.color = Color.Lerp(splash.color, transparent, fadeTime * Time.deltaTime);
        }
        else
        {
            backstoryText.color = Color.Lerp(backstoryText.color, Color.clear, fadeTime * Time.deltaTime);
            splash.color = Color.Lerp(splash.color, Color.clear, fadeTime * Time.deltaTime);
        }
    }
}
