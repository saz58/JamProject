using System;
using System.Collections.Generic;
using GT.Data.Game;
using GT.UI.Game.Component;
using UnityEngine;

namespace GT.UI.Game.Screen
{
    public class MainHud : BaseScreen
    {
        [SerializeField] private ModulesList modulesList = default;
        public void Init(List<MockModuleData> data)
        {
            
        }
        public override void Open(Action onOpen)
        {
            
        }

        public override void Close(Action onClose)
        {
            modulesList.Clear();
        }
    }
}