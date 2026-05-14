using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float mass = 1f;

    [SerializeField] private float radius = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float restitutionCoefficient = 1f;

    [Header("Initial State")]
    [SerializeField] private Vector2 initialPosition;

    [SerializeField] private Vector2 initialVelocity;

    private Vector2 velocity;
    private Vector2 acceleration;

    private Vector2 debugDirection = Vector2.right;

    public Vector2 Position => transform.position;
    public Vector2 Velocity => velocity;
    public float Radius => radius;
    public float Mass => mass;
    public float Restitution => restitutionCoefficient;

    public Circle Circle
    {
        get
        {
            return new Circle(
                Position,
                radius);
        }
    }

    private void Start()
    {
        transform.position = initialPosition;

        velocity = initialVelocity;

        radius = transform.lossyScale.x / 2f;
    }

    private void OnValidate()
    {
        radius = transform.lossyScale.x / 2f;
    }

    public void ApplyFriction(float frictionCoefficient)
    {
        if (velocity.magnitude < 0.01f)
        {
            velocity = Vector2.zero;
            return;
        }

        Vector2 frictionAcceleration =
            -velocity.normalized *
            frictionCoefficient;

        acceleration += frictionAcceleration;
    }

    public void PhysicsStep(float deltaTime)
    {
        velocity += acceleration * deltaTime;

        transform.position +=
            (Vector3)(velocity * deltaTime);

        acceleration = Vector2.zero;
    }

    public void AddAcceleration(Vector2 accleration)
    {
        velocity += accleration / mass;
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }

    public void Move(Vector2 offset)
    {
        transform.position += (Vector3)offset;
    }

    public void SetDebugDirection(Vector2 direction)
    {
        debugDirection = direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + velocity);

    }
}