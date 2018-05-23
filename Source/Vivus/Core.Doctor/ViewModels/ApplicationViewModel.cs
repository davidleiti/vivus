using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivus.Core.Doctor.IoC;
using Vivus.Core.ViewModels;
using Vivus.Core.ViewModels.Base;

namespace Vivus.Core.Doctor.ViewModels
{
    class ApplicationViewModel : BaseViewModel, IApllicationViewModel<Model.Doctor>
    {
        #region Private Members

        private Model.Doctor doctor;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current logged in administrator.
        /// </summary>
        public Model.Doctor User
        {
            get
            {
                if (IoCContainer.Get<WindowViewModel>().CurrentPage == DataModels.ApplicationPage.Login)
                    return null;

                return doctor;
            }

            set
            {
                if (doctor?.PersonID == value?.PersonID)
                    return;

                doctor = new Model.Doctor
                {
                    PersonID = value.PersonID
                };

                OnPropertyChanged();
            }
        }

        #endregion

    }
}
