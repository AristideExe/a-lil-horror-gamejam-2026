using UnityEngine;

[CreateAssetMenu(fileName = "NewCase", menuName = "Scriptable Objects/Case")]
public class CaseData : ScriptableObject
{
    
    [Header(">====== CASE ======<")]
    public int id; 

    [TextArea(3, 8)]
    public string context;

    public string facts;

    [Header(">====== SUSPECTS ======<")]
    public SuspectData[] suspects;
}
