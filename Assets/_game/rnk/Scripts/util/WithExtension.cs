using System;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.util
{
    public static class WithExtension
    {
        public static T With<T>(this T item, Action<T> action)
        {
            action(item);
            return item;
        }
        
        public static string WithValues(this string r, int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                r = r.Replace($"%value{i}%", values[i].ToString());
                if (i == 0)
                    r = r.Replace("%value%", values[i].ToString());
            }

            if (values.Length == 0)
                r = r.Replace("%value%", "");
            
            return r;
        }
        
        public static string WithTagValues(this CMSEntity face)
        {
            var tagValue = face.Get<TagValue>();
            var loc = face.Get<TagValueText>()?.loc ?? "%value%";
            return loc.WithValues(tagValue.values);
        }
    }
}