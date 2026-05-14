using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float frictionCoefficient;
    [SerializeField] private float initialRotation = 0;

    [SerializeField] private float height;
    [SerializeField] private float width;

    [SerializeField] private List<Wall> walls;

    private void OnValidate()
    {
        //Update rotation
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        transform.localScale = new Vector3(width, height, 1);
    }
}
