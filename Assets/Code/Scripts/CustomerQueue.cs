using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{

    [SerializeField] private int maxCustomers;
    [SerializeField] private Spline arrivalPath;
    [SerializeField] private Spline departurePath;
    [SerializeField] private Queue<GameObject> queue;


    [SerializeField]
    private GameObject customerPrefab;

    //introduces another customer
    public void DispatchNewCustomer()
    {
        if (this.queue.Count >= maxCustomers) return; //no room for additional customers

        //create a customer and assign him a location in line.
        GameObject customerObj = Instantiate(customerPrefab);
        Customer customer = customerObj.GetComponent<Customer>();

        customer.order = Order.RandomOrder();
        customer.path = arrivalPath;
        customer.destinationInPath = arrivalPath.locationOfLastCustomerInQueue - arrivalPath.distBetweenCustomers;
        customer.timeToSpendOnPath = arrivalPath.queueSpeed;
        customer.locationInPath    = 0;
        customer.atPathCompletion = false;

        //Customer(customerPrefab,locationOfLastCustomerInQueue-distBetweenCustomers, queueSpeed);
        arrivalPath.locationOfLastCustomerInQueue = customer.destinationInPath;

        queue.Enqueue(customerObj);
    }

    //Is there a customer ready to be received?
    //(customer is first in line, and at the counter)
    public bool CustomerReady()
    {
        if (queue.Count == 0) return false;
        Customer customer = queue.Peek().GetComponent<Customer>();
        if (customer.atPathCompletion) return true;
        return false;
    }

    // first customer in line.
    public Customer GetFirstCustomer()
    {
        if (queue.Count == 0) return null;
        return queue.Peek().GetComponent<Customer>();
    }

    // release the first in line customer from the queue, tell the queue to enter flowing state.
    // this is done at the end of an interaction, and send the customer off to the next destination.
    public GameObject ReleaseCustomer()
    {
        //ensure there is actually a customer to receive
        if (this.queue.Count == 0) return null;
        if (!CustomerReady()) return null;

        GameObject customerObj = this.queue.Dequeue();
        customerObj.transform.position = new Vector3(0, 0, 0);

        //get array of all remaining customers
        GameObject[] customersObjs = queue.ToArray();

        //tell all remaining customers in the same path to move up in line
        for (int i = 0; i < customersObjs.Length; i++)
        {
            Customer c = customersObjs[i].GetComponent<Customer>();
            if (c.path == arrivalPath) c.destinationInPath += arrivalPath.distBetweenCustomers;
        }

        arrivalPath.locationOfLastCustomerInQueue += arrivalPath.distBetweenCustomers;

        //set customer on departure path.
        Customer customer = customerObj.GetComponent<Customer>();
        customer.path = departurePath;
        customer.destinationInPath = departurePath.locationOfLastCustomerInQueue - departurePath.distBetweenCustomers;
        departurePath.locationOfLastCustomerInQueue = customer.destinationInPath;
        customer.timeToSpendOnPath = departurePath.queueSpeed;
        customer.locationInPath = 0;
        customer.atPathCompletion = false;

        return customerObj;
    }

    public void Start()
    {
        this.queue = new Queue<GameObject>();

        arrivalPath.distBetweenCustomers = ((1 - arrivalPath.minLocationInPath) / maxCustomers);
        arrivalPath.locationOfLastCustomerInQueue = 1;

        departurePath.distBetweenCustomers = ((1 - departurePath.minLocationInPath) / maxCustomers);
        departurePath.locationOfLastCustomerInQueue = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) DispatchNewCustomer();
        if (Input.GetKeyDown(KeyCode.H)) ReleaseCustomer();
    }



}
