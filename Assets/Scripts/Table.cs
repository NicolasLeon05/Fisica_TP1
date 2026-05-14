using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float frictionCoefficient = 0.2f;
    [SerializeField] private float initialRotation = 0f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float width = 20f;
    [SerializeField] private List<Wall> walls;

    public List<Wall> Walls => walls;

    public float FrictionCoefficient => frictionCoefficient;

    private void OnValidate()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        transform.localScale = new Vector3(width, height, 1);
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
    }
}