using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPage : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _equiptmentPanel;
    public bool _inventoryPanelIsOpen = true;
    [SerializeField]
        private SlotController itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        List<SlotController> listOfUIItems = new List<SlotController>();
        [SerializeField] SlotController[] equipmentItems = new SlotController[1];

        public event Action ShowEquiptmentData;

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> 
                OnItemActionRequested,
                OnStartDragging,
                OnUnequipFromEquipment;

        public event Action<int,bool> OnDescriptionRequested;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        //private ItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }
        private void Start() {
            for(int i = 0; i<1;i++){
                equipmentItems[i].OnItemClicked += HandleItemSelection;
                // equipmentItems[i].OnItemBeginDrag += HandleBeginDrag;
                // equipmentItems[i].OnItemDroppedOn += HandleSwap;
                // equipmentItems[i].OnItemEndDrag += HandleEndDrag;
                equipmentItems[i].OnRightMouseBtnClick += HandleUnequipFromEquipment;
            }
        }

        public void InitializeInventoryUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                SlotController uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                uiItem.transform.localScale = new Vector3(1.124722f,1.29915f,0.3f);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            if(_inventoryPanelIsOpen){
                listOfUIItems[itemIndex].Select();
            }else{
                equipmentItems[itemIndex].Select();
            }
            
        }

        public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        public void UpdateEquipmentData(int itemIndex,
            Sprite itemImage, int itemQuantity)
        {
            if (equipmentItems.Length > itemIndex)
            {
                Debug.Log("is not  greater than lenaght");
                equipmentItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(SlotController inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(SlotController inventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(SlotController inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            
            if ( currentlyDraggedItemIndex == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(SlotController inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
            
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(SlotController inventoryItemUI)
        {
            // int index = listOfUIItems.IndexOf(inventoryItemUI);
            // if (index == -1){
            //     return;
            // }
            // OnDescriptionRequested?.Invoke(index);
            int index;
            if(_inventoryPanelIsOpen){
                Debug.Log("is open");
                index = listOfUIItems.IndexOf(inventoryItemUI);
                
            }else{
                Debug.Log("is closed");
                index = Array.IndexOf(equipmentItems,inventoryItemUI);
                
            }
            OnDescriptionRequested?.Invoke(index,_inventoryPanelIsOpen);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        // public void AddAction(string actionName, Action performAction)
        // {
        //     actionPanel.AddButon(actionName, performAction);
        // }

        // public void ShowItemAction(int itemIndex)
        // {
        //     actionPanel.Toggle(true);
        //     actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        // }

        private void DeselectAllItems()
        {
            foreach (SlotController item in listOfUIItems)
            {
                item.Deselect();
            }
            foreach(SlotController item in equipmentItems){
                item.Deselect();
            }
            //actionPanel.Toggle(false);
        }

        public void Hide()
        {
            //actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }
        private void HandleUnequipFromEquipment(SlotController inventoryItemUI){
            int index = Array.IndexOf(equipmentItems,inventoryItemUI);
            if(index == -1){
                return;
            }
            OnUnequipFromEquipment?.Invoke(index);
        }
        private void Update() {
            if(Input.GetKeyDown(KeyCode.Tab)){
                _inventoryPanelIsOpen = !_inventoryPanelIsOpen;
                _inventoryPanel.SetActive(_inventoryPanelIsOpen);
                _equiptmentPanel.SetActive(!_inventoryPanelIsOpen);
                ShowEquiptmentData?.Invoke();
                ResetSelection();
            }
            
        }
}
