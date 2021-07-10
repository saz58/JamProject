using System;
using GT.Data.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GT.UI.Game.Item
{
    public class UI_ModuleItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        // todo: change on sprites.
        [SerializeField] private Color32 hlNormal = default;
        [SerializeField] private Color32 hlActive = default;
        [SerializeField] private Color32 bgEnter = default;
        [SerializeField] private Color32 bgExit = default;
        
        [SerializeField] private Image icon = default;
        [SerializeField] private Image highlight = default;
        [SerializeField] private Image bg = default;

        private Action<int> _onSelect;
        private ModuleData _data;
        public ModuleData Data => _data;

        public void Init(ModuleData data, Sprite moduleIcon, Action<int> selection)
        {
            _data = data;
            icon.sprite = moduleIcon;
            _onSelect = selection;
        }

        public void Select(bool isSelected)
        {
            highlight.color = isSelected ? hlActive : hlNormal;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onSelect?.Invoke(_data.Id);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            bg.color = bgEnter;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            bg.color = bgExit;
        }
    }
}