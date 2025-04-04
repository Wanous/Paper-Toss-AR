using UnityEngine;
using System.Collections;

public class RandomTrashCanMovement : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private float _moveInterval = 10f;
    [SerializeField] private float _movementRadius = 3f; // Rayon autour du joueur
    [SerializeField] private float _groundHeight = 0f; // Hauteur du "sol"

    private Transform _trashCan;
    private Coroutine _moveRoutine;

    void Start()
    {
        _trashCan = transform; // Suppose que le script est sur la poubelle
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_moveInterval);
            MoveToRandomPosition();
        }
    }

    void MoveToRandomPosition()
    {
        // Position aléatoire dans un cercle autour de l'origine
        Vector2 randomCircle = Random.insideUnitCircle * _movementRadius;
        Vector3 newPosition = new Vector3(
            randomCircle.x,
            _groundHeight,
            randomCircle.y
        );

        // Faire face à la caméra (optionnel)
        Vector3 lookDirection = Camera.main.transform.position - newPosition;
        lookDirection.y = 0;
        
        _trashCan.SetPositionAndRotation(
            newPosition,
            Quaternion.LookRotation(lookDirection)
        );

        Debug.Log($"Poubelle déplacée à {newPosition}");
    }
}