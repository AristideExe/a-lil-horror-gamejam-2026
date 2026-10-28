using Unity.VisualScripting;
using UnityEngine;

public class ClickOnObject : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] Camera _cam;
    [Tooltip("The maximum distance for the ray to find clicked object")]
    float _dist = 100f;

    // Parent Managment
    [SerializeField] Transform _zoomParent;

    // Default var
    Transform _oldParent;
    Vector3 _oldPos;
    Quaternion _oldRot;
    int _defaultLayerMask;
    void Awake()
    {
        _cam = Camera.main;
        _oldParent = transform.parent;
        _oldParent = transform.parent;      
        _oldPos = transform.position;   
        _oldRot = transform.rotation;   
        _defaultLayerMask = gameObject.layer;
    }

    public void Reset()
    {
        // SetLayer(gameObject, _defaultLayerMask);
        // _blurBackground.SetActive(false);

        transform.position = _oldPos;
        transform.rotation = _oldRot;
        transform.parent = _oldParent;

        Cursor.lockState = CursorLockMode.Locked;
        MouseLook.Instance._canLook = true;
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _dist))
        {
            if (hit.transform != transform) return;

            if (Input.GetMouseButtonDown(0))
            {
                transform.parent = _zoomParent;

                transform.localPosition = Vector3.zero;
                if (TryGetComponent(out RotationScript rs))
                {
                    rs._target = transform;
                    rs.enabled = true;
                    rs.StartScript();
                }
                this.enabled = false;
            }
        }
    }
}
