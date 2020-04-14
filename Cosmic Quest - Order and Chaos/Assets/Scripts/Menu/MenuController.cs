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

    [SerializeField] protected GameObject displayModeDropdown;

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
        SetUpDisplayModeDropdown();
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

    /// <summary>
    /// Add a button to the top of a stack
    /// </summary>
    /// <param name="button">Button to add to the stack</param>
    protected virtual void PushButton(GameObject button)
    {
        selectedButtonsStack.Push(button);
    }

    /// <summary>
    /// Remove a button from the top of a stack
    /// </summary>
    /// <returns>The button that was removed</returns>
    protected virtual GameObject PopButton()
    {
        return selectedButtonsStack.Pop();
    }

    /// <summary>
    /// Description: Clear the stack and return the menu Game Object at the bottom of the stack
    /// Rationale: Should be able to close a menu and reset the menu to it's original state
    /// </summary>
    /// <returns>The root menu GameObject at the bottom of the menu stack</returns>
    protected GameObject GetRootMenu()
    {
        GameObject rootMenu = null;
        for (int i = 0; i <= menuStack.Count; i++)
        {
            rootMenu = menuStack.Pop();
        }
        return rootMenu;
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

    /// <summary>
    /// Get the button that a player's event system has selected
    /// </summary>
    /// <param name="eventSystem">Event system of the player</param>
    /// <returns>The button that the player selected</returns>
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
        mainCamera = Camera.main;
        musicSource = mainCamera.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Add items to the quality settings dropdown and set the default
    /// </summary>
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
            TMPro.TMP_Dropdown dropdown = qualitySettingsDropdown.GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options = qualityDropdownOptions;
            dropdown.SetValueWithoutNotify(current);
        }
    }

    /// <summary>
    /// Add items to the anti aliasing settings dropdown and set the default
    /// </summary>
    protected void SetUpAntiAliasingDropdown()
    {
        if (antiAliasingDropdown)
        {
            antiAliasingLevels = VideoHelper.GetAntiAliasingLevels();
            int current = VideoHelper.GetCurrentAntiAliasingLevel();
            int selected = 0;
            List<TMPro.TMP_Dropdown.OptionData> antiAliasingOptions = new List<TMPro.TMP_Dropdown.OptionData>();
            for (int i = 0; i < antiAliasingLevels.Length; i++)
            {
                antiAliasingOptions.Add(new TMPro.TMP_Dropdown.OptionData(antiAliasingLevels[i].ToString() + "x"));
                if (antiAliasingLevels[i] == current)
                {
                    selected = i;
                }
            }
            TMPro.TMP_Dropdown dropdown = antiAliasingDropdown.GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options = antiAliasingOptions;
            dropdown.SetValueWithoutNotify(current);
        }
    }
    /// <summary>
    /// Add items to the speaker mode settings dropdown and set the default
    /// </summary>
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
            TMPro.TMP_Dropdown dropdown = speakerModesDropdown.GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options = speakerDropdownOptions;
            dropdown.SetValueWithoutNotify(selected);
        }
    }

    /// <summary>
    /// Add items to the speaker mode settings dropdown and set the default
    /// </summary>
    protected void SetUpDisplayModeDropdown()
    {
        if (displayModeDropdown)
        {
            bool isFullScreen = VideoHelper.isFullscreen();
            int selected = isFullScreen ? 1 : 0;
            List<TMPro.TMP_Dropdown.OptionData> displayDropdownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
            displayDropdownOptions.Add(new TMPro.TMP_Dropdown.OptionData("Windowed"));
            displayDropdownOptions.Add(new TMPro.TMP_Dropdown.OptionData("Fullscreen"));
            TMPro.TMP_Dropdown dropdown = displayModeDropdown.GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options = displayDropdownOptions;
            dropdown.SetValueWithoutNotify(selected);
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

    /// <summary>
    /// Set the video quality
    /// </summary>
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

    /// <summary>
    /// Set the anti aliasing level
    /// </summary>
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

    /// <summary>
    /// Set the speaker mode
    /// </summary>
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
    /// Set the screen mode
    /// </summary>
    public void SetScreenMode()
    {
        TMPro.TMP_Dropdown dropdown = displayModeDropdown.GetComponent<TMPro.TMP_Dropdown>();
        VideoHelper.SetFullscreen(dropdown.value == 1);
    }


    /// <summary>
    /// Update any playing music
    /// </summary>
    protected void UpdateMusicVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = AudioHelper.GetAudioModifier(AudioHelper.EntityAudioClip.AudioType.Music);
        }
    }

    public void LoadLevelsScene()
    {
        LevelManager.Instance.StartLevelMenu();
    }

    public void ExitGame()
    {
        LevelManager.Instance.ExitGame();
    }

    public void LoadMenuScene()
    {
        LevelManager.Instance.BackToMenu();
    }
}