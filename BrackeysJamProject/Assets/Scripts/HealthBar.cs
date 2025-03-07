using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Image bar;

    public void HealthBarUpdate(float newPercentage)
    {
        bar.fillAmount = newPercentage;
    }
}
