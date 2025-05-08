using UnityEngine;

namespace HieuBon
{
    public class Money : MonoBehaviour
    {
        public Rigidbody rb;
        public Transform target;
        public bool isOk;
        public bool isRotateLeft;
        public float forceRotate;
        Vector3 dir;
        public GameObject mesh;
        public ParticleSystem fx;
        public ReceiveMoney receiveMoney;

        public void Out(Vector3 dir)
        {
            transform.localPosition = Vector3.zero;
            this.dir = dir;
            mesh.SetActive(true);
            rb.isKinematic = false;
            rb.excludeLayers = 0;
            rb.AddRelativeForce(dir, ForceMode.Impulse);
            isRotateLeft = Random.Range(0, 2) == 0;
            float randomForce = 20;
            forceRotate = isRotateLeft ? -randomForce : randomForce;
            rb.AddTorque(dir * forceRotate, ForceMode.Impulse);
        }

        public void In(Transform target, LayerMask wallLayer)
        {
            this.target = target;   
            rb.excludeLayers = wallLayer;
            rb.AddTorque(dir * -forceRotate, ForceMode.Impulse);
            isOk = true;
        }

        public void FixedUpdate()
        {
            if (isOk)
            {
                Vector3 targetPos = new Vector3(target.transform.position.x, 1f, target.transform.position.z);
                Vector3 newDirection = Vector3.MoveTowards(rb.position, targetPos, 0.5f * Time.timeScale);
                rb.MovePosition(newDirection);
                if (Vector3.Distance(rb.position, targetPos) < 1f)
                {
                    isOk = false;
                    mesh.SetActive(false);
                    rb.isKinematic = true;
                    int coin = receiveMoney.GetCoin();
                    UIInGame.instance.gamePlay.UpdateCoin(coin);
                    fx.Play();
                }
            }
        }

        private void Update()
        {
            if (!mesh.activeSelf)
            {
                fx.transform.position = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
            }
        }
    }
}