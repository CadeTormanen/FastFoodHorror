using Unity.VisualScripting;
using UnityEngine;

public class CirclerMonster : MonoBehaviour
{
    public enum move_state
    {
        idle,
        moving,
        disabled
    }
    private move_state moveState;

    public enum mood
    {
        calm,
        aware,
        agitated,
        angry,
        vicious
    }
    public mood state;

    private MoveNode currentNode;
    private MoveNode previousNode;

    [SerializeField]
    private MoveNode startNode;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField] [Range(0f, 1.0f)]
    private float movementOdds;

    [SerializeField]
    private float timeBetweenOpportunity;

    [SerializeField]
    private float timePerMovement;
    private float timeInCurrentMovement;

    private float timeElapsedSinceMove;
    private float timeBetweenFlashes;
    private float timeSinceLastFlash;

    //Check if the player flashed their light in our direction, react accordingly
    private void CheckIfPlayerFlashing(){
        if (playerObject.GetComponent<Player>().flashingMonster == true){
            timeSinceLastFlash = 0f;
            switch (state){
                case (mood.angry):
                    Advance();
                    break;
                case (mood.vicious):
                    Jumpscare();
                    break;
            }
        }
    }

    private void Jumpscare(){
        playerObject.GetComponent<Player>().state = Player.PLAYERSTATES.jumpscare;
    }

    private void SetNextNode(){
        previousNode = currentNode;
        currentNode  = currentNode.next;
    }
    private void Advance()
    {
        if (state == mood.vicious) { return; }
        state = (mood)(((int)state) + 1);
        currentNode = currentNode.advanceNode;
    }

    private void Retreat(){
        if (state == mood.calm) { return; }
        state = (mood)(((int)state) - 1);
        currentNode = currentNode.retreatNode;
    }

    private bool RollOdds(float outOf)
    {
        if (outOf == 0.0f) { return false;}
        if (outOf >= 1.0f) { return true; }
        int options = (int) (100 - outOf*100);
        if (Random.Range(0,options) == 0){  //'movementOdds' out of 100 chance the monster moves to the next node  
            return true;
        }
        return false;
    }

    //attempts to begin a move - returns true on success
    private bool TryMoveOpportunity()
    {
        switch (state){
        case mood.calm:
            if (RollOdds(movementOdds) == true){
                SetNextNode();
                return true;
            }
            break;
        case mood.aware:
            if (RollOdds(movementOdds) == true){
                SetNextNode();
                return true;
            }
            break;
        case mood.agitated:
            if (RollOdds(movementOdds) == true){
                SetNextNode();
                return true;
            }
            break;
        case mood.angry:
            if (RollOdds(movementOdds) == true){
                SetNextNode();
                return true;
            }
            break;
        case mood.vicious:
            if (RollOdds(movementOdds) == true){
                SetNextNode();
                return true;
            }
            break;
        }
        return false;
    }

    public void Start()
    {
        this.currentNode           = startNode;
        this.previousNode          = null;
        this.timeSinceLastFlash    = 0;
        this.timeBetweenFlashes    = 5;
        this.moveState             = move_state.idle;
        transform.position         = currentNode.position;
    }

    public void Update(){

        //
        if ((timeSinceLastFlash += Time.deltaTime) >= timeBetweenFlashes)
        {
            CheckIfPlayerFlashing();
        }

        switch (moveState){

            //monster is waiting for next move opportunity
            case move_state.idle:
                
                //try to move every 'timeBetweenOpportunity' seconds
                if ((timeElapsedSinceMove += Time.deltaTime) >= timeBetweenOpportunity){
                    timeElapsedSinceMove = 0f;
                    if (TryMoveOpportunity() == true){
                        moveState = move_state.moving;
                        timeInCurrentMovement = 0f;
                    }
                }

                break;

            //monster is engaging in a movement opportunity
            case move_state.moving:

                //interpolate the monsters position between two nodes, and set the transform to that position.
                if (timeInCurrentMovement < timePerMovement){
                    transform.position = Vector3.Lerp(previousNode.transform.position, currentNode.transform.position,timeInCurrentMovement/timePerMovement);
                    timeInCurrentMovement+=1.0f;
                    if (timeInCurrentMovement > timePerMovement) { timeInCurrentMovement = timePerMovement; }
                }else{
                    moveState = move_state.idle;
                }

                break;
        }
    }
}
