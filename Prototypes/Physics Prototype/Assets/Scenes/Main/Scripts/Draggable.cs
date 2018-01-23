using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.Main.Scripts
{
    public class Draggable : MonoBehaviour, IDragHandler
    {
        [SerializeField] protected bool CanBeDragged;
        
        protected Transform Transform;
        protected float InitialZPosition;
        
        private void Start()
        {
            Transform = GetComponent<Transform>();
            InitialZPosition = 0.0f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanBeDragged) return;
            Transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Transform.localPosition = new Vector3(Transform.localPosition.x, Transform.localPosition.y,
                InitialZPosition);
        }

        public bool IsDraggable()
        {
            return CanBeDragged;
        }
    }
}