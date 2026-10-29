using UnityEngine;
using System.Collections.Generic;

public class SuspectExpression : MonoBehaviour
{
    public void UpdateFace(int index)
    {
        Dictionary<SuspectData, GameObject> allSuspects = RoomManager.Instance._allSuspects;
        SuspectData[] suspectDatas = RoomManager.Instance._currentSuspects;

        SuspectData suspectData = suspectDatas[index];

        SkinnedMeshRenderer[] allRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer head = null;
        foreach (var rend in allRenderers)
        {
            if (rend.name.ToLower().Contains("head"))
                head = rend;
        }

        float sad = suspectData.sad;
        float happy = suspectData.happy;
        float angry = suspectData.angry;
        float surprise = suspectData.surprise;

        Debug.Log($"sad {sad} happy {happy} angery {angry} surprise {surprise}");

        // -> SAD
        head.SetBlendShapeWeight(4, sad*100);
        head.SetBlendShapeWeight(9, sad*100);
        head.SetBlendShapeWeight(13, sad*100);
        // -> HAPPY
        head.SetBlendShapeWeight(2, happy*100);
        head.SetBlendShapeWeight(7, happy*100);
        head.SetBlendShapeWeight(14, happy*100);
        // -> ANGRY
        head.SetBlendShapeWeight(3, angry*100);
        head.SetBlendShapeWeight(8, angry*100);
        head.SetBlendShapeWeight(12, angry*100);
        // -> SURPRISE
        head.SetBlendShapeWeight(1, surprise*100);
        head.SetBlendShapeWeight(6, surprise*100);
        head.SetBlendShapeWeight(11, surprise*100);

        Debug.Log("Update Face");
    }
}