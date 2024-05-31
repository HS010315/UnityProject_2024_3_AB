using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    WalkingToShelf,
    PickingItem,
    WalkingToCounter,
    PlacingItem
}

public class Timer                  //Ŀ���� Ÿ�̸� class
{
    private float timeRemaining;    //Ÿ�̸� flaot

    public void Set(float time)     //�ð� ����
    {
        timeRemaining = time;
    }

    public void Update(float deltaTime)     //������Ʈ ����ȭ
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= deltaTime;
        }
    }

    public bool isFinished()                //����üũ
    {
        return timeRemaining <= 0;
    }
}

public class CharacterFSM : MonoBehaviour
{
    public CharacterState curentState;
    private Timer timer;

    public NavMeshAgent agent;
    public Transform target;

    public Transform Counter;
    public List<GameObject> targetPos = new List<GameObject>();

    private static int NextPriority = 0;                            //���� ������Ʈ �� �켱 ����
    private static readonly object priorityLock = new object();     //�켱 ���� �Ҵ��� ���� ����ȭ ��ü

    public bool isMoveDone = false;         //���� �Ǻ��� Bool

    public List<GameObject> myBox = new List<GameObject>();

    public int boxesToPick = 5;
    private int boxesPicked = 0;

    void AssignPriority()
    {
        lock(priorityLock)                          // ����ȭ ������ ����Ͽ� �켱 ������ �Ҵ�
        {
            agent.avoidancePriority = NextPriority;             
            NextPriority = (NextPriority + 1) % 100;            //NavMeshAgent �켱 ������ 0~99
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = new Timer();                    //Ÿ�̸� �Ҵ�
        curentState = CharacterState.Idle;      //ĳ���� ���� ����
        agent = GetComponent<NavMeshAgent>();
        AssignPriority();
    }

    // Update is called once per frame
    void Update()
    {
        timer.Update(Time.deltaTime);

        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoveDone = true;
            }
        }

        switch(curentState)
        {
            case CharacterState.Idle:
                Idle();
                break;
            case CharacterState.WalkingToShelf:
                WalkingToShelf();
                break;
            case CharacterState.PickingItem:
                PickingItem();
                break;
            case CharacterState.WalkingToCounter:
                WalkingToCounter();
                break;
            case CharacterState.PlacingItem:
                PlacingItem();
                break;
        }
    }

    void MoveToTarget()
    {
        isMoveDone = false;

        if(target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void ChangeState(CharacterState nextState, float waitTime = 0.0f)           //FSM Stat ��ȯ �� ���� ������Ʈ�� Ÿ�̸� �ð� ����
    {
        curentState = nextState;
        timer.Set(waitTime);
    }

    void Idle()
    {
        if(timer.isFinished())
        {
            target = targetPos[Random.Range(0, targetPos.Count)].transform;
            MoveToTarget();
            ChangeState(CharacterState.WalkingToShelf, 2.0f);
        }
    }
    void WalkingToShelf()
    {
        if (timer.isFinished() && isMoveDone)
        {
            ChangeState(CharacterState.PickingItem, 2.0f);
        }
    }

    void PickingItem()
    {
        if (timer.isFinished())
        {
            if(boxesPicked < boxesToPick)
            {
                GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myBox.Add(box);
                box.transform.parent = gameObject.transform;
                box.transform.localEulerAngles = Vector3.zero;
                box.transform.localPosition = new Vector3(0, boxesPicked * 2f, 0);

                boxesPicked++;
                timer.Set(0.5f);
            }
            else
            {
                target = Counter;
                MoveToTarget();
                ChangeState(CharacterState.WalkingToCounter, 2.0f);
            }
        }
    }

    void WalkingToCounter()
    {
        if (timer.isFinished() && isMoveDone)
        {
            ChangeState(CharacterState.PlacingItem, 2.0f);
        }
    }

    void PlacingItem()
    {
        if (timer.isFinished())
        {
            if(myBox.Count != 0)
            {
                myBox[0].transform.position = Counter.transform.position;
                myBox[0].transform.parent = Counter.transform;
                myBox.RemoveAt(0);
                timer.Set(0.1f);
            }
            else
            {
                ChangeState(CharacterState.Idle, 2.0f);
            }
        }
    }
}