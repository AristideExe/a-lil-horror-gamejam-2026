using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject backMenu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private Toggle invertY;

    private void Awake()
    {
        audioMixer.GetFloat("Master", out float val);
        volumeSlider.value = val;
        sensitivityXSlider.value = MouseLook.Instance.MouseSensivityX;
        sensitivityYSlider.value = MouseLook.Instance.MouseSensivityY;
        invertY.isOn = MouseLook.Instance.InvertY;
    }
    
    public void Back()
    {
        backMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
    }
    
    public void SetSensitivityX(float sensitivity)
    {
        MouseLook.Instance.MouseSensivityX = sensitivity;
    }
    
    public void SetSensitivityY(float sensitivity)
    {
        MouseLook.Instance.MouseSensivityY = sensitivity;
    }
    
    public void SetInvertY(bool invert)
    {
        MouseLook.Instance.InvertY = invert;
    }
}
