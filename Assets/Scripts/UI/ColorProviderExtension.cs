using System.Linq;
using UnityEngine;

namespace UI
{
    public static class ColorProviderExtensions
    {
        /// <summary>
        /// Возвращает случайный цвет из списка доступных цветов, исключая указанный цвет.
        /// </summary>
        public static Color GetRandomColor(this ColorProvider colorProvider, Color except)
        {
            // Фильтруем доступные цвета, исключая указанный цвет.
            var availableColors = colorProvider.Colors.Where(color => color != except).ToList();
            var randomIndex = Random.Range(0, availableColors.Count);
            return availableColors[randomIndex];
            
        }
        
        /// <summary>
        /// Возвращает случайный цвет из списка доступных цветов.
        /// </summary>
        public static Color GetRandomColor(this ColorProvider colorProvider)
        {
            var randomIndex = Random.Range(0, colorProvider.Colors.Count);
            var color = colorProvider.Colors[randomIndex];
            return color;
        }
        
        
        
        
        
        
        
    }
}