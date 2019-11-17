using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace BlueNova.SubGames.MagicCube
{
    public class CameraRotate : MonoBehaviour
    {

        public Transform cameraTarget;
        public float horizontalSpeed = 2.0f;
        public float verticalSpeed = 2.0f; 
        public float scrollSpeed = 2.0f; 
        public float lerpSpeed = 5.0f; 
        private float h = 0;
        private float v = 0;
        private float caremaDistance;

        void Start()
        {
            Vector3 localRotation = transform.eulerAngles;
            v = localRotation.x;
            h = localRotation.y;
        }

        bool isRotate;
        bool isPause;

        void Update()
        {
            if (isPause)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitStart;
                isRotate = !Physics.Raycast(ray.origin, ray.direction, out hitStart);
            }
            if (Input.GetMouseButton(0) && isRotate)
            {
                h += horizontalSpeed * Input.GetAxis("Mouse X");
                v -= verticalSpeed * Input.GetAxis("Mouse Y");
                v = Mathf.Clamp(v, -50, 50);
            }
            Quaternion rotationTo = Quaternion.Euler(v, h, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTo, Time.deltaTime * lerpSpeed);

            caremaDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            SetCameraDistance(caremaDistance);
        }
        int minDistance = -4;
        int maxDistance = 1;
        public void SetCameraDistance(float distance)
        {
            caremaDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition, new Vector3(0, 0, caremaDistance - 3), Time.deltaTime * lerpSpeed);
        }

        public void Ramble(UnityAction onComplete)
        {
            minDistance = -5;
            SetCameraDistance(-5);
            StartCoroutine(_Ramble(onComplete));
        }

        IEnumerator _Ramble(UnityAction onComplete)
        {
            float t = 0;
            isPause = true;
            while (t < 6)
            {
                t += Time.deltaTime;
                Quaternion rotationTo = Quaternion.Euler(50, t * 60 , 0);
                transform.rotation = rotationTo;
                yield return null;
            }
            if (onComplete != null)
                onComplete();
        }

    }
}
