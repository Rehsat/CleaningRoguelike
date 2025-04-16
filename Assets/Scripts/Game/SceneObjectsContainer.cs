using System.Collections;
using System.Collections.Generic;
using Game.Clothing;
using Game.Interactables.Factories;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UniRx;
using UnityEngine;
using Zenject;

public class SceneObjectsContainer : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<SceneObject, GameObject> _sceneObjects;
    [SerializeField] private List<Transform> _washingMachinePosition; //
    [SerializeField] private List<ClothingChangerConfig> _configs; //
    public List<WashingMachine> WashingMachines; // временно так, придумать решение получше

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

    public GameObject GetObject(SceneObject sceneObject)
    {
        return _sceneObjects[sceneObject];
    }

    public T GetObjectsComponent<T>(SceneObject sceneObject) where T : Component
    {
        return GetObject(sceneObject).GetComponent<T>();
    }
}

public enum SceneObject
{
    QuotaStartButton
}
