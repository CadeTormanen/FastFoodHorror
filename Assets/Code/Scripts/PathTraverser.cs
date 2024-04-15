using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PathTraverser : MonoBehaviour
{
    enum PATH_TRAVERSAL_STATE
    {
        awaiting_path,
        following_path,
        finished_current_path,
        finished_all_paths
    }


    private float timeOnCurrentPath;
    public float timePerPath;
    public Spline startingSpline;
    public Spline currentSpline;
    private PATH_TRAVERSAL_STATE state;

    public void GivePath(Spline start)
    {
        startingSpline = start;
        state = PATH_TRAVERSAL_STATE.following_path;
    }

    private void Start()
    {
        state = PATH_TRAVERSAL_STATE.awaiting_path;
        if (startingSpline != null){
            state = PATH_TRAVERSAL_STATE.following_path;
            currentSpline = startingSpline;
        }
    }

    void Update()
    {

        switch (state)
        {
            //// No path to follow: null operation
            case (PATH_TRAVERSAL_STATE.awaiting_path):
                break;

            //// There is a path to follow, increment transform along path
            case (PATH_TRAVERSAL_STATE.following_path):

                //we have finished the path.
                if (timeOnCurrentPath >= timePerPath)
                {
                    state = PATH_TRAVERSAL_STATE.finished_current_path;
                }

                //we have not finished the path.
                else
                {
                    this.transform.position = currentSpline.CubeLerped(timeOnCurrentPath / timePerPath);
                    timeOnCurrentPath += Time.deltaTime;
                }

                break;

            //// We are done with the current spline, is there another spline to continue on to?
            case (PATH_TRAVERSAL_STATE.finished_current_path):
                timeOnCurrentPath = 0f;
                if (currentSpline.nextSpline == null) state = PATH_TRAVERSAL_STATE.finished_all_paths;
                if (currentSpline.nextSpline != null) currentSpline = currentSpline.nextSpline;
                state = PATH_TRAVERSAL_STATE.following_path;

                break;


            //// we are done with all splines. null operation.
            case (PATH_TRAVERSAL_STATE.finished_all_paths):
                break;
        }


    }
}
