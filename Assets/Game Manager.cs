using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cube Spawn Settings")]
    public Vector3 rightSpawn = new Vector3(8, 4.521f, 0);
    public Vector3 leftSpawn = new Vector3(0, 4.521f, 8);
    [SerializeField] private GameObject Cube;
    public bool isSpawnRight = true;

    [Space]

    [Header("Last and next cube info")]
    public GameObject activeCube = null;
    public GameObject lastCube;
    public Vector3 cubeScaleInfo = new Vector3(6, 1, 6);

    [Space]

    [Header("Camera Settings")]
    public Transform camFollowPoint;

    private void Awake()
    {
        if(instance == null) 
            instance = this;    
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCube();
    }

    private void SpawnCube()
    {
        if (activeCube != null)
            return;

        if(isSpawnRight)
        {
            GameObject newCube = Instantiate(Cube, rightSpawn, Quaternion.identity);
            activeCube = newCube;
        }
        else
        {
            GameObject newCube = Instantiate(Cube, leftSpawn, Quaternion.identity);
            activeCube = newCube;
        }
    }
}
