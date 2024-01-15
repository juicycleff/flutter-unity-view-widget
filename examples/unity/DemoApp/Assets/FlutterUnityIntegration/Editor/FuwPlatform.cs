namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// Represents a builder for Fuw (SomeFramework) projects.
    /// </summary>
    public interface IFuwBuilder
    {
        /// The options for the FuwBuild class.
        /// /
        FuwBuildOptions Options { get; set; }

        /// <summary>
        /// Gets the build directory.
        /// </summary>
        /// <value>
        /// The build directory.
        /// </value>
        public string BuildDir { get; }

        /// <summary>
        /// Gets the output directory where the file will be saved.
        /// </summary>
        /// <value>
        /// The output directory as a string.
        /// </value>
        public string OutputDir { get; }

        /// <summary>
        /// Initializes a new instance of the IFuwBuilder interface.
        /// </summary>
        /// <returns>The initialized IFuwBuilder instance.</returns>
        public IFuwBuilder Init();

        /// <summary>
        /// Builds the object.
        /// </summary>
        /// <remarks>
        /// This method is used to construct the object and initialize its state.
        /// It should be called after all the necessary properties have been set.
        /// </remarks>
        public void Build();

        /// <summary>
        /// Exports the data.
        /// </summary>
        public void Export();
    }
}