using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
public class GuiltyButton : MonoBehaviour, IButton
{
    public int index;
    
    public void SetIndex(int i)
    {
        index = i;
    }

    [SerializeField] ParticleSystem _explosion;
    public void Interact()
    {
        // Press Animation
        StartCoroutine(ClickButton());
        // Explosion of Suspect
        GameObject suspect = RoomManager.Instance._allSuspects[RoomManager.Instance._currentSuspects[index]].gameObject;
        ParticleSystem explosion = Instantiate(_explosion);
        explosion.transform.position = suspect.transform.position + Vector3.up;
        explosion.Play();
        RoomManager.Instance.ChangeLight(index, Color.red, 3f);
        suspect.SetActive(false);
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
