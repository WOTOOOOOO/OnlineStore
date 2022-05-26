namespace someOnlineStore.Data.Enums
{
    [Flags]
    public enum Category
    {
        None = 0,
        Tech = 1,
        Cooking = 2,
        Clothing = 4,
        Accessories = 8,
        Collectibles = 16,
        Toys = 32,
        Games = 64,
        VideoGames = 128,
        Sport = 256,
        Beauty = 512,
        Healthcare = 1024,
        Medical = 2048,
        Houshold = 4096,
        science = 8192,
        Industry = 16384,
        Baby = 32768,
        Instruments = 65536
    }
}
