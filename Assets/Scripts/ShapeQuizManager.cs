using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShapeQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizItem
    {
        public Sprite shapeSprite;
        public string correctAnswer;
        public string[] options;
    }

    public GameObject winPanel;
    public Image shapeBox;
    public TextMeshProUGUI[] answerButtons;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public string[] clipNames;

    public QuizItem[] quizItems;
    private int currentQuestion = 0;
    private int correctAnswers = 0;

    void OnEnable()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        winPanel.SetActive(false);
        correctAnswers = 0;
        yield return new WaitForSeconds(0.05f);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestion >= quizItems.Length)
        {
            ShowWinScreen();
            return;
        }

        QuizItem item = quizItems[currentQuestion];
        shapeBox.sprite = item.shapeSprite;

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
        bool isCorrect = selected.ToLower() == item.correctAnswer.ToLower();

        Debug.Log("Selected: " + selected + " | Correct: " + item.correctAnswer);
        Debug.Log("Match? " + isCorrect);

        Color original = clickedButton.image.color;
        clickedButton.image.color = isCorrect ? Color.green : Color.red;
        yield return new WaitForSeconds(0.3f);
        clickedButton.image.color = original;

        if (isCorrect)
        {
            correctAnswers++;
        }

        currentQuestion++;
        ShowQuestion();
    }

    void ShowWinScreen()
    {
        winPanel.SetActive(true);

        TextMeshProUGUI winText = winPanel.GetComponentInChildren<TextMeshProUGUI>();

        if (correctAnswers == quizItems.Length)
        {
            winText.text = "Sehr gut! Du hast alle richtig!";
            winText.fontSize = 48;
            winText.alignment = TextAlignmentOptions.Center;
            winText.color = new Color32(255, 223, 0, 255); // gold
            winText.transform.localScale = Vector3.one * 0.5f;

            // Pop animation
            StartCoroutine(PopText(winText.transform));

            // Confetti
            Transform confetti = winPanel.transform.Find("Confetti");
            if (confetti != null)
            {
                confetti.gameObject.SetActive(true);
                var ps = confetti.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }
        }
        else
        {
            winText.text = "You finished.";
            winText.fontSize = 36;
            winText.color = Color.white;
            winText.alignment = TextAlignmentOptions.Center;
            winText.transform.localScale = Vector3.one;
        }
    }

    IEnumerator PopText(Transform target)
    {
        Vector3 start = Vector3.one * 0.5f;
        Vector3 mid = Vector3.one * 1.2f;
        Vector3 end = Vector3.one;

        float t = 0f;
        float duration = 0.4f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            if (progress < 0.5f)
            {
                target.localScale = Vector3.Lerp(start, mid, progress * 2);
            }
            else
            {
                target.localScale = Vector3.Lerp(mid, end, (progress - 0.5f) * 2);
            }

            yield return null;
        }

        target.localScale = end;
    }
}
