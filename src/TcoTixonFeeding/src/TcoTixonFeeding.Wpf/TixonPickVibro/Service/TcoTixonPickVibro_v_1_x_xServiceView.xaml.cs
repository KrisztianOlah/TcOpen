﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TcOpen.Inxton.Wpf;

namespace TcoTixonFeeding
{

    public partial class TcoTixonPickVibro_v_1_x_xServiceView : UserControl
    {
        public TcoTixonPickVibro_v_1_x_xServiceView()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new TcoTixonPickVibro_v_1_x_xServiceViewModel();
            }

            InitializeComponent();
        }


       

    }

  

}
