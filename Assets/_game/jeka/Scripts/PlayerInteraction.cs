using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private string leverLayerName = "Lever"; // Название слоя для рычагов
    [SerializeField] private string noteLayerName = "Note"; // Название слоя для рычагов
    [SerializeField] private float rayDistance = 3f; // Дистанция рейкаста
    [SerializeField] private KeyCode interactKey = KeyCode.F; // Клавиша взаимодействия

    private int leverLayerMask;
    private int noteLayerMask;// Закешированный LayerMask

    private void Start()
    {
        leverLayerMask = LayerMask.GetMask(leverLayerName);
        noteLayerMask = LayerMask.GetMask(noteLayerName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            CheckForLever();
            CheckForNote();
        }
    }

    private void CheckForLever()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance, leverLayerMask))
        {
            Lever lever = hit.collider.GetComponent<Lever>();

            if (lever != null)
            {
                lever.ActivateLever(); // Активируем рычаг
            }
        }
    }
    
    private void CheckForNote()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance, noteLayerMask))
        {
            Note note = hit.collider.GetComponent<Note>();
            
            if (note != null)
            {
                note.ActivateNote(); // Активируем рычаг
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
    }
}
