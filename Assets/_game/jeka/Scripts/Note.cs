using _game.Note;
using _game.rnk.Scripts.crawler;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string text;
    private Collider leverCollider;
    private bool isActivated = false;
    public Player player;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        leverCollider = GetComponent<Collider>();
    }

    public void ActivateNote() {
        isActivated = !isActivated;
        if (isActivated) {
            FindObjectOfType<NoteView>().Show(text);
            player.DisableControls();
        }
        else {
            FindObjectOfType<NoteView>().Hide();
            player.EnableControls();
        }
    }
}
