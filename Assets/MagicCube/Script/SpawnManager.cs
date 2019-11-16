using System.Collections.Generic;
using UnityEngine;

namespace BlueNova.SubGames.MagicCube
{
    public class SpawnManager
    {

        public GameObject[] InitCubes(MagicCubeRotate magicCubeRotate, int count)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/cube");
            List<GameObject> cubes = new List<GameObject>();
            for (int x = 0;x < count;x++)
            {
                for (int y = 0; y < count; y++)
                {
                    for (int z = 0; z < count; z++)
                    {
                        if (x == 0 || x == count - 1 || y == 0 || y == count - 1 || z == 0 || z == count -1)
                        {
                            GameObject go = GameObject.Instantiate<GameObject>(prefab);
                            go.transform.SetParent(magicCubeRotate.MagicCube.transform);
                            go.transform.localPosition = new Vector3(-count / 2f + 0.5f, -count / 2f + 0.5f, -count / 2f + 0.5f) + new Vector3(x ,y,z);
                            go.SetActive(true);
                            cubes.Add(go);
                        }
                    }
                }
            }
            return cubes.ToArray();
        }
    }
}
