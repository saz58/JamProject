using System;
using System.Collections.Generic;
using GT.Game;
using UnityEngine;

namespace GT.Data.Game
{
    public static class DataHandler
    {
        static DataHandler()
        {
            _shipModules = new List<ShipPickableModule>();
            _shipModulesData = new Dictionary<int, MockModuleData>();
        }

        private static List<ShipPickableModule> _shipModules;
        private static Dictionary<int, MockModuleData> _shipModulesData;

        public static Action<ShipPickableModule> OnModulePicked;

        // [RuntimeInitializeOnLoadMethod]
        // static void InitOnLoad()
        // {
        //     _shipModules.Clear();
        //     _shipModulesData.Clear();
        //     OnModulePicked = null;
        // }
        
        public static MockModuleData AddInGameShipModule(ShipPickableModule pickableModule)
        {
            _shipModules.Add(pickableModule);
            var data = new MockModuleData(_shipModulesData.Count);
            _shipModulesData.Add(data.Id, data);
            Debug.Log("Added new Module: " + data.Id);

            return data;
        }

        public static bool CheckModulesPosition(Vector2 pos)
        {
            for (int i = 0; i < _shipModules.Count; i++)
            {
                var module = _shipModules[i];
                if (Vector2.Distance(pos, module.Position) < Constants.ModulePickDistance)
                {
                    module.Pick();
                    _shipModules.RemoveAt(i);
                    OnModulePicked?.Invoke(module);
                    return true;
                }
            }
            return false;
        }

        public static void ModuleReceiveDamage(int moduleId, float damage)
        {
            // todo: handle destroy here ?
            if (_shipModulesData.TryGetValue(moduleId, out var data))
                data.ReceiveDamage(damage);
        }
    }
}