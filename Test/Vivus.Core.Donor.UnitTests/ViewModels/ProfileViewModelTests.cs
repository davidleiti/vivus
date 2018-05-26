using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Vivus.Core.DataModels;
using Vivus.Core.Donor.ViewModels;
using Vivus.Core.UnitTests.Dependencies;
using Vivus.Core.UnitTests.Dependencies.UoW;
using Vivus.Core.ViewModels.Base;

namespace Vivus.Core.Donor.UnitTests.ViewModels
{
    class ProfileViewModelTests
    {
        private UnitOfWork unitOfWork;
        private Mock<IApllicationViewModel<Model.Donor>> appViewModel;
        private Core.DataModels.DispatcherWrapper dispatcherWrapper;
       // private Security security;
        private ParentPage parentPage;
        private ProfileViewModel viewModel;
    }
}
