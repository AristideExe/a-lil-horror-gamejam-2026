using System.Collections.Generic;
using UnityEngine;

public class SuspectsManager : MonoBehaviour
{
    public static SuspectsManager Instance {private set; get;}

    [Header(">====== CASE ======<")]
    [SerializeField] CaseData[] _cases;
    [SerializeField] CaseData _currentCases;

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

    string GenerateAnswer(QuestionChoice choice, string answer)
    {
        string res = "Not codded yet";
        int rng;
        if (choice == QuestionChoice.Age)
        {
            rng = Random.Range(0, 3);
            if (rng < 1) return $"I'm {answer} years old.";
            if (rng < 2) return $"You never learnt how to read ?! I'm {answer} years old.";
            else return $"I'm {answer} years old, why ?";
        }
        
        if (choice == QuestionChoice.Alibi)
        {
            rng = Random.Range(0, 3);
            if (rng < 1) return $"{answer}.";
            if (rng < 2) return $"I already told one that : {answer}";
            else return $"Sure: {answer}";
        }

        return res;
    }

    void InitSuspects()
    {
        bool oneLied = false;
        foreach (SuspectData suspect in _suspects)
        {
            // Reste suspect questions
            suspect.allQuestions = new();

            if (!oneLied && Random.Range(0, _suspectsNb) < 1) // this on will lie
            {
                suspect.s_choice = SuspectChoice.Lie;
                oneLied = true;
            }

            // --- Generate Answers --- // 
            // Get all the field where the suspect can lie
            List<Field> fields = GetFields(suspect, true);
            // Getting the number of lie
            int minLie = (int)_levelDict[suspect.s_level].x;
            int maxLie = (int)_levelDict[suspect.s_level].y;
            int rng = Random.Range(minLie, maxLie);

            // Get a field to lie in the list and create an answer
            // Add the alibi
            rng = Random.Range(1, fields[0].field.Length);
            suspect.allQuestions[fields[0].choice] = GenerateAnswer(fields[0].choice, fields[0].field[rng]);
            // Debug.Log($"LIE => {fields[0].choice} : {GenerateAnswer(fields[0].choice, fields[0].field[rng])}");
            fields.Remove(fields[0]);

            for (int i = 1; i < rng; i++)
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
            for (int i = 0; i < fields.Count; i++)
            {
                // Add the answer
                suspect.allQuestions[fields[i].choice] = GenerateAnswer(fields[i].choice, fields[i].field[0]);
                // Debug.Log($"{field[i].choice} : {GenerateAnswer(field[i].choice, field[i].field[0])}");
            }

        }
    }
}
