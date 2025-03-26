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
        var item = quizItems[currentQuestion];
        colorBox.color = item.color;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].text = item.options[i];

            string selectedAnswer = item.options[i];
            answerButtons[i].GetComponentInParent<Button>().onClick.RemoveAllListeners();
            answerButtons[i].GetComponentInParent<Button>().onClick.AddListener(() =>
            {
                CheckAnswer(selectedAnswer);
            });
        }
    }

    void CheckAnswer(string selected)
    {
        if (selected == quizItems[currentQuestion].correctAnswer)
        {
            currentQuestion++;

            if (currentQuestion < quizItems.Length)
                ShowQuestion();
            else
                Debug.Log("Quiz Complete!");
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }
}
