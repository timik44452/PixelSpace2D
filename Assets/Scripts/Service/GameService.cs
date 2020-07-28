public enum ManagerMode
{
    BuildManager,
    StrategyManager
}

public static class GameService
{
    public static bool IsLockInput = false;
    public static ManagerMode managerMode = ManagerMode.StrategyManager;
}