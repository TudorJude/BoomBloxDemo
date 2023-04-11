using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    public class EnemyBase : MonoBehaviour
    {
        public Action OnGotSmoked;
        public bool GetIsDead => _isDead;

        private Animator _animator;
        private EnemyCollider _enemyCollider;
        private bool _isDead;
        private AudioSource _audioSource;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _enemyCollider = GetComponentInChildren<EnemyCollider>();
            _enemyCollider.OnGetHit = () =>
            {
                GetSmoked();
            };
        }

        private void GetSmoked()
        {
            _isDead = true;
            _enemyCollider.enabled = false;
            _animator.Play("Die");
            _audioSource.Play();
            OnGotSmoked?.Invoke();
            StartCoroutine(CleanUp());
        }

        private IEnumerator CleanUp()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}
