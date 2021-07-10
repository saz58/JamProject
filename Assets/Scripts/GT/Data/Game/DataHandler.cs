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
            _shipModulesData = new Dictionary<int, ModuleData>();
        }

        private static List<ShipPickableModule> _shipModules;
        private static Dictionary<int, ModuleData> _shipModulesData;
        public static Action<ShipPickableModule> OnModulePicked;


        private static Action _onModuleDataApply;
        public static void RegisterSelectedData(int dataId, Action onApply)
        {
            _onModuleDataApply = onApply;
            if (_shipModulesData.TryGetValue(dataId, out var data))
                SelectedData = data;
        }
        public static void ApplySelectedData()
        {
            _onModuleDataApply += () => { _shipModulesData.Remove(SelectedData.Id); };
            _onModuleDataApply?.Invoke();
            SelectedData = null;
            _onModuleDataApply = null;
        }

        public static ModuleData SelectedData;

        public static ModuleData AddInGameShipModule(ShipPickableModule pickableModule)
        {
            _shipModules.Add(pickableModule);
            var data = new AttackModuleData(_shipModulesData.Count);
            _shipModulesData.Add(data.Id, data);

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
            //if (_shipModulesData.TryGetValue(moduleId, out var data))
            //    data.ReceiveDamage(damage);
        }
    }
}