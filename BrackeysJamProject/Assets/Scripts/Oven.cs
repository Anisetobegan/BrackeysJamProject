using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Oven : InteractiveObject
{
    [SerializeField] List<PickableObject> preppedIngredients = new List<PickableObject>();
    [SerializeField] GameObject cookingProgressUI;
    [SerializeField] Image circleBar;
    [SerializeField] TextMeshProUGUI donenessTMP;
    [SerializeField] TextMeshProUGUI timerTMP;

    FinishDish finishedDish = null;

    float timeToChangeDoneness = 5f;
    float timer;
    bool isCooking = false;

    public bool IsCooking {  get { return isCooking; } }

    enum Doneness
    {
        Raw,
        Rare,
        Medium,
        WellDone,
        Burnt
    }
    Doneness doneness;

    void Start()
    {
        doneness = Doneness.Rare;
        timer = timeToChangeDoneness;
        timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
        donenessTMP.text = doneness.ToString();
        UpdateCircleBar();
    }

    void Update()
    {
        if (isCooking)
        {
            if (timer > 0 && doneness != Doneness.Burnt)
            {
                timer -= Time.deltaTime;
                UpdateCircleBar();
                timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
            }
            else if(timer <= 0 && doneness != Doneness.Burnt)
            {
                timer = timeToChangeDoneness;
                UpdateCircleBar();
                timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
                doneness++;
                donenessTMP.text = doneness.ToString();
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
        preppedIngredients.Add(pickable);
        pickable.ObjectAnimation(transform.position, pickable.IsPickedUp);
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
        preppedIngredients.Clear();

        isCooking = false;
        timer = timeToChangeDoneness;
        timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
        UpdateCircleBar();
        Debug.Log(doneness);
        doneness = Doneness.Raw;
        donenessTMP.text = doneness.ToString();
        return cookedIngredients;
    }

    public void StartCooking()
    {
        if (preppedIngredients.Count > 0)
        {
            isCooking = true;
            cookingProgressUI.SetActive(true);
            doneness = Doneness.Rare;
            timer = timeToChangeDoneness;
            timerTMP.text = string.Format("{0:0}:{1:00}", 0, timer);
            donenessTMP.text = doneness.ToString();
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
}
