using System;
using GT.Asset;
using GT.Audio;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour, IResourceReferenceHolder
{
    [SerializeField] private Button a;
    [SerializeField] private Button b;
    [SerializeField] private Button c;

    public Action DestroyResource { get; set; }

    
    [EditorButton]
    private void AudioTest()
    {
        GameApplication.Instance.gameAudio.PlaySfxOnce(SoundFx.explosionNear, UnityEngine.Random.value);
    }
}