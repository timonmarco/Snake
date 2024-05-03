using WMPLib;

public class SoundPlayer
{
    private bool playAsLoop;
    private WindowsMediaPlayer player;

    public bool IsPlaying { get; private set; }
    public string CurrentSoundFile { get; private set; }
    public SoundPlayer()
    {
        player = new WindowsMediaPlayer();
    }

    public void Play(string soundFile)
    {
        CurrentSoundFile = soundFile;
        player.URL = soundFile;
        player.uiMode = "none";
        IsPlaying = true;
        player.PlayStateChange += Player_PlayStateChange;
    }

    private void Player_PlayStateChange(int NewState)
    {
        if (NewState == 9)
        {
            if (playAsLoop)
            {
                player.controls.play();
            }
            else
            {
                IsPlaying = false;
                CurrentSoundFile = null;
            }
        }
    }

    public void Stop()
    {
        player?.controls.stop();
        IsPlaying = false;
        CurrentSoundFile = null;
    }
}

