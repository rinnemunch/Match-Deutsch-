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
    }

    IEnumerator CheckAnswer(Button clickedButton, string selected)
    {
        var item = quizItems[currentQuestion];
        bool isCorrect = selected == item.correctAnswer;

        // Flash color (green if correct, red if wrong)
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
