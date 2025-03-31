using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controller.Coin.Model
{
    [CreateAssetMenu(fileName = "CoinMaterialMap", menuName = "Coins/Material Map", order = 0)]
    public class CoinMaterialMap : ScriptableObject
    {
        [field: SerializeField] private List<MatData> coinMaterialData;

        public Material GetMaterial(int id)
        {
            return coinMaterialData.FirstOrDefault(mat => mat.id == id).material;
        }
    }

    [Serializable]
    public struct MatData
    {
        public int id;
        public Material material;
    }
}