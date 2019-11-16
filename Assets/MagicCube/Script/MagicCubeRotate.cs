
/// <summary>
/// ***  wangyel1  ***
/// 魔方旋转使用向量的点乘和叉乘来获取和控制。
/// 首先在鼠标点击魔方时，获取点击点的信息，包括点击的点和此处的法线；
/// 之后在鼠标拖拽后获取鼠标运行的方向，它叉乘法线得到垂直于二者的一条法线；
/// 之后用得到的法线点乘六个原始轴，取值为45度以内的轴为转动轴，则能得到所要转动的层和转动方向；
/// 最后这一层开始转动，之后进入下一轮
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace BlueNova.SubGames.MagicCube
{
    public class MagicCubeRotate : MonoBehaviour
    {

        public GameObject MagicCube;
        public float rotateSpeed = 2f;
        private GameObject cubeObject; //被点中的cube//
        private Vector3 normal; //点击点的法线//
        private Vector3 mouseStart; //鼠标点击的初始点//
        private Vector3 mouseRun; //鼠标拖动时的点//
        private Vector3 mouseCross; //叉乘结果//
        //private Vector3 rotAxis; //魔方某层转动时的转动轴//
        private bool isRotate = false; //某层是否可转动//
        private bool isRun = false; //是否可拖动鼠标//
                                    //private float t = 0; //转动时的递增值的临时变量//
        private Vector3[] vecPoint; //六个原始轴//
        private GameObject[] cubes; //保存所有标签为Cube物体的父物体//

        int randomCount = 0;
        public static int size = 5;
        public static bool isLoad = false;
        SpawnManager spawnManager;
        CameraRotate cameraRotate;
        public UnityAction onStart;
        public UnityAction onSuccess;

        IEnumerator Start()
        {
            vecPoint = new Vector3[] { Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back };
            spawnManager = new SpawnManager();
            if (isLoad) {
                SaveData saveData = Load();
                size = saveData.size;
                cameraRotate = FindObjectOfType<CameraRotate>();
                cameraRotate.SetCameraDistance(-size);
                cubes = spawnManager.InitCubes(this, saveData.size);
                yield return LoadRecords(saveData.records);
                isRun = false;
            }
            else
            {
                cubes = spawnManager.InitCubes(this, size);
                cameraRotate = FindObjectOfType<CameraRotate>();
                cameraRotate.SetCameraDistance(-size);
                yield return new WaitForSeconds(0.1f);
                yield return RandomCubes();
                isRun = false;
            }
            yield return new WaitForSeconds(1);
            isRun = true;
            if (onStart != null)
                onStart();
        }

        IEnumerator RandomCubes()
        {
            int count = randomCount * size;
            while (count > 0)
            {
                if (!isRotate)
                {
                    RandomRotate();
                    count--;
                }
                yield return new WaitForSeconds(0.01f);
            }
            isRotate = false;
            isRun = true;
        }

        IEnumerator LoadRecords(Record[] records)
        {
            int count = 0;
            while (records.Length > count)
            {
                if (!isRotate) {
                    RotateAxis(100, records[count].axisIndex, records[count].point, records[count].cubeObjecePoint,null);
                    count++;
                }
                yield return null;
            }
            isRotate = false;
            isRun = true;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isRun)
            { //按下鼠标记录相关内容//
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitStart;
                if (Physics.Raycast(ray.origin, ray.direction, out hitStart))
                {
                    cubeObject = hitStart.transform.gameObject;
                    normal = hitStart.normal;
                    mouseStart = hitStart.point;
                }
            }

            if (Input.GetMouseButton(0) && cubeObject != null)
            { //拖动鼠标，记录拖动方向，叉乘得到法线方向//
                Ray rayRun = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitRun;
                if (Physics.Raycast(rayRun.origin, rayRun.direction, out hitRun))
                {
                    mouseRun = hitRun.point - mouseStart;
                    if (mouseRun.sqrMagnitude > 0.2f)
                    {
                        mouseCross = Vector3.Cross(normal, mouseRun).normalized;
                        //get axis
                        for (int i = 0; i < 6; i++)
                        {
                            Vector3 axis = vecPoint[i];
                            float dot = Vector3.Dot(axis, mouseCross);
                            //Bewteen degree 0 to degree 45.
                            if (dot > 0.73f && dot <= 1)
                            {
                                for (int point = 0; point < 3; point++)
                                {
                                    if (Mathf.Abs(axis[point]) == 1f)
                                    {
                                        //get the layer.
                                        float cubeObjecePoint = cubeObject.transform.position[point];
                                        RotateAxis(2, i, point, cubeObjecePoint, () => {
                                            if (IsSuccess())
                                            {
                                                isRun = false;
                                                isRotate = false;
                                                if (onSuccess != null)
                                                    onSuccess();

                                            }
                                        });
                                    }
                                }
                                break;
                            }
                        }
                        cubeObject = null;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Undo();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RandomRotate();
            }
        }

        IEnumerator StartRotate(float speed,Vector3 rotAxis,UnityAction onRotateDone)
        {
            float t = 0;
            Vector3 endRatote = rotAxis * 90;
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                Vector3 rotate = rotAxis * Mathf.Clamp01(t) * 90;
                transform.eulerAngles = rotate;
                yield return null;
            }
            transform.eulerAngles = endRatote;
            RotateDone();
            if (onRotateDone != null)
                onRotateDone();
        }

        void RotateDone()
        {
            foreach (GameObject cube in cubes)
            {
                cube.transform.parent = MagicCube.transform;
                cube.transform.localScale = Vector3.one;
            }
            transform.rotation = Quaternion.identity;
            isRotate = false;
            isRun = true;
        }

        //Rotate layers.
        void RotateAxis(float ratateSpeed ,int axisIndex, int point ,float cubeObjecePoint,UnityAction onRotateDone , bool isBack = false)
        {
            if (isRotate)
                return;
            Vector3 axis = vecPoint[axisIndex];
            if (isBack)
                axis = -axis;
            isRotate = true;
            isRun = false;
            for (int i = 0; i < cubes.Length; i++)
            {
                float cubePoint = cubes[i].transform.position[point];
                if (Mathf.Abs(cubePoint - cubeObjecePoint) < 0.1f)
                {
                    cubes[i].transform.parent = gameObject.transform;
                }
            }
            if (!isBack)
                records.Add(new Record(axisIndex, point, cubeObjecePoint));
            StartCoroutine(StartRotate(ratateSpeed,axis, onRotateDone));
               
        }

        const string SaveDataKey = "SaveData";
        public void Save()
        {
            SaveData saveData = new SaveData();
            saveData.records = records.ToArray();
            saveData.size = size;
            PlayerPrefs.SetString(SaveDataKey,JsonUtility.ToJson(saveData));
            PlayerPrefs.Save();
        }

        public bool IsSuccess() {
            Vector3 rat = cubes[0].transform.eulerAngles;
            for (int i = 0; i < cubes.Length; i++)
            {
                if(rat!= cubes[i].transform.eulerAngles)
                {
                    return false;
                }
            }
            return true;
        }

        SaveData Load()
        {
            return JsonUtility.FromJson<SaveData> (PlayerPrefs.GetString(SaveDataKey));
        }

        public void Back()
        {
            SceneManager.LoadScene("Start");
        }

        List<Record> records = new List<Record>();

        public void Undo()
        {
            if (IsBackable())
            {
                Record record = records[records.Count - 1];
                RotateAxis(2, record.axisIndex, record.point, record.cubeObjecePoint, null, true);
                records.RemoveAt(records.Count - 1);
            }
        }

        public bool IsBackable()
        {
            return (!isRotate && records.Count > Mathf.Max(0, randomCount * size));
        }

        void RandomRotate()
        {
            if (!isRotate)
            {
                int axisIndex = Random.Range(0, vecPoint.Length);
                Vector3 axis = vecPoint[axisIndex];
                GameObject cube = cubes[Random.Range(0, cubes.Length)];
                for (int point = 0; point < 3; point++)
                {
                    if (Mathf.Abs(axis[point]) == 1f)
                    {
                        float cubeObjecePoint = cube.transform.position[point];
                        RotateAxis(10, axisIndex, point, cubeObjecePoint, null, false);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Record
    {
        public int axisIndex;
        public int point;
        public float cubeObjecePoint;
        public Record(int axisIndex, int point, float cubeObjecePoint)
        {
            this.axisIndex = axisIndex;
            this.point = point;
            this.cubeObjecePoint = cubeObjecePoint;
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int size = 4;
        public float remainTime;
        public Record[] records;
    }
}