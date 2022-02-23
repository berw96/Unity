/*-----------------------------------------------------------
    THE ROOM (2022)
    
    COPYRIGHT ELLIOT WALKER [3368 6408]
    and HAN XUE [SN: 3367 5676]
-----------------------------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings _instance;

    public enum LANGUAGE {
        ENGLISH,
        MANDARIN,
        KOREAN,
        JAPANESE
    }

    public LANGUAGE _language_setting;
    public float _volume_setting;

    private void Start() {
        _instance = this;
        DontDestroyOnLoad(_instance);

        // Delete duplicates
        foreach (PlayerSettings _setting in FindObjectsOfType<PlayerSettings>()) {
            if (_setting != _instance) {
                Destroy(_setting.gameObject);
            }
        }
    }

    /// <summary>
    /// Apply the current value selected on the volume slider to a public
    /// float which the game manager queries and uses in a for-each loop to
    /// set the audio intensity for every AudioSource component in the scene.
    /// </summary>
    /// <param name="_slider"></param>
    public void SetVolume(Slider _slider) {
        _volume_setting = _slider.value;

        Debug.Log($"Volume = {_volume_setting * 100}%");
        GameManager._instance.UpdateSettings();
    }

    /// <summary>
    /// Apply the current enumerator state selected by the player in a switch
    /// statement which assigns particular string values to each Text field
    /// in the scene based on the language chosen.
    /// </summary>
    /// <param name="_button"></param>
    public void SetLanguage(Button _button) {
        switch (_button.name) {
            case "EnglishButton":
                _language_setting = LANGUAGE.ENGLISH;
                break;
            case "MandarinButton":
                _language_setting = LANGUAGE.MANDARIN;
                break;
            case "KoreanButton":
                _language_setting = LANGUAGE.KOREAN;
                break;
            case "JapaneseButton":
                _language_setting = LANGUAGE.JAPANESE;
                break;
        }
        Debug.Log($"Language set to {_language_setting}");
        GameManager._instance.UpdateSettings();
    }
}
