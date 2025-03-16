using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingInteractable : InteractiveObject
{
    [SerializeField] List<IngredientInfo> allowedIngredients;

    [SerializeField] List<PickableObject> preppedIngredients = new List<PickableObject>();
    [SerializeField] GameObject cookingProgressUI;
    [SerializeField] Image circleBar;
    [SerializeField] TextMeshProUGUI donenessTMP;
    [SerializeField] TextMeshProUGUI timerTMP;

    float timeToChangeDoneness = 5f;
    float timer;
    bool isCooking = false;

    public bool IsCooking { get { return isCooking; } }

    void Start()
    {
        timer = timeToChangeDoneness;
        timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
        donenessTMP.text = PickableObject.Doneness.Raw.ToString();
        UpdateCircleBar();
    }

    void Update()
    {
        if (isCooking)
        {
            if (timer > 0 && preppedIngredients[0].doneness != PickableObject.Doneness.Burnt)
            {
                timer -= Time.deltaTime;
                UpdateCircleBar();
                timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
            }
            else if (timer <= 0 && preppedIngredients[0].doneness != PickableObject.Doneness.Burnt)
            {
                timer = timeToChangeDoneness;
                UpdateCircleBar();
                timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
                ChangeIngredientsDoneness();
                donenessTMP.text = preppedIngredients[0].doneness.ToString();
            }
            else
            {
                timerTMP.text = "";
            }
        }
    }

    public override void OnInteract()
    {
        /*if (!isCooking)
        {
            StartCooking();
            return;
        }

        isCooking = false;*/
        //PickUpFinishedDish();
    }

    public void AddIngredient(PickableObject pickable)
    {
        if (CheckAllowedIngredient(pickable.Info))
        {
            preppedIngredients.Add(pickable);
            pickable.ObjectAnimation(transform.position, pickable.IsPickedUp);
            return;
        }
        Debug.Log("Ingredient does not belong here");
    }

    public List<PickableObject> PickCookedIngredients()
    {
        List<PickableObject> cookedIngredients = new List<PickableObject>();

        for (int i = 0; i < preppedIngredients.Count; i++)
        {
            PickableObject ingredientToPick = preppedIngredients[i];
            cookedIngredients.Add(ingredientToPick);
            ingredientToPick.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, ingredientToPick.IsPickedUp);
        }

        isCooking = false;
        timer = timeToChangeDoneness;
        timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
        UpdateCircleBar();
        Debug.Log(preppedIngredients[0].doneness);

        preppedIngredients.Clear();
        return cookedIngredients;
    }

    public void StartCooking()
    {
        if (preppedIngredients.Count > 0)
        {
            isCooking = true;
            cookingProgressUI.SetActive(true);
            timer = timeToChangeDoneness;
            timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
            donenessTMP.text = preppedIngredients[0].doneness.ToString();
            UpdateCircleBar();
            Debug.Log("Started cooking");
        }
    }

    public void FinishCooking()
    {
        isCooking = false;
        cookingProgressUI.SetActive(false);
    }

    public override void OnTriggerLeave()
    {
        StartCooking();
    }

    void CircleBarUpdate(float newPercentage)
    {
        circleBar.fillAmount = newPercentage;
    }

    public void UpdateCircleBar()
    {
        CircleBarUpdate(timer / timeToChangeDoneness);
    }

    public bool CheckAllowedIngredient(IngredientInfo ingredientToCheck)
    {
        foreach (var ingredient in allowedIngredients)
        {
            if (ingredientToCheck == ingredient)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeIngredientsDoneness()
    {
        if (preppedIngredients.Count > 0)
        {
            foreach (var ingredient in preppedIngredients)
            {
                ingredient.ChangeDoneness();
            }
        }
    }
}
