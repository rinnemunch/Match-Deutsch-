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
    }

    IEnumerator CheckAnswer(Button clickedButton, string selected)
    {
        var item = quizItems[currentQuestion];
        bool isCorrect = selected == item.correctAnswer;

        // Flash feedback
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
