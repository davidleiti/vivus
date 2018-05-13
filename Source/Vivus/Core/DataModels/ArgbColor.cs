namespace Vivus.Core.DataModels
{
    /// <summary>
    /// Represents an ARGB color.
    /// </summary>
    public class ArgbColor
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the alpha channel of the color.
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red channel of the color.
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green channel of the color.
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue channel of the color.
        /// </summary>
        public byte B { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgbColor"/> class with the given values.
        /// </summary>
        /// <param name="a">The alpha channel of the color.</param>
        /// <param name="r">The red channel of the color.</param>
        /// <param name="g">The green channel of the color.</param>
        /// <param name="b">The blue channel of the color.</param>
        public ArgbColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        #endregion
    }
}
