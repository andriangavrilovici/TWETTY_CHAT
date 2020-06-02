namespace TWETTY_CHAT
{
    public static class GenerateRGBValues
    {
        public static string Generate(char value)
        {

            string RGBValue = "#666666";

            if (value == 'a' || value == 'A')
                RGBValue = "#ff4040";
            else if (value == 'n' || value == 'N')
                RGBValue = "#3099c5";
            else if (value == 'l' || value == 'L')
                RGBValue = "#fe4503";
            else if (value == 'i' || value == 'I')
                RGBValue = "#18D995";
            else if (value == 'k' || value == 'K')
                RGBValue = "#3099c5";
            else if (value == 'b' || value == 'B')
                RGBValue = "#fe4503";
            else if (value == 'm' || value == 'M')
                RGBValue = "#40e0d0";
            else if (value == 'd' || value == 'D')
                RGBValue = "#065535";
            else if (value == 'j' || value == 'J')
                RGBValue = "#008080";
            else if (value == 'e' || value == 'E')
                RGBValue = "#ffd700";
            else if (value == 'c' || value == 'C')
                RGBValue = "#40e0d0";
            else if (value == 'h' || value == 'H')
                RGBValue = "#065535";
            else if (value == 'f' || value == 'F')
                RGBValue = "#ff6666";
            else if (value == 'o' || value == 'O')
                RGBValue = "#20b2aa";
            else if (value == 'p' || value == 'P')
                RGBValue = "#20b2aa";
            else if (value == 'r' || value == 'R')
                RGBValue = "#088da5";
            else if (value == 's' || value == 'S')
                RGBValue = "#40e0d0";
            else if (value == 't' || value == 'T')
                RGBValue = "#008080";
            else if (value == 'u' || value == 'U')
                RGBValue = "#40e0d0";
            else if (value == 'v' || value == 'V')
                RGBValue = "#ff7f50";

            return RGBValue;
        }
    }
}
