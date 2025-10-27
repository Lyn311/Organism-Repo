 using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.VisualScripting;

public class OperationOnigiri : OperationOrganism
{
    
    [SerializeField]private float moveSpeed = 3f;
    public GameObject ricePrefab;
    private GameObject target;
    private int riceConsumed = 0;

    [Header("Sprite Change")]
    private SpriteRenderer onigiriSprite;
    [SerializeField] private Sprite smallSpr;
    [SerializeField] private Sprite mediumSpr;
    [SerializeField] private Sprite largeSpr;
    [SerializeField] private Sprite explodeSpr;

    private enum State{

        idle,
        small,
        medium,
        large,
        explode,
      
    }

    private State stateCurrent;


    void AlterState(State toState)
    {
        stateCurrent = toState;

    }

    void Start()
    {
   
      AlterState(State.idle);
      spawnRice();
      riceConsumed = 0;
      onigiriSprite = GetComponent<SpriteRenderer>();
      onigiriSprite.sprite = smallSpr;
  

    }

    
    void spawnRice()
    {
        float riceX = Random.Range(-7.4f, 7.4f);
        float riceY = Random.Range(-3.4f, 3.4f);
        GameObject riceSpawned = Instantiate(ricePrefab,new Vector3(riceX,riceY,0),Quaternion.identity);
        target = riceSpawned;

        RiceOwnership owning = riceSpawned.GetComponent<RiceOwnership>();

        if (owning != null)
        {

            owning.ownership = this;
        
        
        
        }


    }


    void Update()
    {

        Debug.Log(manager.totalOrg);

        switch (stateCurrent)
        {
            case State.idle:
                inIdle();
                break;

            case State.small:
                inSmall();
                break;

            case State.medium:
                inMedium();
                break;

            case State.large:
                inLarge();
                break;

            case State.explode:
                inExplode();
                break;

        }


        if (target != null && (stateCurrent == State.small || stateCurrent == State.medium || stateCurrent == State.large))
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction*moveSpeed*Time.deltaTime,Space.World);

        }


    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Food")
        {

            RiceOwnership owning = other.GetComponent<RiceOwnership>();
            if (owning != null && owning.ownership == this)
            {
                Destroy(other.gameObject);
                riceConsumed += 1;
                spawnRice();
            }

        }



    }

    IEnumerator OnigiriGrowth(Vector3 targetSize,float duration)
    {

        Vector3 currentSize = transform.localScale;
        float aniTimer = 0;

        while (aniTimer < duration)
        {
            transform.localScale = Vector3.Lerp(currentSize,targetSize,aniTimer/duration);
            aniTimer+=Time.deltaTime;
            yield return null;

        }

        transform.localScale = targetSize;


    }



    //States-------------------------------------------------------------------

    void inIdle()
        {

            StartCoroutine(IdleOp());

        }

        IEnumerator IdleOp()
        {
            yield return new WaitForSeconds(2f);
            AlterState(State.small);

        }

        void inSmall()
        {
        if (riceConsumed == 3)
        {
            StartCoroutine(SmallOp());
        }

        }

        IEnumerator SmallOp()
        {
            StartCoroutine(OnigiriGrowth(new Vector3(0.18f, 0.18f, 1f), 0.3f));
            moveSpeed = 2.0f;
            onigiriSprite.sprite = mediumSpr;
            yield return new WaitForSeconds(2f);
            AlterState(State.medium);
        }

        void inMedium()
        {

        if (riceConsumed == 6)
        {
            StartCoroutine(MediumOp());
        }


        }

        IEnumerator MediumOp()
        { 
            StartCoroutine(OnigiriGrowth(new Vector3(0.21f, 0.21f, 1f), 0.3f));
            moveSpeed = 1.5f;
            onigiriSprite.sprite = largeSpr;
            yield return new WaitForSeconds(2f);
            AlterState(State.large);
        }

        void inLarge()
        {
        if (riceConsumed == 9)
        {
            StartCoroutine(LargeOp());
        }

        }

        IEnumerator LargeOp()
        {
            moveSpeed = 0f;
            StartCoroutine(OnigiriGrowth(new Vector3(0.18f, 0.18f, 1f), 0.1f));
            onigiriSprite.sprite = explodeSpr;
            yield return new WaitForSeconds(2f);
            
            AlterState(State.explode);
        }


        void inExplode()
        {
        manager.ReactivateLoop();
        Destroy(gameObject);

        }


    
}
