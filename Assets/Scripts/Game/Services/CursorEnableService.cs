using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Services
{
    public class CursorEnableService
    {
        private readonly List<ICursorRequire> _cursorRequires;

        public CursorEnableService(List<ICursorRequire> cursorRequires)
        {
            _cursorRequires = cursorRequires;
            _cursorRequires.ForEach(objectThatNeedsCursor =>
                objectThatNeedsCursor.IsCursorRequired.Subscribe(b=> UpdateCursorState()));
        }

        private void UpdateCursorState()
        {
            var cursorRequired = IsCursorRequired();
            var lockState = cursorRequired ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.lockState = lockState;
            Cursor.visible = false;
        }

        private bool IsCursorRequired()
        {
            var objectThatNeedsCursor = _cursorRequires
                .Find(cursorRequires => cursorRequires.IsCursorRequired.Value);
            
            return objectThatNeedsCursor != null;
        }
    }

    public interface ICursorRequire
    {
        public IReadOnlyReactiveProperty<bool> IsCursorRequired { get; }
    }
}
