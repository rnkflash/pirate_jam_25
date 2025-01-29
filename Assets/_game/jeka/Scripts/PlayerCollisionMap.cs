using UnityEngine;

public class PlayerCollisionMap : MonoBehaviour
{
    [SerializeField] private string floorLayerName = "MAP"; // Название слоя пола
    [SerializeField] private float rayDistance = 5f; // Дистанция рейкаста вниз

    private int floorLayerMask; // Закешированное значение LayerMask
    private FloorVisibility lastHitFloor; // Хранит последний найденный объект

    private void Start()
    {
        floorLayerMask = LayerMask.GetMask(floorLayerName); // Кешируем LayerMask один раз
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance, floorLayerMask))
        {
            FloorVisibility floor = hit.collider.GetComponent<FloorVisibility>();

            if (floor != null && floor != lastHitFloor)
            {
                floor.ShowMesh();
                lastHitFloor = floor;
            }
        }
        else
        {
            lastHitFloor = null; // Сбрасываем, если луч не попал в пол
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }
}