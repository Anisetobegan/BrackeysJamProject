using UnityEngine;
using System;

public static class Actions
{
    public static Action<float> OnOrderCompleted;
    public static Action<IngredientInfo, int> OnItemBought;
    public static Action<int, float> OnItemRefund;
}
