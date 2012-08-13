﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace CHAOS.Portal.Authentication.EmailPassword.Data
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class EmailPasswordEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new EmailPasswordEntities object using the connection string found in the 'EmailPasswordEntities' section of the application configuration file.
        /// </summary>
        public EmailPasswordEntities() : base("name=EmailPasswordEntities", "EmailPasswordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new EmailPasswordEntities object.
        /// </summary>
        public EmailPasswordEntities(string connectionString) : base(connectionString, "EmailPasswordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new EmailPasswordEntities object.
        /// </summary>
        public EmailPasswordEntities(EntityConnection connection) : base(connection, "EmailPasswordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<EmailPassword> EmailPassword
        {
            get
            {
                if ((_EmailPassword == null))
                {
                    _EmailPassword = base.CreateObjectSet<EmailPassword>("EmailPassword");
                }
                return _EmailPassword;
            }
        }
        private ObjectSet<EmailPassword> _EmailPassword;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the EmailPassword EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToEmailPassword(EmailPassword emailPassword)
        {
            base.AddObject("EmailPassword", emailPassword);
        }

        #endregion

        #region Function Imports
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="password">No Metadata Documentation available.</param>
        public ObjectResult<EmailPassword> EmailPassword_Get(global::System.Byte[] userGUID, global::System.String password)
        {
            ObjectParameter userGUIDParameter;
            if (userGUID != null)
            {
                userGUIDParameter = new ObjectParameter("UserGUID", userGUID);
            }
            else
            {
                userGUIDParameter = new ObjectParameter("UserGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordParameter;
            if (password != null)
            {
                passwordParameter = new ObjectParameter("Password", password);
            }
            else
            {
                passwordParameter = new ObjectParameter("Password", typeof(global::System.String));
            }
    
            return base.ExecuteFunction<EmailPassword>("EmailPassword_Get", userGUIDParameter, passwordParameter);
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="mergeOption"></param>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="password">No Metadata Documentation available.</param>
        public ObjectResult<EmailPassword> EmailPassword_Get(global::System.Byte[] userGUID, global::System.String password, MergeOption mergeOption)
        {
            ObjectParameter userGUIDParameter;
            if (userGUID != null)
            {
                userGUIDParameter = new ObjectParameter("UserGUID", userGUID);
            }
            else
            {
                userGUIDParameter = new ObjectParameter("UserGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordParameter;
            if (password != null)
            {
                passwordParameter = new ObjectParameter("Password", password);
            }
            else
            {
                passwordParameter = new ObjectParameter("Password", typeof(global::System.String));
            }
    
            return base.ExecuteFunction<EmailPassword>("EmailPassword_Get", mergeOption, userGUIDParameter, passwordParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public int PreTest()
        {
            return base.ExecuteFunction("PreTest");
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="password">No Metadata Documentation available.</param>
        public int EmailPassword_Create(global::System.Byte[] userGUID, global::System.String password)
        {
            ObjectParameter userGUIDParameter;
            if (userGUID != null)
            {
                userGUIDParameter = new ObjectParameter("UserGUID", userGUID);
            }
            else
            {
                userGUIDParameter = new ObjectParameter("UserGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordParameter;
            if (password != null)
            {
                passwordParameter = new ObjectParameter("Password", password);
            }
            else
            {
                passwordParameter = new ObjectParameter("Password", typeof(global::System.String));
            }
    
            return base.ExecuteFunction("EmailPassword_Create", userGUIDParameter, passwordParameter);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="EmailPasswordModel", Name="EmailPassword")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class EmailPassword : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new EmailPassword object.
        /// </summary>
        /// <param name="userGUID">Initial value of the UserGUID property.</param>
        /// <param name="password">Initial value of the Password property.</param>
        /// <param name="dateCreated">Initial value of the DateCreated property.</param>
        public static EmailPassword CreateEmailPassword(global::System.Guid userGUID, global::System.String password, global::System.DateTime dateCreated)
        {
            EmailPassword emailPassword = new EmailPassword();
            emailPassword.UserGUID = userGUID;
            emailPassword.Password = password;
            emailPassword.DateCreated = dateCreated;
            return emailPassword;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid UserGUID
        {
            get
            {
                return _UserGUID;
            }
            set
            {
                if (_UserGUID != value)
                {
                    OnUserGUIDChanging(value);
                    ReportPropertyChanging("UserGUID");
                    _UserGUID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UserGUID");
                    OnUserGUIDChanged();
                }
            }
        }
        private global::System.Guid _UserGUID;
        partial void OnUserGUIDChanging(global::System.Guid value);
        partial void OnUserGUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime DateCreated
        {
            get
            {
                return _DateCreated;
            }
            set
            {
                OnDateCreatedChanging(value);
                ReportPropertyChanging("DateCreated");
                _DateCreated = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateCreated");
                OnDateCreatedChanged();
            }
        }
        private global::System.DateTime _DateCreated;
        partial void OnDateCreatedChanging(global::System.DateTime value);
        partial void OnDateCreatedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> DateModified
        {
            get
            {
                return _DateModified;
            }
            set
            {
                OnDateModifiedChanging(value);
                ReportPropertyChanging("DateModified");
                _DateModified = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateModified");
                OnDateModifiedChanged();
            }
        }
        private Nullable<global::System.DateTime> _DateModified;
        partial void OnDateModifiedChanging(Nullable<global::System.DateTime> value);
        partial void OnDateModifiedChanged();

        #endregion

    
    }

    #endregion

    
}
