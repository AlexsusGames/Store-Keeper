using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class MusicClip : MonoBehaviour
{
    [SerializeField] protected AudioSource musicSource;
    [Inject] private SettingsDataProvider dataProvider;

    public virtual void OnEnable()
    {
        UpdateVolume(dataProvider.GetData());
        dataProvider.OnSettingsChanged += UpdateVolume;
    }
    public virtual void OnDisable() => dataProvider.OnSettingsChanged -= UpdateVolume;

    public virtual void UpdateVolume(Settings data)
    {
        musicSource.volume = data.Music;
    }
}
