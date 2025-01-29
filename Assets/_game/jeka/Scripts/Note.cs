using _game.Note;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string text;
    private Collider leverCollider;
    private bool isActivated = false;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        leverCollider = GetComponent<Collider>();
    }

    public void ActivateNote() {
        isActivated = !isActivated;
        if (isActivated) {
            FindObjectOfType<NoteView>().Show(text);
        }
        else {
            FindObjectOfType<NoteView>().Hide();
        }
    }
}
