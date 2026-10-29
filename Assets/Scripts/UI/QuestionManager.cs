using System;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    private SuspectData _suspect;
    [SerializeField] int _index = 0;

    void Start()
    {
        if (SuspectsManager.Instance._suspects.Length == 0) return;
        _suspect = SuspectsManager.Instance._suspects[_index];
    }

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
        DialogueSystem.Instance.ShowDialogue(
            $"{_suspect.s_firstName[0]} {_suspect.s_name[0]}",
            _suspect.allQuestions[IntToChoice(choice)],
            _suspect
        );

        if (TryGetComponent(out ClickOnObject cob))
            cob.Reset();
        
        // question impact

        void ChangeImpact(float sadImpact, float angryImpact, float surpriseImpact, float happyImpact)
        {
            _suspect.sad = Math.Clamp(_suspect.sad + sadImpact, 0, 1);
            _suspect.angry = Math.Clamp(_suspect.sad + angryImpact, 0, 1);
            _suspect.surprise = Math.Clamp(_suspect.sad + surpriseImpact, 0, 1);
            _suspect.happy = Math.Clamp(_suspect.sad + happyImpact, 0, 1);
        }

        switch (choice)
        {
            case 0 : // Name
                ChangeImpact(.5f,.5f,.5f,.5f);
                break;
            default : 
                ChangeImpact(0,0,1,0);
                break;
        }

        RoomManager.Instance._allSuspects[_suspect].GetComponent<SuspectExpression>().UpdateFace(_index);
    } 
}
