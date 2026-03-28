using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Tooltip("El script principal del enemigo para saber si está en estado de ataque")]
    public EnemyFSM enemyFSM;
    
    [Tooltip("Cantidad de daño que inflige esta espada")]
    public float damage = 20f;

    [Tooltip("Tiempo de gracia entre golpes para evitar matar al jugador de un solo toque por colisiones repetitivas")]
    public float hitCooldown = 1.0f;
    private float lastHitTime = 0f;

    void Start()
    {
        // Si no lo asignaste a mano en Unity, busca el EnemyFSM en el objeto padre automáticamente
        if (enemyFSM == null)
        {
            enemyFSM = GetComponentInParent<EnemyFSM>();
        }
    }

    // Si tu MeshCollider Convex NO tiene marcado "Is Trigger"
    private void OnCollisionEnter(Collision collision)
    {
        TryDealDamage(collision.gameObject);
    }

    // Si tu MeshCollider Convex SÍ tiene marcado "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other.gameObject);
    }

    private void TryDealDamage(GameObject hitObject)
    {
        // 1. Evitar daño múltiple instantáneo si la espada hace varios contactos rápidos
        if (Time.time < lastHitTime + hitCooldown) return;

        // 2. Comprobar que realmente estamos en la fase de atacar y no es que la espada te ha rozado al andar
        if (enemyFSM != null && enemyFSM.currentState == EnemyFSM.State.Attack)
        {
            // 3. Buscar tu script de vida
            LifeSystem playerLife = hitObject.GetComponent<LifeSystem>();
            
            if (playerLife != null)
            {
                // ¡Zasca!
                playerLife.DamagePlayer(damage);
                lastHitTime = Time.time;
            }
        }
    }
}
