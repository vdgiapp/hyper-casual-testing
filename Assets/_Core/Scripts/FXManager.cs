using System;
using UnityEngine;

namespace _Core.Scripts
{
    public class FXManager : MonoBehaviour
    {
        public static FXManager Instance { get; private set; }
        
        [SerializeField] private ParticleSystem cubeExplosionFX;
        private ParticleSystemRenderer cubeExplosionFXRenderer;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            cubeExplosionFXRenderer = cubeExplosionFX.GetComponent<ParticleSystemRenderer>();
        }

        public void PlayCubeExplosionFX(Vector3 position, Color color)
        {
            cubeExplosionFXRenderer.material.color = color;
            cubeExplosionFX.transform.position = position;
            cubeExplosionFX.Play();
        }
    }
}