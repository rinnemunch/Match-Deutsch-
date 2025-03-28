using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClick()
    {
        audioSource.Play();
    }
}
