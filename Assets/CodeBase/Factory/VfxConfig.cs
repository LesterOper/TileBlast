using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Factory
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(VfxConfig), fileName = nameof(VfxConfig))]
    public class VfxConfig : ScriptableObject
    {
        public List<Vfx> vfxList;

        public GameObject GetVfxByType(VfxType vfxType) =>
            vfxList.FirstOrDefault(local => local.VfxType == vfxType)?.vfxPrefab;
    }
    
    [Serializable]
    public class Vfx
    {
        public VfxType VfxType;
        public GameObject vfxPrefab;
    }

    public enum VfxType
    {
        BOMB,
    }
}