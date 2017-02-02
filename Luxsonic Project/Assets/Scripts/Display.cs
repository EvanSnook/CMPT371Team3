using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class to manage display objects
/// </summary>
public class Display : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Vector3 screenPoint;
    Vector3 offset;

    /// <summary>
    /// When the mouse pointer is clicked on a display object, this is used to determine the real world
    /// point where the object was selected.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Convert the world point of the position of the display to a screen point
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); 
        // Caclulate the offset from the clicked point to the screen point  
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    /// <summary>
    /// When the object is being dragged by the mouse, update its position.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // Current screen point is where the mouse is on the screen
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        // Current position is the the world point of the mouse cursor
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        // Set the object's position to the world point of the mouse
        transform.position = curPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        //transform.position = startPosition;
    }
}
