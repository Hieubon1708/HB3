using System;
using UnityEngine;

namespace Hunter
{
    public class BotWeaponRadiusRPG : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [Range(0.1f, 5f), SerializeField]
        private float lineThickness = 1f;
        [Range(6, 28), SerializeField]
        private int dottedCount = 8;
        [SerializeField] private uint count = 6;
        [Range(1f, 15f), SerializeField]
        private float radius = 6f;
        [SerializeField] private Plane plane;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Color color;
        [Range(0.1f, 5f), SerializeField]
        private float speed = 0.1f;
        public Transform bot;
        private float currentMaterialOffset_X;

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            GenDottedCircle();
        }

        public void Start()
        {
            transform.SetParent(LevelController.instance.pool);
        }

        [ContextMenu("Gen Dotted Circle")]
        void GenDottedCircle()
        {
            Vector3[] vectors = GetPointsOnCircle(count, radius, plane);

            lineRenderer.startWidth = lineThickness;
            lineRenderer.endWidth = lineThickness;
            lineRenderer.positionCount = vectors.Length;
            lineRenderer.SetPositions(vectors);

            lineRenderer.sharedMaterial.mainTextureScale = new Vector2Int(dottedCount, 1);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }


        public enum Plane
        {
            [Tooltip("z = 0.")]
            XOY,

            [Tooltip("y = 0.")]
            XOZ,

            [Tooltip("x = 0.")]
            YOZ
        }

        public Vector3[] GetPointsOnCircle(uint n, float r, Plane plane)
        {
            Vector3[] points = new Vector3[n];

            for (int i = 0; i < n; i++)
            {
                float theta = 2 * Mathf.PI * i / n;

                switch (plane)
                {
                    case Plane.XOY:
                        points[i] = new Vector3(r * Mathf.Cos(theta) + offset.x,
                                                r * Mathf.Sin(theta) + offset.y,
                                                offset.z);
                        break;
                    case Plane.XOZ:
                        points[i] = new Vector3(r * Mathf.Cos(theta) + offset.x,
                                                offset.y,
                                                r * Mathf.Sin(theta) + offset.z);
                        break;
                    case Plane.YOZ:
                        points[i] = new Vector3(offset.x,
                                                r * Mathf.Cos(theta) + offset.y,
                                                r * Mathf.Sin(theta) + offset.z);
                        break;
                }
            }

            return points;
        }

        public void SetMaterialOffset_X(float x, bool isReset = false)
        {
            if (lineRenderer.sharedMaterial != null)
            {
                if (isReset)
                {
                    lineRenderer.sharedMaterial.mainTextureOffset = Vector2Int.zero;
                }
                else
                {
                    lineRenderer.sharedMaterial.mainTextureOffset += x * Vector2.right;
                }
            }
        }

        private void Update()
        {
            transform.position = bot.position;
            SetMaterialOffset_X(speed * Time.deltaTime);
        }

        private void OnDisable()
        {
            SetMaterialOffset_X(0, true);
        }
    }
}