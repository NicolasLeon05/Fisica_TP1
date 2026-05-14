using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float mass;
    [SerializeField] private float radius;
    [Range(0f, 1f)][SerializeField] private float restitutionCoefficient;

    [SerializeField] private Vector2 initialPosition;
}
