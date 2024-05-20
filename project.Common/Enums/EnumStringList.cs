namespace project.Common.Enums
{
    public class EnumStringList
    {
        public static List<string> GetStringList<TEnum>(bool toLowercase) where TEnum : Enum
        {
            var list = new List<string>();
            foreach (var item in Enum.GetNames(typeof(TEnum)))
            {
                list.Add(toLowercase ? item.ToLower() : item);
            }

            return list;
        }
    }
}