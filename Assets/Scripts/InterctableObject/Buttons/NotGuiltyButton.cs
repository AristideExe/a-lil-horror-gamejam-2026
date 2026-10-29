using UnityEngine;
using System.Collections;
public class NotGuiltyButton : MonoBehaviour, IButton
{
    public int index;

    public void SetIndex(int i)
    {
        index = i;
    }
    public void Interact()
    {
        // Press Animation
        StartCoroutine(ClickButton());
        // Change Lights To Green
        RoomManager.Instance.ChangeLight(index, Color.green, 3f);
        // End of game
    }

    public IEnumerator ClickButton()
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPos = startPosition - new Vector3(0, 0.8f, 0); 
        
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (duration / 2f));
            transform.localPosition = Vector3.Lerp(startPosition, targetPos, t);
            yield return null;
        }

        elapsedTime = duration / 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01((elapsedTime - (duration / 2f)) / (duration / 2f));
            transform.localPosition = Vector3.Lerp(targetPos, startPosition, t);
            yield return null;
        }
        transform.localPosition = startPosition;
    }
}
