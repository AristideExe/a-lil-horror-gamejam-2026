using System;
using System.Collections;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject _interactIcon;

    private bool _showInteractIcon;
    // private Coroutine _interactIconCoroutine;
    
    private bool _shouldShowIcon;

    public void ShowInteractIcon()
    {
        _showInteractIcon = true;
        StartCoroutine(RemoveInteractIcon());
    }

    private void Update()
    {
        if (_showInteractIcon) _interactIcon.SetActive(true);
        else _interactIcon.SetActive(false);
    }

    private IEnumerator RemoveInteractIcon()
    {
        yield return new WaitForEndOfFrame();
        _showInteractIcon = false;
    }
}
