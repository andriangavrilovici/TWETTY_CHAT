﻿#pragma checksum "..\..\..\..\Pages\SettingsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CA480716065D629B9154DC7E7BF45A809ED26649D569CF623CDE4E3E8DE70B6A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TWETTY_CHAT;
using TWETTY_CHAT.Core;


namespace TWETTY_CHAT {
    
    
    /// <summary>
    /// SettingsPage
    /// </summary>
    public partial class SettingsPage : TWETTY_CHAT.BasePage<TWETTY_CHAT.SettingsViewModel>, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\Pages\SettingsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TWETTY_CHAT.SettingsPage Page;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\..\..\Pages\SettingsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox CurrentPasswordText;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\..\..\Pages\SettingsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox NewPasswordText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CHAT By Gavrilovici;component/pages/settingspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\SettingsPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Page = ((TWETTY_CHAT.SettingsPage)(target));
            return;
            case 2:
            this.CurrentPasswordText = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 173 "..\..\..\..\Pages\SettingsPage.xaml"
            this.CurrentPasswordText.PasswordChanged += new System.Windows.RoutedEventHandler(this.CurrentPasswordText_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.NewPasswordText = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 182 "..\..\..\..\Pages\SettingsPage.xaml"
            this.NewPasswordText.PasswordChanged += new System.Windows.RoutedEventHandler(this.NewPasswordText_PasswordChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

