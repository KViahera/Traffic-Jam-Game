using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Workshop
{
    public class ClickReceiver : MonoBehaviour, IPointerClickHandler
    {
        private static GameObject selectedGameObject;

        public static GameObject SelectedGameObject
        {
            get
            {
                return selectedGameObject;
            }

            set
            {
                selectedGameObject = value;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            selectedGameObject = eventData.rawPointerPress;
        }
    }
}
