using TMPro;
using UnityEngine;

namespace CustomTools
{
    public class SwitchRenderPipelineAsset : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Dropdown qualityDpdn = default;

        private void Awake()
        {
            
            qualityDpdn.options.Clear();

            qualityDpdn.options.Add(new TMP_Dropdown.OptionData("Low"));
            qualityDpdn.options.Add(new TMP_Dropdown.OptionData("Med"));
            qualityDpdn.options.Add(new TMP_Dropdown.OptionData("High"));

            qualityDpdn.onValueChanged.RemoveAllListeners();
            qualityDpdn.onValueChanged.AddListener(QualitySettings.SetQualityLevel);
        }
    }
}