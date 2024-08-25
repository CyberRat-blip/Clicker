using UnityEngine;

public class ClickerController : MonoBehaviour
{
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private float currencyPerClick = 10;

    private void Start()
    {
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager is not assigned!");
            return;
        }
    }

    public void OnClick()
    {
        currencyManager.AddCurrency(currencyPerClick);
    }
}
