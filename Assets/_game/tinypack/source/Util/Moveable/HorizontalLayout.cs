using System.Collections.Generic;
using UnityEngine;

public class HorizontalLayout : MonoBehaviour
{
    [SerializeField] float spacing = 10f;
    [SerializeField] float padding = 10f;
    [SerializeField] bool autoResize = true;
    [SerializeField] bool centerChildren;

    List<RectTransform> children = new List<RectTransform>();
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CollectChildren();
    }

    void OnEnable()
    {
        UpdateLayout();
    }

    void CollectChildren()
    {
        children.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            if (child != null && child.gameObject.activeSelf)
            {
                children.Add(child);
            }
        }
    }
    
    public void UpdateLayout()
    {
        CollectChildren();
        
        float currentX = padding;
        float maxHeight = 0f;
        float totalWidth = padding;
        
        // Calculate total width and max height
        foreach (RectTransform child in children)
        {
            totalWidth += child.rect.width;
            if (child != children[children.Count - 1])
            {
                totalWidth += spacing;
            }
            maxHeight = Mathf.Max(maxHeight, child.rect.height);
        }
        totalWidth += padding;
        
        // Auto resize container if needed
        if (autoResize)
        {
            rectTransform.sizeDelta = new Vector2(totalWidth, maxHeight + (padding * 2));
        }
        
        // Calculate starting X position for centering
        if (centerChildren && children.Count > 0)
        {
            float availableSpace = rectTransform.rect.width - totalWidth;
            currentX += availableSpace / 2;
        }
        
        // Position children
        foreach (RectTransform child in children)
        {
            float yPos = (rectTransform.rect.height - child.rect.height) / 2;
            Vector2 position = new Vector2(currentX + (child.rect.width / 2), yPos);
            child.localPosition = position;
            
            currentX += child.rect.width + spacing;
        }
    }
    
    // Call this when children are added or removed at runtime
    public void RefreshLayout()
    {
        UpdateLayout();
    }
    
    // Editor-only: Update in edit mode for preview
    #if UNITY_EDITOR
    void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            rectTransform = GetComponent<RectTransform>();
            UpdateLayout();
        }
    }
    #endif
}