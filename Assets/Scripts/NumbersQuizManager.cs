using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumbersQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizItem
    {
        public string numberText; // "1", "2", etc.
        public string correctAnswer; // "Eins", "Zwei", etc.
        public string[] options;
    }

    public TextMeshProUGUI numberDisplay;
    public Button[] answerButtons;
    public QuizItem[] quizItems;
    public GameObject winPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public string[] clipNames; // Must match correctAnswer text exactly (e.g., "Eins", "Zwei")

    private int currentQuestion = 0;

    void Start()
    {
        winPanel.SetActive(false);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestion >= quizItems.Length)
        {
            winPanel.SetActive(true);
            return;
        }

        var item = quizItems[currentQuestion];
        numberDisplay.text = item.numberText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI tmp = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = item.options[i];

            string selectedAnswer = item.options[i];

            Button btn = answerButtons[i];
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                StartCoroutine(CheckAnswer(btn, selectedAnswer));
            });
        }

        // Play German audio for this question
        PlayAudio(item.correctAnswer);
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

        Debug.LogWarning("No audio found for: " + word);
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
