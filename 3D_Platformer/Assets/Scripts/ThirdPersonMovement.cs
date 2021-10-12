using System.Collections;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour {
    // Scripts for player movement in 3D
    [Header("General")]
    [SerializeField] private float inputSensitivity = 0.1f;

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cam;

    [Header("Falling")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float closeEnough = 0.1f;

    [Header("Jumping")]
    [SerializeField] private float jumpSpeed = 5f;

    private GameStateManager manager;
    private CharacterController controller;
    private Animator animator;
    private float turnSmoothVelocity;
    private float verticalVelocity = 0f;
    private bool isGrounded = false;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        animator = transform.Find("MouseMan").GetComponent<Animator>();
        manager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
    }

    private void Update() {
        // Are we on ground?
        RaycastHit collision;
        if (Physics.Raycast(groundPosition.position, Vector3.down, out collision, closeEnough, platformLayer)) {
            isGrounded = true;
            //Debug.Log("Here");
        } else {
            isGrounded = false;
        }

        // Update Vertical speed
        if (!isGrounded) {
            animator.SetBool("Falling", true);
            verticalVelocity += gravityFactor * -9.81f * Time.deltaTime;
        } else {
            verticalVelocity = 0f;
            animator.SetBool("Falling", false);
        }

        if(isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
            isGrounded = false;
            animator.SetTrigger("Jump");
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 moveDir = Vector3.zero;
        if (dir.magnitude >= inputSensitivity) {
            animator.SetFloat("Speed", speed);
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        } else {
            animator.SetFloat("Speed", 0f);
        }
        Vector3 y = Vector3.up * verticalVelocity * Time.deltaTime;
        moveDir = moveDir.normalized * speed * Time.deltaTime;
        controller.Move(moveDir + y.normalized * Time.deltaTime);
    }

    private void victoryCelebration() {
        animator.SetTrigger("Celebrate");
    }

   IEnumerator Celebrate(float delay) {
        victoryCelebration();
        yield return new WaitForSeconds(delay);
        manager.Victory = true;
        manager.GameOver = true;
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("here");
        if(other.tag == "Bottom") {
            manager.GameOver = true;
        } else if(other.tag == "Star") {
            StartCoroutine("Celebrate", 5f);
        }
    }
}
