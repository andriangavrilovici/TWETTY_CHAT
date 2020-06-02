using TWETTY_CHAT.Core;
using System;
using System.Globalization;

namespace TWETTY_CHAT
{
    /// <summary>
    /// A converter that takes a <see cref="SideMenuContent"/> and converts it to the 
    /// correct UI element
    /// </summary>
    public class SideMenuContentConverter : BaseValueConverter<SideMenuContentConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the side menu type
            var sideMenuType = (SideMenuContent)value;

            // Switch based on type
            switch (sideMenuType)
            {
                // Chat 
                case SideMenuContent.Chat:
                    return new ChatListControl();
                // Contact
                case SideMenuContent.Contacts:
                    return "No UI yet, sorry :)";
                // Unknown
                default:
                    return "No UI yet, sorry :)";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
