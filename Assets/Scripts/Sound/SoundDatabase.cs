using UnityEngine;
using System.Collections.Generic;

namespace SoundManagement
{
    public abstract class SoundDataBase<TData> : ScriptableObject where TData : SoundData
    {
        [SerializeField]
        private List<TData> soundDatas;

        public TData GetSoundData(string identifier)
        {
            return soundDatas.Find(data => data.title == identifier);
        }

        public TData GetSoundData(int index)
        {
            return soundDatas[index];
        }
    }
}
