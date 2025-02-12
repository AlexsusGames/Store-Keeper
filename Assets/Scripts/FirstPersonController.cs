using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tablet;

    public bool TabletActivity
    {
        get => tablet.activeInHierarchy;
        set => tablet.SetActive(value);
    }

    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float spead;
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

        spead = moveSpeed;
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

    void FixedUpdate()
    {
        if(moveBlock)
        {
            rb.velocity = Vector3.zero;
            SetAnimationState(0);
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (z == 0 && x == 0)
        {
            rb.velocity = Vector3.zero;
        }

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? spead * 2 : spead;

        if (x == 0 && z == 0) SetAnimationState(0);
        else if (Input.GetKey(KeyCode.LeftShift)) SetAnimationState(2);
        else SetAnimationState(1);

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        rb.velocity = move * currentSpeed;
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

