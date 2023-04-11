using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        public Action<Bullet, Vector3> OnCollision;
        public Action<Bullet> OnExpire;

        public AudioClip FireSound;
        public List<AudioClip> ImpactSounds;

        public GameObject ImpactEffect;

        private Rigidbody _rigidBody;
        private AudioSource _audioSource;
        private float liveTimer = 0f;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void OnEnable()
        {
            liveTimer = 0;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void Update()
        {
            liveTimer += Time.deltaTime;
            if (liveTimer > Config.BULLET_LIFETIME_SECONDS)
                OnExpire?.Invoke(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke(this, transform.position);
        }


    }
}
