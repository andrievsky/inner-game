using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Config;
using Code.BattleSimulation.Model;
using UnityEngine;
using Zenject;

namespace Code.BattleSimulation.View
{
    public interface IBsHudView
    {
        void SetDetails(IBsActor actor, int health, bool isDead, int level, ActorAttackType type,
            BsFaction factions);
    }
    
    public class BsHudView : MonoInstaller<BsView>, IBsHudView
    {
        [SerializeField] public BsDetailsView DetailsView;
        
        public override void InstallBindings()
        {
            Container.Bind<IBsHudView>().FromInstance(this).AsSingle();
        }
        
        public void SetDetails(IBsActor actor, int health, bool isDead, int level, ActorAttackType type,
            BsFaction factions)
        {
            DetailsView.Id.text = "Player: " + actor.Id().ToString();
            DetailsView.Attack.text = "Attack: " + actor.Config().BaseDamage().ToString();
            DetailsView.Heal.text = "Heal: " + actor.Config().BaseHealing().ToString();
            DetailsView.Helth.text = "Helth: " + health.ToString();
            DetailsView.Level.text = "Level: " + level.ToString();
            DetailsView.Dead.text = "Is Dead: " + isDead.ToString();
            DetailsView.AttackType.text = "Type: " + type.ToString();
            DetailsView.Factions.text = "Factions: " + factions.ToString();
        }
    }
}