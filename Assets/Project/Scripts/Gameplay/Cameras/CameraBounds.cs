using UnityEngine;

namespace BC.Gameplay.Cameras
{
    public class CameraBounds : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D leftWall = null!;
        [SerializeField] private BoxCollider2D rightWall = null!;
        [SerializeField] private BoxCollider2D topWall = null!;
        [SerializeField] private BoxCollider2D bottomWall = null!;

        private void Start()
        {
            InitializeCameraBounds();
        }

        private void InitializeCameraBounds()
        {
            var mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }

            var height = mainCamera.orthographicSize * 2f;
            var width = height * mainCamera.aspect;

            SetWallSizes(height, width);
            SetWallPositions(width, height);
        }

        private void SetWallSizes(float height, float width)
        {
            leftWall.size = new Vector2(1, height);
            rightWall.size = new Vector2(1, height);
            topWall.size = new Vector2(width, 1);
            bottomWall.size = new Vector2(width, 1);
        }

        private void SetWallPositions(float width, float height)
        {
            leftWall.transform.localPosition = new Vector3(-width / 2, 0, 0);
            rightWall.transform.localPosition = new Vector3(width / 2, 0, 0);
            topWall.transform.localPosition = new Vector3(0, height / 2, 0);
            bottomWall.transform.localPosition = new Vector3(0, -height / 2, 0);
        }
    }
}