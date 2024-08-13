using System.Collections.Generic;
using UnityEngine;

public class PetStation : MonoBehaviour
{
    [SerializeField] private float _radiusStepPerPet;
    [SerializeField] private PlayerInventory _linkedInventory;

    private Dictionary<int, GameObject> _exitingPets = new Dictionary<int, GameObject>();

    private void Start()
    {
        // if (_linkedInventory.BucketSize == 0)
        // {
        //     _linkedInventory.PushItem(BaseInventoryItem.ItemId.PetCat);
        // }
    }
    
    public void OnPetAdded(int id, BaseInventoryItem item)
    {
        if (!(item is PetItem))
        {
            Debug.LogError("PetStation OnPetAdded: item is not PetItem");
        }
        PetItem petItem = (PetItem)item;
        PetMovement petMovement = Instantiate(petItem.Prefab, Vector3.zero, Quaternion.identity).GetComponent<PetMovement>();
        _exitingPets[id] = petMovement.gameObject;
        petMovement.Radius = _radiusStepPerPet * _exitingPets.Count;
    }

    public void OnPetRemoved(int id, BaseInventoryItem item)
    {
        if (!_exitingPets.ContainsKey(id))
        {
            Debug.LogError($"PetStation OnPetRemoved: pet on {id} not exists");
        }
        Destroy(_exitingPets[id]);
        _exitingPets.Remove(id);
    }
    
    public void DisablePets()
    {
        foreach (KeyValuePair<int, GameObject> pet in _exitingPets)
        {
            pet.Value.SetActive(false);
        }
    }

    public void EnablePets()
    {
        foreach (KeyValuePair<int, GameObject> pet in _exitingPets)
        {
            pet.Value.SetActive(true);
        }
    }
}
