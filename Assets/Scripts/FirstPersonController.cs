using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tablet;

    public float DampTime;

    public bool TabletActivity
    {
        get => tablet.activeInHierarchy;
        set => tablet.SetActive(value);
    }

    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float speed;
    private int cachedState;
    private bool isHandShowed;

    private bool moveBlock = false;

    public Vector3 PlayerPosition
    {
        get => rb.position;
        set => rb.position = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        speed = moveSpeed;
    }

    public void MovementBlockEnabled(bool enabled)
    {
        moveBlock = enabled;
    }

    public void ShowHand()
    {
        if(TabletActivity)
        {
            isHandShowed = !isHandShowed;

            int weigh = isHandShowed ? 1 : 0;
            animator.SetLayerWeight(1, weigh);
        }
    }

    private void FixedUpdate()
    {
        if (moveBlock)
        {
            rb.velocity = Vector3.zero;
            SetAnimationState(0);
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x == 0 && z == 0)
        {
            SetAnimationState(0);
            rb.velocity = Vector3.zero;
            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && z > 0;
        float currentSpeed = isRunning ? speed * 2 : speed;

        Vector3 moveDirection = (transform.right * x + transform.forward * z).normalized;
        rb.velocity = moveDirection * currentSpeed;

        int animState = GetAnimationState(x, z, isRunning);
        SetAnimationState(animState);
    }

    private int GetAnimationState(float x, float z, bool isRunning)
    {
        if (z < 0)
            x *= -1;

        animator.SetFloat("x", x, DampTime, Time.fixedDeltaTime);
        animator.SetFloat("z", z, DampTime, Time.fixedDeltaTime);

        if (isRunning)
            return 2;

        return 1;
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
}

