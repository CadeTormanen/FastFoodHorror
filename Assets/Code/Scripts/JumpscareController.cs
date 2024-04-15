using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    public GameObject monster;
    public Camera scareCamera;
    public Camera mainCamera;
    public Camera gameoverCamera;

    public GameObject lightOne;
    public GameObject lightTwo;

    private AudioSource jumpscareSound;
    public float jumpscareSeconds;
    private float secondsElapsed;
    public JUMPSCARE_STATES state;

    public enum JUMPSCARE_STATES
    {
        pre,
        during,
        post
    }

    public void Start()
    {
        state = JUMPSCARE_STATES.pre;
        jumpscareSound = GetComponent<AudioSource>();
        gameoverCamera.enabled = false;
        scareCamera.enabled = false;
    }

    public void Update()
    {
        
        if (state == JUMPSCARE_STATES.during)
        {
            monster.SetActive(true);

            scareCamera.GetComponent<Animation>().Play();
            lightOne.GetComponent<Animation>().Play();
            lightTwo.GetComponent<Animation>().Play();

            scareCamera.enabled = true;
            mainCamera.enabled  = false;
            jumpscareSound.Play();

            state = JUMPSCARE_STATES.post;
        }

        if (state == JUMPSCARE_STATES.post)
        {
            secondsElapsed += Time.deltaTime;


            if (secondsElapsed >= jumpscareSeconds){
                scareCamera.enabled = false;
                gameoverCamera.enabled = true;
            }
        }



    }



}
