﻿#pragma checksum "..\..\SimWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B206A6CF00D35703AEF7A8036BF647388095223AB96A270C34DB2C4B6CD69432"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome.WPF;
using FontAwesome.WPF.Converters;
using HelloApp1;
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


namespace HelloApp1 {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FontAwesome.WPF.ImageAwesome Spinner;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LEjecutando;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_PasoAPaso;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Todo;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_P;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tTiempoS;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_SiguientePaso;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvDataBinding;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas miLienzo;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label tSimu;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button mostrarConfig;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\SimWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock info_adicional;
        
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
            System.Uri resourceLocater = new System.Uri("/HelloApp1;component/simwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SimWindow.xaml"
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
            this.Spinner = ((FontAwesome.WPF.ImageAwesome)(target));
            return;
            case 2:
            this.LEjecutando = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.btn_PasoAPaso = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\SimWindow.xaml"
            this.btn_PasoAPaso.Click += new System.Windows.RoutedEventHandler(this.Button_Click_PasoAPaso);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Todo = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\SimWindow.xaml"
            this.btn_Todo.Click += new System.Windows.RoutedEventHandler(this.Button_Click_Todo);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_P = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\SimWindow.xaml"
            this.btn_P.Click += new System.Windows.RoutedEventHandler(this.btn_P_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tTiempoS = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\SimWindow.xaml"
            this.tTiempoS.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btn_SiguientePaso = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\SimWindow.xaml"
            this.btn_SiguientePaso.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lvDataBinding = ((System.Windows.Controls.ListView)(target));
            return;
            case 9:
            this.miLienzo = ((System.Windows.Controls.Canvas)(target));
            return;
            case 10:
            this.tSimu = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.mostrarConfig = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\SimWindow.xaml"
            this.mostrarConfig.Click += new System.Windows.RoutedEventHandler(this.Button_Click_mostrarConfig);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 60 "..\..\SimWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_izq);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 63 "..\..\SimWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_der);
            
            #line default
            #line hidden
            return;
            case 14:
            this.info_adicional = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

