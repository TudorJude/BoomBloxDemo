using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyCollider : MonoBehaviour
    {
        public Action OnGetHit;
        [SerializeField]
        private Rigidbody _rigidBody;
        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision Detected: " + collision.impulse);

            if ((collision.gameObject.tag == "DragonDeafeatingBlocks") && (collision.impulse.magnitude >= Config.COLLISION_FORCE_DAMAGE_THRESHOLD))
                OnGetHit?.Invoke();
        }
    }
}
