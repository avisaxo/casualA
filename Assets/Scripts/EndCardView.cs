using System;
using UnityEngine;
using UnityEngine.UI;

public class EndCardView : MonoBehaviour
{
    public Button option_1, option_2, option_3, skip;
    public Text optionText_1, optionText_2, optionText_3;
    public Text moneyText;
    public Animator animator;
    private static readonly int ShowShop = Animator.StringToHash("ShowShop");
    
    private int _currentMoney;
    private Action<int> _applyDiscount;

    void Start()
    {
        option_1.onClick.AddListener(() => SelectOption(1));
        option_2.onClick.AddListener(() => SelectOption(2));
        option_3.onClick.AddListener(() => SelectOption(3));
        skip.onClick.AddListener(Skip);
    }

    private void Skip()
    {
        gameObject.SetActive(false);
        _applyDiscount?.Invoke(0);
    }

    public void ShowEndCard(int money, Action<int> applyDiscount)
    {
        _applyDiscount = applyDiscount;
        _currentMoney = money;
        moneyText.text = "$" + money;
        gameObject.SetActive(true);
        animator.SetTrigger(ShowShop);
    }
    
    private void SelectOption(int option)
    {
        Debug.Log("Arma seleccionada: " + option);
        switch (option)
        {
            case 1:
                Discount(optionText_1);
                break;
            case 2:
                Discount(optionText_2);
                break;
            default:
                Discount(optionText_3);
                break;
        }
    }

    private void Discount(Text amountText)
    {
        var amountString = amountText.text;
        
        if (amountString.StartsWith("$")) amountString = amountString.Replace("$", "");

        if (int.TryParse(amountString, out int price))
        {
            if (price <= _currentMoney)
            {
                _currentMoney -= price;
                moneyText.text = "$" + _currentMoney;
                
                Debug.Log($"Descuento exitoso. Nuevo saldo: {_currentMoney}");
                gameObject.SetActive(false);
                _applyDiscount?.Invoke(price);
            }
            else
                Debug.LogWarning($"Saldo insuficiente. Requiere ${price}, tiene ${_currentMoney}.");
        }
        else
            Debug.LogError($"Error de formato: El texto '{amountText.text}' no es un número válido.");
    }
}
