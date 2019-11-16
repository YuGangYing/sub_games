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

        public Button btnLoad;

        private void Awake()
        {
            btnSize2.onClick.AddListener(() => { StartMagicCube(2); });
            btnSize3.onClick.AddListener(() => { StartMagicCube(3); });
            btnSize4.onClick.AddListener(() => { StartMagicCube(4); });
            btnSize5.onClick.AddListener(() => { StartMagicCube(5); });
            btnSize6.onClick.AddListener(() => { StartMagicCube(6); });
            btnLoad.onClick.AddListener(() => { StartMagicCube(0,true); });
        }

        void StartMagicCube(int size,bool isLoad =false)
        {
            MagicCubeRotate.size = size;
            MagicCubeRotate.isLoad = isLoad;
            SceneManager.LoadScene("MagicCube");
        }
    }
}
