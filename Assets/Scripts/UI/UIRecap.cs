using System;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRecap : MonoBehaviour
{
    [SerializeField] GameObject _recapPrefab;
    [SerializeField] GameObject _recapScreen;
    [SerializeField] GameObject _parent;
    [SerializeField] GameObject _buttons;

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void CreateRecap()
    {
        _recapScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        SuspectData[] suspects = RoomManager.Instance._currentSuspects;

        foreach (SuspectData suspect in suspects)
        {
            var recap = Instantiate(_recapPrefab);
            recap.transform.parent = _parent.transform;
            recap.SetActive(true);
            Image[] images = recap.GetComponentsInChildren<Image>(); 
            TextMeshProUGUI[] texts = recap.GetComponentsInChildren<TextMeshProUGUI>();

            Image frame = null, picture = null;
            foreach (Image img in images)
            {
                if (img.transform.name.ToLower().Contains("suspectrecap"))
                    frame = img;
                if (img.transform.name.ToLower().Contains("picture"))
                    picture = img;
            }

            TextMeshProUGUI name = null, firstName = null, status = null, verdict = null;
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.transform.name.ToLower().Contains("name"))
                    name = text;
                if (text.transform.name.ToLower().Contains("firstname"))
                    firstName = text;
                if (text.transform.name.ToLower().Contains("status"))
                    status = text;
                if (text.transform.name.ToLower().Contains("verdict"))
                    verdict = text;
            }

            picture.sprite = suspect.picture;
            name.text = suspect.s_name[0];
            firstName.text = suspect.s_firstName[0];
            status.text = RoomManager.Instance._allSuspects[suspect].activeInHierarchy ? "Lived" : "Killed";
            verdict.text = suspect.s_choice == SuspectChoice.Lie ? "Guilty" : "Not Guilty";

            bool correct = RoomManager.Instance._allSuspects[suspect].activeInHierarchy && suspect.s_choice == SuspectChoice.Truth;
            if (correct)
                frame.color = Color.green;
            else
                frame.color = Color.red; 
        }
        GameObject buttons = Instantiate(_buttons);
        buttons.transform.SetParent(_parent.transform);
        buttons.SetActive(true);
    }
}
