using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToolKid.InventorySystem {
    public class SlotBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {
        [SerializeField]
        private Slot props;
        public Text nameText;
        public Image slotImage;
        public Image itemImage;
        public Text stackCount;
        [SerializeField]
        private float stopWatch;
        [SerializeField]
        private bool isDescribing = false;
        private bool isHovering = false;
        private bool isPointerDown = false;

        public float StopWatch { get => stopWatch; set => stopWatch = value; }

        [SerializeField]
        private float describeHoverTime = 0.5f;

        private Image dragging;
        

        public event EventHandler<Slot> Describe;
        public event EventHandler<Slot> Undescribe;
        public UnityEvent onDescribe;
        public UnityEvent onUndescribe;

        void Awake() {
            ModifyTo(props);
        }

        public void ModifyTo(Slot slot) {
            props.SetPropsFrom(slot);
            nameText.text = props.Item.Name;
            stackCount.text = props.StackCount.ToString();
            itemImage.sprite = props.Item.Sprite;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Timer.CentiSecond += Counterdown;
            isHovering = true;
        }

        private void Counterdown(object sender, Watch e) {
            if (isHovering) {
                OnHover();
            }            
        }

        private void OnHover() {
            stopWatch += 0.01f;
            if (stopWatch >= describeHoverTime && !isDescribing) {
                Describe?.Invoke(this, props);
                onDescribe.Invoke();                
                isDescribing = true;
                Debug.Log("Describe", this);                
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHovering = false;
            stopWatch = 0;
            if (isDescribing) {
                Undescribe?.Invoke(this, props);
                onUndescribe.Invoke();
                Debug.Log("Undescribe", this);
            }
        }

        private void Clear() {                     
            Debug.Log("Clear", this);
        }

        public void OnDrop(PointerEventData eventData) {
            Debug.Log("Drop", this);            
            InventoryBase dragFrom = eventData.pointerDrag.GetComponentInParent<InventoryBase>();
            SlotBase dragSlot = eventData.pointerDrag.GetComponent<SlotBase>();
            SlotBase dropSlot = this;
            dragSlot.itemImage.transform.localPosition = Vector3.zero;            
            if (dropSlot != dragSlot) {                
                if (dropSlot.props.Item != null) {
                    Slot temp = new Slot(dragSlot.props);
                    dragSlot.ModifyTo(dropSlot.props);                    
                    dropSlot.ModifyTo(temp);
                    Debug.Log("Replace", this);
                }
                else {
                    dropSlot.ModifyTo(dragSlot.props);                    
                    dragSlot.Clear();
                }
            }
        }

        public void OnDrag(PointerEventData eventData) {            
            dragging.transform.position = eventData.position;
            Debug.Log("Drag", this);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            dragging = Instantiate(itemImage, transform.parent);
            dragging.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData) {
            Destroy(dragging.gameObject);            
        }
    }
}
