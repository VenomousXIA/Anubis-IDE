using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterControllerWAnimations : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Space]

    [Header("Properties")]
    [HideInInspector]
    [Range(0, 1)] [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    [Range(0, 100)] [SerializeField] private float rotSpeed = 80;
    [Range(0, 10)] [SerializeField] private float rot = 0f;

    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool toggleLock = false;
    [SerializeField] private bool locked = false;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 targetLocation;

    private float input_h, input_v;

    private void Awake()
    {
    }

    void Start()
    {
        #region Game objects components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cam = GameObject.Find("/Main Camera").GetComponent<Transform>();
        #endregion

        #region Rigidbody proporties as it was
        rb.freezeRotation = true;
        #endregion

        #region Animator properties as it was
        animator.applyRootMotion = true;
        animator.updateMode = AnimatorUpdateMode.Normal;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        #endregion
    }

    // Update is called once per frame
    private void Update()
    {
        input_h = Input.GetAxis("Horizontal");
        input_v = Input.GetAxis("Vertical");
        isJumping = Input.GetKey(KeyCode.Space);
        isRunning = Input.GetKey(KeyCode.LeftShift);
        toggleLock = Input.GetKeyDown(KeyCode.Z);
    }

    void FixedUpdate()
    {
        move();
    }

    private void move()
    {

        if (input_v != 0)
        {
            animator.SetBool("isWalking", true);
        }
        if (isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        if (!isRunning)
            animator.SetBool("isRunning", false);
        if (input_v == 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        if (isJumping)
        {
            animator.SetBool("midAir", true);
        }
        if (!isJumping) //Will add a function to check isGrounded when u are done
        {
            animator.SetBool("midAir", false);
        }

        //TODO 
        if(toggleLock) //Buggy lockOn System, works with Z.. Makes the character walk on its own will be fixed when u are done
        {
            GameObject[] enemyLocations;
             // Find all game objects with tag Enemy
            enemyLocations = GameObject.FindGameObjectsWithTag("Enemy"); 
            //var closest : GameObject; 
            var distance = Mathf.Infinity; 
            var position = transform.position; 
            // Iterate through them and find the closest one
            for (int i = 0 ;  i < enemyLocations.Length ; i++) 
                { 
                    var diff = (enemyLocations[i].transform.position - position);
                    var curDistance = diff.sqrMagnitude; 

                    if (curDistance < distance) 
                    { 
                        target = enemyLocations[i]; 
                        distance = curDistance; 
                    } 
                }
            targetLocation = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            locked = !locked;
        }
            
        //rot += input_h * rotSpeed * Time.deltaTime;

        //transform.eulerAngles = new Vector3(0, rot, 0);

        if(locked)
        {
            transform.LookAt(targetLocation);
        }

        /*
        Vector3 direction = new Vector3(input_h, 0f, input_v).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f,angle,0f);
            Direction = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
        }
        */
    }
}
