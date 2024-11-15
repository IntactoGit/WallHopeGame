using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Данный класс хранит массив цветов фона, добавленных заранее.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorProvider", menuName = "ColorProvider")]
    public class ColorProvider : ScriptableObject
    {
        public IReadOnlyList<Color> Colors => _colors;
        [field: SerializeField] 
        public Color CurrentColor { get; set; }
    
        [SerializeField]
        private List<Color> _colors;
        
        
        
        
        
        
        
        
        
    }
}