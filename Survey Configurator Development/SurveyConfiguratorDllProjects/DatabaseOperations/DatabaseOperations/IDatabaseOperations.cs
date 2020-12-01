namespace SurveyConfiguratorApp
{
    /// <summary>
    /// Interface to support  Create , Read,  Update , Delete behavior on repository 
    /// </summary>
    /// <typeparam name="T">The object to be managed</typeparam>
    public interface IDatabaseOperations<T> : IDatabaseSelectable<T>, IDatabaseProcessable<T> where T : BaseQuestion
    {

    }
}
