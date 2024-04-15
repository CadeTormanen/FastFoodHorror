using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum ORDER_INDICATORS
{
    fry,
    drink,
    patty,
    cheese,
    lettuce,
    mustard,
    ketchup
}

public class IndividualOrderDisplay : MonoBehaviour
{
    public bool active;
    public Order order;
    private bool[] enabledIndicators;
    [SerializeField] GameObject fryIndicator;
    [SerializeField] GameObject drinkIndicator;
    [SerializeField] GameObject pattyIndicator;
    [SerializeField] GameObject cheeseIndicator;
    [SerializeField] GameObject lettuceIndicator;
    [SerializeField] GameObject mustardIndicator;
    [SerializeField] GameObject ketchupIndicator;

    public void SetOrder(Order order)
    {
        if (order.fry)
        {
            enabledIndicators[(int)ORDER_INDICATORS.fry] = true;
            fryIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (order.drink)
        {
            enabledIndicators[(int)ORDER_INDICATORS.drink] = true;
            drinkIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (order.burger[(int)Ingredients.PATTY])
        {
            enabledIndicators[(int)ORDER_INDICATORS.patty] = true;
            pattyIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (order.burger[(int)Ingredients.CHEESE])
        {
            enabledIndicators[(int)ORDER_INDICATORS.cheese] = true;
            cheeseIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }

        if (order.burger[(int)Ingredients.LETTUCE])
        {
            enabledIndicators[(int)ORDER_INDICATORS.lettuce] = true;
            lettuceIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (order.burger[(int)Ingredients.MUSTARD])
        {
            enabledIndicators[(int)ORDER_INDICATORS.mustard] = true;
            mustardIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (order.burger[(int)Ingredients.KETCHUP])
        {
            enabledIndicators[(int)ORDER_INDICATORS.ketchup] = true;
            ketchupIndicator.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        this.order = order;
        active = true;
    }

    public void ClearOrder()
    {
        int indicatorCount = Enum.GetNames(typeof(ORDER_INDICATORS)).Length;
        for (int i = 0; i < indicatorCount; i++)
        {
            enabledIndicators[i] = false;
        }

        pattyIndicator.GetComponent<UnityEngine.UI.Image>().color       = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        lettuceIndicator.GetComponent<UnityEngine.UI.Image>().color     = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        cheeseIndicator.GetComponent<UnityEngine.UI.Image>().color      = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        mustardIndicator.GetComponent<UnityEngine.UI.Image>().color     = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        ketchupIndicator.GetComponent<UnityEngine.UI.Image>().color     = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        drinkIndicator.GetComponent<UnityEngine.UI.Image>().color       = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        fryIndicator.GetComponent<UnityEngine.UI.Image>().color         = new Color(1.0f, 1.0f, 1.0f, 0.1f);

        active = false;
    }

    public void Start()
    {
        int indicatorCount = Enum.GetNames(typeof(ORDER_INDICATORS)).Length;
        enabledIndicators = new bool[indicatorCount];
        for (int i = 0; i < indicatorCount; i++)
        {
            enabledIndicators[i] = false;
        }
    }


}