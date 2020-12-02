using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    [SerializeField] float waitingTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitForIntroToFinish());
    }
    IEnumerator waitForIntroToFinish()
    {
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene(1);
    }
}
