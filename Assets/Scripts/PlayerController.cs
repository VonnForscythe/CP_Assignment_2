using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    [Header("PlayerSettings")]
    [Space(2)]
    [Tooltip("Speed value must be between 1 and 6.")]
    [Range(1.0f, 6.0f)]
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    public float gravity;
    //public int collectible;

    public int maxHealth = 5;
    public int currentHealth;
    public HealthBar healthBar;

    public int maxAmmo = 3;
    public int currentAmmo;
    public AmmoBar ammoBar;
    //public int score;
    //public int powerUp;

    //private float timeBtwShots;
    //public float startTimeBtwShots;
    // public Rigidbody projectile2;

    public Material[] Materials;
    public Renderer TargetRenderer;


    Vector3 moveDirection;
    GameObject player;

    enum ControllerType { SimpleMove, Move };
    [SerializeField] ControllerType type;

    [Header("Weapon Settings")]
    public float projectileForce;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;



    void Start()
    {

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        controller.minMoveDistance = 0.0f;

        animator.applyRootMotion = false;

        name = "Player";

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentAmmo = maxAmmo;
        ammoBar.SetMaxAmmo(maxAmmo);

        if (speed <= 0)
        {
            speed = 6.0f;

            Debug.LogWarning(name + ": Speed not set. Defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 6.0f;
            // throw new UnassignedReferenceException("Jump Problem");

            Debug.LogWarning(name + ": jumpSpeed not set. Defaulting to " + jumpSpeed);
        }

        if (rotationSpeed <= 0)
        {
            rotationSpeed = 10.0f;

            Debug.LogWarning(name + ": rotationSpeed not set. Defaulting to " + rotationSpeed);
        }

        if (gravity <= 0)
        {
            gravity = 6.0f;

            Debug.LogWarning(name + ": gravity not set. Defaulting to " + gravity);
        }

        if (projectileForce <= 0)
        {
            projectileForce = 10.0f;

            Debug.LogWarning(name + ": projectileForce not set. Defaulting to " + projectileForce);
        }

        if (!projectileSpawnPoint)
            Debug.LogWarning(name + ": Missing projectilePrefab.");

        if (!projectilePrefab)
            Debug.LogWarning(name + ": Missing projectileSpawnPoint.");

        moveDirection = Vector3.zero;
    }

    void Update()
    {
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);


        switch (type)
        {

            case ControllerType.SimpleMove:

                //transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

                controller.SimpleMove(transform.forward * Input.GetAxis("Vertical") * speed);

                break;

            case ControllerType.Move:

                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection *= speed;

                    moveDirection = transform.TransformDirection(moveDirection);

                    if (Input.GetButtonDown("Jump"))
                        moveDirection.y = jumpSpeed;
                }

                moveDirection.y -= gravity * Time.deltaTime;

                controller.Move(moveDirection * Time.deltaTime);

                break;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fire();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            reload(maxAmmo);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    animator.SetTrigger("isShooting");

        //}

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);

        }




    }

    //public void punch()
    //{
    //    animator.SetTrigger("isPunching");

    //}

    public void fire()
    {
        // animator.SetTrigger("isShooting");

        //Debug.Log("Fired");
        if (currentAmmo >= 1) { 

            if (projectileSpawnPoint && projectilePrefab)
            {

                // Make the projectile
                Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

                //Shoot Projectile
                temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

                // Destroy Projectile after 2.0s
                Destroy(temp.gameObject, 2.0f);
                // }
            }
            MinusAmmo(1);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bounce")
        {
            jumpSpeed = 6;
        }

        if (other.gameObject.tag == "Hazard")
        {
            TakeDamage(1);
        }

        if (other.gameObject.tag == "Health")
        {
            Heal(maxHealth);
        }

        if (other.gameObject.tag == "Water")
        {
            speed = 4;
        }

        if (other.gameObject.tag == "Death")
        {
            Death();
        }

        if (other.gameObject.tag == "Objective")
        {
            Objective();
        }
    }

 void Death()
    {
        if (SceneManager.GetActiveScene().name == "World1")
        SceneManager.LoadScene("EndScreen");        
    }

    void Objective()
    {
        if (SceneManager.GetActiveScene().name == "World1")
            SceneManager.LoadScene("VictoryScreen");
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void MinusAmmo(int Subammo)
    {
        currentAmmo -= Subammo;

        ammoBar.SetAmmo(currentAmmo);
    }

    void reload(int maxAmmo)
    {
        currentAmmo = maxAmmo;

        ammoBar.SetAmmo(currentAmmo);
    }

    void Heal(int maxHealth)
    {
        currentHealth = maxHealth;

        healthBar.SetHealth(currentHealth);       
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bounce")
        {
            jumpSpeed = 4;
        }

        if (other.gameObject.tag == "Water")
        {
            speed = 6;
        }
    }

    [ContextMenu("Reset Stats")]
    void ResetStats()
    {
        speed = 6.0f;
    }
}
