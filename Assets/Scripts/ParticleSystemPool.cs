using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParticleSystemPool : MonoBehaviour {

    public ParticleSystem prefab;
    public int size;

    private List<ParticleSystem> pool;

    void Start() {
        pool = new List<ParticleSystem>();
        for (int i = 0; i < size; i++) {
            ParticleSystem ps = Instantiate(prefab);
            ps.transform.parent = transform;
            pool.Add(ps);
        }
    }

    public ParticleSystem GetParticleSystem() {
        foreach (ParticleSystem ps in pool) {
            if (!ps.isPlaying) {
                return ps;
            }
        }
        return null;
    }

}
