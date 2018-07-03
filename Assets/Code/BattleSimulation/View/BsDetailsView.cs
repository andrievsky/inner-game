using UnityEngine;
using UnityEngine.UI;

namespace Code.BattleSimulation.View
{
    public class BsDetailsView : MonoBehaviour
    {
        [SerializeField] public Text Id;
        [SerializeField] public Text Helth;
        [SerializeField] public Text Level;
        [SerializeField] public Text Dead;
        [SerializeField] public Text Attack;
        [SerializeField] public Text Heal;
        [SerializeField] public Text AttackType;
        [SerializeField] public Text Factions;
    }
}