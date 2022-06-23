﻿using MapEditor.Event;
using UnityEngine;

namespace MapEditor.ChangeableInterfaces
{
    public interface IColorable
    {
        public bool OnChange(Color color) { return true; }

        public void OnBeginColor(ColorBeginEvent @event)
        {
        }
    }
}