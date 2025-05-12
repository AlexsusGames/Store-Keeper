using UnityEngine;

public class CartController : MonoBehaviour
{
    [SerializeField] private Rigidbody cartRb;
    [SerializeField] private float followSpeed = 10f;

    private Transform followPoint;
    private bool isAttached = false;

    private void FixedUpdate()
    {
        if (!isAttached || followPoint == null)
            return;

        cartRb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = followPoint.position;
        Quaternion targetRotation = followPoint.transform.rotation * Quaternion.identity;
        cartRb.MoveRotation(targetRotation);

        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        if (distance < 0.01f)
        {
            cartRb.velocity = Vector3.zero;
        }
        else
        {
            direction = direction.normalized;

            float targetSpeed = Mathf.Lerp(0f, followSpeed, distance);

            Vector3 desiredVelocity = direction * targetSpeed;
            cartRb.velocity = Vector3.Lerp(cartRb.velocity, desiredVelocity, Time.fixedDeltaTime * followSpeed);
        }
    }

    public void Attach(Transform point)
    {
        cartRb.isKinematic = false;

        followPoint = point;
        isAttached = true;

        Physics.IgnoreLayerCollision(0, 13, true);
    }

    public void Detach()
    {
        cartRb.isKinematic = true;
        Physics.IgnoreLayerCollision(0, 13, false);

        isAttached = false;

        cartRb.angularVelocity = Vector3.zero;
        cartRb.velocity = Vector3.zero;
    }
}
