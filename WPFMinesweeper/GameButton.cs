using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace WPFMinesweeper
{
    
    
    public class GameButton : Button
    {        
        private int id;        
        public int isButtonOpened; 

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
                
        public GameButton(int id)
        {
            Id = id;
            isButtonOpened = 0;
        }
                
    }

}
