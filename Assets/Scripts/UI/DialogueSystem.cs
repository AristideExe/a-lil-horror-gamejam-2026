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

    [SerializeField] GameObject _dialogueBox;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextMeshProUGUI _pageCount;

    [SerializeField] float _delay;
    private bool _isTyping = false;

    public void ShowDialogue(string name, string text)
    {
        // this function allow us to show a diaologue in game
        _dialogueBox.SetActive(true);
        _name.text = name;
        _text.text = text;
        _text.pageToDisplay = 0;
        Next();
    }

    void Start()
    {
        // ShowDialogue("Jean Dupond", "Breakfast procuring nay end happiness allowance assurance frankness. Met simplicity nor difficulty unreserved who. Entreaties mr conviction dissimilar me astonished estimating cultivated. On no applauded exquisite my additions. Pronounce add boy estimable nay suspected. You sudden nay elinor thirty esteem temper. Quiet leave shy you gay off asked large style.\n\nIs he staying arrival address earnest. To preference considered it themselves inquietude collecting estimating. View park for why gay knew face. Next than near to four so hand. Times so do he downs me would. Witty abode party her found quiet law. They door four bed fail now have.\n\nHer companions instrument set estimating sex remarkably solicitude motionless. Property men the why smallest graceful day insisted required. Inquiry justice country old placing sitting any ten age. Looking venture justice in evident in totally he do ability. Be is lose girl long of up give. Trifling wondered unpacked ye at he. In household certainty an on tolerably smallness difficult. Many no each like up be is next neat. Put not enjoyment behaviour her supposing. At he pulled object others.\n\nAlteration literature to or an sympathize mr imprudence. Of is ferrars subject as enjoyed or tedious cottage. Procuring as in resembled by in agreeable. Next long no gave mr eyes. Admiration advantages no he celebrated so pianoforte unreserved. Not its herself forming charmed amiable. Him why feebly expect future now.\n\nBuilt purse maids cease her ham new seven among and. Pulled coming wooded tended it answer remain me be. So landlord by we unlocked sensible it. Fat cannot use denied excuse son law. Wisdom happen suffer common the appear ham beauty her had. Or belonging zealously existence as by resources.");
    }

    void Update()
    {
        if (!_isTyping && Input.anyKeyDown)
        {
            Next();
        }
    }

    IEnumerator ShowTextRoutine(TextMeshProUGUI tmpText)
    {
        _isTyping = true;

        int startIndex = GetFirstIndex(tmpText);
        int lastIndex = GetLastIndex(tmpText);

        tmpText.maxVisibleCharacters = startIndex;
        yield return null; // Wait one fram to avoid skipping the animation
        for (int i = startIndex; i <= lastIndex; i++)
        {
            if (Input.anyKeyDown)
            {
                tmpText.maxVisibleCharacters = lastIndex;
                break;
            }

            tmpText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(_delay);
        }

        yield return null; 
        _isTyping = false;
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
            _dialogueBox.SetActive(false);
        }
    }

}
