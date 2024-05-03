using System.Linq;
public class SoundManager
{
    private readonly string eatSoundPath;
    private readonly string gameOverSoundPath;
    private readonly string menuSoundPath;
    private readonly string menuButtonSoundPath;
    private readonly string gameStartSoundPath;
    private readonly string gameRunningSoundPath;
    private readonly SoundPlayer[] players = new SoundPlayer[20];
    private readonly SoundPlayer backgorundGameSoundPlayer = new SoundPlayer();
    private readonly SoundPlayer backgorundMenuSoundPlayer = new SoundPlayer();
    private bool soundEnabled;

    public bool SoundEnabled
    {
        get => soundEnabled;
        set
        {
            if (soundEnabled != value)
            {
                soundEnabled = value;
                OnSoundEnabledChanged();
            }
        }
    }

    public SoundManager()
    {
        eatSoundPath = "AppleCrunch.mp3";
        gameOverSoundPath = "GameOver.mp3";
        menuSoundPath = "GameMenuTheme.mp3";
        menuButtonSoundPath = "MenuButton.mp3";
        gameStartSoundPath = "GameStartEnter.mp3";
        gameRunningSoundPath = "GameRunning.mp3";
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new SoundPlayer();
        }
    }

    private void PlaySoundInQueue(string soundFilePath)
    {
        if (!SoundEnabled)
            return;

        var player = players.FirstOrDefault(el => !el.IsPlaying);
        if (player == null)
        {
            return;
        }
        player.Play(soundFilePath);
    }

    public void PlayEatSound() => PlaySoundInQueue(eatSoundPath);

    public void PlayGameRunningSound()
    {
        if (!SoundEnabled)
            return;
        backgorundGameSoundPlayer.Play(gameRunningSoundPath);
    }

    public void StopGameRunningSound()
    {
        if (!SoundEnabled)
            return;
        backgorundGameSoundPlayer.Stop();
    }

    public void PlayGameOverSound() => PlaySoundInQueue(gameOverSoundPath);

    public void PlayMenuSound()
    {
        if (!SoundEnabled)
            return;
        backgorundMenuSoundPlayer.Play(menuSoundPath);
    }

    public void StopMenuSound()
    {
        backgorundMenuSoundPlayer.Stop();
    }

    public void PlayMenuButtonSound() => PlaySoundInQueue(menuButtonSoundPath);

    public void PlayGameStartSound() => PlaySoundInQueue(gameStartSoundPath);

    private void OnSoundEnabledChanged()
    {
        if (!SoundEnabled)
        {
            StopMenuSound();
            StopGameRunningSound();
        }
    }
}