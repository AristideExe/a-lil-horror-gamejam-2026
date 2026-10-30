using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float manualOpenSpeed = 4f;
    [SerializeField] private float automaticCloseSpeed = 2f;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float waitBeforeClose = 2f;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closeDoorSound;
    [SerializeField] private AudioClip _screamerSound;
    
    private bool _isOpen;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _coroutine;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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

    // void Screamer()
    // {
    //     if (RoomManager.Instance._startScreamer)
    //     {
    //         if ((int)Random.Range(0, (RoomManager.Instance._timeMax - RoomManager.Instance._time)/30f) < 1);
    //         {
    //             Debug.Log("SCREAMER");
    //             // generate Screamer
    //             GameObject screamer = Instantiate(RoomManager.Instance._screamers[Random.Range(0, RoomManager.Instance._screamers.Length)]);
    //             screamer.transform.position = transform.position;
    //             _audioSource.PlayOneShot(_screamerSound);
    //         }
    //     }
    // }

    private IEnumerator ToggleDoor(bool manual)
    {
        // Screamer();

        Quaternion targetRotation = _isOpen ? _closedRotation : _openRotation;
        float speed =  manual ? manualOpenSpeed : automaticCloseSpeed;
        _audioSource.PlayOneShot(_isOpen ? closeDoorSound : openDoorSound);
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
