using PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Hero
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Hero hero;
        [SerializeField] private CheckCircleOverlap interactionPosition;
        
        private GameSession _gameSession;
        private string _currentLevel;

        private void Awake()
        {
            _currentLevel = SceneManager.GetActiveScene().name;
        }
        public void LinkGameSession(GameSession gameSession)
        {
            _gameSession = gameSession;
            
            var lps = _gameSession.Data.lastPositions;
            var index = lps.FindIndex(item => item.levelName == _currentLevel);
            if (index < 0)
            {
                HeroData.LevelPosition levelPosition;
                levelPosition.levelName = _currentLevel;
                levelPosition.levelPosition = hero.transform.position;
                lps.Add(levelPosition);
            }

            LoadFromSession();
        }

        private void LoadFromSession()
        {
            var lp = _gameSession.Data.lastPositions.Find(item => item.levelName == _currentLevel);
            hero.transform.position = lp.levelPosition;
        }

        private void SaveToSession()
        {
            var index = _gameSession.Data.lastPositions.FindIndex(item => item.levelName == _currentLevel);
            HeroData.LevelPosition lp;
            lp.levelName = _currentLevel;
            lp.levelPosition = hero.transform.position;
            _gameSession.Data.lastPositions[index] = lp;
        }
        
        public void Interact()
        {
            if (interactionPosition == null) return;

            var gos = interactionPosition.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var interactable = item.GetComponent<InteractableComponent>();
                if (interactable == null) continue;
                
                SaveToSession();
                
                interactable.Interact();
            }
        }
    }
}