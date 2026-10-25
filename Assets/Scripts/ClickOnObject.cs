using Unity.VisualScripting;
using UnityEngine;

public class ClickOnObject : MonoBehaviour
{
    [SerializeField] int _overlayLayerMask;
    Camera _cam;
    [Tooltip("The maximum distance for the ray to find clicked object")]
    float _dist = 100f;

    // Parent Managment
    Transform _oldParent;
    [SerializeField] Transform _zoomParent;
    [SerializeField] GameObject _blurBackground;
    void Awake()
    {
        _cam = Camera.main;
        _oldParent = transform.parent;
    }

    void Onable()
    {
        transform.parent = _oldParent;        
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

                // set th e object in the overlay layermask so it's on top of the others
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
