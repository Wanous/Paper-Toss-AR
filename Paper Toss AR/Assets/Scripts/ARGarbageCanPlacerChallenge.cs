using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using UnityEngine.XR.ARSubsystems;

public class ARGarbageCanPlacerChallenge : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private Camera _arCamera;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject _garbageCanPrefab;
    [SerializeField] private float _moveInterval = 10f; // Intervalle de déplacement
    
    private GameObject _spawnedGarbageCan;
    private bool _isPlaced = false;
    private Coroutine _moveRoutine;

    void Start()
    {
        StartCoroutine(MoveGarbageCanRoutine());
    }

    IEnumerator MoveGarbageCanRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_moveInterval);
            
            if (_isPlaced && _spawnedGarbageCan != null)
            {
                MoveToRandomPosition();
            }
        }
    }

    void MoveToRandomPosition()
    {
        // Trouve un plan AR valide
        foreach (var plane in _planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                // Position aléatoire sur le plan
                Vector3 randomPosition = plane.transform.position + 
                                      new Vector3(
                                          Random.Range(-plane.size.x/2, plane.size.x/2),
                                          0,
                                          Random.Range(-plane.size.y/2, plane.size.y/2)
                                      );
                
                _spawnedGarbageCan.transform.position = randomPosition;
                
                // Faire face à la caméra
                Vector3 lookDirection = _arCamera.transform.position - randomPosition;
                lookDirection.y = 0;
                _spawnedGarbageCan.transform.rotation = Quaternion.LookRotation(lookDirection);
                
                break;
            }
        }
    }

    // ... (le reste de votre code existant: PlaceGarbageCan, StartRepositioning, etc.)
}