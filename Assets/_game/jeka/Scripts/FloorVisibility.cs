using UnityEngine;

public class FloorVisibility : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // По умолчанию невидимый
        }
    }

    public void ShowMesh()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
    }
}