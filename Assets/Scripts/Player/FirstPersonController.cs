using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Tablet clipboard;

    public float DampTime;
    public bool IsHasCart { private get; set; }

    public bool TabletActivity => clipboard.gameObject.activeInHierarchy;

    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float speed;
    private int cachedState;

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

    public void PlayTabletSound()
    {
        Core.Sound.PlayClip(AudioType.Tablet);
    }

    public void ShowTablet()
    {
        if (TabletActivity)
        {
            PlayTabletSound();

            clipboard.Interact();
        }
        else Core.Clues.Show("To show the clipboard, pick it up from the table when the truck arrives.");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x == 0 && z == 0 ||  moveBlock)
        {
            SetAnimationState(0);
            Core.Sound.EnableStepsSound(false, IsHasCart);
            rb.velocity = Vector3.zero;
            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && z > 0;
        float currentSpeed = isRunning ? speed * 2 : speed;

        Vector3 moveDirection = (transform.right * x + transform.forward * z).normalized;
        rb.velocity = moveDirection * currentSpeed;

        int animState = GetAnimationState(x, z, isRunning);
        SetAnimationState(animState);

        Core.Sound.EnableStepsSound(true, IsHasCart, isRunning);
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowTablet();
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

