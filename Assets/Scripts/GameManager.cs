using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball[] balls;
    public Hole[] holes;

    public float tableWidth = 10f;
    public float tableHeight = 5f;

    public float forcePower = 10f;

    void Update()
    {
        HandleInput();
        HandleCollisions();
        HandleWalls();
        CheckHoles();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (balls.Length == 0) return;

            Ball ball = balls[0];

            if (!ball.isActive) return;

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = ((Vector2)mouse - (Vector2)ball.transform.position).normalized;

            ball.velocity += dir * (forcePower / ball.mass);
        }
    }

    void HandleCollisions()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            for (int j = i + 1; j < balls.Length; j++)
            {
                if (!balls[i].isActive || !balls[j].isActive) continue;

                float dist = Vector2.Distance(
                    balls[i].transform.position,
                    balls[j].transform.position
                );

                if (dist < balls[i].radius + balls[j].radius)
                {
                    balls[i].Collide(balls[j]);
                    balls[i].Separate(balls[j]);
                }
            }
        }
    }

    void HandleWalls()
    {
        foreach (var b in balls)
        {
            if (!b.isActive) continue;

            Vector2 pos = b.transform.position;

            if (pos.x - b.radius < 0)
            {
                pos.x = b.radius;
                b.velocity.x *= -b.restitution;
            }

            if (pos.x + b.radius > tableWidth)
            {
                pos.x = tableWidth - b.radius;
                b.velocity.x *= -b.restitution;
            }

            if (pos.y - b.radius < 0)
            {
                pos.y = b.radius;
                b.velocity.y *= -b.restitution;
            }

            if (pos.y + b.radius > tableHeight)
            {
                pos.y = tableHeight - b.radius;
                b.velocity.y *= -b.restitution;
            }

            b.transform.position = pos;
        }
    }

    void CheckHoles()
    {
        foreach (var b in balls)
        {
            if (!b.isActive) continue;

            foreach (var h in holes)
            {
                float dist = Vector2.Distance(
                    b.transform.position,
                    h.transform.position
                );

                if (dist < h.radius)
                {
                    b.isActive = false;
                    b.gameObject.SetActive(false);
                }
            }
        }
    }
}