using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Slider BGM;
    public Slider soundEffects;
    public Toggle SFX;
    public MainManager gameManager;

    public GameObject settingsPanel;
    public GameObject cardOptionPanel;
    public Button settingsButton;
    public Button cardPanelButton;
    public GameObject winPanel;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        gameManager = FindAnyObjectByType<MainManager>();
    }

    public void ActivateSettingPanel()
    {
        settingsPanel.SetActive(true);
        cardOptionPanel.SetActive(false);
        settingsButton.gameObject.SetActive(true);
        cardPanelButton.gameObject.SetActive(true);
        this.GetComponent<Canvas>().sortingOrder = 1;
        gameManager.isPanel = true;
    }
    public void DeactivateSettingPanel()
    {
        settingsPanel.SetActive(false);
        cardOptionPanel.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        cardPanelButton.gameObject.SetActive(false);
        this.GetComponent<Canvas>().sortingOrder = 0;
        gameManager.isPanel = false;
    }
    public void DeactivateSettingAndActivateCardPanel()
    {
        settingsPanel.SetActive(false);
        cardOptionPanel.SetActive(true);
        this.GetComponent<Canvas>().sortingOrder = 1;
    }
    public void ActivateWinPanel()
    {
        winPanel.SetActive(true);
        this.GetComponent<Canvas>().sortingOrder = 1;
        gameManager.isPanel = true;
    }
    public void RestartGame()
    {
        winPanel.SetActive(false);
        this.GetComponent<Canvas>().sortingOrder = 0;
        FindObjectOfType<MainManager>().RestartGame();
        gameManager.isPanel = false;
    }

    public void ChangeBGMVolume()
    {
        AudioManager.instance.ChangeBGMVolume(BGM.value);
    }
    public void ChangeSoundEffectsVolume()
    {
        AudioManager.instance.ChangeSoundEffectsVolume(soundEffects.value);
    }

    public void ChangeSFXEnabled()
    {
        gameManager.SFXEnabled(SFX.isOn);
    }
}
