using UnityEngine;
using System.Collections.Generic;

namespace SoundSystem
{
    public abstract class SoundDataBase<TData> : ScriptableObject where TData : SoundData
    {
        [SerializeField]
        private List<TData> soundDatas;

        public TData GetSoundData(string identifier)
        {
            return soundDatas.Find(data => data.Title == identifier);
        }

        public TData GetSoundData(int index)
        {
            return soundDatas[index];
        }
    }
}
