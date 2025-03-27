using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizItem
    {
        public Color color;
        public string correctAnswer;
        public string[] options;
    }

    public Image colorBox;
    public TextMeshProUGUI[] answerButtons;

    public QuizItem[] quizItems;
    private int currentQuestion = 0;

    void Start()
    {
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestion >= quizItems.Length)
        {
            Debug.Log("Quiz Complete!");
            return;
        }

        QuizItem item = quizItems[currentQuestion];

        // Force alpha to 1 (fully visible)
        Color visibleColor = item.color;
        visibleColor.a = 1f;
        colorBox.color = visibleColor;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < item.options.Length)
            {
                answerButtons[i].text = item.options[i];

                // Capture loop variable safely
                string selectedAnswer = item.options[i];

                // Reset button listener
                Button btn = answerButtons[i].GetComponentInParent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => CheckAnswer(selectedAnswer));
            }
        }
    }

    void CheckAnswer(string selected)
    {
        if (selected == quizItems[currentQuestion].correctAnswer)
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
