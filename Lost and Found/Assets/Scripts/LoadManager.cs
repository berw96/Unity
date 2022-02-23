
/*-----------------------------------------------------------
    THE ROOM (2022)
    
    COPYRIGHT ELLIOT WALKER [3368 6408]
    and HAN XUE [SN: 3367 5676]

    WITH THANKS TO DR ALAN ZUCCONI
    and BRACKEYS
-----------------------------------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Slider))]
public class LoadManager : MonoBehaviour
{
    public GameObject loadScreen;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public Button optionsButton;
    public Button backButton;
    public Slider slider;
    public Text text;

    [SerializeField] AudioClip _button_click_sfx;

    /// <summary>
    /// Activates the loading screen overlay and invokes
    /// a coroutine which loads the specified scene asynchronously
    /// via its build index.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadNextLevel(int sceneIndex)
    {
        GameManager._instance._loading_bar_prompt.gameObject.SetActive(false);
        loadScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    /// <summary>
    /// Loads a specified scene asynchronously while setting the
    /// loading bar’s fill amount to its current progress
    /// (clamped between 0 and 1 for 0% and 100% completion).
    /// Once loaded, the player is presented with an onscreen message
    /// prompting them to trigger any input to advance
    /// (invoke the activation stage of the scene).
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            text.text = Mathf.RoundToInt(progress * 100.0f) + "%";
            
            if(operation.progress >= 0.9f)
            {
                slider.value = 1;

                GameManager._instance._loading_bar_prompt.gameObject.SetActive(true);

                if (Input.anyKeyDown)
                    operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Attached to the options and back buttons in the Menu scene.
    /// The options screen overlay is toggled based on which button is clicked.
    /// </summary>
    /// <param name="buttonClicked"></param>
    public void ToggleMenu(Button buttonClicked) {
        if (buttonClicked == optionsButton &&
            !optionsMenu.activeSelf) {
            optionsMenu.SetActive(true);
            if (mainMenu.activeSelf)
                mainMenu.SetActive(false);
        }

        if(buttonClicked == backButton &&
            optionsMenu.activeSelf) {
            optionsMenu.SetActive(false);
            if (!mainMenu.activeSelf)
                mainMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Plays an <c>AudioClip</c> when an attached button is clicked.
    /// </summary>
    public void PlayButtonSFX() {
        AudioSource.PlayClipAtPoint(_button_click_sfx, transform.position);
    }
}
