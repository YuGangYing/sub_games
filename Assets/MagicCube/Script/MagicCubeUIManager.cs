using UnityEngine;
using UnityEngine.UI;

namespace BlueNova.SubGames.MagicCube.UI
{
    public class MagicCubeUIManager : MonoBehaviour
    {
        MagicCubeRotate mMagicCubeRotate;

        public Button btnUndo;

        public Button btnBack;

        public Button btnSave;

        public Text txtInformation;

        public DialogCommon dialogCommon;

        void Awake()
        {
            mMagicCubeRotate = FindObjectOfType<MagicCubeRotate>();

            btnUndo.onClick.AddListener(mMagicCubeRotate.Undo);

            btnSave.onClick.AddListener(mMagicCubeRotate.Save);

            btnBack.onClick.AddListener(mMagicCubeRotate.Back);

            mMagicCubeRotate.onStart = () => { 
                btnUndo.gameObject.SetActive(true);
                btnSave.gameObject.SetActive(true);
                btnBack.gameObject.SetActive(true); 
            };

            mMagicCubeRotate.onSuccess = () =>
            {
                dialogCommon.gameObject.SetActive(true);
                dialogCommon.onOk = mMagicCubeRotate.Back;
                dialogCommon.txtInformation.text = "Success";
            };
        }
    }
}