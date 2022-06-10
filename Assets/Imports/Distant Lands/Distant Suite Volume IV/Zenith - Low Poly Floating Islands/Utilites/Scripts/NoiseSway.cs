using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands
{
    public class NoiseSway : MonoBehaviour
    {
        [Tooltip("Controls the strength of the noise.")]
        [SerializeField] private float depth = 1;
        [Tooltip("Controls the distance between peaks of the noise.")]
        [SerializeField] private float width = 1;
        [Tooltip("Controls the seed of the noise. Leave 0 for a random seed.")]
        [SerializeField] private int seed;
        private Vector3 rot;

        void Start()
        {
            if (seed == 0)
                seed = Random.Range(0, 1000000);
            rot = transform.localEulerAngles;

        }

        void Update()
        {

            float t = Time.time / width;

            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(seed, t) - .5f,
                Mathf.PerlinNoise(seed + 1000, t) - .5f,
                Mathf.PerlinNoise(seed + 2000, t) - .5f);


            transform.localEulerAngles = rot + (offset * depth);



        }
    }
}