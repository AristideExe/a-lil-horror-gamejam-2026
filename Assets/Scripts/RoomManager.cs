using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance {private set; get;}
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [HideInInspector] public CaseData _currentCase;
    [HideInInspector] public List<SuspectData> _suspects;
    [HideInInspector] public SuspectData[] _currentSuspects;

    [SerializeField] public float _timeMax = 600f; // 5 min
    [SerializeField] public float _time = 0;
    [SerializeField] public bool _gameRun = true;
    [SerializeField] public int livedNb = 0;
    [SerializeField] public int KilledNb = 0;
    // [SerializeField] public bool _startScreamer = false;
    [SerializeField] public UIRecap _UIRecap;

    [Header("====== ROOM ======")]
    [SerializeField] GameObject _roomPrefab;
    [SerializeField] GameObject _wallBegin;
    [SerializeField] GameObject _wallEnd;
    public List<Light[]> _lights = new List<Light[]>();

    private Level _level;

    [Header("====== ANIMATION ======")]
    [SerializeField] RuntimeAnimatorController _animController;
    [SerializeField] Avatar _avatar;

    [Header("====== OBJECTS =====")]
    [SerializeField] GameObject _clipBoardPrefab;
    [SerializeField] GameObject _buttonsPrefab;

    [Header("====== SUSPECTS =====")]
    [HideInInspector] public Dictionary<SuspectData, GameObject> _allSuspects = new Dictionary<SuspectData, GameObject>();

    [Header("====== PARENTS ======")]
    [SerializeField] GameObject _suspectParent;
    [SerializeField] GameObject _objectParent;
    [SerializeField] GameObject _roomParent;

    int nbSuspect = 0;

    public void ChangeLight(int index, Color color, float intensity)
    {
        foreach (Light light in _lights[index])
        {
            light.color = color;
            light.intensity = intensity;
        }
    }

    public SuspectData[] GetSuspects(List<SuspectData> suspects)
    {
        if (_currentSuspects.Length > 0) return _currentSuspects;
        // Only get a number of suspect depend on the difficulty
         nbSuspect = suspects.Count;
        if (_level == Level.Easy) nbSuspect = 3;
        if (_level == Level.Normal) nbSuspect = 5;
        if (_level == Level.Hard) nbSuspect = 8;

        List<SuspectData> currentSuspects = new();
        for (int i = 0; i < nbSuspect; i++)
        {
            int rng = Random.Range(0, suspects.Count);
            SuspectData suspect = suspects[rng];
            currentSuspects.Add(suspect);
            suspects.Remove(suspect);
        }

        return currentSuspects.ToArray();
    }



    IEnumerator TimerC(float time)
    {
        _gameRun = true;
        // _startScreamer = false;
        yield return new WaitForSeconds(time/2);
        // _startScreamer = true;
        yield return new WaitForSeconds(time/2);
        _gameRun = false;
    }

    void Update()
    {
        _time += Time.deltaTime;
        
        if (livedNb == nbSuspect-1)
            _gameRun = false;
        if (KilledNb == nbSuspect/3)
            _gameRun = false;

        if (!_gameRun)
            EndGame();
    }

    void EndGame()
    {
        _UIRecap.CreateRecap();
        Destroy(this);
    }

    IEnumerator Start()
    {
        _level = DifficultyManager.Instance.Difficulty;
        StartCoroutine(TimerC(_timeMax));
        _currentSuspects = GetSuspects(_suspects);

        while (!SuspectsManager.Instance.suspectsInitiated)
            yield return null;

        for (int i = 0; i < nbSuspect; i++)
        {
            SuspectData suspect = _currentSuspects[i];
            suspect.s_level = _level;

            // Create suspects
            GameObject[] model = SuspectGeneration.Instance.GenerateSuspect(
                suspect.s_sex[0], suspect.hair_id, suspect.hair_color, suspect.shirt_color, suspect.pants_color, suspect.scary_color
            );
            Destroy(model[1].gameObject);
            model[0].transform.position = new Vector3(i*10, 0, 1f);
            model[0].transform.rotation = Quaternion.Euler(0, 180, 0);
            model[0].transform.localScale = Vector3.one * 1.69f;
            model[0].transform.parent = _suspectParent.transform;
            model[0].transform.name = $"suspect_{suspect.s_ID} -> {suspect.s_choice}";

            if (model[0].TryGetComponent(out Animator anim))
                anim.SetBool("sit", true);
            else
            {
                model[0].AddComponent<Animator>();
                if (model[0].TryGetComponent(out Animator animator))
                {
                    animator.runtimeAnimatorController = _animController;
                    animator.avatar = _avatar;
                    animator.applyRootMotion = true;
                    animator.animatePhysics = true;
                    animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    animator.SetBool("sit", true);
                }
            }

            // Add suspect Exrpession Component to manage the fac expression
            model[0].AddComponent<SuspectExpression>();

            _allSuspects.Add(suspect, model[0]);
        }

        // generating the rooms
        _wallBegin.transform.position = new Vector3(0, 0, -10);
        _wallEnd.transform.position = new Vector3((nbSuspect-1)*10.1f, 0, -10);
        for (int i = 0; i < nbSuspect; i++)
        {
            GameObject newRoom = Instantiate(_roomPrefab);
            newRoom.transform.position = new Vector3(i*10, 0, 0);
            newRoom.transform.parent = _roomParent.transform;

            Light[] lights = newRoom.GetComponentsInChildren<Light>();
            _lights.Add(lights);
        }

        // Genereting Objects
        // -> clip board
        for (int i = 0; i < nbSuspect; i++)
        {
            GameObject clipBoard = Instantiate(_clipBoardPrefab);
            clipBoard.transform.localPosition = new Vector3(i*10.1f - 1, 1.69f, 0);
            clipBoard.transform.localRotation = Quaternion.identity;
            clipBoard.transform.parent = _objectParent.transform;
            clipBoard.transform.localScale = 0.5f * Vector3.one;

            if (clipBoard.TryGetComponent(out CreateSuspectClipBoard cscb))
                cscb._index = i;
        }

        // -> buttons
        for (int i = 0; i < nbSuspect; i++)
        {
            GameObject buttons = Instantiate(_buttonsPrefab);
            buttons.transform.localPosition = new Vector3(i*10.1f, 1.69f, 0);
            buttons.transform.localRotation = Quaternion.identity;
            buttons.transform.parent = _objectParent.transform;

            IButton[] buttonsScript = buttons.GetComponentsInChildren<IButton>();

            foreach (IButton button in buttonsScript)
                button.SetIndex(i);
        }

    }
}
