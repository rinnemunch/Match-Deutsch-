using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadColorsLevel()
    {
        SceneManager.LoadScene("ColorsLevel");
    }

    public void LoadNumbersLevel()
    {
        SceneManager.LoadScene("NumbersLevel");
    }
}
