using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Creatures.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Creatures.Controllers
{
    public class InteractionController : AController
    {
        [SerializeField] private Creature creature;
        [SerializeField] private CircleOverlapCheckComponent interactionChecker;
        
        private SessionController _sessionController;
        private string _currentLevel;

        private void Start()
        {
            _currentLevel = SceneManager.GetActiveScene().name;

            _sessionController = creature.SessionController;
            if (_sessionController == null) return;
            
            var lps = _sessionController.GetModel().lastPositions;
            var index = lps.FindIndex(item => item.levelName == _currentLevel);
            if (index < 0)
            {
                CreatureModel.LevelPosition levelPosition;
                levelPosition.levelName = _currentLevel;
                levelPosition.levelPosition = creature.Transform.position;
                lps.Add(levelPosition);
            }

            LoadFromSession();
        }

        private void LoadFromSession()
        {
            if (_sessionController == null) return;
            
            var lp = _sessionController.GetModel().lastPositions.Find(item => item.levelName == _currentLevel);
            creature.Transform.position = lp.levelPosition;
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;
            
            var index = _sessionController.GetModel().lastPositions.FindIndex(item => item.levelName == _currentLevel);
            CreatureModel.LevelPosition lp;
            lp.levelName = _currentLevel;
            lp.levelPosition = creature.Transform.position;
            _sessionController.GetModel().lastPositions[index] = lp;
        }
        
        public void Interact()
        {
            if (interactionChecker == null) return;

            var gos = interactionChecker.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var interactable = item.GetComponent<InteractableComponent>();
                if (interactable == null) continue;
                
                SaveToSession();
                
                interactable.Interact(creature.gameObject);
            }
        }

        protected override Creature GetCreature()
        {
            return creature;
        }
    }
}