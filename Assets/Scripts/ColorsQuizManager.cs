using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ColorQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizItem
    {
        public Color color;
        public string correctAnswer;
        public string[] options;
    }

    public GameObject winPanel;
    public Image colorBox;
    public TextMeshProUGUI[] answerButtons;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public string[] clipNames;

    public QuizItem[] quizItems;
    private int currentQuestion = 0;

    void OnEnable()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        winPanel.SetActive(false);
        yield return new WaitForSeconds(0.05f); // Give Unity a frame to initialize audio
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestion >= quizItems.Length)
        {
            winPanel.SetActive(true);
            return;
        }

        QuizItem item = quizItems[currentQuestion];

        Color visibleColor = item.color;
        visibleColor.a = 1f;
        colorBox.color = visibleColor;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < item.options.Length)
            {
                answerButtons[i].text = item.options[i];
                string selectedAnswer = item.options[i];

                Button btn = answerButtons[i].GetComponentInParent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    StartCoroutine(CheckAnswer(btn, selectedAnswer));
                });
            }
        }

        // Play voice clip after short delay
        StartCoroutine(PlayAudioDelayed(item.correctAnswer));
    }

    IEnumerator PlayAudioDelayed(string word)
    {
        yield return new WaitForSeconds(0.1f);
        PlayAudio(word);
    }

    void PlayAudio(string word)
    {
        for (int i = 0; i < clipNames.Length; i++)
        {
            if (clipNames[i].ToLower() == word.ToLower())
            {
                audioSource.clip = audioClips[i];
                audioSource.Play();
                return;
            }
        }

        Debug.LogWarning("No audio clip found for: " + word);
    }

    IEnumerator CheckAnswer(Button clickedButton, string selected)
    {
        var item = quizItems[currentQuestion];
        bool isCorrect = selected == item.correctAnswer;

        Color original = clickedButton.image.color;
        clickedButton.image.color = isCorrect ? Color.green : Color.red;
        yield return new WaitForSeconds(0.3f);
        clickedButton.image.color = original;

        if (isCorrect)
        {
            currentQuestion++;
            ShowQuestion();
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }
}
