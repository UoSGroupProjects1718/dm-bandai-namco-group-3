using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler
{
    public bool Draggable;
    
    [SerializeField] private Collider2D _physicalCollider;
    [SerializeField] private LayerMask _outOfBoundsLayerMask;

    public void OnDrag(PointerEventData eventData)
    {
        // If the object is not flagged as draggable then do nothing.
        if (!Draggable) return;
        
        // Obtain the position of the touch in world coordinates and calculate the object's potential next position. 
        var touchPostion = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        var nextPosition = new Vector3(touchPostion.x, touchPostion.y, transform.position.z);

        // Calculate the distance and direction of the desired movement.
        var heading = nextPosition - transform.position;
        var distance = _physicalCollider.bounds.extents.y + heading.magnitude;
        var direction = heading / distance;

        // Cast a ray in the movement direction to detect if it is out of bounds. If it isn't, move the object.
        Debug.DrawRay(transform.position, transform.position + (direction * distance), Color.red);
        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, distance, _outOfBoundsLayerMask);
        if (!raycastHit2D)
        {
            transform.position = nextPosition;
        }
        else
        {
            Debug.LogFormat("Ray Collided With: \"{0}\".", raycastHit2D.collider.gameObject.name);
        }
    }
}
