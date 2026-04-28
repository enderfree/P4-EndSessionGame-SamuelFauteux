using UnityEngine;

public class GameManagerHyperRestriction: MonoBehaviour
{
    public delegate void GameStateChange(GameStates oldGameState, GameStates newGameState);
    public static event GameStateChange OnGameStateChange;

    private static GameStates gameState = GameStates.Overworld;

    public static GameStates GameState
    {
        get
        {
            return gameState;
        }
        protected set // this class exists to force the use of the setter in TurnManager
        {
            OnGameStateChange(gameState, value);
            gameState = value;
        }
    }
}