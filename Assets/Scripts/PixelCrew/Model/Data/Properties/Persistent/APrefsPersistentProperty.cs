namespace PixelCrew.Model.Data.Properties.Persistent
{
    public abstract class APrefsPersistentProperty<TPropertyType> : APersistentProperty<TPropertyType>
    {
        protected string key;
        
        protected APrefsPersistentProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            this.key = key;
        }
    }
}