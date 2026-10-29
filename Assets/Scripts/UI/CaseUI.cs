using System;
using TMPro;
using UnityEngine;

public class CaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI caseNameText;
    
    private void Start()
    {
        gameObject.SetActive(true);
        caseNameText.text = SuspectsManager.Instance.CurrentCases.context;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }
}
