using System;
using System.Collections;
using System.Collections.Generic;
using Game.Clothing;
using Game.Interactables;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Collider _sellCollider;
    [SerializeField] private ClothingView _clothingPrefab;
    [SerializeField] private Transform _clothingSpawnPosition;
    [SerializeField] private InteractableButton _clothingSpawnButton;
    private void Awake()
    {
        Application.targetFrameRate = 144;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var clothingFactory = new ClothingFactory(_clothingPrefab);
        var clothingSpawner = new ClothingSpawner(clothingFactory, _clothingSpawnPosition);
        var objectSeller = new ObjectSeller(_sellCollider);
        _clothingSpawnButton.Construct();
        _clothingSpawnButton.AddActionApplier(clothingSpawner);
    }
}
