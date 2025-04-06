using UnityEngine;
using TMPro;

public class ChronoUI : MonoBehaviour
{
    [SerializeField] private float startTime = 60f; // Temps de départ en secondes
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject GameOver;

    [SerializeField] private GameObject WinMenu;

    [SerializeField] private GameObject game;

    public int goal = 50;



    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        UpdateTimerDisplay(currentTime);
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnTimerEnd()
    {
        Debug.Log("Temps écoulé !");

        if (PointCollider.Instance.score < goal){
            GameOver.SetActive(true);
        }
        else{
            WinMenu.SetActive(true);
        }

        game.SetActive(false);

        // Tu peux ici ajouter la logique de fin (désactiver objets, passer à une scène, etc.)
    }

    // Méthodes publiques si tu veux contrôler le chrono depuis un autre script :
    public void StopTimer() => isRunning = false;
    public void StartTimer() => isRunning = true;
    public void ResetTimer() => currentTime = startTime;
}