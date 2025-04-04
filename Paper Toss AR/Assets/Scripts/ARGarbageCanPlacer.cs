using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ARGarbageCanPlacer : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private Camera _arCamera;

    public Transform camTransform;


    [Header("UI Components")]
    [SerializeField] private Button _placeButton;
    [SerializeField] private Button _repositionButton;
    [SerializeField] private Button _playButton;

    [Header("Game Objects")]
    [SerializeField] private GameObject _garbageCanPrefab;
    
    private GameObject _spawnedGarbageCan;
    private bool _isPlaced = false;
    private bool _isPlaying = false;

    private void Start()
    {
        float x = camTransform.position.x + 2;
        float y = camTransform.position.y ;
        float z = camTransform.position.z ;
        Vector3 spawnPos = new Vector3(x, y, z);
        Instantiate(_garbageCanPrefab, spawnPos, Quaternion.identity);


        _placeButton.onClick.AddListener(PlaceGarbageCan);
        _repositionButton.onClick.AddListener(StartRepositioning);
        _playButton.onClick.AddListener(StartGame);
        
        _playButton.interactable = false;
        _repositionButton.interactable = false;
    }

    private void Update()
    {
        if (_isPlaying) return;
        if (_isPlaced && !_repositionButton.interactable) return;

        // Raycast depuis le centre de l'écran
        Ray ray = _arCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Version 1 : Raycast physique (nécessite des colliders sur les plans)
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<ARPlane>() != null)
            {
                UpdateGarbageCanPosition(hit.point);
            }
        }
        // Version 2 : Parcours des plans AR (sans colliders)
        else
        {
            Vector3 screenCenter = _arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
            
            // Nouvelle méthode utilisant trackables
            foreach (var plane in _planeManager.trackables)
            {
                if (plane.alignment == PlaneAlignment.HorizontalUp && 
                    IsPointNearPlane(screenCenter, plane))
                {
                    UpdateGarbageCanPosition(plane.transform.position);
                    break;
                }
            }
        }
    }

    private bool IsPointNearPlane(Vector2 screenPoint, ARPlane plane)
    {
        // Convertir la position du plan en coordonnées écran
        Vector3 planeScreenPos = _arCamera.WorldToScreenPoint(plane.transform.position);
        
        // Vérifier la proximité (seuil réglable)
        return Vector2.Distance(screenPoint, planeScreenPos) < 150f; // en pixels
    }

     private void UpdateGarbageCanPosition(Vector3 position)
    {
        if (_spawnedGarbageCan == null)
        {
            _spawnedGarbageCan = Instantiate(_garbageCanPrefab, position, Quaternion.identity);
        }
        else
        {
            _spawnedGarbageCan.transform.position = position;
            
            // Faire face à la caméra (optionnel)
            Vector3 lookDirection = _arCamera.transform.position - position;
            lookDirection.y = 0;
            _spawnedGarbageCan.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private void PlaceGarbageCan()
    {
        if (_spawnedGarbageCan == null) return;
        
        _isPlaced = true;
        _placeButton.interactable = false;
        _repositionButton.interactable = true;
        _playButton.interactable = true;
        
        SetAllPlanesActive(false);
        _planeManager.enabled = false;
    }

    private void StartRepositioning()
    {
        _isPlaced = false;
        _placeButton.interactable = true;
        _repositionButton.interactable = false;
        _playButton.interactable = false;
        
        _planeManager.enabled = true;
        SetAllPlanesActive(true);
    }

    private void StartGame()
    {
        if (!_isPlaced) return;
        
        _isPlaying = true;
        _placeButton.interactable = false;
        _repositionButton.interactable = false;
        _playButton.interactable = false;
        
        Debug.Log("Game Started! La poubelle est placée.");
    }

    private void SetAllPlanesActive(bool active)
    {
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }


}