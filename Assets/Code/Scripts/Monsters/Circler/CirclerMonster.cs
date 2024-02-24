using UnityEngine;

public class CirclerMonster : MonoBehaviour
{
    public enum mood
    {
        calm,
        aware,
        agitated,
        angry,
        vicious
    }

    private MoveNode currentNode;

    [SerializeField]
    private MoveNode startNode;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField] [Range(0f, 1.0f)]
    private float movementOdds;

    [SerializeField]
    private float timeBetweenOpportunity;

    public mood state;
    private float timeElapsedSinceMove;
    private float timeBetweenFlashes;
    private float timeSinceLastFlash;


    //Check if the player flashed their light in our direction, react accordingly
    private void CheckIfPlayerFlashing()
    {
        if (playerObject.GetComponent<Player>().flashingMonster == true)
        {
            timeSinceLastFlash = 0f;
            switch (state)
            {
                case (mood.calm):
                    break;
                case (mood.aware):
                    break;
                case (mood.agitated):
                    break;
                case (mood.angry):
                    Advance();
                    break;
                case (mood.vicious):
                    Jumpscare();
                    break;
            }
        }
    }

    private void Jumpscare()
    {
        playerObject.GetComponent<Player>().state = Player.PLAYERSTATES.jumpscare;

    }

    private void NextNode()
    {
        currentNode = currentNode.next;
    }
    private void Advance()
    {
        if (state == mood.vicious) { return; }
        state = (mood)(((int)state) + 1);
        currentNode = currentNode.advanceNode;
    }

    private void Retreat()
    {
        if (state == mood.calm) { return; }
        state = (mood)(((int)state) - 1);
        currentNode = currentNode.retreatNode;
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
                    NextNode();
                }
                break;
            case mood.vicious:
                if (RollOdds(movementOdds) == true)
                {
                    NextNode();
                }
                break;
        }
    }



    public void Start()
    {
        this.currentNode           = startNode;
        this.timeSinceLastFlash    = 0;
        this.timeBetweenFlashes    = 5;
    }

    private void MapPaths()
    {


    }

    public void Update()
    {

        if ((timeSinceLastFlash += Time.deltaTime) >= timeBetweenFlashes)
        {
            CheckIfPlayerFlashing();
        }

        
        transform.position = currentNode.position;
        if ((timeElapsedSinceMove += Time.deltaTime) >= timeBetweenOpportunity){
            timeElapsedSinceMove = 0f;
            ExecuteMoveOpportunity();
        }
    }
}
