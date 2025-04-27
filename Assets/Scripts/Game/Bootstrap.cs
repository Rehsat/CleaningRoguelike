using System;
using System.Collections;
using System.Collections.Generic;
using Game.Clothing;
using Game.GameStateMachine;
using Game.Interactables;
using Game.UI.Resources;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Collider _sellCollider;
    [SerializeField] private ClothingView _clothingPrefab;
    [SerializeField] private Transform _clothingSpawnPosition;
    [SerializeField] private InteractableButton _clothingSpawnButton;
    
    [SerializeField] private ResourceView _quotaResourceView;
    [SerializeField] private SerializableDictionary<PlayerValue, ResourceView> _resourceViews;

    private GameValuesContainer _gameValuesContainer;
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(GameValuesContainer resources, ObjectSeller seller, GameStateMachine gameStateMachine)
    {
        _gameValuesContainer = resources;
        _gameStateMachine = gameStateMachine;
        seller.SetSellCollider(_sellCollider);
    }
    private void Awake()
    {
        Application.targetFrameRate = 144;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var clothingFactory = new ClothingFactory(_clothingPrefab, _gameValuesContainer.GetPlayerValue(PlayerValue.ActiveClothing));
        var clothingSpawner = new ClothingSpawner(clothingFactory, _clothingSpawnPosition);
        _clothingSpawnButton.Construct();
        _clothingSpawnButton.AddActionApplier(clothingSpawner);
        InitUI();
        
        _gameStateMachine.EnterState<UpgradeGameState>();
    }

    private void InitUI()
    {
        foreach (var resourcePair in _resourceViews)
        {
            var resourceType = resourcePair.Key;
            var resourceView = resourcePair.Value;
            var resourcePresenter = new ResourcePresenter(_gameValuesContainer.GetPlayerValue(resourceType), resourceView);
        }
    }
}
