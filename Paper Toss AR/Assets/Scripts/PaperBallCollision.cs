using UnityEngine;

public class PaperBallCollision : MonoBehaviour
{
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private ParticleSystem bounceParticles;
    [SerializeField] private float minBounceForce = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si la collision est assez forte
        if (collision.relativeVelocity.magnitude > minBounceForce)
        {
            // Son
            if (bounceSound != null)
                AudioSource.PlayClipAtPoint(bounceSound, transform.position, 0.3f);
            
            // Particules
            if (bounceParticles != null)
                Instantiate(bounceParticles, transform.position, Quaternion.identity);
            
            // Réduction progressive de la vélocité pour un effet réaliste
            GetComponent<Rigidbody>().velocity *= 0.7f;
        }

        // Détection spécifique de la poubelle
        if (collision.gameObject.CompareTag("TrashCan"))
        {
            Debug.Log("Atteint la poubelle !");
            // Ajoutez ici des points ou effets spéciaux
        }
    }
}