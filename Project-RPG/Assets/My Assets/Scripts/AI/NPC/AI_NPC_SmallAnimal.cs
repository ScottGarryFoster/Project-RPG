using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NPC_SmallAnimal : MonoBehaviour
{
    //Public Vars
    [Tooltip("If the model looks like it's always turned the opposite direction tick this and it'll flip the model around.")]
    public bool FlipForward;
    [Tooltip("If you want to limit the animal to an area drag this here")]
    public Collider AnimalArea;
    [Tooltip("If you want to tell this AI about other things in Storage drag the object with the storage on it in here")]
    public GameObject LevelStorage;

    //Private
    //Links to Components
    private Rigidbody rb;
    private Animator m_Animator;

    //Behaviours
    enum AI_NPC_SA_Behaviour { Idle = 0, Moving };
    private AI_NPC_SA_Behaviour currentBehaviour = AI_NPC_SA_Behaviour.Idle;

    //Counters
    private float timeIdle = 0.0f;
    private float timeToBeIdle = 5.0f;

    //Behaviour Storage
    private Vector2 m_v2MoveToLocation;
    private Storage_BuildingOrNoPass storageBuildingNoPass;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animator.SetBool("OnGround", true);

        rb = GetComponent<Rigidbody>();

        //This just avoids the restriction code by removing the collider if the start position is not in the area.
        if (AnimalArea != null)
            if (!AnimalArea.bounds.Contains(transform.position))
                AnimalArea = null;

        if (LevelStorage != null)
            storageBuildingNoPass = LevelStorage.GetComponent<Storage_BuildingOrNoPass>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentBehaviour)
        {
            case AI_NPC_SA_Behaviour.Idle:
                m_Animator.SetFloat("Speed", 0);
                timeIdle += Time.deltaTime;
                if (timeIdle > timeToBeIdle)
                    RandomBehaviour();
                break;
            case AI_NPC_SA_Behaviour.Moving:
                PerformBehaviour();
                break;
        }
    }

    public bool SetAnimalArea(Collider collider)
    {
        AnimalArea = collider;
        if (!AnimalArea.bounds.Contains(transform.position))
        {
            AnimalArea = null;
            return false;
        }
        return true;
    }

    void RandomBehaviour()
    {
        currentBehaviour = AI_NPC_SA_Behaviour.Moving;//We are now moving
        m_Animator.SetFloat("Speed", 0.2f);//Ensure the animal moves
        timeIdle = 0;//Reset this so that Idle doesn't happen instantly

        Vector2 l_v2CurrentPosition = new Vector2(transform.position.x, transform.position.z);

        if(AnimalArea == null)
            m_v2MoveToLocation = new Vector2(l_v2CurrentPosition.x + Random.Range(-10.0f, 10.0f), l_v2CurrentPosition.y + Random.Range(-10.0f, 10.0f));//Find a random location to move to
        else
        {
            //This keeps trying to find points in the collider.
            while(true)
            {
                m_v2MoveToLocation = new Vector2(l_v2CurrentPosition.x + Random.Range(-10.0f, 10.0f), l_v2CurrentPosition.y + Random.Range(-10.0f, 10.0f));//Find a random location to move to
                if (VaildatePoint(new Vector3(m_v2MoveToLocation.x, transform.position.y, m_v2MoveToLocation.y)))
                    break;
            }
        }
        //Debug.Log("m_v2MoveToLocation: " + m_v2MoveToLocation.x + ", " + m_v2MoveToLocation.y);
    }

    void PerformBehaviour()
    {
        Vector3 v3_target = new Vector3(m_v2MoveToLocation.x, transform.position.y, m_v2MoveToLocation.y); //Where I'm aiming to

        if(FlipForward)//Occasionally forward flips on the models, if so set the public var
            transform.rotation = Quaternion.LookRotation(transform.position - v3_target);//Flips forward
        else
            transform.rotation = Quaternion.LookRotation(v3_target);

        float step = 5 * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, v3_target, step);//Moves slowly towards target

        rb.MovePosition(transform.position + transform.right * Time.fixedDeltaTime);//Actually Move

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, v3_target) < 0.001f || !VaildatePoint(transform.position))
        {
            CompleteBehaviour();//We're there or close enough
            return;
        }
    }

    void CompleteBehaviour()
    {
        timeToBeIdle = Random.Range(5.0f, 10.0f);
        currentBehaviour = AI_NPC_SA_Behaviour.Idle;
    }

    bool VaildatePoint(Vector3 v3Location)
    {
        if (!VaildateInAnimalArea(v3Location)) return false;
        if (VaildateNotInNoGoArea(v3Location)) return false;
        return true;
    }

    bool VaildateInAnimalArea(Vector3 v3Location)
    {
        if (AnimalArea.bounds.Contains(v3Location))
            return true;
        return false;
    }

    bool VaildateNotInNoGoArea(Vector3 v3Location)
    {
        if (storageBuildingNoPass == null) return false;
        for(int i = 0; i < storageBuildingNoPass.OutsideBuildings.Length; i++)
        {
            if (storageBuildingNoPass.OutsideBuildings[i].bounds.Contains(v3Location))
                return true;
        }
        return false;
    }
}
