using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Basics")]
    public GameObject[] orgPrefabs;
    public float spawnCoolDown = 3f;
    [SerializeField] private int totalOrg = 0;

    [Header("Spawn Coordinates")]
    [SerializeField] public float spawnCoordinateRangeX1 = -7.2f;
    [SerializeField] public float spawnCoordinateRangeX2 = 7.24f;
    [SerializeField] public float spawnCoordinateRangeY1 = 3.4f;
    [SerializeField] public float spawnCoordinateRangeY2 = -3.8f;

    void Start()
    {
        StartCoroutine(SpawnChain());
        totalOrg = 0;
    }

    IEnumerator SpawnChain()
    {
        while(totalOrg<7){ 

        SpawnOrg();
        totalOrg += 1;
        yield return new WaitForSeconds(spawnCoolDown);


    }

    }

    void SpawnOrg()
    {
        float spawnCoordinateX = Random.Range(spawnCoordinateRangeX1,spawnCoordinateRangeX2);
        float spawnCoordinateY = Random.Range(spawnCoordinateRangeY1, spawnCoordinateRangeY2);
        GameObject spawningOrg = orgPrefabs[Random.Range(0,orgPrefabs.Length)];
        GameObject spawnedOrg = Instantiate(spawningOrg,new Vector3(spawnCoordinateX,spawnCoordinateY,0),Quaternion.identity);

     

    }




}
