using UnityEngine;
using System.Collections;
/// <summary
/// Particle generator.
/// 
/// The particle generator simply spawns particles with custom values. 
/// See the Dynamic particle script to know how each particle works..
/// 
/// Visit: www.codeartist.mx for more stuff. Thanks for checking out this example.
/// Credit: Rodrigo Fernandez Diaz
/// Contact: q_layer@hotmail.com
/// </summary>

public class ParticleGenerator : MonoBehaviour {
    public float SPAWN_INTERVAL = 0.001f; // How much time until the next particle spawns
    float lastSpawnTime = float.MinValue; //The last spawn time
    public int MAX_PARTICLES;
    public int PARTICLE_LIFETIME = 100; //How much time will each particle live
    public Vector3 particleForce; //Is there a initial force particles should have?
    public DynamicParticle.STATES particlesState = DynamicParticle.STATES.WATER; // The state of the particles spawned
    public Transform particlesParent; // Where will the spawned particles will be parented (To avoid covering the whole inspector with them)

    private int _particles;
    void Start() {
        if (MAX_PARTICLES == 0) {
            MAX_PARTICLES = int.MaxValue;
        }
    }

    void Update() {
        if (MAX_PARTICLES > _particles && lastSpawnTime + SPAWN_INTERVAL < Time.time) { // Is it time already for spawning a new particle?
            GameObject newLiquidParticle = (GameObject)Instantiate(Resources.Load("LiquidPhysics/DynamicParticle")); //Spawn a particle
            newLiquidParticle.GetComponent<Rigidbody2D>().AddForce(particleForce); //Add our custom force
            DynamicParticle particleScript = newLiquidParticle.GetComponent<DynamicParticle>(); // Get the particle script
            particleScript.SetLifeTime(PARTICLE_LIFETIME); //Set each particle lifetime
            particleScript.SetState(particlesState); //Set the particle State
            newLiquidParticle.transform.position = transform.position;// Relocate to the spawner position
            newLiquidParticle.transform.parent = particlesParent;// Add the particle to the parent container	
            lastSpawnTime = Time.time; // Register the last spawnTime		
            ++_particles;
        }
    }

    public void Reset(){
        _particles = 0;
    }
}
