using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public void LoadColorsLevel()
    {
        StartCoroutine(DelayedLoad("ColorsLevel"));
    }

    public void LoadNumbersLevel()
    {
        StartCoroutine(DelayedLoad("NumbersLevel"));
    }

    IEnumerator DelayedLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.5f); // gives sound time to play
        SceneManager.LoadScene(sceneName);
    }
}
