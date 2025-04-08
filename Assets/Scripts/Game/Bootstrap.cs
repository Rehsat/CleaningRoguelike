using System;
using System.Collections;
using System.Collections.Generic;
using Game.Clothing;
using Game.Interactables;
using Game.UI.Resources;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Collider _sellCollider;
    [SerializeField] private ClothingView _clothingPrefab;
    [SerializeField] private Transform _clothingSpawnPosition;
    [SerializeField] private InteractableButton _clothingSpawnButton;
    
    [SerializeField] private QuotaMoneyView _quotaMoneyView;

    private PlayerResources _playerResources;

    [Inject]
    public void Construct(PlayerResources resources, ObjectSeller seller)
    {
        _playerResources = resources;
        seller.SetSellCollider(_sellCollider);
    }
    private void Awake()
    {
        Application.targetFrameRate = 144;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var clothingFactory = new ClothingFactory(_clothingPrefab);
        var clothingSpawner = new ClothingSpawner(clothingFactory, _clothingSpawnPosition);
        _clothingSpawnButton.Construct();
        _clothingSpawnButton.AddActionApplier(clothingSpawner);
        
        InitUI();
    }

    private void InitUI()
    {
        var quotaMoneyPresenter =
            new ResourcePresenter(_playerResources.GetResource(Resource.QuotaMoney), _quotaMoneyView);
    }
}
