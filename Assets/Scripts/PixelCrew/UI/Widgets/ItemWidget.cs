using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Text value;

        public void Set(ItemWithCount itemWithCount)
        {
            var def = DefsFacade.I.Items.Get(itemWithCount.Id);
            Set(def.Icon, itemWithCount.Count);
        }

        public void Set(Sprite icon, string count)
        {
            image.sprite = icon;
            value.text = count;
        }

        public void Set(Sprite icon, int count)
        {
            Set(icon, count.ToString());
        }

        public void Set(string id, string count)
        {
            var def = DefsFacade.I.Items.Get(id);
            Set(def.Icon, count);
        }

        public void Set(string id, int count)
        {
            Set(id, count.ToString());
        }
    }
}