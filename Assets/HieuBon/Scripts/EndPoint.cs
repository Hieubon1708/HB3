using DG.Tweening;
using HieuBon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public Transform target1;
    public Transform target2;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")
            && !UIInGame.instance.layerCover.raycastTarget)
        {
            StartCoroutine(Win());
        }
    }

    public IEnumerator Win()
    {
        UIInGame.instance.layerCover.raycastTarget = true;
        PlayerController.instance.Win();
        LevelController.instance.SetAngularSpeed(500);

        PlayerController.instance.player.navMeshAgent.destination = target1.position;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => PlayerController.instance.player.navMeshAgent.remainingDistance == PlayerController.instance.player.navMeshAgent.stoppingDistance);
        yield return new WaitForSeconds(0.3f);

        PlayerController.instance.player.navMeshAgent.destination = target2.position;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.5f);

        GameManager.instance.Level++;
        UIInGame.instance.ChangeMap();
    }
}
