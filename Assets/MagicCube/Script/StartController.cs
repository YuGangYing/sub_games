using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BlueNova.SubGames.MagicCube
{
    public class StartController : MonoBehaviour
    {
        public Button btnSize2;
        public Button btnSize3;
        public Button btnSize4;
        public Button btnSize5;
        public Button btnSize6;
        public GameObject sizeRoot;

        public Button btnLoad;
        public Button btnNew;
        public AudioClip clickClip;

        private void Awake()
        {
            btnSize2.onClick.AddListener(() => { StartMagicCube(2); });
            btnSize3.onClick.AddListener(() => { StartMagicCube(3); });
            btnSize4.onClick.AddListener(() => { StartMagicCube(4); });
            btnSize5.onClick.AddListener(() => { StartMagicCube(5); });
            btnSize6.onClick.AddListener(() => { StartMagicCube(6); });
            if (PlayerPrefs.HasKey(MagicCubeController.SaveDataKey))
            {
                btnLoad.onClick.AddListener(() => { StartMagicCube(0, true); });
            }
            else
            {
                btnLoad.GetComponent<Image>().color = Color.gray;
                btnLoad.GetComponentInChildren<Text>().color = Color.gray;
                btnLoad.enabled = false;
            }
            btnNew.onClick.AddListener(() => { btnNew.gameObject.SetActive(false); btnLoad.gameObject.SetActive(false); sizeRoot.SetActive(true); });
            Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
            for (int i=0;i<buttons.Length;i++)
            {
                buttons[i].onClick.AddListener(()=> { AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position + Camera.main.transform.forward); });
            }
        }

        void StartMagicCube(int size,bool isLoad =false)
        {
            MagicCubeController.size = size;
            MagicCubeController.isLoad = isLoad;
            SceneManager.LoadScene("MagicCube");
        }
    }
}
