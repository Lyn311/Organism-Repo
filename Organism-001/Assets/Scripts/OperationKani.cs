using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class OperationKani : OperationOrganism
{
    
    [Header("Cani Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    private float targetX;

    [SerializeField] private float shiftScale = 0.3f;
    [SerializeField] private float shiftFrequency = 1f;
    private float initalY;

    [Header("Sprite Change")]
    private SpriteRenderer kaniSprite;
    [SerializeField] private Sprite allHaveSpr;
    [SerializeField] private Sprite rightGoneSpr;
    [SerializeField] private Sprite leftGoneSpr;

    private enum State{

        idle,
        allHave,
        rightGone,
        leftGone,
        eaten,
      
    }

    private State stateCurrent;


    void AlterState(State toState)
    {
        stateCurrent = toState;

    }

    void Start()
    {
      
      initalY = transform.position.y;
      RandomTargetGen();

      AlterState(State.idle);
      kaniSprite = GetComponent<SpriteRenderer>();
      kaniSprite.sprite = allHaveSpr;
  

    }


    void Update()
    {
        if (stateCurrent == State.allHave || stateCurrent == State.rightGone || stateCurrent == State.leftGone)
        {
            KaniMovement();
        }

        switch (stateCurrent)
        {
            case State.idle:
                inIdle();
                break;

            case State.allHave:
                inallHave();
                break;

            case State.rightGone:
                inrightGone();
                break;

            case State.leftGone:
                inleftGone();
                break;

            case State.eaten:
                inEaten();
                break;

        }


    }

    void RandomTargetGen()
    {

        targetX = Random.Range(minX, maxX);
    }

    void KaniMovement()
    {
        Vector3 direction = new Vector3 (targetX - transform.position.x,0,0).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        float shiftInY = initalY + Mathf.Sin(Time.time * shiftFrequency) * shiftScale;
        transform.position = new Vector3(transform.position.x,shiftInY,0);


        if (Mathf.Abs(transform.position.x - targetX) <= 0.01)
        {

            StartCoroutine(PauseAtEnd());

        }


    }

    IEnumerator PauseAtEnd()
    {
      yield return new WaitForSeconds(0.15f);
      RandomTargetGen();


    }



    //States-------------------------------------------------------------------

    void inIdle()
        {

            StartCoroutine(IdleOp());

        }

        IEnumerator IdleOp()
        {
            yield return new WaitForSeconds(2f);
            AlterState(State.allHave);

        }

        void inallHave()
        {
       
            StartCoroutine(allHaveOp());
        

        }

        IEnumerator allHaveOp()
        {
            moveSpeed = Random.Range(1f,5f);
            kaniSprite.sprite = rightGoneSpr;
            yield return new WaitForSeconds(2f);
            AlterState(State.rightGone);
        }

        void inrightGone()
        {

       
            StartCoroutine(rightGoneOp());
        


        }

        IEnumerator rightGoneOp()
        {
            moveSpeed = Random.Range(1f, 5f);
            kaniSprite.sprite = leftGoneSpr;
            yield return new WaitForSeconds(2f);
            AlterState(State.leftGone);
        }

        void inleftGone()
        {
    
            StartCoroutine(leftGoneOp());
        

        }

        IEnumerator leftGoneOp()
        {
            moveSpeed = 0f;
            yield return new WaitForSeconds(2f);
            manager.totalOrg--;
            StartCoroutine(manager.SpawnChain());
            AlterState(State.eaten);
        }


        void inEaten()
        {

            Destroy(gameObject);

        }


    
}
