using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNova.SubGames.MagicCube.UI
{
    [System.Serializable]
    public class DialogConfirm
    {
        public GameObject root;
        public Button btnOk;
        public Button btnNo;
        public UnityAction onConfirm;


    }
}
