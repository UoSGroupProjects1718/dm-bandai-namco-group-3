using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler
{
    [SerializeField] private bool _draggable;
    
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggable)
        {
            var currentZ = _transform.position.z;
            _transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            _transform.localPosition = new Vector3(_transform.localPosition.x,
                _transform.localPosition.y, currentZ);
        }
    }

    public void setDraggable(bool draggable)
    {
        _draggable = draggable;
    }

    public bool isDraggable()
    {
        return _draggable;
    }
}
