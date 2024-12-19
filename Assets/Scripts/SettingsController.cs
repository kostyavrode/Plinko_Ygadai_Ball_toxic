using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioSource audioSourcePrefab;

    private AudioSource currentSource;

    public GameObject translator;

    public Button soundButton;
    public TMP_Text soundButtonText;

    public Button vibrateButton;
    public TMP_Text vibrateButtonText;

    private bool isVibrateEnabled;
    private bool isSoundEnabled;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            isSoundEnabled = false;
        }
        else
        {
            isSoundEnabled = true;
        }

        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            isVibrateEnabled = false;
        }
        else
        {
            isVibrateEnabled = true;
        }
        if (currentSource == null)
        {
            try
            {

                currentSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
            }
            catch
            {
                currentSource = Instantiate(audioSourcePrefab);
                DontDestroyOnLoad(currentSource.gameObject);
                translator.gameObject.SetActive(true);

            }
            isSoundEnabled = !isSoundEnabled;
            isVibrateEnabled = !isVibrateEnabled;
            SoundButton();
            VibrateButton();
        }

        gameObject.SetActive(false);
    }
    public void SoundButton()
    {
        if (isSoundEnabled)
        {
            isSoundEnabled = false;
            soundButtonText.text = "Off";
            if (Translator.instance.isPortu)
            {
                soundButtonText.text = "Não";
            }

            currentSource.Pause();
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            isSoundEnabled = true;
            soundButtonText.text = "On";
            if (Translator.instance.isPortu)
            {
                soundButtonText.text = "Sim";
            }
            currentSource.Play();
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void VibrateButton()
    {
        if (isVibrateEnabled)
        {
            isVibrateEnabled = false;
            vibrateButtonText.text = "Off";
            if (Translator.instance.isPortu)
            {
                vibrateButtonText.text = "Não";
            }
            //currentSource.Pause();
            PlayerPrefs.SetInt("Vibrate", 0);
        }
        else
        {
            isVibrateEnabled = true;
            vibrateButtonText.text = "On";
            if (Translator.instance.isPortu)
            {
                vibrateButtonText.text = "Sim";
            }
            currentSource.Play();
            PlayerPrefs.SetInt("Vibrate", 1);
        }
    }
}