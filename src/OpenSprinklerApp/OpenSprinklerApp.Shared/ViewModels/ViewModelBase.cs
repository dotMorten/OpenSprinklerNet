using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
		protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
		{
			var handler = PropertyChanged;
			if(handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyname));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
