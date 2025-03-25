using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace Hunter
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        public GameController.WeaponType weaponType;

        public float force;
        public Player player;
        public float attackRange;
        public int damage;
        public Outline outline;
        public MeshRenderer meshRenderer;
        public Material defaultMaterial;
        public ParticleSystem parWeapon;
        public Transform aim;
        public AimConstraint aimConstraint;

        public virtual void Start()
        {
            if (aim == null) return;
            aim.SetParent(LevelController.instance.pool);
            aimConstraint.rotationOffset = Vector3.zero;
        }

        public void Die()
        {
            if(aimConstraint != null) aimConstraint.enabled = false;
        }

        public abstract IEnumerator Attack(Transform target);

        public virtual void Init(Player player)
        {
            this.player = player;
        }
    }
}
