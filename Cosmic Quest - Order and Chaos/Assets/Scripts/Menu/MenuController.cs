﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Singleton
    public static MenuController Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one menu controller should be in the scene!");
    }
    #endregion

    // Maintains the currently active menu in the menu canvas
    [SerializeField] protected GameObject activeMenu;

    // Maintains the menus that the player has navigated through
    protected Stack<GameObject> menuStack;
    protected Stack<GameObject> selectedButtonsStack;

    protected Camera mainCamera;

    protected AudioSource musicSource;

    [SerializeField] protected GameObject speakerModesDropdown;

    [SerializeField] protected GameObject qualitySettingsDropdown;

    [SerializeField] protected GameObject antiAliasingDropdown;

    protected List<KeyValuePair<string, AudioSpeakerMode>> speakerModes;

    protected string[] qualitySettings;

    protected int[] antiAliasingLevels;

    protected virtual void Start()
    {
        menuStack = new Stack<GameObject>();
        selectedButtonsStack = new Stack<GameObject>();
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);
        FindCameraAndMusic();
        SetUpSpeakerModesDropdown();
        SetUpQualitySettingsDropdown();
        SetUpAntiAliasingDropdown();
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    /// <param name="menu">The menu to navigate to</param>
    public virtual void PushMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
        menuStack.Push(menu);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    public virtual void PopMenu()
    {
        if (menuStack.Count > 1) // always want to have root menu remaining on stack
        {
            activeMenu.SetActive(false);
            menuStack.Pop();
            activeMenu = menuStack.Peek();
            activeMenu.SetActive(true);
        }
    }

    protected virtual void PushButton(GameObject button)
    {
        selectedButtonsStack.Push(button);
    }

    protected virtual GameObject PopButton()
    {
        return selectedButtonsStack.Pop();
    }

    /// <summary>
    /// Gets the first selectable `GameObject` found in the specified menu
    /// </summary>
    /// <param name="menu">Game object to search for buttons in</param>
    protected virtual GameObject GetDefaultButton(GameObject menu)
    {
        if (menu == null)
        {
            return null;
        }
        Selectable btn = menu.GetComponentInChildren<Selectable>();
        if (btn)
        {
            return btn.gameObject;
        }
        else
        {
            return null;
        }
    }

    protected virtual GameObject GetSelectedButton(MultiplayerEventSystem eventSystem)
    {
        return eventSystem.currentSelectedGameObject;
    }

    /// <summary>
    /// Sets the root of a multiplayer event system to a submenu that can only be controlled by that player
    /// </summary>
    /// <param name="eventSystem">A multiplayer event system that corresponds to `playerNumber`</param>
    protected virtual void SetPlayerRoot(MultiplayerEventSystem eventSystem)
    {
        eventSystem.playerRoot = activeMenu;
        GameObject defaultButton = GetDefaultButton(activeMenu);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    /// <summary>
    /// Find the main camera and the audio source attached to it
    /// </summary>
    protected void FindCameraAndMusic()
    {
        mainCamera = FindObjectOfType<Camera>();
        musicSource = mainCamera.GetComponent<AudioSource>();
    }

    protected void SetUpQualitySettingsDropdown()
    {
        if (qualitySettingsDropdown)
        {
            qualitySettings = VideoHelper.GetQualityLevels();
            int current = VideoHelper.GetCurrentQualityLevel();
            List<TMPro.TMP_Dropdown.OptionData> qualityDropdownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
            for (int i = 0; i < qualitySettings.Length; i++)
            {
                qualityDropdownOptions.Add(new TMPro.TMP_Dropdown.OptionData(qualitySettings[i]));
            }
            qualitySettingsDropdown.GetComponent<TMPro.TMP_Dropdown>().options = qualityDropdownOptions;
            qualitySettingsDropdown.GetComponent<TMPro.TMP_Dropdown>().SetValueWithoutNotify(current);
        }
    }

    protected void SetUpAntiAliasingDropdown()
    {
        if (antiAliasingDropdown)
        {
            antiAliasingLevels = VideoHelper.GetAntiAliasingLevels();
            int current = VideoHelper.GetCurrentAntiAliasingLevel();
            int selected = 0;
            List<TMPro.TMP_Dropdown.OptionData> qualityDropdownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
            for (int i = 0; i < antiAliasingLevels.Length; i++)
            {
                qualityDropdownOptions.Add(new TMPro.TMP_Dropdown.OptionData(antiAliasingLevels[i].ToString() + "x"));
                if (antiAliasingLevels[i] == current)
                {
                    selected = i;
                }
            }
            antiAliasingDropdown.GetComponent<TMPro.TMP_Dropdown>().options = qualityDropdownOptions;
            antiAliasingDropdown.GetComponent<TMPro.TMP_Dropdown>().SetValueWithoutNotify(selected);
        }
    }

    protected void SetUpSpeakerModesDropdown()
    {
        if (speakerModesDropdown)
        {
            speakerModes = AudioHelper.GetAudioSpeakerModes();
            AudioSpeakerMode current = AudioHelper.GetCurrentSpeakerMode();
            int selected = 0;
            List<TMPro.TMP_Dropdown.OptionData> speakerDropdownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
            for (int i = 0; i < speakerModes.Count; i++)
            {
                speakerDropdownOptions.Add(new TMPro.TMP_Dropdown.OptionData(speakerModes[i].Key));
                if (speakerModes[i].Value == current)
                {
                    selected = i;
                }
            }
            speakerModesDropdown.GetComponent<TMPro.TMP_Dropdown>().options = speakerDropdownOptions;
            speakerModesDropdown.GetComponent<TMPro.TMP_Dropdown>().SetValueWithoutNotify(selected);
        }
    }

    /// <summary>
    /// Set the master volume
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetMasterVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetMasterVolume(value);
        UpdateMusicVolume();
    }

    /// <summary>
    /// Set the volume of music
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetMusicVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetMusicVolume(value);
        UpdateMusicVolume();
        
    }

    /// <summary>
    /// Set the volume of SFX
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetSfxVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetSfxVolume(value);
    }

    /// <summary>
    /// Set the volume of vocal audio
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetVoiceVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetVoiceVolume(value);
    }

    public void SetVideoQualityLevel()
    {
        TMPro.TMP_Dropdown dropdown = qualitySettingsDropdown.GetComponent<TMPro.TMP_Dropdown>();
        for(int i = 0; i < qualitySettings.Length; i++)
        {
            if (qualitySettings[i] == dropdown.itemText.text)
            {
                VideoHelper.SetQualityLevel(i);
            }
        }
    }

    public void SetAntiAliasingLevel()
    {
        TMPro.TMP_Dropdown dropdown = antiAliasingDropdown.GetComponent<TMPro.TMP_Dropdown>();
        for (int i = 0; i < qualitySettings.Length; i++)
        {
            if (qualitySettings[i] == dropdown.itemText.text)
            {
                VideoHelper.SetQualityLevel(i);
            }
        }
    }

    public void SetAudioSpeakerMode()
    {
        TMPro.TMP_Dropdown dropdown = speakerModesDropdown.GetComponent<TMPro.TMP_Dropdown>();
        foreach(KeyValuePair<string, AudioSpeakerMode> speakerMode in speakerModes)
        {
            if (speakerMode.Key == dropdown.itemText.text)
            {
                AudioHelper.SetAudioSpeakerMode(speakerMode.Value);
                return;
            }
        }
    }

    /// <summary>
    /// Update any playing music
    /// </summary>
    protected void UpdateMusicVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = AudioHelper.GetAudioModifier(AudioHelper.EntityAudioClip.AudioType.Music);
            Debug.Log(musicSource.volume);
        }
    }
}