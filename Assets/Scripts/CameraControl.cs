using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 0.02f;
        [SerializeField]
        private float _mouseSensitivity = 2.0f;
        [SerializeField]
        private Vector2 _rotationLimit = new Vector2(80, 80);

        private Vector3 _currentInput;
        private Vector3 _currentRotation;

        private Transform _cachedTransform;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        void Update()
        {
            //WASD movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float jump = Input.GetAxis("Jump");

            _currentInput = new Vector3(horizontal, jump, vertical);
            _currentInput.Normalize();

            _cachedTransform.position += _cachedTransform.TransformDirection(_currentInput) * _moveSpeed * Time.deltaTime;

            //mouse movement
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _currentRotation.x += mouseY;
            _currentRotation.y += mouseX;

            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_rotationLimit.x, _rotationLimit.y);

            _cachedTransform.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        }
    }
}
