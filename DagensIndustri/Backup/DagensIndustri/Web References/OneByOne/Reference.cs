﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace DagensIndustri.OneByOne {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DocumentFactorySoapBinding", Namespace="http://obo.par.se/jboss-net/services/DocumentFactory")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RequestParameter))]
    public partial class DocumentFactoryService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getSchemaOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDocumentOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDocumentTypeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public DocumentFactoryService() {
            this.Url = global::DagensIndustri.Properties.Settings.Default.DagensIndustri_OneByOne_DocumentFactoryService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event getSchemaCompletedEventHandler getSchemaCompleted;
        
        /// <remarks/>
        public event getDocumentCompletedEventHandler getDocumentCompleted;
        
        /// <remarks/>
        public event getDocumentTypeCompletedEventHandler getDocumentTypeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("DocumentFactory", RequestNamespace="http://obo.par.se/jboss-net/services/DocumentFactory", ResponseNamespace="http://obo.par.se/jboss-net/services/DocumentFactory")]
        [return: System.Xml.Serialization.SoapElementAttribute("getSchemaReturn")]
        public string getSchema(string documentName) {
            object[] results = this.Invoke("getSchema", new object[] {
                        documentName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getSchemaAsync(string documentName) {
            this.getSchemaAsync(documentName, null);
        }
        
        /// <remarks/>
        public void getSchemaAsync(string documentName, object userState) {
            if ((this.getSchemaOperationCompleted == null)) {
                this.getSchemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSchemaOperationCompleted);
            }
            this.InvokeAsync("getSchema", new object[] {
                        documentName}, this.getSchemaOperationCompleted, userState);
        }
        
        private void OngetSchemaOperationCompleted(object arg) {
            if ((this.getSchemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSchemaCompleted(this, new getSchemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("DocumentFactory", RequestNamespace="http://obo.par.se/jboss-net/services/DocumentFactory", ResponseNamespace="http://obo.par.se/jboss-net/services/DocumentFactory")]
        [return: System.Xml.Serialization.SoapElementAttribute("getDocumentReturn")]
        public string getDocument(string name, RequestParameter[] @params) {
            object[] results = this.Invoke("getDocument", new object[] {
                        name,
                        @params});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getDocumentAsync(string name, RequestParameter[] @params) {
            this.getDocumentAsync(name, @params, null);
        }
        
        /// <remarks/>
        public void getDocumentAsync(string name, RequestParameter[] @params, object userState) {
            if ((this.getDocumentOperationCompleted == null)) {
                this.getDocumentOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDocumentOperationCompleted);
            }
            this.InvokeAsync("getDocument", new object[] {
                        name,
                        @params}, this.getDocumentOperationCompleted, userState);
        }
        
        private void OngetDocumentOperationCompleted(object arg) {
            if ((this.getDocumentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDocumentCompleted(this, new getDocumentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("DocumentFactory", RequestNamespace="http://obo.par.se/jboss-net/services/DocumentFactory", ResponseNamespace="http://obo.par.se/jboss-net/services/DocumentFactory")]
        [return: System.Xml.Serialization.SoapElementAttribute("getDocumentTypeReturn")]
        public string getDocumentType(string name) {
            object[] results = this.Invoke("getDocumentType", new object[] {
                        name});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getDocumentTypeAsync(string name) {
            this.getDocumentTypeAsync(name, null);
        }
        
        /// <remarks/>
        public void getDocumentTypeAsync(string name, object userState) {
            if ((this.getDocumentTypeOperationCompleted == null)) {
                this.getDocumentTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDocumentTypeOperationCompleted);
            }
            this.InvokeAsync("getDocumentType", new object[] {
                        name}, this.getDocumentTypeOperationCompleted, userState);
        }
        
        private void OngetDocumentTypeOperationCompleted(object arg) {
            if ((this.getDocumentTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDocumentTypeCompleted(this, new getDocumentTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://extranet.par.se/obows")]
    public partial class RequestParameter {
        
        private string nameField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getSchemaCompletedEventHandler(object sender, getSchemaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSchemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSchemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDocumentCompletedEventHandler(object sender, getDocumentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDocumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDocumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDocumentTypeCompletedEventHandler(object sender, getDocumentTypeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDocumentTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDocumentTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591