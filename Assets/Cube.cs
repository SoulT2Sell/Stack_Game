using UnityEngine;
public class Cube : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool canMove = true;

    private float speed = 200f;
    private int dir = -1;

    [Header("FallingCube Info")]
    [SerializeField] private GameObject fallingCubePrefab;

    private void Awake()
    {
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        transform.localScale = gameManager.cubeScaleInfo;
    }
    // Update is called once per frame
    void Update()
    {
        HandleChangeDir();
        AddMovement();
        HandleInput();
        if (transform.position.y < gameManager.lastCube.transform.position.y)
        {
            if(isGrounded)
                return;

            Time.timeScale = 0;
        }
    }

    private void HandleChangeDir()
    {
        if (transform.position.x <= -8 || transform.position.z <= -8)
        {
            dir = 1;
        }
        if (transform.position.x >= gameManager.rightSpawn.x || transform.position.z >= gameManager.leftSpawn.z)
        {
            dir = -1;
        }
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = false;
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true;
        }
    }
    public void AddMovement()
    {
        if (!canMove)
            return;

        if (gameManager.isSpawnRight)
        {
            rb.linearVelocity = new Vector3(dir * speed * Time.deltaTime, 0, 0);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, 0, dir * speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded)
            return;

        float posDif;
        GameObject lastCube = gameManager.lastCube;

        CubeGrounded();

        if (gameManager.isSpawnRight)
        {
            posDif = transform.position.x - lastCube.transform.position.x;
            ChangeScaleRightSpawn(posDif);
            FallingCubeOnRightSpawn(posDif);
        }
        else
        {
            posDif = transform.position.z - lastCube.transform.position.z;
            ChangeScaleLeftSpawn(posDif);
            FallingCubeOnLeftSpawn(posDif);
        }

        UpdateSavedInfo();

        gameManager.activeCube = null;
    }
    private void CubeGrounded()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isGrounded = true;
    }
    private void ChangeScaleRightSpawn(float posDif)
    {
        if (posDif >= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - posDif, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - posDif * -1, transform.localScale.y, transform.localScale.z);
        }
        transform.position = new Vector3(transform.position.x - posDif / 2f, transform.position.y, transform.position.z);
    }
    private void ChangeScaleLeftSpawn(float posDif)
    {
        if (posDif >= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - posDif);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - (posDif * -1));
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - posDif / 2f);
    }
    private void FallingCubeOnRightSpawn(float posDif)
    {
        GameObject fallingCube;
        Vector3 fallingPos;

        if (posDif >= 0)
        {
            fallingPos =
                new Vector3(transform.position.x + transform.localScale.x / 2f + posDif / 2f + 0.021f, transform.position.y, transform.position.z);

            fallingCube = Instantiate(fallingCubePrefab, fallingPos, Quaternion.LookRotation(transform.forward));
            fallingCube.transform.localScale = new Vector3(posDif, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            fallingPos =
                new Vector3(transform.position.x - transform.localScale.x / 2f + posDif / 2f - 0.021f, transform.position.y, transform.position.z);

            fallingCube = Instantiate(fallingCubePrefab, fallingPos, Quaternion.LookRotation(transform.forward));
            fallingCube.transform.localScale = new Vector3(posDif * -1, transform.localScale.y, transform.localScale.z);
        }
        Destroy(fallingCube, 10f);
    }
    private void FallingCubeOnLeftSpawn(float posDif)
    {
        GameObject fallingCube;
        Vector3 fallingPos;

        if (posDif >= 0)
        {
            fallingPos =
                new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z / 2f + posDif / 2f + 0.1f);

            fallingCube = Instantiate(fallingCubePrefab, fallingPos, Quaternion.LookRotation(transform.forward));
            fallingCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, posDif);
        }
        else
        {
            fallingPos =
                new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.localScale.z / 2f + posDif / 2f - 0.1f);

            fallingCube = Instantiate(fallingCubePrefab, fallingPos, Quaternion.LookRotation(transform.forward));
            fallingCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, posDif * -1);
        }
        Destroy(fallingCube, 10f);
    }
    private void UpdateSavedInfo()
    {
        gameManager.isSpawnRight = !gameManager.isSpawnRight;

        gameManager.leftSpawn =
            new Vector3(transform.position.x, transform.position.y + 1.021f, gameManager.leftSpawn.z);
        gameManager.rightSpawn =
            new Vector3(gameManager.rightSpawn.x, transform.position.y + 1.021f, transform.position.z);

        gameManager.cubeScaleInfo = transform.localScale;
        gameManager.lastCube = gameObject;

        gameManager.camFollowPoint.position =
            new Vector3(gameManager.camFollowPoint.position.x, gameManager.camFollowPoint.position.y + 1.021f, gameManager.camFollowPoint.position.z);
    }
}
