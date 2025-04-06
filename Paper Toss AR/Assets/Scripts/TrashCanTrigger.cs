using UnityEngine;

public class TrashCanTrigger : MonoBehaviour
{

    [SerializeField] private ParticleSystem moveParticles; // Référence aux particules

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PaperBall"))
        {
            // Appel direct de la fonction de score

            PointCollider.Instance.AddScore();
            //Destroy(other.gameObject); // Détruit la boulette
        }
    }
}