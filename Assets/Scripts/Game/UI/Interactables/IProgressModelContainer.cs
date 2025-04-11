using System.Collections;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using UniRx;
using UnityEngine;

public interface IProgressModelContainer
{
    public IReadOnlyReactiveProperty<float> CurrentProgress { get; }
    public IReadOnlyReactiveProperty<float> GoalProgress{ get; }
}
