using System;
using System.Collections.Generic;
using System.Linq;
using CustomExtension;
using GT.Game;
using GT.Game.Modules;
using UnityEngine;

namespace GT.Data.Game
{
    public static class DataHandler
    {
        static DataHandler()
        {
            _pickableModules = new List<PickableModule>();
            _modulesData = new Dictionary<int, ModuleData>();
        }

        private static List<PickableModule> _pickableModules;
        private static Dictionary<int, ModuleData> _modulesData;
        public static Action<PickableModule> OnModulePicked;
        private static List<ModuleType> _types = new List<ModuleType>();
        public static ModuleData SelectedData;
        private static Action _onModuleDataApply;

        public static void RegisterSelectedData(int dataId, Action onApply)
        {
            _onModuleDataApply = onApply;
            if (_modulesData.TryGetValue(dataId, out var data))
                SelectedData = data;
        }

        public static void ApplySelectedData()
        {
            _onModuleDataApply += () => { _modulesData.Remove(SelectedData.Id); };
            _onModuleDataApply?.Invoke();
            SelectedData = null;
            _onModuleDataApply = null;

            InputManager.Instance.constructToggle = true;
        }

        public static ModuleData AddInGameModule(PickableModule pickableModule)
        {
            _pickableModules.Add(pickableModule);
            var data = GetModuleData();
            _modulesData.Add(data.Id, data);
            return data;
            
            ModuleData GetModuleData()
            {
                _types = EnumExts.GetCastValues<ModuleType>().ToList();
                _types.Remove(0); // remove core;
                
                switch (_types.PickRandom())
                {
                    case ModuleType.Attack:
                        return new AttackModuleData(_modulesData.Count);
                    case ModuleType.Shield:
                        return new ShieldModuleData(_modulesData.Count);
                    case ModuleType.Speed:
                        return new SpeedModuleData(_modulesData.Count, 0.2f, 1f, 2, 5);
                }
                return new SpeedModuleData(_modulesData.Count, 0.2f, 1f, 2, 5);
            }
        }
        

        public static void CheckModulesPosition(Vector2 pos)
        {
            for (int i = 0; i < _pickableModules.Count; i++)
            {
                var module = _pickableModules[i];
                if (Vector2.Distance(pos, module.Position) < Constants.ModulePickDistance)
                {
                    module.Pick();
                    _pickableModules.RemoveAt(i);
                    OnModulePicked?.Invoke(module);
                    break;
                }
            }
        }
    }
}