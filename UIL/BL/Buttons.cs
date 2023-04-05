using System;

namespace BL
{
    public class Buttons
    {
        public static Buttons Instance = new Buttons();

        Buttons()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
}
