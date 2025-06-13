namespace Localization
{
    public interface ILocalizable 
    {
        LocalizationData Data { get; set; }
        void Localize();
    }
}
