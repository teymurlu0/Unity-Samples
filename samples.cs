using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComprehensiveScript : MonoBehaviour
{
    // 1. Basic Movement
    public float speed = 5f;

    void Update()
    {
        BasicMovement();
    }

    void BasicMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // 2. Jumping
    public float jumpForce = 10f;
    private bool isGrounded;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // 3. Rotating Object
    public float rotationSpeed = 100f;

    void RotateObject()
    {
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);
    }

    // 4. Shooting Projectile
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 20f;

    void ShootProjectile()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject proj = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            proj.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
        }
    }

    // 5. Health System
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle death
        Debug.Log("Player died");
    }

    // 6. Collectible Items
    public int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            score += 10;
            Destroy(other.gameObject);
        }
    }

    // 7. Changing Scenes
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // 8. UI: Updating Text
    public Text scoreText;

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // 9. UI: Health Bar
    public Image healthBar;

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    // 10. Enemy AI: Follow Player
    public Transform player;
    public float enemySpeed = 2f;

    void FollowPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * enemySpeed * Time.deltaTime, Space.World);
    }

    // 11. Enemy AI: Patrol
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Patrol()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * enemySpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // 12. Opening Doors
    public Animator doorAnimator;

    void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }

    // 13. Spawning Enemies
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }

    // 14. Triggering Events
    public UnityEngine.Events.UnityEvent myEvent;

    void TriggerEvent()
    {
        myEvent.Invoke();
    }

    // 15. Picking Up Objects
    public Transform holdPoint;

    void PickUpObject(GameObject obj)
    {
        obj.transform.position = holdPoint.position;
        obj.transform.parent = holdPoint;
    }

    // 16. Throwing Objects
    void ThrowObject(GameObject obj)
    {
        obj.transform.parent = null;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(holdPoint.forward * 10f, ForceMode.Impulse);
    }

    // 17. Pause Game
    private bool isPaused = false;

    void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    // 18. Saving Game Data
    void SaveGame()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Health", currentHealth);
    }

    // 19. Loading Game Data
    void LoadGame()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        currentHealth = PlayerPrefs.GetInt("Health", maxHealth);
    }

    // 20. Basic Animation Control
    public Animator characterAnimator;

    void ControlAnimation()
    {
        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetBool("IsJumping", !isGrounded);
    }

    // 21. Raycasting
    void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
    }

    // 22. Playing Sound Effects
    public AudioSource audioSource;
    public AudioClip clip;

    void PlaySound()
    {
        audioSource.PlayOneShot(clip);
    }

    // 23. Using Coroutines
    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Waited for " + waitTime + " seconds.");
    }

    // 24. Camera Follow
    public Transform cameraTransform;

    void CameraFollow()
    {
        cameraTransform.position = new Vector3(transform.position.x, cameraTransform.position.y, transform.position.z - 10f);
    }

    // 25. Changing Materials
    public Material newMaterial;

    void ChangeMaterial(GameObject obj)
    {
        obj.GetComponent<Renderer>().material = newMaterial;
    }

    // 26. Changing Colors
    void ChangeColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    // 27. Lighting Control
    public Light sceneLight;

    void ToggleLight()
    {
        sceneLight.enabled = !sceneLight.enabled;
    }

    // 28. Particle Systems
    public ParticleSystem particleSystem;

    void PlayParticles()
    {
        particleSystem.Play();
    }

    // 29. Simple UI Interaction
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }

    // 30. Simple Timer
    private float timer = 0f;
    public float timeLimit = 10f;

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            Debug.Log("Time's up!");
            timer = 0f;
        }
    }
}
