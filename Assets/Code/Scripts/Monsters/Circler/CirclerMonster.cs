using UnityEngine;

public class CirclerMonster : MonoBehaviour
{

    private enum mood
    {
        calm,
        aware,
        agitated,
        angry,
        vicious
    }

    private mood state;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField] [Range(0f, 1.0f)]
    private float movementOdds;
    [SerializeField]
    private MoveNode startNode;
    [SerializeField]
    private float timeBetweenOpportunity;
    private MoveNode currentNode;

    private float timeElapsedSinceMove;
    private int circuits;
    private float timeBetweenFlashes;
    private float timeSinceLastFlash;

    //Check if the player flashed their light in our direction, react accordingly
    private void CheckIfPlayerFlashing()
    {
        if (playerObject.GetComponent<PlayerInteractions>().flashingMonster == true)
        {
            switch (state)
            {
                case (mood.calm):
                    Escalate();
                    break;
                case (mood.aware):
                    Deescalate();
                    break;
                case (mood.agitated):
                    Deescalate();
                    break;
                case (mood.angry):
                    Deescalate();
                    break;
                case (mood.vicious):
                    JumpscarePlayer();
                    break;
            }
        }
    }

    private void JumpscarePlayer()
    {

    }

    private void NextNode()
    {
        currentNode = currentNode.next;
        if (this.currentNode == this.startNode) { this.circuits++; }
    }

    private bool RollOdds(float outOf)
    {
        if (outOf == 0.0f) { return false;}
        if (outOf >= 1.0f) { return true; }
        int options = (int) (100 - outOf*100);
        Debug.Log(options);
        if (Random.Range(0,options) == 0)  //'movementOdds' out of 100 chance the monster moves to the next node  
        {
            return true;
        }
        return false;
    }

    private void ExecuteMoveOpportunity()
    {
        switch (state)
        {
            case mood.calm:
                if (RollOdds(movementOdds) == true)
                {
                    NextNode();
                }
                break;
            case mood.aware:
                if (RollOdds(movementOdds) == true)
                {
                    NextNode();
                }
                break;
            case mood.agitated:
                if (RollOdds(movementOdds) == true)
                {
                    NextNode();
                }
                break;
            case mood.angry:
                if (RollOdds(movementOdds) == true)
                {
                    JumpscarePlayer();
                }
                break;
            case mood.vicious:
                if (RollOdds(movementOdds) == true)
                {
                    JumpscarePlayer();
                }
                break;
        }
    }

    private void Escalate()
    {
        if (state == mood.vicious) { return; }
        state = (mood) ( ((int) state) + 1);
    }

    private void Deescalate()
    {
        if (state == mood.calm) { return; }
        state = (mood)(((int)state) - 1);
    }

    public void Start()
    {
        this.currentNode           = startNode;
        this.circuits              = 0;
        this.timeSinceLastFlash    = 0;
        this.timeBetweenFlashes    = 5;
    }

    public void Update()
    {
        Debug.Log(state);

        if ((timeSinceLastFlash += Time.deltaTime) >= timeBetweenFlashes)
        {
            timeSinceLastFlash = 0f;
            CheckIfPlayerFlashing();
        }

        
        transform.position = currentNode.position;
        if ((timeElapsedSinceMove += Time.deltaTime) >= timeBetweenOpportunity){
            timeElapsedSinceMove = 0f;
            ExecuteMoveOpportunity();
        }
    }
}
