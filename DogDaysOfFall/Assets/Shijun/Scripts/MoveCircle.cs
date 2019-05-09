using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCircle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 startPosition;
    public Vector3 backPosition;
    public Vector3 positionone;

    public RectTransform originalRectTransform;
    //public RectTransform rt;
    //public Transform startParent;

    public static GameObject itemBeingDragged;
    private Vector3 aPosition;

    private Vector3 initial;
    


    public void Awake()
    {
        positionone = transform.position;

        originalRectTransform = this.gameObject.GetComponent<RectTransform>();
        aPosition = originalRectTransform.anchoredPosition;

    }

    public void ResetTransform()
    {
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = aPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initial = this.transform.position;
        itemBeingDragged = gameObject;
        backPosition = transform.position;
        startPosition = transform.position;
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {

        //Vector3 currentPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initial.z);
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(currentPosition);
        this.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initial.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        itemBeingDragged = null;

        transform.position = startPosition;
        //GetComponent<CanvasGroup>().blocksRaycasts = true;
        
    }


    public void MoveBack()
    {

        //transform.SetParent(startParent);
        transform.position = positionone;
        //GetComponent<CanvasGroup>().blocksRaycasts = true;

    }


}

