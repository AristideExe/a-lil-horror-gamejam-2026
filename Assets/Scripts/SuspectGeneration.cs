using UnityEngine;
using System.Collections.Generic;

public class SuspectGeneration : MonoBehaviour
{
    [Header("=== Prefab & References ===")]
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private Transform _spawnPoint;
    
    [SerializeField]private List<SkinnedMeshRenderer> _hairs = new();
    [Tooltip("Number of hair pieces for the male variant")]
    [SerializeField] private int _hairNbH;
    [Tooltip("Number of hair pieces for the female variant")]
    [SerializeField] private int _hairNbF;
    [SerializeField]private SkinnedMeshRenderer _pants;
    [SerializeField]private SkinnedMeshRenderer _shirt;

    [Header("=== Materials ===")]
    [SerializeField] private Material[] _hairMaterials;
    [SerializeField] private Material[] _clothingMaterials;

    [Header("=== Output ===")]
    public Sprite suspectPortrait;

    private GameObject _currentInstance;

    public void GenerateSuspect(string gender)
    {
        if (_currentInstance != null)
            Destroy(_currentInstance);

        // Create new blank suspect
        Transform spawnParent = _spawnPoint != null ? _spawnPoint : transform;
        _currentInstance = Instantiate(_characterPrefab, spawnParent.position, spawnParent.rotation);
        
        // Get the suspect's parts
        SkinnedMeshRenderer[] allRenderers = _currentInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
        _hairs.Clear();
        foreach (var rend in allRenderers)
        {
            if (rend.name.ToLower().Contains("pants"))
                _pants = rend;
            if (rend.name.ToLower().Contains("shirt"))
                _shirt = rend;
            if (gender.ToLower() == "h")
                if (rend.name.ToLower().Contains("hair.h"))
                    _hairs.Add(rend);
            else
                if (rend.name.ToLower().Contains("hair.f"))
                        _hairs.Add(rend);
        }
        // Generate random hair
        SkinnedMeshRenderer currentHair = null;
        if (_hairs.Count > 0)
        {
            int randomIndex = Random.Range(0, _hairs.Count);
            currentHair = _hairs[randomIndex];
            for (int i = 0; i < _hairs.Count; i++)
                _hairs[i].gameObject.SetActive(i == randomIndex);
        }

        // Generate colors
        if (_hairMaterials.Length > 0)
        {
            Material hairMaterial = _hairMaterials[Random.Range(0, _hairMaterials.Length)];
            if (currentHair != null) currentHair.material = hairMaterial;
        }

        if (_clothingMaterials.Length > 0)
        {
            Material shirtMaterial = _clothingMaterials[Random.Range(0, _clothingMaterials.Length)];
            Material pantsMaterial = _clothingMaterials[Random.Range(0, _clothingMaterials.Length)];

            if (_pants != null) _pants.material = pantsMaterial;
            if (_shirt != null) _shirt.material = shirtMaterial;
        }
    }
}