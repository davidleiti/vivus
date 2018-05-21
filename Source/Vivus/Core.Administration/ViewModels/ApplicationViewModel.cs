namespace Vivus.Core.Administration.ViewModels
{
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Model;
    using Vivus.Core.ViewModels;

    /// <summary>
    /// Represents a view model for the application.
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        private Administrator admin;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current logged in administrator.
        /// </summary>
        public Administrator Administrator
        {
            get
            {
                if (IoCContainer.Get<WindowViewModel>().CurrentPage == DataModels.ApplicationPage.Login)
                    return null;

                return admin;
            }

            set
            {
                if (admin?.PersonID == value?.PersonID)
                    return;

                admin = new Administrator
                {
                    PersonID = value.PersonID,
                    IsOwner = value.IsOwner,
                    Active = value.Active
                };

                OnPropertyChanged();
            }
        }

        #endregion
    }
}
