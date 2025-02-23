using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Order : MonoBehaviour
{
    DishRecipe _recipe;
    List<Ingredient> _ingredients = new List<Ingredient>();

    float _timer = 60f;
    bool _dishComplete = false;

    [SerializeField] TextMeshProUGUI _dishNameTMP;
    [SerializeField] TextMeshProUGUI _timerTMP;
    [SerializeField] TextMeshProUGUI _ingredientTMPPrefab;
    [SerializeField] GameObject _gridLayout;

    public List<Ingredient> Ingredients { get { return _ingredients; } }

    /*public Order(DishRecipe recipe)
    {
        _recipe = recipe;
        int ingredientCount = _recipe.ingredientList.Count;
        for (int i = 0; i < ingredientCount; i++)
        {
            _ingredients.Add(new Ingredient(_recipe.ingredientList[i]));
        }

        _dishNameTMP.text = _recipe.dishName;

        CalculateTimer();

        TextMeshProUGUI newIngredientTMP = Instantiate(_ingredientTMPPrefab, _gridLayout.transform);
    }*/

    /*private void Awake()
    {
        _recipe = recipe;
        int ingredientCount = _recipe.ingredientList.Count;
        for (int i = 0; i < ingredientCount; i++)
        {
            _ingredients.Add(new Ingredient(_recipe.ingredientList[i]));
        }

        _dishNameTMP.text = _recipe.dishName;

        CalculateTimer();

        TextMeshProUGUI newIngredientTMP = Instantiate(_ingredientTMPPrefab, _gridLayout.transform);
    }*/

    private void Update()
    {
        if (_timer > 0 && !_dishComplete)
        {
            _timer -= Time.deltaTime;
            CalculateTimer();
        }
        else if (_timer <= 0 && !_dishComplete)
        {
            _timer = 0;
            CalculateTimer();
            DishFailed();
        }
        else
        {
            DishCompleted();
        }
    }

    public void InitializeOrder(DishRecipe recipe)
    {
        _recipe = recipe;
        int ingredientCount = _recipe.ingredientList.Count;
        for (int i = 0; i < ingredientCount; i++)
        {
            string ingredientName = _recipe.ingredientList[i];
            Ingredient newIngredient = new Ingredient(_recipe.ingredientList[i]);
            _ingredients.Add(newIngredient);

            TextMeshProUGUI newIngredientTMP = Instantiate(_ingredientTMPPrefab, _gridLayout.transform);
            newIngredientTMP.text = $"-{newIngredient.Info.Name} x{newIngredient.Quantity}";
        }

        _dishNameTMP.text = _recipe.dishName;

        CalculateTimer();
    }

    public void CalculateTimer()
    {
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer % 60);
        _timerTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DishCompleted()
    {
        _dishComplete = true;
        Debug.Log("Order Completed!");
        //Remove order from the UI
    }

    public void DishFailed()
    {
        //Spawn Enemy
        //Remove order from the UI
    }
}
