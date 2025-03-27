public class SoundClip : MusicClip
{
    public override void UpdateVolume(Settings data)
    {
        musicSource.volume = data.Sound;
    }
}
