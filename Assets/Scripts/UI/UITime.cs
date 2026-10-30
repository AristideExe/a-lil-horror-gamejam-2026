using TMPro;
using UnityEngine;

public class UITime : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int total = (int)RoomManager.Instance._time;
        int min = (int)(total / 60f);
        int sec = total - min * 60;
        if (min > 0)
            text.text = $"{min}m{sec}s";
        else
            text.text= $"{sec}s";
    }
}
