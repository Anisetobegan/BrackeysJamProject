using UnityEngine;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    [SerializeField] StoreItem storeItemPrefab;
    [SerializeField] List<IngredientInfo> itemsInfo;
    [SerializeField] Transform layout;

    private void Awake()
    {
        InstantiateStoreItems();
    }

    public void InstantiateStoreItems()
    {
        for (int i = 0; i < itemsInfo.Count; i++)
        {
            StoreItem newStoreItem = Instantiate(storeItemPrefab, layout);
            newStoreItem.InitializeStoreItem(itemsInfo[i]);
        }
    }
}
