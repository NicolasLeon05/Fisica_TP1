using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private Vector2 initialPosition;

    public Vector2 Position => transform.position;
    public float Radius => radius;

    private void Start()
    {
        transform.position = initialPosition;
    }
    private void OnValidate()
    {
        radius = transform.lossyScale.x / 2f;
    }
}