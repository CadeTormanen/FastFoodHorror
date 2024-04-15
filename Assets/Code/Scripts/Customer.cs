using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float destinationInPath;
    public float locationInPath;
    public float timeToSpendOnPath;
    public bool atPathCompletion;
    private Animator customerAnimator;
    public Order order;
    public Spline path;

    private bool isAnimationPlaying(Animator animator, string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            return true;
        }
        return false;

    }

    public void Start()
    {
        customerAnimator = GetComponent<Animator>();
    }

    public void Update()
    {

        Vector3 previous_position = transform.position;
        transform.position = path.CubeLerped(locationInPath);

        //if customer moves, update look direction of customer to point in direction of movement.
        if (previous_position != transform.position)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - previous_position);

            if (!isAnimationPlaying(customerAnimator, "RunForward"))
            {
                customerAnimator.SetTrigger(Animator.StringToHash("ChangePosition"));
            }
        }
        else
        {
            if (!isAnimationPlaying(customerAnimator, "Idle"))
            {
                customerAnimator.SetTrigger(Animator.StringToHash("NoChangePosition"));
            }
        }

        if (locationInPath < destinationInPath) locationInPath += (Time.deltaTime / timeToSpendOnPath);
        if (locationInPath > destinationInPath) locationInPath = destinationInPath;
        if (locationInPath == destinationInPath) atPathCompletion = true;
        else atPathCompletion = false;
    }




}
