using WMPLib;

public class SoundManager
{
    private WindowsMediaPlayer player;


    public SoundManager(string soundFile)
    {
        player = new WindowsMediaPlayer();
        player.URL = soundFile;
        player.uiMode = "none";
    }

    public void Play()
    {
        player.controls.play();
    }

    public void Stop()
    {
        player.controls.stop();
    }
}

