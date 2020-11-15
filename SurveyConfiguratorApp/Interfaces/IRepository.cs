namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support  Create , Read,  Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface IRepository<T> : ICUDable<T>, IQueryable<T> where T : class
    {
    }
}
