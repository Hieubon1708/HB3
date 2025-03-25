using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class PlayerModelInShop : MonoBehaviour
    {
        Animator animator;

        public GameController.PlayerType playerType;

        public int danceType;

        public void Awake()
        {
            animator = GetComponent<Animator>();

            // animator.SetInteger("Dance Type", danceType);
        }

        public void OnEnable()
        {
            // animator.SetBool("Dance", PlayerInformation.instance.isDancing);
        }

        public void Upgrade()
        {
            animator.SetTrigger("Upgrade");
        }

        public void Use()
        {
            animator.SetTrigger("Use");
        }

        public void Unlock()
        {
            animator.SetTrigger("Unlock");
        }

        public void Dance(bool isDance)
        {
            animator.SetBool("Dance", isDance);
        }
    }
}
