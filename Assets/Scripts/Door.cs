using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float manualOpenSpeed = 4f;
    [SerializeField] private float automaticCloseSpeed = 2f;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float waitBeforeClose = 2f;
    
    private bool _isOpen;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _coroutine;

    private void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new  Vector3(0, openAngle, 0));
    }

    public string Interaction => _isOpen ? "close door" : "open door";

    public void Interact(GameObject interactor)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ToggleDoor(true));
    }

    private IEnumerator ToggleDoor(bool manual)
    {
        Quaternion targetRotation = _isOpen ? _closedRotation : _openRotation;
        float speed =  manual ? manualOpenSpeed : automaticCloseSpeed;
        _isOpen = !_isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }

        transform.rotation = targetRotation;
        if (manual && _isOpen) _coroutine = StartCoroutine(CloseAutomatically());
    }

    private IEnumerator CloseAutomatically()
    {
        yield return new WaitForSeconds(waitBeforeClose);
        _coroutine = StartCoroutine(ToggleDoor(false));
    }
}
