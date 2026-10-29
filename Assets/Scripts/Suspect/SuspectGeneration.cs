using UnityEngine;
using System.Collections.Generic;

public class ID
{
    public Sprite picture;
    public GameObject[] models;

    public ID(Sprite picture, GameObject[] models)
    {
        this.picture = picture;
        this.models = models;
    }

}

public class SuspectGeneration : MonoBehaviour
{
    public static SuspectGeneration Instance {private set; get;}

    void Awake()
    {
        if (Instance == null) Instance = this;
        else return;
    }

    [Header("=== Create Sprite ===")]
    [SerializeField] Camera _cam;
    [SerializeField] int _textureWidth;
    [SerializeField] int _textureHeight;

    [Header("=== Prefab & References ===")]
    [SerializeField] GameObject _characterPrefab;
    [SerializeField] Transform _spawnPoint;
    
    SkinnedMeshRenderer _hair;
    SkinnedMeshRenderer _pants;
    SkinnedMeshRenderer _shirt;

    [Header("=== Materials ===")]
    [SerializeField] Material[] _hairMaterials;
    [SerializeField] Material[] _clothingMaterials;
    [SerializeField] Material[] _scarySkin;

    [Header("=== Output ===")]
    public Sprite suspectPortrait;

    public GameObject[] GenerateSuspect(string gender, int hair_id, int hair_color, int shirt_color, int pants_color, int scary_color)
    {
        Debug.Log($"Generate Suspect : {gender}, {hair_id}, {hair_color}, {shirt_color}, {pants_color}, {scary_color}");
        // Create new blank suspect
        GameObject _currentInstance = Instantiate(_characterPrefab);
        
        // Get the suspect's parts
        SkinnedMeshRenderer[] allRenderers = _currentInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var rend in allRenderers)
        {
            if (rend.name.ToLower().Contains("pants"))
                _pants = rend;
            if (rend.name.ToLower().Contains("shirt"))
                _shirt = rend;
            if (rend.name.ToLower().Contains($"hair.{gender.ToLower()}.{hair_id}"))
                _hair = rend;
                    
        }

        // Generate colors
        if (_hairMaterials.Length > 0)
        {
            Material hairMaterial = _hairMaterials[hair_color];
            if (_hair != null) _hair.material = hairMaterial;
        }

        if (_clothingMaterials.Length > 0)
        {
            Material shirtMaterial = _clothingMaterials[shirt_color];
            Material pantsMaterial = _clothingMaterials[pants_color];

            if (_pants != null) _pants.material = pantsMaterial;
            if (_shirt != null) _shirt.material = shirtMaterial;
        }

        // Create the scary version with red skin
        GameObject _scaryInstance = Instantiate(_currentInstance);
        allRenderers = _currentInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
        Material scarySkin = _scarySkin[scary_color];
        foreach (var rend in allRenderers)
        {
            if (rend.name.ToLower().Contains("main"))
                rend.material = scarySkin;
            if (rend.name.ToLower().Contains("head"))
                rend.material = scarySkin;
        }

        GameObject[] models = new GameObject[2]
        {
            _scaryInstance, _currentInstance
        };

        return models;
    }

    public Sprite CreatePicture(GameObject target, Vector3 camPosition)
    {
        if (_cam == null)
        {
            Debug.LogError("Aucune caméra de capture assignée !");
            return null;
        }
        
        _cam.transform.Rotate(0, 180, 0);
        _cam.transform.position = camPosition;
        //_cam.transform.LookAt(target.transform.position);

        // Create RenderTexture
        RenderTexture renderTex = new RenderTexture(_textureWidth, _textureHeight, 24);
        _cam.targetTexture = renderTex;

        _cam.Render();

        // Convert to Texture2D
        RenderTexture.active = renderTex;
        Texture2D texture2D = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, _textureWidth, _textureHeight), 0, 0);
        texture2D.Apply();

        _cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTex);

        // Create Sprite
        Sprite picture = Sprite.Create(
            texture2D, 
            new Rect(0, 0, _textureWidth, _textureHeight), 
            new Vector2(0.5f, 0.5f)
        );

        return picture;
    }

    public ID CreateID(string gender, int hair_id, int hair_color, int shirt_color, int pants_color, int scary_color)
    {
        GameObject[] models = GenerateSuspect(gender,hair_id, hair_color, shirt_color, pants_color, scary_color);

        // Create the suspect at the pictureRoom
        models[0].transform.position = new Vector3(0, 20, 0);

        Sprite picture = CreatePicture(models[0], new Vector3(0, 20 + 1.69f, .5f));

        Destroy(models[0].gameObject);
        Destroy(models[1].gameObject);

        return new ID(picture, models);
    }
}