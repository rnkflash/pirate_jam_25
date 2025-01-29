using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Door door; // Ссылка на дверь
    private Animator animator;
    private Collider leverCollider;
    private bool isActivated = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        leverCollider = GetComponent<Collider>();
    }

    public void ActivateLever()
    {
        if (isActivated) return; // Если уже активирован, ничего не делаем

        isActivated = true;
        animator.SetTrigger("Activate"); // Запускаем анимацию рычага
        leverCollider.enabled = false;   // Отключаем коллайдер для исключения повторных взаимодействий

        if (door != null)
        {
            door.OpenDoor(); // Открываем дверь
        }
    }
}
