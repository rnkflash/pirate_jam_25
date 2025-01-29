using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetTrigger("Open"); // Запускаем анимацию открытия двери
        Invoke("DisableDoor", 1f);   // Выключаем объект через 2 секунды
    }

    private void DisableDoor()
    {
        gameObject.SetActive(false);
    }
}
