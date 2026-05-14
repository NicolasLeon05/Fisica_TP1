using UnityEngine;


public static class Collision
{
    public struct CollisionInfo
    {
        public bool collision;
        public Vector2 normal;
        public float penetration;
    }

    public static float DotProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static CollisionInfo CircleVsOBB(Circle circle, OBB box)
    {
        CollisionInfo info = new CollisionInfo();
        Vector2 localCirclePosition = circle.center - box.center;
        float rad = -box.rotation * Mathf.Deg2Rad;

        Vector2 local;
        local.x = localCirclePosition.x * Mathf.Cos(rad) - localCirclePosition.y * Mathf.Sin(rad);
        local.y = localCirclePosition.x * Mathf.Sin(rad) + localCirclePosition.y * Mathf.Cos(rad);

        Vector2 closestPoint;
        closestPoint.x = Mathf.Clamp(local.x, -box.halfSize.x, box.halfSize.x);
        closestPoint.y = Mathf.Clamp(local.y, -box.halfSize.y, box.halfSize.y);

        Vector2 difference = local - closestPoint;
        float distance = difference.magnitude;

        if (distance > circle.radius)
        {
            info.collision = false;
            return info;
        }

        info.collision = true;

        if (distance != 0)
        {
            Vector2 localNormal = difference.normalized;
            float cos = Mathf.Cos(-rad);
            float sin = Mathf.Sin(-rad);

            Vector2 worldNormal;
            worldNormal.x = localNormal.x * cos - localNormal.y * sin;
            worldNormal.y = localNormal.x * sin + localNormal.y * cos;

            info.normal = worldNormal.normalized;
        }
        else
        {
            info.normal = Vector2.up;
        }

        info.penetration = circle.radius - distance;

        return info;
    }

    public static CollisionInfo CircleVsCircle(Circle a, Circle b)
    {
        CollisionInfo info = new CollisionInfo();

        Vector2 difference = b.center - a.center;

        float distance = difference.magnitude;

        float radiusSum = a.radius + b.radius;

        if (distance >= radiusSum)
        {
            info.collision = false;
            return info;
        }

        info.collision = true;

        if (distance > 0.0001f)
            info.normal = difference.normalized;
        else
            info.normal = Vector2.right;

        info.penetration = radiusSum - distance;
        return info;
    }
}