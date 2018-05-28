namespace Vivus.Core.DCPersonnel.ViewModels
{
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.Model;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;

    /// <summary>
    /// Represents a view model for the application.
    /// </summary>
    public class ApplicationViewModel : BaseViewModel, IApplicationViewModel<DCPersonnel>
    {
        #region Private Members

        private DCPersonnel personnel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current logged in administrator.
        /// </summary>
        public DCPersonnel User
        {
            get
            {
                if (IoCContainer.Get<WindowViewModel>().CurrentPage == DataModels.ApplicationPage.Login)
                    return null;

                return personnel;
            }

            set
            {
                if (personnel?.PersonID == value?.PersonID)
                    return;

                personnel = new DCPersonnel
                {
                    PersonID = value.PersonID
                };

                OnPropertyChanged();
            }
        }

        #endregion
    }
}
