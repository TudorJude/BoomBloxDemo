using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    public class RenderTextureDragon : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.Play("Fly Float");
        }
    }
}
