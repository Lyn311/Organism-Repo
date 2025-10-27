using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering.Universal;

public class OperationTakoyaki : OperationOrganism
{
    
    [Header("Takoyaki Movement")]
    [SerializeField] private float rotateSpeed = 3f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float rotatingAngle = 0f;
    private Vector3 circularCenter;
    

    [Header("Sprite Change")]
    private SpriteRenderer takoyakiSprite;
    [SerializeField] private Sprite mildSpr;
    [SerializeField] private Sprite avidSpr;
    [SerializeField] private Sprite extremeSpr;
    [SerializeField] private Sprite boomSpr;

    private Light2D burningLight;

    private bool died = false;

    private enum State{

        idle,
        mild,
        avid,
        extreme,
        boom,
        disappear
      
    }

    private State stateCurrent;


    void AlterState(State toState)
    {
        stateCurrent = toState;

    }

    void Start()
    {
     
      float circularCenterX = Random.Range(manager.spawnCoordinateRangeX1, manager.spawnCoordinateRangeX2);
      float circularCenterY = Random.Range(manager.spawnCoordinateRangeY1, manager.spawnCoordinateRangeY2);
      circularCenter= new Vector3(circularCenterX, circularCenterY,0);

      AlterState(State.idle);
      takoyakiSprite = GetComponent<SpriteRenderer>();
      takoyakiSprite.sprite = mildSpr;

      burningLight = GetComponentInChildren<Light2D>();
  

    }


    void Update()
    {
        if (stateCurrent == State.idle || stateCurrent == State.mild || stateCurrent == State.avid || stateCurrent == State.extreme)
        {

            takoyakiMovement();

        }

        switch (stateCurrent)
        {
            case State.idle:
                inIdle();
                break;

            case State.mild:
                inmild();
                break;

            case State.avid:
                inavid();
                break;

            case State.extreme:
                inextreme();
                break;

            case State.boom:
                inboom();
                break;

            case State.disappear:
                indisappear();
                break;

        }


    }


    void takoyakiMovement()
    {
        rotatingAngle += rotateSpeed * Time.deltaTime;

        float alterX = Mathf.Cos(rotatingAngle) * radius;
        float alterY = Mathf.Sin(rotatingAngle) * radius;

        transform.position = circularCenter + new Vector3(alterX, alterY, 0);



    }



    //States-------------------------------------------------------------------

    void inIdle()
        {
            burningLight.pointLightOuterRadius = 1.0f;
            StartCoroutine(IdleOp());

        }

        IEnumerator IdleOp()
        {
            yield return new WaitForSeconds(2f);
            AlterState(State.mild);

        }

        void inmild()
        {
       
            StartCoroutine(mildOp());
        

        }

        IEnumerator mildOp()
        {
            yield return new WaitForSeconds(10f);
            burningLight.pointLightOuterRadius = 1.3f;
            rotateSpeed = Random.Range(1f,3f);
            takoyakiSprite.sprite = avidSpr;
            AlterState(State.avid);
        }

        void inavid()
        {

       
            StartCoroutine(avidOp());
        


        }

        IEnumerator avidOp()
        {
            yield return new WaitForSeconds(8f);
            burningLight.pointLightOuterRadius = 1.6f;
            rotateSpeed = Random.Range(5f, 10f);
            takoyakiSprite.sprite = extremeSpr;
            AlterState(State.extreme);
        }

        void inextreme()
        {

        if (died == false)
        {
            StartCoroutine(extremeOp());
        }

        }

        IEnumerator extremeOp()
        {
            died = true;
            yield return new WaitForSeconds(6f);
            rotateSpeed = 0f;
            takoyakiSprite.sprite = boomSpr;
            AlterState(State.boom);
            died = false;
        }


        void inboom()
        {
        StartCoroutine(boomOp());
        

        }

       IEnumerator boomOp()
    {
   
        yield return new WaitForSeconds(3f);
        AlterState(State.disappear);
    
    }

       void indisappear()
    {
        manager.ReactivateLoop();
        Destroy(gameObject);


    }



}
