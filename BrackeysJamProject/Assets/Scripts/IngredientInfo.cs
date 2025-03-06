using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "IngredientInfo", menuName = "Scriptable Objects/IngredientInfo")]
public class IngredientInfo : ScriptableObject
{
    public string Name;
    public Image Icon;
    public float Price;
}
