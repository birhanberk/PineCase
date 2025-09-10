using UnityEngine;

namespace Utils
{
    public class OrthographicCameraScaler : MonoBehaviour
    {
        public float referenceWidth = 1080f;
        public float referenceHeight = 1920f;
        public float designOrthographicSize = 5f;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            var targetAspect = referenceWidth / referenceHeight;
            var currentAspect = (float)Screen.width / Screen.height;

            _camera.orthographicSize = designOrthographicSize * (targetAspect / currentAspect);
        }
    }
}
