using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveApp
{
    internal static class HoneyVault
    {
        public const float NECTAR_CONVERSION_RATIO = .19F;
        public const float LOW_LEVEL_WARNING = 10F;

        private static float honey = 25F;
        private static float nectar = 100F;

        public static void ConvertNectarToHoney(float amount)
        {
            float nectarToConvert = amount;
            if (nectarToConvert > nectar)
                nectarToConvert = nectar;

            nectar -= nectarToConvert;
            honey += nectarToConvert * NECTAR_CONVERSION_RATIO;
        }

        public static bool ConsumeHoney(float amount)
        {
            if (honey >= amount)
            {
                honey -= amount;
                return true;
            }
            else
                return false;
        }

        public static void CollectNectar(float amount)
        {
            if (amount > 0F)
                nectar += amount;
        }

        public static string StatusReport
        {
            get
            {
                if (honey < LOW_LEVEL_WARNING || nectar < LOW_LEVEL_WARNING)
                    return $"Miód: {honey:0.0}\nNektar: {nectar:0.0}\nMało miodu! Dodaj prodecentów miodu!";
                else
                    return $"Miód:{honey:0.0}\nNektar:{nectar:0.0}";
            }
        }
    }
}
