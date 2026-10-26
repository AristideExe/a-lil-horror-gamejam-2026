using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class CreateSuspectClipBoard : MonoBehaviour
{
    SuspectData _suspect;
    [Tooltip("-> Room Index")]
    [SerializeField] int _index;

    [Header(">====== UI ELEMENTS ======<")]
    [SerializeField] Sprite _picture;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _firstName;
    [SerializeField] TextMeshProUGUI _pseudo;
    void Start()
    {
        _suspect = SuspectsManager.Instance._suspects[_index];
    }
}
