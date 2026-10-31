using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UITime : MonoBehaviour
{
    public static UITime Instance {private set; get;}
    private TextMeshProUGUI text;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int total = (int)RoomManager.Instance._timeMax - (int)RoomManager.Instance._time;
        int min = (int)(total / 60f);
        int sec = total - min * 60;
        if (min > 0)
            text.text = $"{min}:{sec:00}";
        else
            text.text= $"{sec:00}";
    }

    private float duration = 1f; 

    public void AnimateTime()
    {
        StartCoroutine(AnimateTimeC());
    }

    IEnumerator AnimateTimeC()
    {
        float current = 0;
        float targetScale = 1f;
        while (current < duration)
        {
            current += Time.deltaTime;

            float scale = Mathf.Lerp(10, targetScale, current/duration);
            transform.parent.localScale = Vector3.one * scale;

            yield return null;
        }
        transform.parent.localScale = Vector3.one;

    }
}
