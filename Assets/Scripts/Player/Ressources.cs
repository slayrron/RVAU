using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ressources : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentMoneyText;

    public void UpdateMoney(int currentMoney)
    {
        currentMoneyText.text = currentMoney.ToString();
    } 
}
