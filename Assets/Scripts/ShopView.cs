using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    public Button option_1, option_2, option_3, skip;
    public Text optionText_1, optionText_2, optionText_3;
    public Text moneyText;
    public Animator animator;
    private static readonly int ShowShop = Animator.StringToHash("ShowShop");
    
    private int currentMoney;
    private StatsScreen statsScreen;

    private void Awake()
    {
        statsScreen = GameObject.Find("StatsScreen").GetComponent<StatsScreen>();
    }

    void Start()
    {
        option_1.onClick.AddListener(() => SelectOption(1));
        option_2.onClick.AddListener(() => SelectOption(2));
        option_3.onClick.AddListener(() => SelectOption(3));
        skip.onClick.AddListener(Skip);
        currentMoney = statsScreen.coins;
        moneyText.text = "$" + currentMoney;
        animator.SetTrigger(ShowShop);
    }

    private void Skip()
    {
        SceneManager.LoadScene(1);
        //gameObject.SetActive(false);
    }

    private void SelectOption(int option)
    {
        Debug.Log("Arma seleccionada: " + option);
        switch (option)
        {
            case 1:
                statsScreen.weaponType = 1;
                Discount(optionText_1);
                break;
            case 2:
                statsScreen.weaponType = 2;
                Discount(optionText_2);
                break;
            default:
                statsScreen.weaponType = 3;
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
            if (price <= currentMoney)
            {
                currentMoney -= price;
                moneyText.text = "$" + currentMoney;
                
                Debug.Log($"Descuento exitoso. Nuevo saldo: {currentMoney}");
                statsScreen.coins -= price;
                //gameObject.SetActive(false);
            }
            else
                Debug.LogWarning($"Saldo insuficiente. Requiere ${price}, tiene ${currentMoney}.");
        }
        else
            Debug.LogError($"Error de formato: El texto '{amountText.text}' no es un número válido.");
    }
}
