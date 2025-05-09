using DG.Tweening;
using HieuBon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public Transform target;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Win());
        }
    }

    public IEnumerator Win()
    {
        GameController.instance.gameState = GameController.GameState.Pause;

        PlayerController.instance.HideTouch();
        PlayerController.instance.AngularSpeed = 500;

        PlayerController.instance.player.navMeshAgent.destination = target.position;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.5f);

        GameManager.instance.Level++;
        UIInGame.instance.ChangeMap();
    }
}
