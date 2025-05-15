using System.Collections;
using System.Collections.Generic;
using Game.Clothing;
using Game.Interactables.Factories;
using Game.UI;
using Game.UI.GameStateView;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UniRx;
using UnityEngine;
using Zenject;

public class SceneObjectsContainer : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<SceneObject, GameObject> _sceneObjects;
    [SerializeField] private List<GameObject> _gameStateChangeObserverViews;
    [SerializeField] private List<Transform> _washingMachinePosition; //
    [SerializeField] private List<ClothingChangerConfig> _configs; //
    public List<WashingMachine> WashingMachines; // временно  так, придумать решение получше

    [Inject]
    public void Construct(WashingMachineFactory washingMachineFactory)
    {
        WashingMachines = new List<WashingMachine>();
        washingMachineFactory.OnWashingMachineCreated.SubscribeWithSkip(WashingMachines.Add);
        _washingMachinePosition.ForEach(position =>
        {
            var config = _configs[Random.Range(0, _configs.Count)];
            var newMachine = washingMachineFactory.Create(config);
            newMachine.transform.position = position.position;
        });
    }

    private void SetupGameStateObserver(GameObject observerView, CurrentGameStateObserver currentGameStateObserver)
    {
        if (observerView.TryGetComponent<IGameStateChangeView>(out var observerComponent))
        {
            var presenter = new GameStateChangePresenter(currentGameStateObserver, observerComponent);
        }
    }
    public GameObject GetObject(SceneObject sceneObject)
    {
        return _sceneObjects[sceneObject];
    }

    public T GetObjectsComponent<T>(SceneObject sceneObject)
    {
        return GetObject(sceneObject).GetComponent<T>();
    }

    public void SetupGameStateObserver(CurrentGameStateObserver currentGameStateObserver)
    {
        _gameStateChangeObserverViews.ForEach(view => SetupGameStateObserver(view, currentGameStateObserver));
    }
}

public enum SceneObject
{
    QuotaStartButton,
    QuotaTimeProgressBar,
    UpgradeView
}
