using UnityEngine;
using UnityEngine.EventSystems;

public class UIMap : MonoBehaviour, IDragHandler, IBeginDragHandler
{
	[SerializeField]
	private RectTransform _mapRectTransform;

    private Vector2 _lastMousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastMousePosition = eventData.position;
    }

    // How to keep the map within bounds? Is there a better way than comparing x and y values?
    // How to make it work with different resolutions? 
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = eventData.position;
        Vector3 direction = mousePosition - _lastMousePosition;

//        Debug.Log($"Mouse position: {mousePosition}, Last mouse position: {_lastMousePosition}, Direction vector: {direction}, Map position: {_mapRectTransform.position}");

        _mapRectTransform.position += direction;

        _lastMousePosition = mousePosition;
    }
}