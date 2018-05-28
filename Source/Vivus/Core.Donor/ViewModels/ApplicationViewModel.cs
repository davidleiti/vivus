using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivus.Core.Donor.IoC;
using Vivus.Core.ViewModels;
using Vivus.Core.ViewModels.Base;

namespace Vivus.Core.Donor.ViewModels
{
    class ApplicationViewModel : BaseViewModel, IApplicationViewModel<Model.Donor>
    {
        #region Private Members

        private Model.Donor donor;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current logged in administrator.
        /// </summary>
        public Model.Donor User
        {
            get
            {
                if (IoCContainer.Get<WindowViewModel>().CurrentPage == DataModels.ApplicationPage.Login)
                    return null;

                return donor;
            }

            set
            {
                if (donor?.PersonID == value?.PersonID)
                    return;

                donor = new Model.Donor
                {
                    PersonID = value.PersonID
                };

                OnPropertyChanged();
            }
        }

        #endregion
    }
}

