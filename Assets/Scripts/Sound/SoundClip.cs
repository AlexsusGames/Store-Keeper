public class SoundClip : MusicClip
{
    public override void UpdateVolume(SoundData data)
    {
        musicSource.volume = data.Sound;
    }
}
