﻿

#pragma checksum "C:\Users\Diego Reyes\documents\visual studio 2013\Projects\EncuentraTuPromedioUniversal\EncuentraTuPromedio\EncuentraTuPromedio.WindowsPhone\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BC9BAF500F0B398F0FC9305280B8DE89"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EncuentraTuPromedio
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 150 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.StackPanel_Holding;
                 #line default
                 #line hidden
                #line 150 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.llcursoItem_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 154 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MFI_EdicionCurso_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 155 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MFI_EliminarCurso_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 309 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnbar_añadircurso_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 310 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnbar_acerca_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 312 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.menu_configuracionCiclo_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 313 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.menu_feedback_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


