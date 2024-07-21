using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Invetory Items/BaseItem", order = 51)]
public class BaseInventoryItem : ScriptableObject 
{
    public BaseInventoryItem(BaseInventoryItem item)
    {
        _itemId = item._itemId;
        _icon = item._icon;
    }

    public BaseInventoryItem() 
    { 

    }

    public enum ItemId : int
    {
        PetCat = 0,
        PetBunny,
        PetBear,
        PetBurger,
        PetPumkin,
        PetDemon, 
        PetMax,
        PickaxeStandart = 0,
        PickaxeWooden,
        PickaxeCrowbar,
        PickaxeHammer,
        PickaxeShovel,
        PickaxeWrench,
        PickaxeAxe,
        PickaxeCrusher,
        PickaxeSaw,
        PickaxeWepAxe,
        PickaxeSword,
        PickaxeSledgehammer,
        PickaxeMax
    }

    public int Id
    {
        get => (int)_itemId;
    }

    public Sprite Icon
    {
        get => _icon;
    }
    
    public string RuLabel {
        get => _ruLabel;
    }

    public string EnLabel {
        get => _enLabel;
    }

    [SerializeField] protected ItemId _itemId;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected string _ruLabel;
    [SerializeField] protected string _enLabel;
}
