using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Order
{
    public String id;
    public bool drink;
    public bool fry;
    public bool[] burger;

    //Create an order with these requirements
    public Order(String id,  bool fry, bool drink, bool[] burger)
    {
        this.id = id;
        this.drink = drink;
        this.fry = fry;

        if (burger.Length != Enum.GetNames(typeof(Ingredients)).Length) burger = BurgerAssembler.CreateEmptyBurger();
        else this.burger = burger;
    }

    //Create an order with these requirements
    public Order(String id, bool fry, bool drink, bool patty = false, bool cheese = false, bool lettuce = false, bool ketchup = false, bool mustard = false)
    {
        this.id = id;
        this.drink = drink;
        this.fry = fry;

        this.burger = BurgerAssembler.CreateEmptyBurger();
        this.burger[(int) Ingredients.LOWER_BUN] = true;
        this.burger[(int) Ingredients.PATTY]     = patty;
        this.burger[(int) Ingredients.CHEESE]    = cheese;
        this.burger[(int) Ingredients.LETTUCE]   = lettuce;
        this.burger[(int) Ingredients.KETCHUP]   = ketchup;
        this.burger[(int) Ingredients.MUSTARD]   = mustard;
        this.burger[(int) Ingredients.UPPER_BUN] = true;
    }

    public static Order RandomOrder()
    {
        bool[] burger = BurgerAssembler.CreateEmptyBurger();
        for (int i = 2; i < burger.Length-1; i++)
        {
            burger[i] = (UnityEngine.Random.value > 0.5);
        }

        return new Order(
            "id",
            (UnityEngine.Random.value > 0.5),
            (UnityEngine.Random.value > 0.5),
            burger
            );
    }

}

public class OrderList : MonoBehaviour
{
    public int maxOrders;
    public IndividualOrderDisplay[] orderDisplays;

    //Add an order to the list
    public bool AddOrder(Order order)
    {
        foreach (IndividualOrderDisplay ord in orderDisplays)
        {
            if (ord.active == false)
            {
                ord.SetOrder(order);
                return true;
            }
        }
        return false;
    }

    //Get the order with 'id'
    public IndividualOrderDisplay GetOrder(String id)
    {
        foreach (IndividualOrderDisplay ord in orderDisplays)
        {
            if (ord.order.id == id) { return ord; }
        }
        return null;
    }

    public bool MatchOrder(Order comparison_order)
    {
        foreach (IndividualOrderDisplay ord in orderDisplays)
        {
            if (ord.order == null) { continue; }
            if (ValidateOrder(ord.order, comparison_order)){
                ord.ClearOrder();
                return true;
            }
        }
        return false;
    }

    //Compare an order to see if it's correct.
    public bool ValidateOrder(Order order, Order prepared)
    {
        Debug.Log(order.fry);
        Debug.Log(prepared.fry);

        Debug.Log(prepared.drink);
        Debug.Log(order.drink);
        


        if (order.fry != prepared.fry) return false;
        if (order.drink != prepared.drink) return false;
        for (int i = 2; i < order.burger.Length-1; i++)
        {
            Debug.Log("----");
            Debug.Log("----");
            Debug.Log("----");
            Debug.Log((Ingredients)i);
            Debug.Log(order.burger[i]);
            Debug.Log(prepared.burger[i]);
            Debug.Log("----");
            Debug.Log("----");
            Debug.Log("----");
            if (order.burger[i] != prepared.burger[i]) return false;
        }
        return true;
    }

    public void Start()
    {
        if (maxOrders != orderDisplays.Length) { maxOrders = orderDisplays.Length; }    //resize maxOrders to match the number of order objects.
    }
}
