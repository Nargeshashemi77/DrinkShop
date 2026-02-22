using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DrinkShop.Shared
{
    public static class DisplayExtensions
    {
        public enum DisplayProperty
        {
            Description,
            GroupName,
            Name,
            Prompt,
            ShortName,
            Order
        }

        //
        // Summary:
        //     دریافت نام ای که برای ویژگی
        //     System.ComponentModel.DataAnnotations.DisplayAttribute
        //     تعریف شده است
        //
        // Parameters:
        //   value:
        public static string GetDisplayName(this object value)
        {
            return value.GetDisplayName("نامشخص");
        }

        //
        // Summary:
        //     دریافت نام ای که برای ویژگی
        //     System.ComponentModel.DataAnnotations.DisplayAttribute
        //     تعریف شده است
        //
        // Parameters:
        //   value:
        //
        //   defaultDisplayName:
        public static string GetDisplayName(this object value, string defaultDisplayName)
        {
            try
            {
                Type type = value.GetType();
                string name = Enum.GetName(type, value);
                MemberInfo memberInfo = type.GetMember(name)[0];
                object[] customAttributes = memberInfo.GetCustomAttributes(typeof(DisplayAttribute), inherit: false);
                string name2 = ((DisplayAttribute)customAttributes[0]).Name;
                if (((DisplayAttribute)customAttributes[0]).ResourceType != null)
                {
                    name2 = ((DisplayAttribute)customAttributes[0]).GetName();
                }

                return name2;
            }
            catch (Exception exception)
            {
                return defaultDisplayName;
            }
        }

        //
        // Summary:
        //     دریافت نام ای که برای ویژگی
        //     System.ComponentModel.DataAnnotations.DisplayAttribute
        //     تعریف شده است
        //
        // Parameters:
        //   value:
        //
        //   property:
        public static string GetDisplayName(this Enum value, DisplayProperty property = DisplayProperty.Name)
        {
            return value.GetDisplayName("نامشخص", property);
        }

        //
        // Summary:
        //     دریافت نام ای که برای ویژگی
        //     System.ComponentModel.DataAnnotations.DisplayAttribute
        //     تعریف شده است
        //
        // Parameters:
        //   value:
        //
        //   defaultDisplayName:
        //
        //   property:
        public static string GetDisplayName(this Enum value, string defaultDisplayName, DisplayProperty property = DisplayProperty.Name)
        {
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }

                DisplayAttribute displayAttribute = value.GetType().GetField(value.ToString()).GetCustomAttributes<DisplayAttribute>(inherit: false)
                    .FirstOrDefault();
                if (displayAttribute == null)
                {
                    return value.ToString();
                }

                object value2 = displayAttribute.GetType().GetProperty(property.ToString()).GetValue(displayAttribute, null);
                return value2.ToString();
            }
            catch (Exception exception)
            {
                return defaultDisplayName;
            }
        }
    }
}
