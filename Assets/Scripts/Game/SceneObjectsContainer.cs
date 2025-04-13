using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

public class SceneObjectsContainer : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<SceneObject, GameObject> _sceneObjects;

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
