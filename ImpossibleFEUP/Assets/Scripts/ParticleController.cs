using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    private ParticleSystem particles;

    public void Start() {
        particles = GetComponent<ParticleSystem>();
    }

    public void Update(){
        if (particles) {
            if (!particles.IsAlive())
            {
                Destroy(gameObject);
                Debug.Log("Remove");
            }
        }
    }
}
