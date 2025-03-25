using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hunter
{
    public class PlayerUIRotate : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        float yOffset;
        float startX;

        public void OnBeginDrag(PointerEventData eventData)
        {
            yOffset = PlayerInformation.instance.playerModel.transform.localEulerAngles.y;
            startX = Input.mousePosition.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float y = (yOffset + (startX - Input.mousePosition.x) / 5);
            PlayerInformation.instance.playerModel.transform.localRotation = Quaternion.Euler(0f, y, 0f);
        }
    }
}
