using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class SlotController : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text quantityTxt;

    [SerializeField]
    private Image borderImage;

    public event Action<SlotController> OnItemClicked,
        OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
        OnRightMouseBtnClick;

    public bool Empty {get;private set;}

    public void Awake()
    {
        ResetData();
        Deselect();
    }
    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        Empty = true;
    }
    public void Deselect()
    {
        borderImage.enabled = false;
    }
    public void SetData(Sprite sprite, int quantity)
    {
        Debug.Log("2" + gameObject.name);
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quantityTxt.text = quantity + "";
        Empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         Debug.Log("hello from on end drag");
        OnItemEndDrag?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Helllo");
        if (Empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("hello from on drop");
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
