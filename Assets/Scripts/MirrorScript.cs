using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    [SerializeField] RenderTexture _texture;
    [SerializeField] Vector2 _resolution = new Vector2(256, 256);
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] Material _mirrorMaterial;
    [SerializeField] Camera _cam;

    void Start()
    {
        // create a new RenderTexture with the same dimensions as the camera's target texture
        _texture = new RenderTexture((int)_resolution.x, (int)_resolution.y, 24);
        _cam.targetTexture = _texture;

        Material mirrorMaterial = new Material(_mirrorMaterial)
        {
            mainTexture = _texture
        };
        _meshRenderer.material = mirrorMaterial;
    }
}
