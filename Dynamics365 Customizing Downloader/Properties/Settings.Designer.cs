﻿// <auto-generated/>
//-----------------------------------------------------------------------
// <copyright file="Settings.Designer.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = (Settings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings());
        
        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://api.github.com/repos/jhueppauff/Dynamics365-Customizing-Downloader")]
        public string GitHubAPIURL
        {
            get
            {
                return (string)this["GitHubAPIURL"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CheckForUpdates
        {
            get
            {
                return (bool)this["CheckForUpdates"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DisableErrorReports
        {
            get
            {
                return (bool)this["DisableErrorReports"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DebuggingEnabled
        {
            get
            {
                return (bool)this["DebuggingEnabled"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("en-us")]
        public string Culture
        {
            get
            {
                return (string)this["Culture"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("solutionPackager.log")]
        public string SolutionPackagerLogPath
        {
            get
            {
                return (string)this["SolutionPackagerLogPath"];
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("577567a3-fe74-4b16-88c1-aa0ad85c1b7b")]
        public string AppInsights
        {
            get
            {
                return (string)this["AppInsights"];
            }
        }
    }
}
