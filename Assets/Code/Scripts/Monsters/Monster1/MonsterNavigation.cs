using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterNavigation : MonoBehaviour
{
    public enum MONSTER_STATES
    {
        sleep,
        alert,
        chasing,
        jumpscare,
        gameover
    }

    public NavMeshAgent agent;
    public GameObject playerObject;
    public JumpscareController jumpscareController;
    public MONSTER_STATES state;
    public Jukebox jukebox;

    public float footstep_freq;
    private AudioSource[] footstep_sounds;
    private int footstepCurrent;
    private float timeSinceFootstep;
    
    private float distanceToPlayer;
    public float jumpscareRange;

    private Vector3 current_position;
    private Vector3 previous_position;

    public void StartChasing()
    {
        state = MONSTER_STATES.chasing;
        //jukebox.SetScary();
    }

    private bool isAnimationPlaying(Animator animator, string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            return true;
        }
        return false;

    }

    public void Update()
        {
        previous_position = current_position;
        current_position = transform.position;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Animator animator = gameObject.transform.GetChild(i).GetComponent<Animator>();
                if (!isAnimationPlaying(animator, "Walking"))
                {
                    animator.SetTrigger(Animator.StringToHash("start_walking"));
                }

        }

        if (state == MONSTER_STATES.chasing)
        {
            timeSinceFootstep += Time.deltaTime;
            if (timeSinceFootstep > footstep_freq)
            {
                int newFootstep = footstepCurrent;
                while (newFootstep == footstepCurrent) {
                    footstepCurrent = Random.Range(0, footstep_sounds.Length);
                }
                footstep_sounds[footstepCurrent].Play();
                timeSinceFootstep = 0;
            }

            distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            Vector3 playerLocation = playerObject.transform.position;
            agent.SetDestination(playerLocation);
            if (distanceToPlayer <= jumpscareRange) state = MONSTER_STATES.jumpscare;
        }

        if (state == MONSTER_STATES.jumpscare)
        {
            playerObject.GetComponent<Player>().state = Player.PLAYERSTATES.jumpscare;
            jumpscareController.state = JumpscareController.JUMPSCARE_STATES.during;
            state = MONSTER_STATES.gameover;
        }

    }




public void Start()
    {
        footstep_sounds = GetComponents<AudioSource>();
        distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        timeSinceFootstep = 0f;
    }

}
