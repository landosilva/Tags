using System;
using UnityEngine;

namespace Lando.Tags
{
    [Serializable]
    public class TagEntity
    {
        public string Name;
        [ColorUsage(false)] public Color Color;
    }
}