using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Core.Scripts
{
    public class CubeCollision : MonoBehaviour
    {
        private Cube _cube;

        private void Awake()
        {
            _cube = GetComponent<Cube>();
        }

        private void OnCollisionEnter(Collision other)
        {
            Cube otherCube = other.gameObject.GetComponent<Cube>();
            if (otherCube != null && _cube.cubeID > otherCube.cubeID)
            {
                if (_cube.cubeNumber == otherCube.cubeNumber)
                {
                    Vector3 contactPoint = other.contacts[0].point;
                    if (otherCube.cubeNumber < CubeSpawner.Instance.maxCubeNumber)
                    {
                        Cube newCube = CubeSpawner.Instance.SpawnCube(_cube.cubeNumber * 2, contactPoint + Vector3.up * 1.6f);
                        float pushForce = 2.5f;
                        newCube.cubeRigidbody.AddForce(new Vector3(0, 0.3f, 1f) * pushForce, ForceMode.Impulse);
                        
                        float randomValue = Random.Range(-20f, 20f);
                        Vector3 randomDirection = Vector3.one * randomValue;
                        newCube.cubeRigidbody.AddTorque(randomDirection);
                    }

                    Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                    float explosionForce = 400f;
                    float explosionRadius = 1.5f;
                    for (int i = 0; i < surroundedCubes.Length; i++)
                    {
                        Collider c = surroundedCubes[i];
                        if (c.attachedRigidbody != null)
                        {
                            c.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                        }
                    }
                    FXManager.Instance.PlayCubeExplosionFX(contactPoint, _cube.cubeColor);
                    
                    CubeSpawner.Instance.DestroyCube(_cube);
                    CubeSpawner.Instance.DestroyCube(otherCube);
                }
            }
        }
    }
}