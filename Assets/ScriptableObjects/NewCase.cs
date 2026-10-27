using UnityEngine;

[CreateAssetMenu(fileName = "NewCase", menuName = "Scriptable Objects/Case")]
public class CaseData : ScriptableObject
{
    
    [Header(">====== CASE ======<")]
    [TextArea(3, 8)]
    public string context;

    public string accusation;

    [Header(">====== SUSPECTS ======<")]
    public SuspectData[] suspects;
}
