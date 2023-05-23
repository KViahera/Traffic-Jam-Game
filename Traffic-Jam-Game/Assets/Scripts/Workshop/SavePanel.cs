using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop
{
    public class SavePanel : MonoBehaviour
    {
        public GameObject savePanel;
        public void Accept()
        {
            savePanel.SetActive(false);
        }
    }
}