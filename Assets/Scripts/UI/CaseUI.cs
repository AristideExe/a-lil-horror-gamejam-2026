using System;
using TMPro;
using UnityEngine;

public class CaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI caseNameText;
    private bool canFinish = false;
    
    private void Start()
    {
        gameObject.SetActive(true);
        caseNameText.text = SuspectsManager.Instance.CurrentCases.context.Split("\n")[0];
    }

    void Next()
    {
        caseNameText.text = SuspectsManager.Instance.CurrentCases.context.Split("\n")[1];
        canFinish = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFinish)
            gameObject.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Space) && !canFinish)
            Next();

    }
}
