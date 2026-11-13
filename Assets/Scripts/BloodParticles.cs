using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticles : MonoBehaviour
{
    private ParticleSystem _ps;
    [SerializeField] private GameObject _particleFloor;
    public List<ParticleCollisionEvent> collisionEvents;
    public int _particleAmount;

    private void Awake()
    {
        _ps = gameObject.GetComponent<ParticleSystem>();        // Get ParticleSystem reference
        collisionEvents = new List<ParticleCollisionEvent>();   // List of each collision event
        var emis = _ps.emission;                                // Make emmision a variable
        emis.burstCount = _particleAmount;                      // Change burst count
    }

    private void Start()
    {

    }

    [System.Obsolete]
    private void OnParticleCollision(GameObject other)
    {
        // Initialize collision events
        int numCollisionEvents = _ps.GetCollisionEvents(other, collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
            // Collision position
            Vector3 pos = collisionEvents[i].intersection;
            // Spawn new collision that player can walk on
            GameObject particleFloor = Instantiate(_particleFloor, pos, Quaternion.EulerAngles(other.transform.rotation.x, other.transform.rotation.y, other.transform.rotation.z * 2f));
            particleFloor.GetComponent<BoxCollider2D>().size = new Vector2(.1f,.1f);
            i++;
        }
    }

    public void SetParticleAmount(int amt)
    {
        var emis = _ps.emission;
        emis.SetBursts(new ParticleSystem.Burst[]{ new ParticleSystem.Burst(0f, amt) });           
    }

}