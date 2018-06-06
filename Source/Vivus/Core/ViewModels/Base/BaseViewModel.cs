﻿namespace Vivus.Core.ViewModels
{
    using Vivus.Core.DataModels;
    using Vivus.Core.Expressions;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a viewmodel that implements the basic behaviour.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Private Members

        private string popupMessage;
        private PopupType popupType;
        private IPage parentPage;

        #endregion

        #region Protected Members

        protected IDictionary<string, List<string>> errors;
        protected IDispatcherWrapper dispatcherWrapper;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the viewmodel.
        /// </summary>
        public IPage ParentPage
        {
            get => parentPage;

            set
            {
                if (parentPage == value)
                    return;

                parentPage = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current visibility of the popup.
        /// </summary>
        public bool ShowPopup
        {
            get => !string.IsNullOrEmpty(popupMessage) && popupMessage.Length > 0;
        }

        /// <summary>
        /// Gets or sets the type of the popup.
        /// </summary>
        public PopupType PopupType
        {
            get => popupType;

            set
            {
                if (popupType == value)
                    return;

                popupType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message of the popup.
        /// </summary>
        public string PopupMessage
        {
            get => popupMessage;

            set
            {
                if (popupMessage == value)
                    return;

                popupMessage = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowPopup));
            }
        }

        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public virtual string this[string propertyName] => throw new NotImplementedException();

        /// <summary>
        /// Gets the number of fields that need to be validated.
        /// </summary>
        public virtual int Errors => errors.Count;

        /// <summary>
        /// Gets a string containing all the errors.
        /// </summary>
        //public string Error => string.Join("\n", errors.Values);
        public string Error
        {
            get {
                string errs = "";

                for (int i = 0; i < errors.Values.Count; i++)
                {
                    for (int j = 0; j < errors.Values.ToArray()[i].Count; j++)
                    {
                        errs += "\n" + errors.Values.ToArray()[i][j];
                    }
                }

                return errs;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class with the given values.
        /// </summary>
        public BaseViewModel(IDispatcherWrapper dispatcherWrapper = null)
        {
            errors = new Dictionary<string, List<string>>();
            this.dispatcherWrapper = dispatcherWrapper;
        }

        #endregion

        #region Public Event Handlers

        /// <summary>
        /// Represents an <see cref="PropertyChanged"/> event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event handler.
        /// </summary>
        /// <param name="property">The name of the caller.</param>
        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Shows a popup on the screen.
        /// </summary>
        /// <param name="message">The message of the popup.</param>
        /// <param name="popupType">The type of the popup.</param>
        public void Popup(string message, PopupType popupType = PopupType.Error)
        {
            // Make sure this code is run on the ui thread
            dispatcherWrapper.InvokeAsync(() =>
            {
                PopupMessage = null;
                PopupType = popupType;
                PopupMessage = message;
            });
        }

        #endregion

        #region Private/Protected Methods

        /// <summary>
        /// Converts a list of errors to a string.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns></returns>
        protected string GetErrorString(string propertyName, List<string> errors)
        {
            if (errors is null || errors.Count == 0)
            {
                this.errors.Remove(propertyName);
                return null;
            }

            this.errors[propertyName] = errors;
            return string.Join("\n", errors.ToArray());
        }

        /// <summary>
        /// Converts a list of not mandatory errors to a string.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns></returns>
        protected string GetNotMandatoryErrorString(string propertyName, List<string> errors)
        {
            if (errors is null)
            {
                this.errors.Remove(propertyName);
                return null;
            }
            
            return GetErrorString(propertyName, errors.Where(error => !error.Contains("mandatory")).ToList());
        }

        /// <summary>
        /// Runs a command if the updating flag is not set.
        /// If the flag is true (indicating the function is already running), then the action is not run.
        /// If the flag is false (indicating no running function), then the action is run.
        /// Once the action is finished if it was run, then the flag is reset to false.
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag defining if the command is already running.</param>
        /// <param name="action">The action to run of the command is not already running.</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Check if the flag property is true (meaning the function is already running)
            if (updatingFlag.GetPropertyValue())
                return;

            // Set the property flag to true to indicate we are running
            updatingFlag.SetPropertyValue(true);

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion
    }
}
