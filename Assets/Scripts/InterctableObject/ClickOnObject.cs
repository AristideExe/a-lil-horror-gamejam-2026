using UnityEngine;
using UnityEngine.UI;

public class ClickOnObject : MonoBehaviour
{
    public int _overlayLayerMask;
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
        _cam = Camera.main;
           
        _defaultLayerMask = gameObject.layer;
        SetLayer(gameObject, 0);
    }

    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }

    public void Reset()
    {
        SetLayer(gameObject, 0);
        // _blurBackground.SetActive(false);

        transform.position = _oldPos;
        transform.rotation = _oldRot;
        transform.parent = _oldParent;

        // Disable all buttons
        Button[] allButtons = GetComponentsInChildren<Button>();
        foreach (Button b in allButtons)
            b.enabled = false;
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(_cam.transform.position, Input.mousePosition - _cam.transform.position, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, _dist))
        {
            // Debug.Log(hit.transform.name);
            if (hit.transform != transform) return;

            if (Input.GetMouseButtonDown(0))
            {
                // Remember everything
                _oldParent = transform.parent;      
                _oldPos = transform.position;   
                _oldRot = transform.rotation;

                transform.parent = _zoomParent;

                // Set the layer of the object and its children to the overlay layer
                SetLayer(gameObject, _overlayLayerMask);
                
                // Enable all buttons
                Button[] allButtons = GetComponentsInChildren<Button>();
                foreach (Button b in allButtons)
                    b.enabled = true;

                transform.localPosition = Vector3.zero;
                if (TryGetComponent(out RotationScript rs))
                {
                    rs._target = transform;
                    rs.enabled = true;
                }
                this.enabled = false;
            }
        }
    }
}
