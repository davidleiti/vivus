namespace Vivus.Core.DataModels
{
    /// <summary>
    /// Represents an enumeration of finish states of an operation.
    /// </summary>
    public enum FinishState
    {
        /// <summary>
        /// The operation was not performed.
        /// </summary>
        Closed,

        /// <summary>
        /// The operation failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The operation succeded.
        /// </summary>
        Succeded
    }
}
