using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        var item = quizItems[currentQuestion];
        numberDisplay.text = item.numberText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Set the button's text
            TextMeshProUGUI tmp = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = item.options[i];

            string selectedAnswer = item.options[i];

            // Setup click behavior
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(selectedAnswer));
        }
    }

    void CheckAnswer(string selected)
    {
        Debug.Log("You clicked: " + selected);

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
