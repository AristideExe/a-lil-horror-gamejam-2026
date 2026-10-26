using Unity.VisualScripting;
using UnityEngine;

public class ClickOnObject : MonoBehaviour
{
    [SerializeField] int _overlayLayerMask;
    [Header("Cameras")]
    [SerializeField] Camera _cam;
    [SerializeField] Camera _overlayCam;
    [Tooltip("The maximum distance for the ray to find clicked object")]
    float _dist = 100f;

    // Parent Managment
    [SerializeField] Transform _zoomParent;
    [SerializeField] GameObject _blurBackground;

    // Default var
    Transform _oldParent;
    Vector3 _oldPos;
    Quaternion _oldRot;
    LayerMask _defaultLayerMask;
    void Awake()
    {
        _cam = Camera.main;
        _oldParent = transform.parent;
        _overlayCam.gameObject.layer = _overlayLayerMask;
        _oldParent = transform.parent;      
        _oldPos = transform.position;   
        _oldRot = transform.rotation;   
    }

    public void Reset()
    {
        gameObject.layer = _defaultLayerMask;
        _blurBackground.SetActive(false);

        transform.position = _oldPos;
        transform.rotation = _oldRot;
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

                // set the object in the overlay layermask so it's on top of the others
                gameObject.layer = _overlayLayerMask;
                _blurBackground.SetActive(true);

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
