using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyDisplay;
    [SerializeField] private TextMeshProUGUI energyDisplay;
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider energySlider;
    [SerializeField] private float autoCollectInterval = 5f;
    [SerializeField] private float energyRestoreInterval = 10f;
    [SerializeField] private float delayAfterClick = 3f;
    [SerializeField] private FloatingTextManager floatingTextManager;

    private float currency = 0;
    private float maxEnergy = 100;
    private float currentEnergy;
    private Coroutine autoCollectCoroutine;
    private Coroutine energyRestoreCoroutine;
    private static readonly string[] suffixes = { "", "K", "M", "B", "T" };
    private float lastClickTime = -Mathf.Infinity;
    private bool isAutoCollectActive = false;

    private void Start()
    {
        currentEnergy = maxEnergy;
        UpdateDisplays();
        StartEnergyRestore();
    }

    public void EnableAutoCollect()
    {
        if (autoCollectCoroutine == null)
        {
            isAutoCollectActive = true;
            autoCollectCoroutine = StartCoroutine(AutoCollectCoroutine());
        }
    }

    public void DisableAutoCollect()
    {
        if (autoCollectCoroutine != null)
        {
            isAutoCollectActive = false;
            StopCoroutine(autoCollectCoroutine);
            autoCollectCoroutine = null;
        }
    }

    public void StartEnergyRestore()
    {
        if (energyRestoreCoroutine != null)
        {
            StopCoroutine(energyRestoreCoroutine);
        }
        energyRestoreCoroutine = StartCoroutine(EnergyRestoreCoroutine());
    }

    private IEnumerator AutoCollectCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(FillImageCoroutine());
            AddCurrencyAutoCollect(100);
        }
    }

    private IEnumerator EnergyRestoreCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (Time.time >= lastClickTime + delayAfterClick && currentEnergy < maxEnergy)
            {
                currentEnergy++;
                UpdateEnergyDisplay();
            }
        }
    }

    private IEnumerator FillImageCoroutine()
    {
        float timer = 0;
        while (timer < autoCollectInterval)
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Min(timer / autoCollectInterval, 1f);
            yield return null;
        }
        fillImage.fillAmount = 0;
    }

    public void AddCurrency(float amount)
    {
        if (currentEnergy > 0)
        {
            currentEnergy--;
            float bonus = isAutoCollectActive ? 0.1f * 100 : 0;
            float totalAmount = amount + bonus;
            currency += totalAmount;
            lastClickTime = Time.time;
            UpdateDisplays();

            if (floatingTextManager != null)
            {
                floatingTextManager.ShowFloatingText(totalAmount.ToString());
            }
        }
    }

    public void AddCurrencyAutoCollect(float amount)
    {
        currency += amount;
        UpdateDisplays();
    }

    private void UpdateDisplays()
    {
        UpdateCurrencyDisplay();
        UpdateEnergyDisplay();
    }

    private void UpdateCurrencyDisplay()
    {
        currencyDisplay.text = FormatCurrency(currency);
    }

    private void UpdateEnergyDisplay()
    {
        energyDisplay.text = $"{currentEnergy}/{maxEnergy}";
        energySlider.value = currentEnergy / maxEnergy;
    }

    private string FormatCurrency(float value)
    {
        int index = 0;
        while (value >= 1000 && index < suffixes.Length - 1)
        {
            value /= 1000;
            index++;
        }
        return value.ToString(index == 0 ? "N0" : "N1") + suffixes[index];
    }

    public float GetCurrencyValue()
    {
        return currency;
    }
}
