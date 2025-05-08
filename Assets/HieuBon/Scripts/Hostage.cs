using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HieuBon
{
    public class Hostage : MonoBehaviour
    {
        Animator animator;
        [HideInInspector]
        public NavMeshAgent navmesh;
        [HideInInspector]
        public bool isRelease;

        public GameObject blindfold;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            navmesh = GetComponent<NavMeshAgent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !LevelController.instance.hostages.Contains(this))
            {
                blindfold.SetActive(false);

                animator.SetTrigger("Release");

                LevelController.instance.hostages.Add(this);

                navmesh.stoppingDistance = 1.5f;
                navmesh.speed = PlayerController.instance.player.navMeshAgent.speed;

                DOVirtual.DelayedCall(0.5f, delegate
                {
                    isRelease = true;
                });
            }
        }

        private void Update()
        {
            animator.SetBool("Running", navmesh.velocity.magnitude > 0f);
            if (isRelease)
            {
                int index = LevelController.instance.hostages.IndexOf(this);

                Vector3 target = index == 0 ? PlayerController.instance.player.transform.position : LevelController.instance.hostages[index - 1].transform.position;
                navmesh.SetDestination(target);
            }
        }

        public IEnumerator MoveUpHelicopter(Vector3 point1, Vector3 point2, ParticleSystem par, bool isStart)
        {
            isRelease = false;
            navmesh.stoppingDistance = 0f;
            navmesh.SetDestination(point1);
            navmesh.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => navmesh.remainingDistance == navmesh.stoppingDistance);

            if(isStart)
            {
                UIInGame.instance.virtualCam.CamZoom(25f, 1f, 0f);
            }

            navmesh.SetDestination(point2);

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => navmesh.remainingDistance == navmesh.stoppingDistance);

            GameController.instance.FlyMoney(PlayerController.instance.player.gameObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Random.Range(4, 9));

            if (!par.isPlaying) par.Play();

            yield return new WaitForSeconds(0.5f);

            gameObject.SetActive(false);
        }
    }
}
