using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNova.SubGames.MagicCube.UI
{
    public class MagicCubeUIManager : MonoBehaviour
    {
        MagicCubeController mMagicCubeController;

        public Button btnUndo;

        public Button btnMenu;

        public Button btnSave;

        public Text txtInformation;

        public DialogConfirm dialogConfirm;

        public DialogMenu dialogMenu;

        public DialogFinish dialogFinish;

        public AudioClip clickClip;

        void Awake()
        {
            mMagicCubeController = FindObjectOfType<MagicCubeController>();

            btnUndo.onClick.AddListener(mMagicCubeController.Undo);

            btnSave.onClick.AddListener(mMagicCubeController.Save);

            btnUndo.gameObject.SetActive(false);
            btnSave.gameObject.SetActive(false);
            btnMenu.gameObject.SetActive(false);
            txtInformation.gameObject.SetActive(false);

            mMagicCubeController.onStart = () => { 
                btnUndo.gameObject.SetActive(true);
                //btnSave.gameObject.SetActive(true);
                btnMenu.gameObject.SetActive(true);
                txtInformation.gameObject.SetActive(true);
            };

            mMagicCubeController.onSuccess = () =>
            {
                btnUndo.enabled = false;
                btnUndo.GetComponent<Image>().color = Color.gray;
                dialogFinish.txtInformation.text = "Congratulation!";
            };

            mMagicCubeController.onFailure = () =>
            {
                btnUndo.enabled = false;
                btnUndo.GetComponent<Image>().color = Color.gray;
                dialogFinish.txtInformation.text = "You lost.";
            };

            mMagicCubeController.onAnimationDone = () =>
            {
                dialogFinish.root.SetActive(true);
            };

            dialogFinish.btnBack.onClick.AddListener(() => { ShowConfirm(mMagicCubeController.Back); });

            dialogFinish.btnRestart.onClick.AddListener(() => { ShowConfirm(mMagicCubeController.Restart); });

            dialogFinish.btnClose.onClick.AddListener(() => { dialogFinish.root.SetActive(false);  });

            dialogFinish.btnCancel.onClick.AddListener(() => { dialogFinish.root.SetActive(false); });

            btnMenu.onClick.AddListener(()=> { dialogMenu.root.SetActive(true); mMagicCubeController.isPause = true; });

            dialogMenu.btnBack.onClick.AddListener(() => { ShowConfirm(mMagicCubeController.Back); });

            dialogMenu.btnRestart.onClick.AddListener(() => { ShowConfirm(mMagicCubeController.Restart); } );

            dialogMenu.btnClose.onClick.AddListener(() => { dialogMenu.root.SetActive(false); mMagicCubeController.isPause = false; });

            dialogMenu.btnCancel.onClick.AddListener(() => { dialogMenu.root.SetActive(false); mMagicCubeController.isPause = false; });

            dialogMenu.btnSwitch.onClick.AddListener(() => { txtInformation.gameObject.SetActive(!txtInformation.gameObject.activeSelf); });

            dialogConfirm.btnNo.onClick.AddListener(() => { dialogConfirm.root.SetActive(false); });

            dialogConfirm.btnOk.onClick.AddListener(() => { if (dialogConfirm.onConfirm != null) { dialogConfirm.onConfirm(); } dialogConfirm.root.SetActive(false); });

            Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(() => { AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position + Camera.main.transform.forward); });
            }
        }

        private void Update()
        {
            DateTime dtfommls = DateTime.MinValue.AddMilliseconds(Mathf.CeilToInt(mMagicCubeController.timeRemain) * 1000);
            txtInformation.text = string.Format("Time:{0:HH:mm:ss}", dtfommls);
        }

        void ShowConfirm(UnityAction onConfirm)
        {
            dialogConfirm.root.SetActive(true);
            dialogConfirm.onConfirm = onConfirm;
        }
    }
}