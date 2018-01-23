using UnityEngine.EventSystems;

namespace Scenes.Main.Scripts
{
	public class DraggableOnce : Draggable, IEndDragHandler
	{
		public void OnEndDrag(PointerEventData eventData)
		{
			CanBeDragged = false;
		}
	}
}
