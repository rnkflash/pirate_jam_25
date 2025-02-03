using System;
using System.Collections.Generic;
using UnityEngine;

namespace _game.rnk.Scripts.ui
{
    public class BuffList : MonoBehaviour
    {
        BuffView prefab;

        List<BuffView> buffList;

        void Awake()
        {
            prefab = "prefab/BuffView".Load<BuffView>();
            buffList = new List<BuffView>();
        }

        public void AddBuff(BuffState buffState)
        {
            buffList.Add(CreateView(buffState));
        }

        public void RemoveBuff(BuffState buffState)
        {
            var found = buffList.FindAll(view => view.state == buffState);
            foreach (var f in found)
            {
                
                DestroyView(f);
            }
        }

        public void Clear()
        {
            foreach (var buffView in buffList)
            {
                DestroyView(buffView, false);
            }
            buffList.Clear();
        }

        BuffView CreateView(BuffState buffState)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetState(this, buffState);
            return obj;
        }

        public void DestroyView(BuffView view, bool removeFromList = true)
        {
            if (removeFromList)
                buffList.Remove(view);
            view.FreeState();
            Destroy(view.gameObject);
        }
    }
}