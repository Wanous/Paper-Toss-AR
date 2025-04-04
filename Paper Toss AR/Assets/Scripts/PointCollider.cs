using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCollider : MonoBehaviour
{
    public static PointCollider Instance; // Singleton pour accès facile

    public int score = 0;
    [SerializeField] private RawImage Dizaine;
    [SerializeField] private RawImage Unite;
    [SerializeField] private List<Texture> numberTextures;

    void Awake()
    {
        Instance = this; // Initialise le singleton
    }

    // Fonction publique pour incrémenter le score
    public void AddScore()
    {
        score++;
        ChangeImage();
    }

    private void ChangeImage()
    {        
        Unite.texture = numberTextures[score % 10];
        Dizaine.texture = numberTextures[Mathf.Clamp(score / 10, 0, 9)];
    }
}