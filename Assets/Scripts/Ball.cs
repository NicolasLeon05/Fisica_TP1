using UnityEngine;

public class Ball : MonoBehaviour
{
    public float radius = 0.5f;
    public float mass = 1f;
    public float restitution = 0.9f;

    public Vector2 velocity;
    public bool isActive = true;

    public float friction = 0.95f;

    void Update()
    {
        if (!isActive) return;

        transform.position += (Vector3)(velocity * Time.deltaTime);

        velocity *= friction;

        if (velocity.magnitude < 0.01f)
            velocity = Vector2.zero;
    }

    public void Collide(Ball other)
    {
        Vector2 posA = transform.position;
        Vector2 posB = other.transform.position;

        Vector2 normal = (posB - posA).normalized;

        Vector2 relativeVelocity = other.velocity - velocity;
        float velAlongNormal = Vector2.Dot(relativeVelocity, normal);

        if (velAlongNormal > 0) return;

        float e = Mathf.Min(restitution, other.restitution);

        float j = -(1 + e) * velAlongNormal;
        j /= (1 / mass + 1 / other.mass);

        Vector2 impulse = j * normal;

        velocity -= impulse / mass;
        other.velocity += impulse / other.mass;
    }

    public void Separate(Ball other)
    {
        Vector2 posA = transform.position;
        Vector2 posB = other.transform.position;

        float dist = Vector2.Distance(posA, posB);
        float overlap = (radius + other.radius) - dist;

        if (overlap <= 0) return;

        Vector2 dir = (posB - posA).normalized;

        float totalMass = mass + other.mass;

        transform.position -= (Vector3)(dir * (overlap * (other.mass / totalMass)));
        other.transform.position += (Vector3)(dir * (overlap * (mass / totalMass)));
    }
}