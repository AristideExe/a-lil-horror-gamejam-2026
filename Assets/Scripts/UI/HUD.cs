using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private TextMeshProUGUI interactText;

    [SerializeField] private bool _showInteractIcon;
    private string _interaction;
    private bool _shouldShowIcon;

    public void ShowInteractIcon(String interaction)
    {
        _showInteractIcon = true;
        _interaction = interaction;
        StartCoroutine(RemoveInteractIcon());
    }

    private void Update()
    {
        if (_showInteractIcon)
        {
            interactText.text = "Press E to " + _interaction;
            interactIcon.gameObject.SetActive(true);
        }
        else interactIcon.gameObject.SetActive(false);
    }

    private IEnumerator RemoveInteractIcon()
    {
        yield return new WaitForEndOfFrame();
        _showInteractIcon = false;
    }
}
