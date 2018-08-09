using System;

namespace WasteProducts.Logic.Common.Enums
{
    [Flags]

    public enum UserCategory
    {
        /// <summary>
        /// Sportsmen prefer healthy food.
        /// </summary>
        sportsman = 1,

        /// <summary>
        /// Diabetics prefer low carb food.
        /// </summary>
        diabetic = 2
    }
}
