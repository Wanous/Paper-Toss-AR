using UnityEngine;
using System.Collections;

public class RandomTrashMovement : MonoBehaviour 
{
    [Header("Paramètres")]
    [SerializeField] private float moveInterval = 10f;
    [SerializeField] private float moveRadius = 2f; // Rayon autour de la position initiale
    
    private Vector3 _originalPosition;
    private bool _isActive = false;

    public void ActivateMovement()
    {
        _isActive = true;
        _originalPosition = transform.position;
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(moveInterval);
            
            // Position aléatoire autour du point d'origine
            Vector2 randomCircle = Random.insideUnitCircle * moveRadius;
            Vector3 newPos = _originalPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            transform.position = newPos;
            
            Debug.Log($"Poubelle déplacée à {newPos}");
        }
    }
}