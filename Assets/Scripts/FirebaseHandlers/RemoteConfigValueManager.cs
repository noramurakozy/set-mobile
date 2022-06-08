namespace FirebaseHandlers
{
    public class RemoteConfigValueManager
    {
        private static RemoteConfigValueManager _instance;
        public static RemoteConfigValueManager Instance => _instance ??= new RemoteConfigValueManager();
        
        public bool CustomAchievements { get; set; }
        // Default value has to be set, because remoteconfig data takes too much time to arrive, and it's better
        // to show the experiment info by default than not to show.
        public bool IsABTestRunning { get; set; } = true;
        
        private RemoteConfigValueManager() {}
    }
}