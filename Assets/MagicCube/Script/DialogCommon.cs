using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNova.SubGames.MagicCube.UI
{
    public class DialogCommon : MonoBehaviour
    {
        public Text txtInformation;

        public Button btnOk;

        public UnityAction onOk;

        private void Awake()
        {
            btnOk.onClick.AddListener(()=> {
                if (onOk!=null)
                {
                    onOk();
                }
            });
        }
    }
}