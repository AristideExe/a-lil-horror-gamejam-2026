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

    string GenerateAnswer(QuestionChoice choice, string answer)
    {
        switch (choice)
        {
            case QuestionChoice.Name:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Je m'appelle {answer}.";
                    case 1: return $"Mon nom ? {answer}.";
                    case 2: return $"C'est {answer}, pourquoi ?";
                    case 3: return $"Tu peux m'appeler {answer}.";
                    case 4: return $"Mon nom est {answer}, retiens-le.";
                    default: return $"C'est bien {answer} qui est écrit sur mes papiers.";
                }

            case QuestionChoice.FirstName:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Mon prénom, c'est {answer}.";
                    case 1: return $"Je m'appelle {answer}.";
                    case 2: return $"{answer}, tout simplement.";
                    case 3: return $"C'est {answer}. Mes parents ont choisi.";
                    case 4: return $"Pourquoi ? Tu veux m'appeler par mon prénom ?";
                    default: return $"On m'a donné le prénom {answer}, oui.";
                }

            case QuestionChoice.Pseudo:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Mon pseudo, c'est {answer}.";
                    case 1: return $"Tout le monde m'appelle {answer}.";
                    case 2: return $"Sur Internet, c'est {answer}.";
                    case 3: return $"{answer}. Ça te plaît ?";
                    case 4: return $"J'utilise {answer}, ne me demande pas pourquoi.";
                    default: return $"Tu peux me retrouver sous le nom {answer}.";
                }

            case QuestionChoice.Age:
                switch (Random.Range(0, 7))
                {
                    case 0: return $"J'ai {answer} ans.";
                    case 1: return $"J'ai {answer} ans, pourquoi ?";
                    case 2: return $"Tu n'as pas appris à compter ? {answer} ans.";
                    case 3: return $"{answer} ans. Et je les assume.";
                    case 4: return $"Je suis âgé(e) de {answer} ans.";
                    case 5: return $"Devine... Non, allez, j'ai {answer} ans.";
                    default: return $"J'ai {answer} ans. Ça te suffit comme réponse ?";
                }

            case QuestionChoice.Sex:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Je suis {answer}.";
                    case 1: return $"Mon sexe ? {answer}.";
                    case 2: return $"{answer}, si c'est vraiment ce que tu veux savoir.";
                    case 3: return $"Je suis de sexe {answer}.";
                    case 4: return $"C'est marqué quelque part, non ? {answer}.";
                    default: return $"La réponse, c'est {answer}. Autre question ?";
                }

            case QuestionChoice.Work:
                switch (Random.Range(0, 7))
                {
                    case 0: return $"Je travaille comme {answer}.";
                    case 1: return $"Mon métier ? {answer}.";
                    case 2: return $"Je suis {answer}, pour gagner ma vie.";
                    case 3: return $"En ce moment, je travaille dans le domaine de {answer}.";
                    case 4: return $"Je fais {answer}. Ça te pose un problème ?";
                    case 5: return $"Je gagne ma vie en travaillant comme {answer}.";
                    default: return $"Professionnellement, je suis {answer}.";
                }

            case QuestionChoice.Height:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Je mesure {answer}.";
                    case 1: return $"Ma taille ? {answer}.";
                    case 2: return $"Je fais {answer}, pourquoi ?";
                    case 3: return $"En hauteur, je mesure {answer}.";
                    case 4: return $"Dernière fois que j'ai vérifié, je faisais {answer}.";
                    default: return $"Je mesure exactement {answer}, si ça t'intéresse.";
                }

            case QuestionChoice.Weight:
                switch (Random.Range(0, 6))
                {
                    case 0: return $"Je pèse {answer}.";
                    case 1: return $"Mon poids ? {answer}.";
                    case 2: return $"Je fais {answer} sur la balance.";
                    case 3: return $"Pourquoi tu veux savoir ça ? Je pèse {answer}.";
                    case 4: return $"La dernière fois, la balance indiquait {answer}.";
                    default: return $"Je pèse {answer}, voilà.";
                }

            case QuestionChoice.Alibi:
                switch (Random.Range(0, 8))
                {
                    case 0: return $"{answer}.";
                    case 1: return $"Je te l'ai déjà dit : {answer}.";
                    case 2: return $"J'étais occupé(e) à {answer}.";
                    case 3: return $"Pour faire simple : {answer}.";
                    case 4: return $"Tu veux vraiment les détails ? {answer}.";
                    case 5: return $"Je te jure que {answer}.";
                    case 6: return $"C'est pourtant simple : {answer}.";
                    default: return $"Si tu veux tout savoir, {answer}.";
                }

            default:
                return "Je ne sais pas quoi répondre à ça.";
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
            if (fields.Count == 0)
                Debug.LogError($"No Field for {suspect.s_ID} {suspect}");
            rng = Random.Range(1, fields[0].field.Length);
            suspect.allQuestions[fields[0].choice] = GenerateAnswer(fields[0].choice, fields[0].field[rng]);
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
