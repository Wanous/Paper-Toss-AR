using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PaperBallLauncher : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject paperBallPrefab;
    [SerializeField] private Transform spawnPoint; // Devant la caméra
    [SerializeField] private float maxPower = 15f;
    [SerializeField] private float minPower = 5f;
    [SerializeField] private Slider powerSlider; // Optionnel - UI feedback

    [Header("Paramètres")]
    [SerializeField] private float ballLifetime = 10f;
    [SerializeField] private float spawnDistance = 1f;

    private GameObject currentBall;
    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isCharging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartCharge(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended && isCharging)
            {
                ReleaseBall(touch.position);
            }
        }

        // Mise à jour visuelle (optionnelle)
        if (isCharging && powerSlider != null)
        {
            float swipeDistance = Vector2.Distance(touchStartPos, Input.GetTouch(0).position);
            powerSlider.value = Mathf.Clamp(swipeDistance / 500f, 0f, 1f); // 500px = swipe max
        }
    }

    // Appelé par le bouton UI
    public void SpawnPaperBall()
    {
        if (currentBall != null) Destroy(currentBall);

        Vector3 spawnPos = Camera.main.transform.position + 
                         Camera.main.transform.forward * spawnDistance;

        currentBall = Instantiate(paperBallPrefab, spawnPos, Quaternion.identity);
        currentBall.GetComponent<Rigidbody>().isKinematic = true;
        
        if (powerSlider != null) powerSlider.value = 0f;
    }

    private void StartCharge(Vector2 touchPos)
    {
        if (currentBall == null) return;

        touchStartPos = touchPos;
        touchStartTime = Time.time;
        isCharging = true;
    }

    private void ReleaseBall(Vector2 touchEndPos)
    {
        if (currentBall == null || !isCharging) return;

        // Calcul de la puissance
        float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);
        float swipeDuration = Time.time - touchStartTime;
        float swipeSpeed = swipeDistance / swipeDuration;

        float power = Mathf.Clamp(swipeSpeed * 0.01f, minPower, maxPower);

        // Direction basée sur le swipe
        Vector3 direction = CalculateDirection(touchStartPos, touchEndPos);

        // Appliquer la force
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(direction * power, ForceMode.Impulse);

        // Destruction après un délai
        Destroy(currentBall, ballLifetime);
        currentBall = null;
        isCharging = false;
    }

    private Vector3 CalculateDirection(Vector2 startPos, Vector2 endPos)
    {
        // Convertir le swipe en direction 3D
        Vector2 swipeVector = endPos - startPos;
        Vector3 worldDirection = new Vector3(
            swipeVector.x * 0.1f, // Sensibilité horizontale
            Mathf.Clamp(swipeVector.y * 0.002f, 0.2f, 0.8f), // Garder une trajectoire arquée
            1 // Force principale vers l'avant
        );

        return Camera.main.transform.TransformDirection(worldDirection).normalized;
    }
}