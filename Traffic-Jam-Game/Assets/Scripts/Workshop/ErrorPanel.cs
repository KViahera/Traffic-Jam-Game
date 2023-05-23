using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop
{
    public class ErrorPanel : MonoBehaviour
    {
        public GameObject errorPanel;
        public void Accept()
        {
            errorPanel.SetActive(false);
        }
    }
}