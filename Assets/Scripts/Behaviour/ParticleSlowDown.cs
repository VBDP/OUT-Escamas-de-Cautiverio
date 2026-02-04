using UnityEngine;

public class ParticleSlowDown : MonoBehaviour
{
    public ParticleSystem particleSystemTarget;
    public float delay = 2f;
    public float newSimulationSpeed = 0.3f;

    void Start()
    {
        Invoke(nameof(SlowParticles), delay);
    }

    void SlowParticles()
    {
        if (particleSystemTarget != null)
        {
            var main = particleSystemTarget.main;
            main.simulationSpeed = newSimulationSpeed;
        }
    }
}
