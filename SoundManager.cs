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
    public bool SoundEnabled { get; set; }

    public SoundManager()
    {
        eatSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\AppleCrunch.mp3";
        gameOverSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameOver.mp3";
        menuSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameMenuTheme.mp3";
        menuButtonSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\MenuButton.mp3";
        gameStartSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameStartEnter.mp3";
        gameRunningSoundPath = "C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameRunning.mp3";
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
        if (!SoundEnabled)
            return;
        backgorundMenuSoundPlayer.Stop();
    }

    public void PlayMenuButtonSound() => PlaySoundInQueue(menuButtonSoundPath);

    public void PlayGameStartSound() => PlaySoundInQueue(gameStartSoundPath);
}