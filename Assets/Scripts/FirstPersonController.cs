using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float spead;
    private int cachedState;
    private bool isHandShowed;

    private bool moveBlock = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        spead = moveSpeed;
    }

    public void MovementBlockEnabled(bool enabled)
    {
        moveBlock = enabled;
    }

    private void ShowHand()
    {
        isHandShowed = !isHandShowed;

        int weigh = isHandShowed ? 1 : 0;
        animator.SetLayerWeight(1, weigh);
    }

    void FixedUpdate()
    {
        if(moveBlock)
        {
            rb.velocity = Vector3.zero;
        }

        if(moveBlock == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if(z == 0 && x == 0)
            {
                rb.velocity = Vector3.zero;
            }

            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? spead * 2 : spead;

            if (x == 0 && z == 0) SetAnimationState(0);
            else if (Input.GetKey(KeyCode.LeftShift)) SetAnimationState(2);
            else SetAnimationState(1);

            //Vector3 move = transform.right * x + transform.forward * z;
            //rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);

            Vector3 move = (transform.right * x + transform.forward * z).normalized;

            rb.velocity = move * currentSpeed;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && moveBlock == false)
        {
            ShowHand();
        }
    }

    private void SetAnimationState(int state)
    {
        if(cachedState != state)
        {
            cachedState = state;
            animator.SetInteger("walkingState", state);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //spead = 1;
    }

    private void OnCollisionExit(Collision collision)
    {
        //spead = moveSpeed;
    }
}

