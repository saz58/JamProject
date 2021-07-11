using System;
using System.Collections.Generic;
using GT.Asset;
using GT.Data.Game;
using GT.Game;
using GT.Game.Modules;
using GT.UI.Game.Item;
using UnityEngine;

namespace GT.UI.Game.Component
{
    public class ModulesList : MonoBehaviour, IResourceReferenceHolder
    {
        [SerializeField] private RectTransform content = default;
        [SerializeField] private Sprite speedIcon;
        [SerializeField] private Sprite attackIcon;
        [SerializeField] private Sprite shieldIcon;
        private Dictionary<int, UI_ModuleItem> _items = new Dictionary<int, UI_ModuleItem>();
        private int _currentSelected = 0;
        

        public Action DestroyResource { get; set; }

        private void Awake()
        {
            DataHandler.OnModulePicked = AddPickedItem;
        }

        public void AddPickedItem(PickableModule module)
        {
            StartCoroutine(AddressableHelper.InstantiateAsset<UI_ModuleItem>(this, nameof(UI_ModuleItem), parent:
                content, item =>
                {
                    item.Init(module.Data, GetIcon(module.Data.Type), Selection);
                    _items.Add(module.Data.Id, item);
                }));
        }

        private void DestroyItem(int id)
        {
            Destroy(_items[id].gameObject);
            _items.Remove(id);
        }

        private Sprite GetIcon(ModuleType type)
        {
            switch (type)
            {
                case ModuleType.Attack:
                    return attackIcon;
                case ModuleType.Shield:
                    return shieldIcon;
                case ModuleType.Speed:
                    return speedIcon;
            }
            return attackIcon;
        }
        


        private void Selection(int idSelected)
        {
            if (_items.TryGetValue(idSelected, out var itemSelect))
            {
                itemSelect.Select(true);
                DataHandler.RegisterSelectedData(idSelected, () =>
                {
                    DestroyItem(idSelected);
                });
            }

            if (_items.TryGetValue(_currentSelected, out var itemDeselect) && idSelected != _currentSelected)
                itemDeselect.Select(false);

            _currentSelected = idSelected;
        }

        public void Clear()
        {
            foreach (var moduleItem in _items)
                Destroy(moduleItem.Value);

            _items.Clear();
        }

        private void OnDestroy()
        {
            Clear();
            DestroyResource?.Invoke();
        }
    }
}