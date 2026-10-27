using Unity.VisualScripting;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance {private set; get;}

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] SuspectData _currentSuspect;

    QuestionChoice IntToChoice(int i)
    {
        if (i == 0) return QuestionChoice.Name;
        if (i == 1) return QuestionChoice.FirstName;
        if (i == 2) return QuestionChoice.Pseudo;
        if (i == 3) return QuestionChoice.Age;
        if (i == 4) return QuestionChoice.Sex;
        if (i == 5) return QuestionChoice.Work;
        if (i == 8) return QuestionChoice.Height;
        if (i == 9) return QuestionChoice.Weight;
        if (i == 10) return QuestionChoice.Alibi;
        else return QuestionChoice.Name;
    }

    public void AskQuestion(int choice)
    {
        Debug.Log($"Ask Question about {IntToChoice(choice)} {choice}");
        DialogueSystem.Instance.ShowDialogue(
            $"{_currentSuspect.s_firstName[0]} {_currentSuspect.s_name[0]}",
            _currentSuspect.allQuestions[IntToChoice(choice)]
        );
    } 
}
