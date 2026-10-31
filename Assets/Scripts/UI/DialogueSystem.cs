using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance {private set; get;}

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] public GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextMeshProUGUI _pageCount;

    [SerializeField] float _delay;
    private bool _isTyping = false;

    private SuspectData _suspect;

    public void ShowDialogue(string name, string text, SuspectData suspect)
    {
        // this function allow us to show a diaologue in game
        if (suspect.status == 3)
        {
            text = SuspectsManager.Instance.GenerateAnswer(QuestionChoice.Null, " ");   
        }

        dialogueBox.SetActive(true);
        _name.text = name;
        _text.text = text;
        _text.pageToDisplay = 0;
        _suspect = suspect;
        Next();
    }

    void Start()
    {
        // ShowDialogue("Jean Dupond", "Breakfast procuring nay end happiness allowance assurance frankness. Met simplicity nor difficulty unreserved who. Entreaties mr conviction dissimilar me astonished estimating cultivated. On no applauded exquisite my additions. Pronounce add boy estimable nay suspected. You sudden nay elinor thirty esteem temper. Quiet leave shy you gay off asked large style.\n\nIs he staying arrival address earnest. To preference considered it themselves inquietude collecting estimating. View park for why gay knew face. Next than near to four so hand. Times so do he downs me would. Witty abode party her found quiet law. They door four bed fail now have.\n\nHer companions instrument set estimating sex remarkably solicitude motionless. Property men the why smallest graceful day insisted required. Inquiry justice country old placing sitting any ten age. Looking venture justice in evident in totally he do ability. Be is lose girl long of up give. Trifling wondered unpacked ye at he. In household certainty an on tolerably smallness difficult. Many no each like up be is next neat. Put not enjoyment behaviour her supposing. At he pulled object others.\n\nAlteration literature to or an sympathize mr imprudence. Of is ferrars subject as enjoyed or tedious cottage. Procuring as in resembled by in agreeable. Next long no gave mr eyes. Admiration advantages no he celebrated so pianoforte unreserved. Not its herself forming charmed amiable. Him why feebly expect future now.\n\nBuilt purse maids cease her ham new seven among and. Pulled coming wooded tended it answer remain me be. So landlord by we unlocked sensible it. Fat cannot use denied excuse son law. Wisdom happen suffer common the appear ham beauty her had. Or belonging zealously existence as by resources.");
    }

    void Update()
    {
        if (!_isTyping && Input.anyKeyDown)
            Next();
    }

    IEnumerator ShowTextRoutine(TextMeshProUGUI tmpText)
    {
        _isTyping = true;

        int startIndex = GetFirstIndex(tmpText);
        int lastIndex = GetLastIndex(tmpText);

        // Get suspect head for mouth animation
        SkinnedMeshRenderer[] allRenderers = RoomManager.Instance._allSuspects[_suspect].GetComponentsInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer head = null;
        foreach (var rend in allRenderers)
        {
            if (rend.name.ToLower().Contains("head"))
                head = rend;
        }

        tmpText.maxVisibleCharacters = startIndex;
        yield return null; // Wait one fram to avoid skipping the animation
        StartCoroutine(MouthTalking(head));
        for (int i = startIndex; i <= lastIndex; i++)
        {
            tmpText.maxVisibleCharacters = i;

            float timer = 0f;
            while (timer < _delay)
            {
                if (Input.anyKeyDown)
                {
                    tmpText.maxVisibleCharacters = lastIndex;
                    head.SetBlendShapeWeight(11, 0); 
                    _isTyping = false;
                    yield break;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return null; 
        _isTyping = false;
    }

    Quaternion rot = Quaternion.identity;
    Vector3 pos = Vector3.one;
    IEnumerator MouthTalking(SkinnedMeshRenderer head)
    {
        Animator anim =  RoomManager.Instance._allSuspects[_suspect].GetComponent<Animator>();

        if (!anim.GetBool("talking"))
        {
            rot = Camera.main.transform.rotation;
            pos = Camera.main.transform.localPosition;
            Camera.main.transform.LookAt(head.transform);
            Camera.main.transform.localPosition = new Vector3(0,1.3f,0);
        }

        anim.SetBool("talking", true);

        while (dialogueBox.activeInHierarchy)
        {
            // Animate the mouth
            float speed = 15f;
            float mouthOpen = Mathf.Abs(Mathf.Sin(Time.time * speed)/3f);
            head.SetBlendShapeWeight(11, (int)(mouthOpen * 100));
            yield return null;
        }
        head.SetBlendShapeWeight(11, 0);
        Camera.main.transform.rotation = rot;
        Camera.main.transform.localPosition = pos;

        anim.SetBool("talking", false);
    }

    int GetFirstIndex(TextMeshProUGUI tmpText)
    {
        int pageIndex = tmpText.pageToDisplay - 1;

        TMP_PageInfo pageInfo = tmpText.textInfo.pageInfo[pageIndex];

        return pageInfo.firstCharacterIndex;
    }

    int GetLastIndex(TextMeshProUGUI tmpText)
    {
        return GetFirstIndex(tmpText) + GetTextLength(tmpText);
    }

    int GetTextLength(TextMeshProUGUI tmpText)
    {
        int pageIndex = tmpText.pageToDisplay - 1;

        TMP_PageInfo pageInfo = tmpText.textInfo.pageInfo[pageIndex];

        int startIndex = pageInfo.firstCharacterIndex;

        return pageInfo.lastCharacterIndex - startIndex + 1;
    }

    string GetCurrentPageText(TextMeshProUGUI tmpText)
    {
        int startIndex = GetFirstIndex(tmpText);
        int length = GetTextLength(tmpText);

        if (startIndex + length > tmpText.text.Length)
            length = tmpText.text.Length - startIndex;
        return tmpText.text.Substring(startIndex, length);
    }

    void Next()
    {
        _text.ForceMeshUpdate();

        if (_text.pageToDisplay < _text.textInfo.pageCount)
        {
            _text.pageToDisplay++;

            _pageCount.text = $"{_text.pageToDisplay}/{_text.textInfo.pageCount}";

            StartCoroutine(ShowTextRoutine(_text));
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }

}
