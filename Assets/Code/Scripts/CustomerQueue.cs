using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{

    [SerializeField] private int maxCustomers;
    [SerializeField] private Spline path;
    [SerializeField] private Queue<GameObject> queue;

    [SerializeField]
    [Range(0,1)]
    private float minLocationInPath;
    private float distBetweenCustomers;
    private float locationOfLastCustomerInQueue;

    [SerializeField]
    private float queueSpeed;

    [SerializeField]
    private GameObject customerPrefab;

    //introduces another customer
    public void DispatchNewCustomer()
    {
        if (this.queue.Count >= maxCustomers) return; //no room for additional customers

        //create a customer and assign him a location in line.
        GameObject customerObj = Instantiate(customerPrefab);
        Customer customer = customerObj.GetComponent<Customer>();
        customer.destinationInPath = locationOfLastCustomerInQueue - distBetweenCustomers;
        customer.timeToSpendOnPath = queueSpeed;
        customer.locationInPath    = 0;

        //Customer(customerPrefab,locationOfLastCustomerInQueue-distBetweenCustomers, queueSpeed);
        locationOfLastCustomerInQueue = customer.destinationInPath;

        queue.Enqueue(customerObj);
    }

    public GameObject GetFirstCustomer()
    {
        if (queue.Count == 0) return null;
        return queue.Peek();
    }

    // release a customer from the queue, tell the queue to enter flowing state.
    // this is done at the end of an interaction, and gets rid of the customer
    public GameObject ReleaseCustomer()
    {
        if (this.queue.Count == 0) return null;
        GameObject customerObj = this.queue.Dequeue();
        customerObj.transform.position = new Vector3(0, 0, 0);

        //get array of all remaining customers
        GameObject[] customersObjs = queue.ToArray();

        //tell all remaining customers to move up in line
        for (int i = 0; i < customersObjs.Length; i++)
        {
            customersObjs[i].GetComponent<Customer>().destinationInPath += distBetweenCustomers;
        }

        //update the end of the line
        locationOfLastCustomerInQueue += distBetweenCustomers;



        return customerObj;
    }

    public void Start()
    {
        this.queue = new Queue<GameObject>();
        distBetweenCustomers = ((1 - minLocationInPath) / maxCustomers);
        locationOfLastCustomerInQueue = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) DispatchNewCustomer();
        if (Input.GetKeyDown(KeyCode.H)) ReleaseCustomer();

        //update the location of each customer
        foreach (var customerObj in queue)
        {
            Customer customer = customerObj.GetComponent<Customer>();
            customer.transform.position = path.CubeLerped(customer.locationInPath);

            if (customer.locationInPath < customer.destinationInPath) customer.locationInPath += (Time.deltaTime / customer.timeToSpendOnPath);
            if (customer.locationInPath > customer.destinationInPath) customer.locationInPath  = customer.destinationInPath;

        }
    }



}
