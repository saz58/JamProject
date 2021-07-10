using System;
using System.Collections.Generic;
using GT.Asset;
using GT.Data.Game;
using GT.Game;
using GT.UI.Game.Item;
using UnityEngine;

namespace GT.UI.Game.Component
{
    public class ModulesList : MonoBehaviour, IResourceReferenceHolder
    {
        [SerializeField] private RectTransform spawnContainer = default;
        private Dictionary<int, UI_ModuleItem> _items = new Dictionary<int, UI_ModuleItem>();
        private int _currentSelected = 0;

        public Action DestroyResource { get; set; }

        private void Awake()
        {
            DataHandler.OnModulePicked = AddPickedItem;
        }

        public void AddPickedItem(ShipPickableModule module)
        {
            StartCoroutine(AddressableHelper.InstantiateAsset<UI_ModuleItem>(this, nameof(UI_ModuleItem),
                spawnContainer, item =>
                {
                    item.Init(module.Data, module.Icon, Selection);
                    _items.Add(module.Data.Id, item);
                }));
        }

        private void DestroyItem(int id)
        {
            Destroy(_items[id].gameObject);
            _items.Remove(id);
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