using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Core.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 16;
        [SerializeField] private float pushForce = 20;
        [SerializeField] private float cubeMaxPosX = 1.54f;
        [Space]
        [SerializeField] private TouchSlider touchSlider;
        
        private Cube _mainCube;
        private bool _isPointerDown;
        private Vector3 _cubePos;

        private void Start()
        {
            SpawnCube();
            touchSlider.OnPointerDownEvent += OnPointerDown;
            touchSlider.OnPointerDragEvent += OnPointerDrag;
            touchSlider.OnPointerUpEvent += OnPointerUp;
        }
        
        private void OnDestroy()
        {
            touchSlider.OnPointerDownEvent -= OnPointerDown;
            touchSlider.OnPointerDragEvent -= OnPointerDrag;
            touchSlider.OnPointerUpEvent -= OnPointerUp;
        }

        private void Update()
        {
            if (_isPointerDown)
            {
                _mainCube.transform.position = Vector3.Lerp(
                    _mainCube.transform.position,
                    _cubePos,
                    moveSpeed * Time.deltaTime
                );
            }
        }

        private void OnPointerDown()
        {
            _isPointerDown = true;
        }
        
        private void OnPointerDrag(float value)
        {
            if (_isPointerDown)
            {
                _cubePos = _mainCube.transform.position;
                _cubePos.x = value * cubeMaxPosX;
            }
        }
        
        private void OnPointerUp()
        {
            if (_isPointerDown)
            {
                _isPointerDown = false;
                _mainCube.cubeRigidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);
                Invoke("SpawnNewCube", 0.3f);
            }
        }

        private void SpawnNewCube()
        {
            _mainCube.isMainCube = false;
            SpawnCube();
        }

        private void SpawnCube()
        {
            _mainCube = CubeSpawner.Instance.SpawnRandomCube();
            _mainCube.isMainCube = true;
            _cubePos = _mainCube.transform.position;
        }
    }
}