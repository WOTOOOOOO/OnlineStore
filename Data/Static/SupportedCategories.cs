using someOnlineStore.Data.Enums;

namespace someOnlineStore.Data.Static
{
    public static class SupportedCategories
    {
        public static List<Category> Categories = Enum.GetValues(typeof(Category))
                            .Cast<Category>().Where(c => c != Category.None)
                            .ToList();
    }
}
