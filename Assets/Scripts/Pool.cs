using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YieldMagic
{
    public class YieldPool
    {
        private static readonly Dictionary<Type, List<YieldBase>> freeInstruction =
        new Dictionary<Type, List<YieldBase>>();
        private static readonly Dictionary<Type, List<YieldBase>> activeInstructions =
            new Dictionary<Type, List<YieldBase>>();


        /// <summary>
        /// wipe cached clean
        /// </summary>
        public static void Clear()
        {
            freeInstruction.Clear();
            activeInstructions.Clear();
        }
        /// <summary>
        /// /// This should clear stuck elemets
        /// as this is self cleaning pool it can be interupted by:
        /// GameObject.Destroy();
        /// Removeing or deactivating component;
        /// </summary>
        /// <param name="forceMode">care, this flag will force free, but can result in overriding working instructions!</param>
        public static void UnStuck(bool forceMode = false)
        {
            foreach (var pair in activeInstructions)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    var value = pair.Value[i];
                    if (value.IsOutdated || forceMode)
                    {
                        Free(value);
                        i--;
                    }
                }
            }
        }
        public static int FreeCount { get { return freeInstruction.Sum(x => x.Value.Count); } }
        public static int ActiveCount { get { return activeInstructions.Sum(x => x.Value.Count); } }
        internal static T Get<T>() where T : YieldBase, new()
        {
            var type = typeof(T);
            if (!freeInstruction.ContainsKey(type))
            {
                freeInstruction.Add(type, new List<YieldBase>());
                activeInstructions.Add(type, new List<YieldBase>());
            }
            else
            {
                if (freeInstruction[type].Count > 0)
                {
                    var item = freeInstruction[type][0];
                    freeInstruction[type].RemoveAt(0);
                    activeInstructions[type].Add(item);
                    return item as T;
                }
            }
            var transition = new T {ReturnToPool = true};
            activeInstructions[type].Add(transition);
            return transition;
        }

        internal static void Free(YieldBase item)
        {
            //        Debug.Log("Free:" + Progress);
            item.Initialized = false;
            Type type = item.GetType();
            //incase we don't use pool
            if (activeInstructions.ContainsKey(type) && activeInstructions[type].Contains(item))
            {
                activeInstructions[type].Remove(item);
                freeInstruction[type].Add(item);
            }
        }
    }
}