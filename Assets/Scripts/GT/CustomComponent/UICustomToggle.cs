using UnityEngine;
using UnityEngine.UI;

namespace GT.CustomComponent
{
    /// <summary>
    /// Simple UI toggle item based-on 2 icons. (Active Inactive states). 
    /// </summary>
    public class UICustomToggle : MonoBehaviour
    {
        [SerializeField] private Image activeStateIcon;
        [SerializeField] private Image inactiveStateIcon;
        [SerializeField] private Button toggleBtn;

        private bool _state;

        public void Init(System.Action<bool> onToggleAction, bool defaultState = false)
        {
            _state = defaultState;
            toggleBtn.onClick.RemoveAllListeners();
            toggleBtn.onClick.AddListener(() =>
            {
                ChangeState();
                onToggleAction.Invoke(_state);
            });

            ChangeState();
        }

        private void ChangeState()
        {
            activeStateIcon.enabled = _state;
            inactiveStateIcon.enabled = !_state;
            _state = !_state;
        }
    }
}