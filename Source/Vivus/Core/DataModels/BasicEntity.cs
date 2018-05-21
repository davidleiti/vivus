namespace Vivus.Core.DataModels
{
    /// <summary>
    /// Represents a basic entity that has an identificator.
    /// </summary>
    /// <typeparam name="Type">The type of the entity</typeparam>
    public class BasicEntity<Type>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the identificator of the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The value of the entity.
        /// </summary>
        public Type Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEntity{Type}"/> class with the given values.
        /// </summary>
        /// <param name="id">The identificator of the entity.</param>
        /// <param name="value">The value of the entity.</param>
        public BasicEntity(int id, Type value)
        {
            Id = id;
            Value = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a string that represents the current <see cref="BasicEntity{Type}"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="BasicEntity{Type}"/>.
        /// </summary>
        /// <param name="obj">The object o compare with the current <see cref="BasicEntity{Type}"/>.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // If the object is null or has a different type
            if (obj is null || obj.GetType() != GetType())
                return false;

            BasicEntity<Type> be = obj as BasicEntity<Type>;

            return Value.Equals(be.Value);
        }

        /// <summary>
        /// Returns the hash code of the current <see cref="BasicEntity{Type}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion
    }
}
