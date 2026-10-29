using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateSuspectClipBoard : MonoBehaviour
{
    SuspectData _suspect;
    [Tooltip("-> Room Index")]
    [SerializeField] int _index;

    [Header(">====== UI ELEMENTS ======<")]
    [SerializeField] Image _picture;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _firstName;
    [SerializeField] TextMeshProUGUI _pseudo;
    [SerializeField] TextMeshProUGUI _work;
    [SerializeField] TextMeshProUGUI _age;
    [SerializeField] TextMeshProUGUI _sex;
    [SerializeField] TextMeshProUGUI _height;
    [SerializeField] TextMeshProUGUI _weight;
    [SerializeField] TextMeshProUGUI _alibi;
    void Start()
    {
        if (SuspectsManager.Instance._suspects.Length == 0) return;
        _suspect = SuspectsManager.Instance._suspects[_index];

        _picture.sprite = _suspect.picture;

        _name.text = _suspect.s_name[0];
        _firstName.text = _suspect.s_name[0];
        _pseudo.text = _suspect.s_pseudo[0];
        _work.text = _suspect.s_work[0];
        _age.text = _suspect.s_age[0];
        _sex.text = _suspect.s_sex[0];
        _height.text = _suspect.s_height[0];
        _weight.text = _suspect.s_weight[0];
        _alibi.text = _suspect.s_alibi[0];
    }
}
