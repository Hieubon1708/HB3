using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class EndCard1 : MonoBehaviour
    {
        public ParticleSystem par;
        public Transform point1;
        public Transform helicopter;
        public Animation aniFlying;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Win());
            }
        }

        public IEnumerator Win()
        {
            PlayerController.instance.Pause();

            yield return new WaitForSeconds(0.3f);

            PlayerController.instance.player.navMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

            List<Hostage> hostages = LevelController.instance.hostages;

            for (int i = 0; i < hostages.Count; i++)
            {
                StartCoroutine(hostages[i].MoveUpHelicopter(point1.position, par.transform.position, par, i == 0));
                yield return new WaitForSeconds(0.3f);
            }

            par.Stop();

            if (hostages.Count > 0) yield return new WaitForSeconds(2.5f);

            PlayerController.instance.player.navMeshAgent.destination = point1.position;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => PlayerController.instance.player.navMeshAgent.remainingDistance == PlayerController.instance.player.navMeshAgent.stoppingDistance);

            PlayerController.instance.player.navMeshAgent.destination = par.transform.position;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => PlayerController.instance.player.navMeshAgent.remainingDistance == PlayerController.instance.player.navMeshAgent.stoppingDistance);

            par.Play();

            yield return new WaitForSeconds(1.5f);

            par.Stop();

            PlayerController.instance.player.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            aniFlying.Stop();
            helicopter.DOMove(new Vector3(helicopter.position.x - 10, helicopter.position.y + 10, helicopter.position.z), 2.5f).SetEase(Ease.InOutQuad);
        }
    }
}
