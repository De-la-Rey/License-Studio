using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MernokPasswords
{
    public class PasswordCreator : INotifyPropertyChanged
    {
        #region OnProperty Changed
        /////////////////////////////////////////////////////////////
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /////////////////////////////////////////////////////////////
        #endregion


        public enum AccessLevel_enum
        {
            Operator = 'O',
            Temporary_Operator = 'T',
            Trainee_Operator = 'R',
            Training_Officer = 'C',                     //- Only this person can issue licenses!!
            Maintenance = 'M',
            Electrician = 'E',
            Foreman = 'F',
            Shift_Boss = 'S',
            Engineer = 'G',
            Field_Technician = 'H',                     //- Password required
            Mernok_Technician = 'X',                    //- Password required
            Mernok_Field_Technician = 'Y',              //- Password required
            Mernok_Engineer = 'Z'                       //- Password required
        };

        private string requestName;
        public string RequestName
        {
            get
            {
                return requestName;
            }

            set
            {
                requestName = StringConditioner(value);
                OnPropertyChanged("RequestName");
            }
        }

        private uint _requesterUID;

        public uint RequesterUID
        {
            get { return _requesterUID; }
            set
            {
                _requesterUID = value;
                OnPropertyChanged("RequesterUID");
            }
        }


        private string _CreatorName;

        public string CreatorName
        {
            get { return _CreatorName; }
            set
            {
                _CreatorName = StringConditioner(value);
                OnPropertyChanged("CreatorName");
            }
        }

        private uint _creatorUID;

        public uint CreatorUID
        {
            get { return _creatorUID; }
            set
            {
                _creatorUID = value;
                OnPropertyChanged("CreatorUID");
            }
        }

        private AccessLevel_enum _AccessLevel;

        public AccessLevel_enum AccessLevel
        {
            get { return _AccessLevel; }
            set { _AccessLevel = value; OnPropertyChanged("AccessLevel"); }
        }

        private string _PassWordOut;

        public string PassWordOut
        {
            get { return _PassWordOut; }
            set
            {
                _PassWordOut = value;
                OnPropertyChanged("PassWordOut");
            }
        }

        public string PassWordCreate(uint UIDr, string Requester, uint UIDc, string Creator)
        {
            string Password = "";
            Password = UIDr.ToString("X") + "$" + Requester + "%" + Creator + "#" + UIDc.ToString("X") + ">" + AccessLevel.ToString();
            byte[] ba = Encoding.Default.GetBytes(Password);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        public string StringConditioner(string value)
        {
            string string2 = value;
            string[] name2;
            Regex regexItem = new Regex(@"[^A-Z0-9 _]");
            if (string2.Length <= 15)
            {
                string2 = string2.ToUpper();
            }
            else
            {
                string2 = string2.Substring(0, 15).ToUpper();
 //               MessageBox.Show("Tag name my not exceed a length of 15");
            }


            if (!regexItem.IsMatch(string2))
            {
                value = string2;
            }
            else
            {
//                MessageBox.Show("Tag name my not not contain any special charcters");
                name2 = regexItem.Split(string2);
                string2 = "";
                for (int i = 0; i < name2.Length; i++)
                {
                    if (name2[i] != "")
                    {
                        string2 = string2 + name2[i];
                    }

                }
                //name = regexItem.Replace(name, "$");
                value = string2;
            }

            return value;
        }
    }
}


