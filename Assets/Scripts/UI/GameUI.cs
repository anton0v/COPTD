using UnityEngine;

[RequireComponent(typeof (IGame))]
[DisallowMultipleComponent]
public class GameUI : MonoBehaviour
{
    private IGame _game;

    public void Awake()
    {
        _game = GetComponent<Game>();
    }
    public void OnGUI()
    {
        if (_game.Status == IGame.GameResult.NotStarted)
        {
            if (GUILayout.Button("Начать игру"))
                _game.StartGame();
            return;
        }
        if (_game.Status == IGame.GameResult.InBuildProcess)
        {
            GUILayout.Label("Необходимо посмотрить башни");
            return;
        }
        GUILayout.Label(string.Format("Волна {0} из {1}", _game.CurrentWave, _game.WaveCounts));
        if (_game.Status != IGame.GameResult.InProcess)
            GUILayout.Label(string.Format("Игра окончена. Вы {0} ", _game.Status == IGame.GameResult.Victory ? "выиграли":"проиграли"));
    }
}
