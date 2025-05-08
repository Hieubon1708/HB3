using UnityEngine;

namespace HieuBon
{
    public class MoneyOnFloor : MonoBehaviour
    {
        public Transform target;
        public bool isOk;
        public ParticleSystem fx;

        Collider[] colliders = new Collider[1];

        LayerMask playerLayer;
        LayerMask wallLayer;

        MeshRenderer meshRenderer;

        float time;

        private void Awake()
        {
            wallLayer = LayerMask.GetMask("Wall");
            playerLayer = LayerMask.GetMask("Player");

            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void FixedUpdate()
        {
            if (!meshRenderer.enabled) return;

            if (isOk)
            {
                Vector3 targetPos = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);

                Vector3 newDirection = Vector3.MoveTowards(transform.position, targetPos, 0.25f);

                transform.position = newDirection;

                if (Vector3.Distance(transform.position, targetPos) < 1f)
                {
                    meshRenderer.enabled = false;

                    fx.Play();

                    UIInGame.instance.gamePlay.UpdateCoin(1);
                }
            }
        }

        private void Update()
        {
            if (!meshRenderer.enabled)
            {
                fx.transform.position = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);

                time += Time.deltaTime;

                if(time == 1) gameObject.SetActive(false);

                return;
            }

            int amountEnemy = Physics.OverlapSphereNonAlloc(transform.position, 3f, colliders, playerLayer);

            if (amountEnemy > 0)
            {
                RaycastHit hit;

                Vector3 from = transform.position;

                Vector3 to = PlayerController.instance.transform.position;

                to.y += 0.5f;

                Physics.Linecast(from, to, out hit, wallLayer);

                if (hit.collider == null)
                {
                    target = PlayerController.instance.transform;

                    isOk = true;
                }
            }
        }
    }
}
