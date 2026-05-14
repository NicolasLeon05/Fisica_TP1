using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Table table;
    [SerializeField] private List<Ball> balls;
    [SerializeField] private List<Hole> holes;

    [Header("Shot")]
    [SerializeField] private Ball selectedBall;
    [SerializeField] private float shotForce = 10f;

    private Vector2 shootDirection = Vector2.right;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleInput();
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        foreach (Ball ball in balls)
        {
            ball.ApplyFriction(table.FrictionCoefficient);
            ball.PhysicsStep(dt);
        }

        ResolveWallCollisions();
        ResolveBallCollisions();
        ResolveHoles();
    }

    private void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            Debug.Log("Mouse position: " + mouseWorldPosition);
            Vector2 direction = (mouseWorldPosition - selectedBall.transform.position);

            if (direction != Vector2.zero)
            {
                shootDirection = direction.normalized;
                selectedBall.SetDebugDirection(shootDirection);
            }

            selectedBall.SetVelocity(Vector2.zero);

            Vector2 impulse = shootDirection * shotForce;
            selectedBall.AddImpulse(impulse);
        }
    }

    private void ResolveWallCollisions()
    {
        foreach (Ball ball in balls)
        {
            foreach (Wall wall in table.Walls)
            {
                Collision.CollisionInfo info = Collision.CircleVsOBB(ball.Circle, wall.OBB);

                if (!info.collision)
                    continue;

                ResolveWallCollision(ball, info);
            }
        }
    }

    private void ResolveWallCollision(Ball ball, Collision.CollisionInfo info)
    {
        ball.Move(info.normal * info.penetration);

        Vector2 velocity = ball.Velocity;
        float velocityAlongNormal = Collision.DotProduct(velocity, info.normal);

        if (velocityAlongNormal > 0)
            return;

        Vector2 reflected = velocity - (1 + ball.Restitution) * velocityAlongNormal * info.normal;
        ball.SetVelocity(reflected);
    }

    private void ResolveBallCollisions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Ball a = balls[i];
                Ball b = balls[j];

                Collision.CollisionInfo info = Collision.CircleVsCircle(a.Circle, b.Circle);

                if (!info.collision)
                    continue;

                ResolveBallCollision(a, b, info);
            }
        }
    }

    private void ResolveBallCollision(Ball a, Ball b, Collision.CollisionInfo info)
    {
        float totalMass = a.Mass + b.Mass;

        //Separacion de las bolas respecto a su masa
        Vector2 separation = info.normal * info.penetration;
        a.Move(-separation * (b.Mass / totalMass));
        b.Move(separation * (a.Mass / totalMass));


        //Como se mueve B respecto de A
        Vector2 relativeVelocity = b.Velocity - a.Velocity;
        //Que tan rapido se mueven en la direccion de la colision
        float velocityAlongNormal = Collision.DotProduct(relativeVelocity, info.normal);

        if (velocityAlongNormal > 0) //Se alejan
            return;

        //Cuanta momento lineal se conserva
        float restitution = Mathf.Min(a.Restitution, b.Restitution);


        float impulseMagnitude = -(1 + restitution) * velocityAlongNormal;
        impulseMagnitude /= (1f / a.Mass) + (1f / b.Mass); //Cuanto van a cambiar los momentos
        Vector2 impulse = impulseMagnitude * info.normal; //Se divide en x e y en base a la normal

        //Aplica el impulso
        Vector2 newVelocityA = a.Velocity - impulse / a.Mass;
        Vector2 newVelocityB = b.Velocity + impulse / b.Mass;

        a.SetVelocity(newVelocityA);
        b.SetVelocity(newVelocityB);
    }

    private void ResolveHoles()
    {
        List<Ball> ballsToRemove = new List<Ball>();

        foreach (Ball ball in balls)
        {
            foreach (Hole hole in holes)
            {
                float distance = Vector2.Distance(ball.Position, hole.Position);
                if (distance <= hole.Radius)
                {
                    ballsToRemove.Add(ball);
                    break;
                }
            }
        }

        foreach (Ball ball in ballsToRemove)
        {
            balls.Remove(ball);
            Destroy(ball.gameObject);
        }

        if (balls.Count == 0)
            Debug.Log("Simulation Finished");
    }
}