using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    float orderTimer = 5f;

    [SerializeField] List<DishRecipe> dishRecipes;
    [SerializeField] List<IngredientInfo> ingredientInfoList;
    [SerializeField] Order orderPrefab;
    [SerializeField] Transform layoutGroup;

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
        PickUpOrder();
    }

    private void Update()
    {
        if (orderTimer > 0)
        {
            orderTimer -= Time.deltaTime;
        }
        else if (orderTimer < 0)
        {
            PickUpOrder();
            orderTimer = 5f;
        }
    }

    public void PickUpOrder()
    {
        int index = UnityEngine.Random.Range(0, dishRecipes.Count);
        //Order newOrder = new Order(dishRecipes[index]);
        Order newOrder = Instantiate(orderPrefab, layoutGroup);
        newOrder.InitializeOrder(dishRecipes[index]);
        currentOrders.Add(newOrder);
    }
}
