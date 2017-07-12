public interface IGameStateChange
{
    void NotifyTimeChange(int oldTime, int newTime);
    void NotifyStateChange(GameStateEnum oldState, GameStateEnum newState);
}
