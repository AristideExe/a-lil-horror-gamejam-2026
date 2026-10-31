using System.Collections;
using UnityEngine;
using UnityEngine.UI;

interface IButton
{
    public void SetIndex(int i);
    public void Interact();
    public IEnumerator ClickButton();
}
interface IObject
{
    public void Interact();
}

public class ClickOnObject : MonoBehaviour
{
    public static ClickOnObject Instance {private set; get;}

    public int _overlayLayerMask;
    private Transform currentObject;
    [Header("Cameras")]
    public Camera _cam;
    public Camera _overlayCam;
    [Tooltip("The maximum distance for the ray to find clicked object")]
    float _dist = 100f;

    // Parent Managment
    public Transform _zoomParent;

    // Default var
    Transform _oldParent;
    Vector3 _oldPos;
    Quaternion _oldRot;
    int _defaultLayerMask;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _cam = Camera.main;
           
        _defaultLayerMask = gameObject.layer;
    }

    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }

    public void ResetObject(Transform transform)
    {
        Debug.Log("REST : " + currentObject.name);
        SetLayer(transform.gameObject, 0);
        // _blurBackground.SetActive(false);

        transform.position = _oldPos;
        transform.rotation = _oldRot;
        transform.parent = _oldParent;

        // Disable all buttons
        Button[] allButtons = GetComponentsInChildren<Button>();
        foreach (Button b in allButtons)
            b.enabled = false;

        // Disable rotationScript
        if (currentObject.TryGetComponent(out RotationScript rs))
            rs.enabled = false;
        this.enabled = true;
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(_cam.transform.position, Input.mousePosition - _cam.transform.position, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, _dist))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.TryGetComponent(out IButton button))
                    button.Interact();

                if (hit.transform.TryGetComponent(out IObject obj))
                {
                    currentObject = hit.transform;

                    // Remember everything
                    _oldParent = hit.transform.parent;      
                    _oldPos = hit.transform.position;   
                    _oldRot = hit.transform.rotation;

                    hit.transform.parent = _zoomParent;

                    // Set the layer of the object and its children to the overlay layer
                    SetLayer(hit.transform.gameObject, _overlayLayerMask);
                    
                    // Enable all buttons
                    Button[] allButtons = GetComponentsInChildren<Button>();
                    foreach (Button b in allButtons)
                        b.enabled = true;

                    hit.transform.localPosition = Vector3.zero;
                    if (hit.transform.TryGetComponent(out RotationScript rs))
                    {
                        rs._target = hit.transform;
                        rs.enabled = true;
                    }
                    this.enabled = false;
                }
            }
        }
    }
}
