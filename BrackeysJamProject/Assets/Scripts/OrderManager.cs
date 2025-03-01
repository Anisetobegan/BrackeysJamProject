using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    float orderTimer = 5f;
    float timer;

    [SerializeField] List<DishRecipe> dishRecipes;
    [SerializeField] List<IngredientInfo> ingredientInfoList;
    //[SerializeField] Order orderPrefab;
    //[SerializeField] Transform layoutGroup;

    List<Order> currentOrders = new List<Order>();

    public List<IngredientInfo> InfoList { get { return ingredientInfoList; } }
    public List<Order> CurrentOrders { get { return currentOrders; } }

    public static OrderManager Instance { get; private set; }

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
    private void Start()
    {
        timer = orderTimer;
        PickUpOrder();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer < 0)
        {
            PickUpOrder();
            timer = orderTimer;
        }
    }

    public void PickUpOrder()
    {
        int index = UnityEngine.Random.Range(0, dishRecipes.Count);
        /*Order newOrder = new Order(dishRecipes[index]);
        Order newOrder = Instantiate(orderPrefab, layoutGroup);
        newOrder.InitializeOrder(dishRecipes[index]);*/
        currentOrders.Add(UIManager.Instance.CreateNewOrder(dishRecipes[index]));
    }
}
