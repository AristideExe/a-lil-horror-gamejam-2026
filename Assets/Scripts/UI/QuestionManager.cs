using System;
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
            cob.ResetObject(transform);
        
        // question impact

        void ChangeImpact(float sadImpact, float angryImpact, float surpriseImpact, float happyImpact)
        {
            _suspect.sad = Math.Clamp(_suspect.sad + sadImpact, 0, 1);
            _suspect.angry = Math.Clamp(_suspect.angry + angryImpact, 0, 1);
            _suspect.surprise = Math.Clamp(_suspect.surprise + surpriseImpact, 0, 1);
            _suspect.happy = Math.Clamp(_suspect.happy + happyImpact, 0, 1);
        }

        float amount;
        switch (_suspect.s_level)
        {
            case Level.Normal:
                amount = 1/4;
                break;
            case Level.Hard:
                amount = 1/2;
                break;
            case Level.Expert:
                amount = 1/1;
                break;
            default:
                amount = 1/6;
                break;

        }

        switch (choice)
        {
            case 0:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 1:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 2:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 3:
                ChangeImpact(2*amount,amount,amount,amount);
                break;
            case 4:
                ChangeImpact(2*amount,2*amount,amount,amount);
                break;
            case 5:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 6:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 7:
                ChangeImpact(amount,amount,amount,amount);
                break;
            case 8:
                ChangeImpact(amount/2,amount/2,amount,amount);
                break;
            case 9:
                ChangeImpact(amount,amount,amount,amount);
                break;
            default:
                ChangeImpact(amount/2,amount/2,amount/2,amount/2);
                break;
        }

        int statusMax = 3;
        _suspect.status = Mathf.RoundToInt((_suspect.sad + _suspect.angry + _suspect.surprise + _suspect.happy) / 4 * statusMax);

        RoomManager.Instance._allSuspects[_suspect].GetComponent<SuspectExpression>().UpdateFace(_index);
    } 
}
