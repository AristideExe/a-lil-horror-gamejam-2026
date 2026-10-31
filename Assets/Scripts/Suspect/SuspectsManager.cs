using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SuspectsManager : MonoBehaviour
{
    public static SuspectsManager Instance {private set; get;}

    [Header(">====== CASE ======<")]
    [SerializeField] CaseData[] _cases;
    [SerializeField] CaseData _currentCases;
    public CaseData CurrentCases => _currentCases;

    [Header(">====== SUSPECTS ======<")]
    public SuspectData[] _suspects;    
    private int _suspectsNb;

    [Header(">====== LEVELS ======<")]
    [Tooltip("All the available levels")]
    [SerializeField] Level[] _levels;
    [Tooltip("Min and max of lie per suspect (min, max)")]
    [SerializeField] Vector2[] _levelsBound;
    private Dictionary<Level, Vector2> _levelDict = new Dictionary<Level, Vector2>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < _levels.Length; i++)
            _levelDict.Add(_levels[i], _levelsBound[i]);

        _currentCases = _cases[Random.Range(0, _cases.Length)];
        _suspects = _currentCases.suspects;
    }
    
    IEnumerator Start()
    {
        RoomManager.Instance._currentCase = _currentCases;
        RoomManager.Instance._suspects = _suspects.ToList();

        while (RoomManager.Instance._currentSuspects.Length <= 0)
            yield return null;
        _suspects = RoomManager.Instance._currentSuspects;

        _suspectsNb = _suspects.Length;
        InitSuspects();
    }

    class Field { public string[] field; public QuestionChoice choice; };
    List<Field> GetFields(SuspectData suspect, bool lie)
    {
        List<Field> fields = new List<Field>();

        void AddField(Field field)
        {
            if (lie && field.field?.Length > 1)
                fields.Add(field);
            if (!lie && field.field?.Length == 1)
                fields.Add(field);
        }
        // Alibi is the first one
        AddField(new Field {field = suspect.s_alibi, choice = QuestionChoice.Alibi});

        AddField(new Field {field = suspect.s_name, choice = QuestionChoice.Name});
        AddField(new Field {field = suspect.s_firstName, choice = QuestionChoice.FirstName});
        AddField(new Field {field = suspect.s_pseudo, choice = QuestionChoice.Pseudo});
        AddField(new Field {field = suspect.s_age, choice = QuestionChoice.Age});
        AddField(new Field {field = suspect.s_sex, choice = QuestionChoice.Sex});
        AddField(new Field {field = suspect.s_work, choice = QuestionChoice.Work});
        AddField(new Field {field = suspect.s_height, choice = QuestionChoice.Height});
        AddField(new Field {field = suspect.s_weight, choice = QuestionChoice.Weight});
    
        return fields;
    }

    public string GenerateAnswer(QuestionChoice choice, string answer)
    {
        switch (choice)
        {
            case QuestionChoice.Name:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"My name is {answer}.";
                    case 1: return $"My name? {answer}.";
                    case 2: return $"It's {answer}, why?";
                    case 3: return $"You can call me {answer}.";
                    case 4: return $"My name is {answer}, remember it.";
                    default: return $"It's {answer} that's written on my ID.";
                }

            case QuestionChoice.FirstName:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"My first name is {answer}.";
                    case 1: return $"My name is {answer}.";
                    case 2: return $"{answer}, simple as that.";
                    case 3: return $"It's {answer}. My parents chose it.";
                    case 4: return $"Why? Do you want to call me by my first name?";
                    default: return $"I was given the first name {answer}, yes.";
                }

            case QuestionChoice.Pseudo:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"My nickname is {answer}.";
                    case 1: return $"Everyone calls me {answer}.";
                    case 2: return $"Online, it's {answer}.";
                    case 3: return $"{answer}. Do you like it?";
                    case 4: return $"I use {answer}, don't ask me why.";
                    default: return $"You can find me under the name {answer}.";
                }

            case QuestionChoice.Age:
                switch (Random.Range(0, 7))
                {
                    case 0: return $"I am {answer} years old.";
                    case 1: return $"I'm {answer} years old, why?";
                    case 2: return $"Didn't you learn how to count? {answer} years old.";
                    case 3: return $"{answer} years old. And I own it.";
                    case 4: return $"I am {answer} years old.";
                    case 5: return $"Guess... No, come on, I'm {answer} years old.";
                    default: return $"I'm {answer} years old. Is that answer enough for you?";
                }

            case QuestionChoice.Sex:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"I am {answer}.";
                    case 1: return $"My gender? {answer}.";
                    case 2: return $"{answer}, if that's really what you want to know.";
                    case 3: return $"I am of {answer} sex.";
                    case 4: return $"It's written somewhere, right? {answer}.";
                    default: return $"The answer is {answer}. Next question?";
                }

            case QuestionChoice.Work:
                switch (Random.Range(0, 7))
                {
                    case 0: return $"I work as a {answer}.";
                    case 1: return $"My job? {answer}.";
                    case 2: return $"I am a {answer}, to make a living.";
                    case 3: return $"Right now, I work in the field of {answer}.";
                    case 4: return $"I do {answer}. Got a problem with that?";
                    case 5: return $"I earn my living working as a {answer}.";
                    default: return $"Professionally, I am a {answer}.";
                }

            case QuestionChoice.Height:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"I am {answer} tall.";
                    case 1: return $"My height? {answer}.";
                    case 2: return $"I'm {answer}, why?";
                    case 3: return $"Height-wise, I measure {answer}.";
                    case 4: return $"Last time I checked, I was {answer}.";
                    default: return $"I am exactly {answer}, if you're interested.";
                }

            case QuestionChoice.Weight:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"I weigh {answer}.";
                    case 1: return $"My weight? {answer}.";
                    case 2: return $"I weigh {answer} on the scale.";
                    case 3: return $"Why do you want to know that? I weigh {answer}.";
                    case 4: return $"Last time, the scale showed {answer}.";
                    default: return $"I weigh {answer}, there you go.";
                }

            case QuestionChoice.Alibi:
                switch (Random.Range(0, 8))
                {
                    case 0: return $"{answer}.";
                    case 1: return $"I already told you: {answer}.";
                    case 2: return $"I was busy {answer}.";
                    case 3: return $"To keep it simple: {answer}.";
                    case 4: return $"You really want the details? {answer}.";
                    case 5: return $"I swear that {answer}.";
                    case 6: return $"It's quite simple: {answer}.";
                    default: return $"If you really must know, {answer}.";
                }

            default:
                return "I don't want to talk to you anymore.";
        }
    }

    public bool suspectsInitiated = false;
    public void InitSuspects()
    {
        bool oneLied = false;
        for (int i = 0; i < _suspectsNb; i++)
        {
            SuspectData suspect = _suspects[i];

            // Reset suspect questions
            suspect.allQuestions = new();

            int rng = Random.Range(0, _suspectsNb);
            if (!oneLied && rng <= i) // this on will lie
            {
                suspect.s_choice = SuspectChoice.Lie;
                oneLied = true;
            }
            else
                suspect.s_choice = SuspectChoice.Truth;

            // --- Generate Answers --- // 
            // Get all the field where the suspect can lie
            List<Field> fields = GetFields(suspect, true);
            // Getting the number of lie
            int minLie = (int)_levelDict[suspect.s_level].x;
            int maxLie = (int)_levelDict[suspect.s_level].y;
            rng = Random.Range(minLie, maxLie);

            // Get a field to lie in the list and create an answer
            // Add the alibi
            if (suspect.s_choice == SuspectChoice.Lie)
            {
                if (fields.Count == 0)
                    Debug.LogError($"No Field for {suspect.s_ID} {suspect}");
                rng = Random.Range(1, fields[0].field.Length);
                suspect.allQuestions[fields[0].choice] = GenerateAnswer(fields[0].choice, fields[0].field[rng]);
            }
            else
                suspect.allQuestions[fields[0].choice] = GenerateAnswer(fields[0].choice, fields[0].field[0]);
            // Debug.Log($"LIE => {fields[0].choice} : {GenerateAnswer(fields[0].choice, fields[0].field[rng])}");
            fields.Remove(fields[0]);

            for (int j = 1; j < rng; j++)
            {
                if (fields.Count == 0) break;
                // Random item
                rng = Random.Range(0, fields.Count);
                Field rng_field = fields[rng];

                // Random answer
                rng = Random.Range(1, rng_field.field.Length);
                string rng_answer = rng_field.field[rng];

                // Add the answer
                suspect.allQuestions[rng_field.choice] = GenerateAnswer(rng_field.choice, rng_answer);
                // Debug.Log($"LIE => {rng_field.choice} : {GenerateAnswer(rng_field.choice, rng_answer)}");

                fields.Remove(rng_field);
            }

            // Get all the field with the suspect tell the truth
            fields.AddRange(GetFields(suspect, false));

            // Create an answer
            for (int j = 0; j < fields.Count; j++)
            {
                // Add the answer
                suspect.allQuestions[fields[j].choice] = GenerateAnswer(fields[j].choice, fields[j].field[0]);
                // Debug.Log($"{field[i].choice} : {GenerateAnswer(field[i].choice, field[i].field[0])}");
            }

        }
        suspectsInitiated = true;
    }
}
