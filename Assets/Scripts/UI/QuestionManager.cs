using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    private SuspectData _suspect;
    [SerializeField] int _index = 0;

    void Start()
    {
        if (SuspectsManager.Instance._suspects.Length == 0) return;
        _suspect = SuspectsManager.Instance._suspects[_index];
    }

    QuestionChoice StringToChoice(string name)
    {
        if (name.ToLower().Contains("name")) return QuestionChoice.Name;
        if (name.ToLower().Contains("firstname")) return QuestionChoice.FirstName;
        if (name.ToLower().Contains("pseudo")) return QuestionChoice.Pseudo;
        if (name.ToLower().Contains("age")) return QuestionChoice.Age;
        if (name.ToLower().Contains("sex")) return QuestionChoice.Sex;
        if (name.ToLower().Contains("work")) return QuestionChoice.Work;
        if (name.ToLower().Contains("height")) return QuestionChoice.Height;
        if (name.ToLower().Contains("weight")) return QuestionChoice.Weight;
        if (name.ToLower().Contains("alibi")) return QuestionChoice.Alibi;
        else return QuestionChoice.Name;
    }

    public void AskQuestion(TextMeshProUGUI field)
    {
        DialogueSystem.Instance.ShowDialogue(
            $"{_suspect.s_firstName[0]} {_suspect.s_name[0]}",
            _suspect.allQuestions[StringToChoice(field.name)],
            _suspect
        );

        // disable text
        field.enabled = false;
        GameObject Parent = field.GetComponentInParent<Image>().gameObject;
        Button Button = Parent.GetComponentInParent<Button>();
        Button.interactable = false;

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
                amount = 1/4f;
                break;
            case Level.Hard:
                amount = 1/2f;
                break;
            case Level.Expert:
                amount = 1/1f;
                break;
            default:
                amount = 1/6f;
                break;

        }

        if (field.name.ToLower().Contains("name"))
            ChangeImpact(amount,amount,amount,amount);
        if (field.name.ToLower().Contains("firstname"))
            ChangeImpact(amount,amount,amount,amount);
        if (field.name.ToLower().Contains("pseudo"))
            ChangeImpact(2*amount,amount,amount,amount);
        if (field.name.ToLower().Contains("age"))
            ChangeImpact(2*amount,amount,amount,amount);
        if (field.name.ToLower().Contains("sex"))
            ChangeImpact(2*amount,2*amount,2*amount,amount);
        if (field.name.ToLower().Contains("work"))
            ChangeImpact(amount/2,amount/2,amount,amount);
        if (field.name.ToLower().Contains("height"))
            ChangeImpact(amount/2,amount,amount,amount);
        if (field.name.ToLower().Contains("weight"))
            ChangeImpact(2*amount,2*amount,amount,amount);
        if (field.name.ToLower().Contains("alibi"))
            ChangeImpact(amount/2,amount/2,amount/2,amount/2);

        int statusMax = 3;
        _suspect.status = Mathf.RoundToInt((_suspect.sad + _suspect.angry + _suspect.surprise + _suspect.happy) / 4 * statusMax);

        RoomManager.Instance._allSuspects[_suspect].GetComponent<SuspectExpression>().UpdateFace(_index);
    } 
}
