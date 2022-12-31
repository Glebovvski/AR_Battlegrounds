using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInput
{
    public class Dragger : MonoBehaviour
    {
        [field: SerializeField] private LayerMask LayerMask { get; set; }
        [SerializeField] private Material freeMat;
        [SerializeField] private Material holdMat;
        private Transform parent { get; set; }

        private Vector3 parentOffset;

        private Vector3 screenPoint;
        private Vector3 offset;

        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

        void OnMouseDrag()
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            parent.position = new Vector3(cursorPosition.x - parentOffset.x, parent.position.y, cursorPosition.z - parentOffset.z);
        }

        private void Start()
        {
            parent = this.transform.parent;
            parentOffset = this.transform.position - parent.position;
        }

        void Update()
        {
            if (Input.touchCount == 0) return;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)// && !IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, float.MaxValue, LayerMask))
                {
                    parent.position = rayHit.point;
                }
            }
        }

        private bool IsPointerOverGameObject()
        {
            PointerEventData eventDataCurrentPos = new PointerEventData(EventSystem.current);
            eventDataCurrentPos.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPos, results);
            return results.Count > 0;
        }
    }
}