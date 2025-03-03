using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Order orderPrefab;
    [SerializeField] Transform layoutGroup;
    [SerializeField] GameObject storeScreen;
    [SerializeField] TextMeshProUGUI moneyTMP;

    public static UIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        UpdateMoney(GameManager.Instance.PlayerGet.Money);
    }

    void Update()
    {
        
    }

    public Order CreateNewOrder(DishRecipe recipe)
    {
        Order newOrder = Instantiate(orderPrefab, layoutGroup);
        newOrder.InitializeOrder(recipe);
        return newOrder;
    }

    public void OpenStoreScreen()
    {
        storeScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseStoreScreen()
    {
        storeScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpdateMoney(float moneyOwned)
    {
        moneyTMP.text = $"${moneyOwned}";
    }
}
