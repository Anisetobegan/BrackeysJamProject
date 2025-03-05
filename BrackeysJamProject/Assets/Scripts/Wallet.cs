using UnityEngine;

public class Wallet
{
    float moneyOwned;

    public Wallet()
    {
        moneyOwned = 0;
    }

    public float Money { get { return moneyOwned; } }

    public void AddMoney(float moneyToAdd)
    {
        moneyOwned += moneyToAdd;
        UIManager.Instance.UpdateMoney(moneyOwned);
    }

    public bool TryBuyItem(float itemPrice)
    {
        if (itemPrice <= moneyOwned)
        {
            moneyOwned -= itemPrice;
            UIManager.Instance.UpdateMoney(moneyOwned);
            return true;
        }
        return false;
    }
}
