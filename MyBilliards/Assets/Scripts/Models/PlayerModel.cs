using UnityEngine;

namespace Mybilliards.Stolzenberg.Models
{
    [CreateAssetMenu(menuName = "Models/Player")]
    public class PlayerModel : ScriptableObject
    {
        public float TimeToFullPower;
        public float Force;
    }
}
