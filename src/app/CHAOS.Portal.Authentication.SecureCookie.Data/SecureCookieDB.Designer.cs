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

[assembly: EdmSchemaAttribute()]
namespace CHAOS.Portal.Authentication.SecureCookie.Data
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class SecureCookieEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new SecureCookieEntities object using the connection string found in the 'SecureCookieEntities' section of the application configuration file.
        /// </summary>
        public SecureCookieEntities() : base("name=SecureCookieEntities", "SecureCookieEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new SecureCookieEntities object.
        /// </summary>
        public SecureCookieEntities(string connectionString) : base(connectionString, "SecureCookieEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new SecureCookieEntities object.
        /// </summary>
        public SecureCookieEntities(EntityConnection connection) : base(connection, "SecureCookieEntities")
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
        public ObjectSet<SecureCookie> SecureCookie
        {
            get
            {
                if ((_SecureCookie == null))
                {
                    _SecureCookie = base.CreateObjectSet<SecureCookie>("SecureCookie");
                }
                return _SecureCookie;
            }
        }
        private ObjectSet<SecureCookie> _SecureCookie;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the SecureCookie EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToSecureCookie(SecureCookie secureCookie)
        {
            base.AddObject("SecureCookie", secureCookie);
        }

        #endregion

        #region Function Imports
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="secureCookieGUID">No Metadata Documentation available.</param>
        /// <param name="passwordGUID">No Metadata Documentation available.</param>
        /// <param name="sessionGUID">No Metadata Documentation available.</param>
        public int SecureCookie_Create(global::System.Byte[] userGUID, global::System.Byte[] secureCookieGUID, global::System.Byte[] passwordGUID, global::System.Byte[] sessionGUID)
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
    
            ObjectParameter secureCookieGUIDParameter;
            if (secureCookieGUID != null)
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", secureCookieGUID);
            }
            else
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordGUIDParameter;
            if (passwordGUID != null)
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", passwordGUID);
            }
            else
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter sessionGUIDParameter;
            if (sessionGUID != null)
            {
                sessionGUIDParameter = new ObjectParameter("SessionGUID", sessionGUID);
            }
            else
            {
                sessionGUIDParameter = new ObjectParameter("SessionGUID", typeof(global::System.Byte[]));
            }
    
            return base.ExecuteFunction("SecureCookie_Create", userGUIDParameter, secureCookieGUIDParameter, passwordGUIDParameter, sessionGUIDParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="whereUserGUID">No Metadata Documentation available.</param>
        /// <param name="whereSecureCookieGUID">No Metadata Documentation available.</param>
        public int SecureCookie_Delete(global::System.Byte[] whereUserGUID, global::System.Byte[] whereSecureCookieGUID)
        {
            ObjectParameter whereUserGUIDParameter;
            if (whereUserGUID != null)
            {
                whereUserGUIDParameter = new ObjectParameter("WhereUserGUID", whereUserGUID);
            }
            else
            {
                whereUserGUIDParameter = new ObjectParameter("WhereUserGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter whereSecureCookieGUIDParameter;
            if (whereSecureCookieGUID != null)
            {
                whereSecureCookieGUIDParameter = new ObjectParameter("WhereSecureCookieGUID", whereSecureCookieGUID);
            }
            else
            {
                whereSecureCookieGUIDParameter = new ObjectParameter("WhereSecureCookieGUID", typeof(global::System.Byte[]));
            }
    
            return base.ExecuteFunction("SecureCookie_Delete", whereUserGUIDParameter, whereSecureCookieGUIDParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="secureCookieGUID">No Metadata Documentation available.</param>
        /// <param name="passwordGUID">No Metadata Documentation available.</param>
        public ObjectResult<SecureCookie> SecureCookie_Get(global::System.Byte[] userGUID, global::System.Byte[] secureCookieGUID, global::System.Byte[] passwordGUID)
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
    
            ObjectParameter secureCookieGUIDParameter;
            if (secureCookieGUID != null)
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", secureCookieGUID);
            }
            else
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordGUIDParameter;
            if (passwordGUID != null)
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", passwordGUID);
            }
            else
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", typeof(global::System.Byte[]));
            }
    
            return base.ExecuteFunction<SecureCookie>("SecureCookie_Get", userGUIDParameter, secureCookieGUIDParameter, passwordGUIDParameter);
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="mergeOption"></param>
        /// <param name="userGUID">No Metadata Documentation available.</param>
        /// <param name="secureCookieGUID">No Metadata Documentation available.</param>
        /// <param name="passwordGUID">No Metadata Documentation available.</param>
        public ObjectResult<SecureCookie> SecureCookie_Get(global::System.Byte[] userGUID, global::System.Byte[] secureCookieGUID, global::System.Byte[] passwordGUID, MergeOption mergeOption)
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
    
            ObjectParameter secureCookieGUIDParameter;
            if (secureCookieGUID != null)
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", secureCookieGUID);
            }
            else
            {
                secureCookieGUIDParameter = new ObjectParameter("SecureCookieGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter passwordGUIDParameter;
            if (passwordGUID != null)
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", passwordGUID);
            }
            else
            {
                passwordGUIDParameter = new ObjectParameter("PasswordGUID", typeof(global::System.Byte[]));
            }
    
            return base.ExecuteFunction<SecureCookie>("SecureCookie_Get", mergeOption, userGUIDParameter, secureCookieGUIDParameter, passwordGUIDParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="whereUserGUID">No Metadata Documentation available.</param>
        /// <param name="whereSecureCookieGUID">No Metadata Documentation available.</param>
        /// <param name="wherePasswordGUID">No Metadata Documentation available.</param>
        public int SecureCookie_Update(global::System.Byte[] whereUserGUID, global::System.Byte[] whereSecureCookieGUID, global::System.Byte[] wherePasswordGUID)
        {
            ObjectParameter whereUserGUIDParameter;
            if (whereUserGUID != null)
            {
                whereUserGUIDParameter = new ObjectParameter("WhereUserGUID", whereUserGUID);
            }
            else
            {
                whereUserGUIDParameter = new ObjectParameter("WhereUserGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter whereSecureCookieGUIDParameter;
            if (whereSecureCookieGUID != null)
            {
                whereSecureCookieGUIDParameter = new ObjectParameter("WhereSecureCookieGUID", whereSecureCookieGUID);
            }
            else
            {
                whereSecureCookieGUIDParameter = new ObjectParameter("WhereSecureCookieGUID", typeof(global::System.Byte[]));
            }
    
            ObjectParameter wherePasswordGUIDParameter;
            if (wherePasswordGUID != null)
            {
                wherePasswordGUIDParameter = new ObjectParameter("WherePasswordGUID", wherePasswordGUID);
            }
            else
            {
                wherePasswordGUIDParameter = new ObjectParameter("WherePasswordGUID", typeof(global::System.Byte[]));
            }
    
            return base.ExecuteFunction("SecureCookie_Update", whereUserGUIDParameter, whereSecureCookieGUIDParameter, wherePasswordGUIDParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public int PreTest()
        {
            return base.ExecuteFunction("PreTest");
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SecureCookieModel", Name="SecureCookie")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class SecureCookie : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new SecureCookie object.
        /// </summary>
        /// <param name="userGUID">Initial value of the UserGUID property.</param>
        /// <param name="secureCookieGUID">Initial value of the SecureCookieGUID property.</param>
        /// <param name="passwordGUID">Initial value of the PasswordGUID property.</param>
        /// <param name="sessionGUID">Initial value of the SessionGUID property.</param>
        /// <param name="dateCreated">Initial value of the DateCreated property.</param>
        public static SecureCookie CreateSecureCookie(global::System.Guid userGUID, global::System.Guid secureCookieGUID, global::System.Guid passwordGUID, global::System.Guid sessionGUID, global::System.DateTime dateCreated)
        {
            SecureCookie secureCookie = new SecureCookie();
            secureCookie.UserGUID = userGUID;
            secureCookie.SecureCookieGUID = secureCookieGUID;
            secureCookie.PasswordGUID = passwordGUID;
            secureCookie.SessionGUID = sessionGUID;
            secureCookie.DateCreated = dateCreated;
            return secureCookie;
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
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid SecureCookieGUID
        {
            get
            {
                return _SecureCookieGUID;
            }
            set
            {
                if (_SecureCookieGUID != value)
                {
                    OnSecureCookieGUIDChanging(value);
                    ReportPropertyChanging("SecureCookieGUID");
                    _SecureCookieGUID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("SecureCookieGUID");
                    OnSecureCookieGUIDChanged();
                }
            }
        }
        private global::System.Guid _SecureCookieGUID;
        partial void OnSecureCookieGUIDChanging(global::System.Guid value);
        partial void OnSecureCookieGUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid PasswordGUID
        {
            get
            {
                return _PasswordGUID;
            }
            set
            {
                if (_PasswordGUID != value)
                {
                    OnPasswordGUIDChanging(value);
                    ReportPropertyChanging("PasswordGUID");
                    _PasswordGUID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("PasswordGUID");
                    OnPasswordGUIDChanged();
                }
            }
        }
        private global::System.Guid _PasswordGUID;
        partial void OnPasswordGUIDChanging(global::System.Guid value);
        partial void OnPasswordGUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid SessionGUID
        {
            get
            {
                return _SessionGUID;
            }
            set
            {
                OnSessionGUIDChanging(value);
                ReportPropertyChanging("SessionGUID");
                _SessionGUID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("SessionGUID");
                OnSessionGUIDChanged();
            }
        }
        private global::System.Guid _SessionGUID;
        partial void OnSessionGUIDChanging(global::System.Guid value);
        partial void OnSessionGUIDChanged();
    
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
        public Nullable<global::System.DateTime> DateUsed
        {
            get
            {
                return _DateUsed;
            }
            set
            {
                OnDateUsedChanging(value);
                ReportPropertyChanging("DateUsed");
                _DateUsed = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateUsed");
                OnDateUsedChanged();
            }
        }
        private Nullable<global::System.DateTime> _DateUsed;
        partial void OnDateUsedChanging(Nullable<global::System.DateTime> value);
        partial void OnDateUsedChanged();

        #endregion

    
    }

    #endregion

    
}
