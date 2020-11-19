namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support  Create , Read,  Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface ICRUDable<T> : IReadable<T>, ICUDable<T> where T : class
    {
    }
}
