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
        customer.atPathCompletion = false;

        //Customer(customerPrefab,locationOfLastCustomerInQueue-distBetweenCustomers, queueSpeed);
        locationOfLastCustomerInQueue = customer.destinationInPath;

        queue.Enqueue(customerObj);
    }

    public GameObject GetFirstCustomer()
    {
        if (queue.Count == 0) return null;
        return queue.Peek();
    }

    // release the first in line customer from the queue, tell the queue to enter flowing state.
    // this is done at the end of an interaction, and gets rid of the customer
    public GameObject ReleaseCustomer()
    {
        if (this.queue.Count == 0) return null;

        //only release if customer is at the end of the queue.
        Customer customer = this.queue.Peek().GetComponent<Customer>();
        if (customer.atPathCompletion == false) { return null; }
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


    private bool isAnimationPlaying(Animator animator, string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation)){
            return true;
        }
        return false;

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
            Animator customerAnimator = customerObj.GetComponent<Animator>();

            Vector3 previous_position = customer.transform.position;
            customer.transform.position = path.CubeLerped(customer.locationInPath);

            //if customer moves, update look direction of customer to point in direction of movement.
            if (previous_position != customer.transform.position) { 
                customer.transform.rotation = Quaternion.LookRotation(customer.transform.position - previous_position);
                
                if (! isAnimationPlaying(customerAnimator, "RunForward")) {
                    customerObj.GetComponent<Animator>().SetTrigger(Animator.StringToHash("ChangePosition"));
                }

            }
            else
            {
                if (!isAnimationPlaying(customerAnimator, "Idle"))
                {
                    customerObj.GetComponent<Animator>().SetTrigger(Animator.StringToHash("NoChangePosition"));
                }
            }

            if (customer.locationInPath < customer.destinationInPath) customer.locationInPath += (Time.deltaTime / customer.timeToSpendOnPath);
            if (customer.locationInPath > customer.destinationInPath) customer.locationInPath  = customer.destinationInPath;
            if (customer.locationInPath == customer.destinationInPath) customer.atPathCompletion = true;
            else customer.atPathCompletion = false;

        }
    }



}
