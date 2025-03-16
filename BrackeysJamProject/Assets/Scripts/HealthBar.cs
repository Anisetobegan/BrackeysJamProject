using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Image bar;

    [SerializeField] protected RectTransform heartIcon;

    private void Start()
    {
        heartIcon.DOPunchScale(Vector3.one * 0.1f, 1f, 0).SetLoops(-1);
    }

    public void HealthBarUpdate(float newPercentage)
    {
        bar.fillAmount = newPercentage;
    }
}
