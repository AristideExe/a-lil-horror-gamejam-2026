using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

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

    [Header("====== ROOM ======")]
    [SerializeField] GameObject _roomPrefab;
    [SerializeField] GameObject _wallBegin;
    [SerializeField] GameObject _wallEnd;

    [Header("====== DIFFICULTY ======")]
    [SerializeField] Level _level;

    [Header("====== ANIMATION ======")]
    [SerializeField] AnimatorController _animController;
    [SerializeField] Avatar _avatar;

    [Header("====== OBJECTS =====")]
    [SerializeField] GameObject _clipBoardPrefab;
    [SerializeField] Camera _playerCam;
    [SerializeField] Camera _overleyCam;
    [SerializeField] GameObject _zoomParent;
    [SerializeField] int _overlayLayerMask;

    [Header("====== SUSPECTS =====")]
    [HideInInspector] public Dictionary<SuspectData, GameObject> _allSuspects = new Dictionary<SuspectData, GameObject>();

    [Header("====== PARENTS ======")]
    [SerializeField] GameObject _suspectParent;
    [SerializeField] GameObject _objectParent;
    [SerializeField] GameObject _roomParent;

    int nbSuspect = 0;
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

    public void Start()
    {
        _currentSuspects = GetSuspects(_suspects);

        Debug.Log($"nbSustect: {nbSuspect}");

        for (int i = 0; i < nbSuspect; i++)
        {
            SuspectData suspect = _currentSuspects[i];

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
        _wallBegin.transform.position = new Vector3(0, 0, -5);
        _wallEnd.transform.position = new Vector3((nbSuspect-1)*10, 0, -5);
        for (int i = 0; i < nbSuspect; i++)
        {
            GameObject newRoom = Instantiate(_roomPrefab);
            newRoom.transform.position = new Vector3(i*10, 0, 0);
            newRoom.transform.parent = _roomParent.transform;
        }

        // Genereting Objects
        // -> clip board
        for (int i = 0; i < nbSuspect; i++)
        {
            GameObject clipBoard = Instantiate(_clipBoardPrefab);
            clipBoard.transform.localPosition = new Vector3(i*10 - 1, 1.69f, 0);
            clipBoard.transform.localRotation = Quaternion.identity;
            clipBoard.transform.parent = _objectParent.transform;
            clipBoard.transform.localScale = 0.5f * Vector3.one;

            if (clipBoard.TryGetComponent(out CreateSuspectClipBoard cscb))
                cscb._index = i;

            ClickOnObject clickOnObject = clipBoard.GetComponentInChildren<ClickOnObject>();
            clickOnObject._cam = _playerCam;
            clickOnObject._overlayCam = _overleyCam;
            clickOnObject._zoomParent = _zoomParent.transform;
            clickOnObject._overlayCam.gameObject.layer = _overlayLayerMask;
            clickOnObject._overlayLayerMask = _overlayLayerMask;
        }

    }
}
