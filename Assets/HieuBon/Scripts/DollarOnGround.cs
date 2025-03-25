using UnityEngine;

namespace Hunter
{
    public class DollarOnGround : MonoBehaviour
    {
        public Transform target;
        public bool isOk;
        public Rigidbody rb;
        public ParticleSystem fx;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = LevelController.instance.GetPlayer(other.gameObject);
                target = player.transform;
                fx.transform.SetParent(player.transform);
                fx.transform.localPosition = Vector3.up * 1.35f;
                isOk = true;
            }
        }

        public void FixedUpdate()
        {
            if (isOk)
            {
                Vector3 targetPos = new Vector3(target.position.x, 1f, target.position.z);
                Vector3 newDirection = Vector3.MoveTowards(rb.position, targetPos, 0.25f);
                rb.MovePosition(newDirection);
                if (Vector3.Distance(rb.position, targetPos) < 1f)
                {
                    isOk = false;
                    gameObject.SetActive(false);
                    fx.Play();
                    UIInGame.instance.gamePlay.UpdateCoin(1);
                }
            }
        }
    }
}
