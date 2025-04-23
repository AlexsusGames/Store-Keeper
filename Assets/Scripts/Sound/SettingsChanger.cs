using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsChanger : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensivitySlider;

    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown localizationDropdown;

    [SerializeField] private Toggle fullScreenToggle;

    [Inject] private SettingsDataProvider dataProvider;

    private Resolution[] resolutions;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        CreateResolutionOptions();

        musicSlider.onValueChanged.AddListener(dataProvider.ChangeMusic);
        soundSlider.onValueChanged.AddListener(dataProvider.ChangeSound);
        sensivitySlider.onValueChanged.AddListener(dataProvider.ChangeSensivity);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        qualityDropdown.onValueChanged.AddListener(dataProvider.ChangeQuality);
        fullScreenToggle.onValueChanged.AddListener(dataProvider.ChangeFullScreen);
        localizationDropdown.onValueChanged.AddListener(ChangeLocalization);
    }

    private void OnEnable()
    {
        var data = dataProvider.GetData();

        musicSlider.value = data.Music;
        soundSlider.value = data.Sound;
        sensivitySlider.value = data.MouseSensivity;
        fullScreenToggle.SetIsOnWithoutNotify(data.IsFullScreen);
        qualityDropdown.SetValueWithoutNotify(data.QualityIndex);
        localizationDropdown.SetValueWithoutNotify(Core.Localization.LocalizationIndex);
    }

    private void ChangeLocalization(int index)
    {
        Core.Localization.LocalizationIndex = index;
    }

    public void CreateResolutionOptions()
    {
        resolutionDropdown.ClearOptions();

        var data = dataProvider.GetData();

        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height} {resolutions[i].refreshRateRatio}Hz";
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.SetValueWithoutNotify(data.ResolutionIndex);

        if(currentResolutionIndex != data.ResolutionIndex && data.ResolutionIndex != -1)
        {
            resolutionDropdown.SetValueWithoutNotify(data.ResolutionIndex);
            ChangeResolution(data.ResolutionIndex);
        }
        else resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
    }

    private void ChangeResolution(int index)
    {
        var resolution = resolutions[index];

        dataProvider.ChangeResolution(index);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
