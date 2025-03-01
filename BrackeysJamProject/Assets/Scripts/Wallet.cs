using UnityEngine;

public class Wallet
{
    float moneyOwned = 0;

    public void AddMoney(float moneyEarned)
    {
        moneyOwned += moneyEarned;
    }

    public void BuyItem(float itemPrice)
    {
        if (itemPrice < moneyOwned)
        {
            moneyOwned -= itemPrice;
        }
        else
        {
            Debug.Log("Money Insufficient");
        }
    }
}
