namespace FirebaseHandlers
{
    public class RemoteConfigValueManager
    {
        private static RemoteConfigValueManager _instance;
        public static RemoteConfigValueManager Instance => _instance ??= new RemoteConfigValueManager();
        
        public bool CustomAchievements { get; set; }
        
        private RemoteConfigValueManager() {}
    }
}