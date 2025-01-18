using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSmoothDamp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public MoveableBase moveable;
    public bool isDragging; 

    Vector2 origin;
    Transform parentAfterDrag;

    void Start()
    {
        isDragging = false;
        moveable.targetPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 cursorPosition = GetMousePosition();
        cursorPosition.z = transform.position.z; 

        moveable.targetPosition = cursorPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        
        var interactiveObject = GetComponent<InteractiveObject>();
        if (interactiveObject && interactiveObject.zone && !interactiveObject.zone.canDrag)
            return;
        interactiveObject.OnPointerExit(null);
        G.main.StartDrag(this);
        
        isDragging = true;
        
        origin = moveable.targetPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        parentAfterDrag = null;
        
        G.main.StopDrag();
        
        isDragging = false; 
        moveable.targetPosition = origin;
    }
    Vector3 GetMousePosition()
    {
        Vector3 screenMousePos = Input.mousePosition;
        return screenMousePos;
        //Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(screenMousePos);
        //Vector2 worldMousePos2D = new Vector2(worldMousePos.x, worldMousePos.y);

        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenMousePos,
            null, // Use null for overlay canvas, or your canvas camera for camera space
            out Vector2 canvasMousePos
        );

        return new Vector3(canvasMousePos.x, canvasMousePos.y, transform.position.z);*/
    }
}