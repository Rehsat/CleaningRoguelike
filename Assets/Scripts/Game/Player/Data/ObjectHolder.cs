using UniRx;
using UnityEngine;

namespace Game.Player.Data
{
    public class ObjectHolder
    {
        private ReactiveProperty<Rigidbody> _currentPickedUpObject;
        private ReactiveProperty<float> _currentThrowPower;
        private CompositeDisposable _throwStartDisposable;
        public IReadOnlyReactiveProperty<Rigidbody> CurrentPickedUpObject => _currentPickedUpObject;
        public IReadOnlyReactiveProperty<float> CurrentThrowPower => _currentThrowPower;

        public ObjectHolder()
        {
            _currentPickedUpObject = new ReactiveProperty<Rigidbody>();
            _currentThrowPower = new ReactiveProperty<float>();
        }

        public bool TryPickUpObject(Rigidbody pickUp)
        {
            if (_currentPickedUpObject.Value != null) return false;
            _currentPickedUpObject.Value = pickUp;
            _currentPickedUpObject.Value.isKinematic = true;
            return true;
        }
        
        // Решил что бросок достсаточно сильно связан с держанием, чтоб не выделять доп класс под него
        public void StartThrowing(float throwPowerCollectSpeed)
        {
            if(_currentPickedUpObject.Value == null) return;
            
            _throwStartDisposable?.Dispose();
            _throwStartDisposable = new CompositeDisposable();
            Observable.EveryUpdate().Subscribe((l =>
            {
                if (_currentThrowPower.Value < 1)
                    _currentThrowPower.Value += throwPowerCollectSpeed * Time.deltaTime;
            })).AddTo(_throwStartDisposable);
        }

        public void Throw(float throwPowerScale, Vector3 throwDirection)
        {
            if(_currentPickedUpObject.Value == null) return;
            
            _throwStartDisposable?.Dispose();
            _currentPickedUpObject.Value.isKinematic = false;
            
            var force = throwDirection * throwPowerScale * _currentThrowPower.Value;
            _currentPickedUpObject.Value.AddForce(force, ForceMode.Impulse);

            _currentPickedUpObject.Value = null;
            _currentThrowPower.Value = 0;
        }

        public void ForceRemoveCurrentObject()
        {
            var currentObject = _currentPickedUpObject.Value;
            _currentPickedUpObject.Value = null;
            Object.Destroy(currentObject.gameObject);
        }
    }
}
