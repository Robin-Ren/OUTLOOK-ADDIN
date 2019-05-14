using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddIn.Domain
{
    public class Room : ABaseViewModel
    {
        private string _roomName;

        public string RoomName
        {
            get { return _roomName; }
            set
            {
                _roomName = value;
                OnPropertyChanged();
            }
        }
    }
}
