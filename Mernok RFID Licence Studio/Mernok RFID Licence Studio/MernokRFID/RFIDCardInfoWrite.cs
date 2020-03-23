using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RFID;

namespace Mernok_RFID_Licence_Studio
{
    public class RFIDCardInfoWrite
    {
        CardDetails cardDetailsW = new CardDetails();

        public byte[] DateTimetoByte(DateTime dateTime)
        {
            byte[] Date = new byte[3];
            String year = dateTime.Year.ToString();
            year = year.Remove(0, 2);
            String month = dateTime.Month.ToString();
            String day = dateTime.Day.ToString();
            Date[0] = Convert.ToByte(year, 16);
            Date[1] = Convert.ToByte(month, 16);
            Date[2] = Convert.ToByte(day, 16);

            return Date;
        }

        public byte[] NameTo16Bytes(string Name)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(Name);

            return temp;
        }

        public byte[] Block1(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];

            //temp[0] = (byte)(RFIDCardInfoRead.AccessLevel_enum)Enum.Parse(typeof(RFIDCardInfoRead.AccessLevel_enum), WriteCardDetails.AccessLevel);
            temp[0] = Convert.ToByte(WriteCardDetails.AccessLevel);
            if(WriteCardDetails.Warning_Date.Year > 1000)
            {
                temp[1] = Convert.ToByte(WriteCardDetails.Warning_Date.Year.ToString().Remove(0, 2), 16);
                temp[2] = Convert.ToByte(WriteCardDetails.Warning_Date.Month.ToString(), 16);
                temp[3] = Convert.ToByte(WriteCardDetails.Warning_Date.Day.ToString(), 16);
            }                
            else
            {
                temp[1] = 0;
                temp[2] = 0;
                temp[3] = 0;
            }
                

            
            if (WriteCardDetails.Expiry_Date.Year > 1000)
            {
                temp[4] = Convert.ToByte(WriteCardDetails.Expiry_Date.Year.ToString().Remove(0, 2), 16);
                temp[5] = Convert.ToByte(WriteCardDetails.Expiry_Date.Month.ToString(), 16);
                temp[6] = Convert.ToByte(WriteCardDetails.Expiry_Date.Day.ToString(), 16);
            }               
            else
            {
                temp[4] = 0;
                temp[5] = 0;
                temp[6] = 0;
            }
                
            
            byte[] clientG = new byte[2];
            clientG = BitConverter.GetBytes(WriteCardDetails.Client_Group);
            temp[7] = clientG[1];
            temp[8] = clientG[0];

            byte[] clientC = new byte[2];
            clientC = BitConverter.GetBytes(WriteCardDetails.Client_Site);
            temp[9] = clientC[1];
            temp[10] = clientC[0];
            //ToDo: add cliient site here to write
            //           temp[10] = 2;

            byte[] OperationS= new byte[4];
            OperationS = BitConverter.GetBytes(WriteCardDetails.OperationalArea);

            temp[11] = OperationS[1];
            temp[12] = OperationS[0];
            //temp[11] = Convert.ToByte(WriteCardDetails.OpperationalArea); //ToDo: add operational bit here to write
            temp[13] = Convert.ToByte(WriteCardDetails.ByPassBits);
            temp[14] = Convert.ToByte(WriteCardDetails.Options);
            temp[15] = Convert.ToByte(0);   //Check byte - last column
            return temp;
        }

        public byte[] Block2(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            if(WriteCardDetails.Options == 0)   //Set down from 16
            {
                for (int i = 0; i < 15; i++)
                {
                    if (WriteCardDetails.VehicleLicenceType[i] > 0) temp[i] = (byte)WriteCardDetails.VehicleLicenceType[i];
                    else temp[i] = (byte)0;
                    //(byte)(RFIDCardInfoRead.Vehicle_Type)Enum.Parse(typeof(RFIDCardInfoRead.Vehicle_Type), WriteCardDetails.VehicleLicenceType[i]);
                }
            }
            else
            {
                for (int i = 0; i < 15; i++)   //Set down from 16
                {
                    temp[i] = (byte)0;
                }
            }
            temp[15] = Convert.ToByte(1);   //Check byte - last column

            return temp;
        }
        public byte[] Block3(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            if (WriteCardDetails.Options == 0)
            {
                for (int i = 0; i < 15; i++)  //Set down from 16
                {
                    if (WriteCardDetails.VehicleLicenceType[i+15] >0) temp[i] = (byte)WriteCardDetails.VehicleLicenceType[i+15];
                    else temp[i] = (byte)0;
                }
                //if (WriteCardDetails.VehicleLicenceType[i + 15] != null) temp[i] = (byte)(RFIDCardInfoRead.Vehicle_Type)Enum.Parse(typeof(RFIDCardInfoRead.Vehicle_Type), WriteCardDetails.VehicleLicenceType[i + 15]);
            }
            else
            {
                for (int i = 0; i < 15; i++)  //Set down from 16
                {
                    temp[i] = (byte)0;
                }
            }
            temp[15] = Convert.ToByte(2);   //Check byte - last column

            return temp;
        }

        public byte[] Block4(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            if (WriteCardDetails.Options == 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (WriteCardDetails.VID[i] != 0)
                    {
                        byte[] ViDsplit = BitConverter.GetBytes(WriteCardDetails.VID[i]);
                        temp[i * 2] = ViDsplit[1];
                        temp[i * 2 + 1] = ViDsplit[0];
                        
                    }     

                }
                byte[] ViDsplit2 = BitConverter.GetBytes(WriteCardDetails.VID[7]);
                temp[14] = ViDsplit2[1];
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = (byte)0;
                }
            }

            temp[15] = Convert.ToByte(3);   //Check byte - last column

            return temp;
        }

        public byte[] Block5(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            if (WriteCardDetails.Options == 1)
            {
                byte[] ViDsplit2 = BitConverter.GetBytes(WriteCardDetails.VID[7]);
                temp[0] = ViDsplit2[0];
                for (int i = 1; i < 8; i++)
                {
                    if (WriteCardDetails.VID[i] != 0)
                    {
                        byte[] ViDsplit = BitConverter.GetBytes(WriteCardDetails.VID[i+7]);
                        temp[i * 2-1] = ViDsplit[1];
                        temp[i * 2] = ViDsplit[0];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = (byte)0;
                }
            }

            temp[15] = Convert.ToByte(4);   //Check byte - last column

            return temp;
        }

        public byte[] Block6(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = WriteCardDetails.OperatorName.Length; i < 16; i++)
            {
                WriteCardDetails.OperatorName = WriteCardDetails.OperatorName + " ";
            }
            //temp[15] = 5;
            temp = ASCIIEncoding.ASCII.GetBytes(WriteCardDetails.OperatorName);
            temp[15] = Convert.ToByte(5);   //Check byte - last column

            return temp;
        }

        public byte[] Block12(CardDetails WriteCardDetails) // New info
        {
            byte[] temp = new byte[16];
            for (int i = WriteCardDetails.OperatorID.Length; i < 16; i++)
            {
                WriteCardDetails.OperatorID = WriteCardDetails.OperatorID + " ";
            }
            temp = ASCIIEncoding.ASCII.GetBytes(WriteCardDetails.OperatorID);
            temp[15] = Convert.ToByte(11);   //Check byte - last column
            return temp;
        }

        public byte[] Block7(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];

            byte[] UIDsplit = BitConverter.GetBytes(WriteCardDetails.IssuerUID);
            temp[0] = UIDsplit[0];
            temp[1] = UIDsplit[1];
            temp[2] = UIDsplit[2];
            temp[3] = UIDsplit[3];
            if(WriteCardDetails.Training_Date.Year>1000)
                temp[4] = Convert.ToByte(WriteCardDetails.Training_Date.Year.ToString().Remove(0,2), 16);
            else
                temp[4] = 0;
            temp[5] = Convert.ToByte(WriteCardDetails.Training_Date.Month.ToString(), 16);
            temp[6] = Convert.ToByte(WriteCardDetails.Training_Date.Day.ToString(), 16);

            if(WriteCardDetails.Issue_Date.Year>1000)
                temp[7] = Convert.ToByte(WriteCardDetails.Issue_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[7] =0;
            temp[8] = Convert.ToByte(WriteCardDetails.Issue_Date.Month.ToString(), 16);
            temp[9] = Convert.ToByte(WriteCardDetails.Issue_Date.Day.ToString(), 16);

            UIDsplit = BitConverter.GetBytes(WriteCardDetails.EngineerUID);
            temp[10] = UIDsplit[0];
            temp[11] = UIDsplit[1];
            temp[12] = UIDsplit[2];
            temp[13] = UIDsplit[3];

            temp[15] = Convert.ToByte(6);   //Check byte - last column
            return temp;
        }

        public byte[] Block8(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = WriteCardDetails.EngineerName.Length; i < 16; i++)
            {
                WriteCardDetails.EngineerName = WriteCardDetails.EngineerName + " ";
            }

            temp = ASCIIEncoding.ASCII.GetBytes(WriteCardDetails.EngineerName);
            temp[15] = Convert.ToByte(7);   //Check byte - last column
            return temp;
        }

        public byte[] Block9(CardDetails WriteCardDetails)
        {

            byte[] temp = new byte[16];
            if(WriteCardDetails.HotFlagedDate.Year>1000)
                temp[0] = Convert.ToByte(WriteCardDetails.HotFlagedDate.Year.ToString().Remove(0, 2), 16);
            else
                temp[0] = 0;
            temp[1] = Convert.ToByte(WriteCardDetails.HotFlagedDate.Month.ToString(), 16);
            temp[2] = Convert.ToByte(WriteCardDetails.HotFlagedDate.Day.ToString(), 16);

            byte[] UIDsplit = BitConverter.GetBytes(WriteCardDetails.HotFlagedVID);

            temp[3] = UIDsplit[1];
            temp[4] = UIDsplit[0];

            temp[5] = Convert.ToByte(WriteCardDetails.Hotflaged_status);

            if (WriteCardDetails.Training_Warn_Date.Year > 1000)
                temp[8] = Convert.ToByte(WriteCardDetails.Training_Warn_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[8] = 0;
            temp[9] = Convert.ToByte(WriteCardDetails.Training_Warn_Date.Month.ToString(), 16);
            temp[10] = Convert.ToByte(WriteCardDetails.Training_Warn_Date.Day.ToString(), 16);

            if (WriteCardDetails.Training_Exp_Date.Year > 1000)
                temp[11] = Convert.ToByte(WriteCardDetails.Training_Exp_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[11] = 0;
            temp[12] = Convert.ToByte(WriteCardDetails.Training_Exp_Date.Month.ToString(), 16);
            temp[13] = Convert.ToByte(WriteCardDetails.Training_Exp_Date.Day.ToString(), 16);

            temp[15] = Convert.ToByte(8);   //Check byte - last column
            return temp;
        }

        public byte[] Block10(CardDetails WriteCardDetails)
        {

            byte[] temp = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                temp[i] = WriteCardDetails.VehicleGroup[i];
            }

            temp[15] = Convert.ToByte(9);   //Check byte - last column
            return temp;
        }

        public byte[] Block11(CardDetails WriteCardDetails)
        {

            byte[] temp = new byte[16];
            temp[0] = Convert.ToByte(WriteCardDetails.ProductCode);

            if (WriteCardDetails.Medical_Date.Year > 1000)
                temp[1] = Convert.ToByte(WriteCardDetails.Medical_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[1] = 0;
            temp[2] = Convert.ToByte(WriteCardDetails.Medical_Date.Month.ToString(), 16);
            temp[3] = Convert.ToByte(WriteCardDetails.Medical_Date.Day.ToString(), 16);

            if (WriteCardDetails.Medical_Warn_Date.Year > 1000)
                temp[4] = Convert.ToByte(WriteCardDetails.Medical_Warn_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[4] = 0;
            temp[5] = Convert.ToByte(WriteCardDetails.Medical_Warn_Date.Month.ToString(), 16);
            temp[6] = Convert.ToByte(WriteCardDetails.Medical_Warn_Date.Day.ToString(), 16);

            if (WriteCardDetails.Medical_Exp_Date.Year > 1000)
                temp[7] = Convert.ToByte(WriteCardDetails.Medical_Exp_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[7] = 0;
            temp[8] = Convert.ToByte(WriteCardDetails.Medical_Exp_Date.Month.ToString(), 16);
            temp[9] = Convert.ToByte(WriteCardDetails.Medical_Exp_Date.Day.ToString(), 16);

            temp[15] = Convert.ToByte(10);   //Check byte - last column
            return temp;
        }

        public byte[] Block17_31_VehicleNames(CardDetails WriteCardDetails,int index)
        {
            int end_nr = index;
            byte[] temp = new byte[16];
            if (index >= 9)
            {
                index = index - 3;
            }
            if (WriteCardDetails.VehicleNames[index] != null)
            {
                for (int i = WriteCardDetails.VehicleNames[index].Length; i < 16; i++)
                {
                    WriteCardDetails.VehicleNames[index] = WriteCardDetails.VehicleNames[index] + " ";
                }
                temp = ASCIIEncoding.ASCII.GetBytes(WriteCardDetails.VehicleNames[index]);
                temp[15] = Convert.ToByte(12  + end_nr);   //Check byte - last column
                return temp;
            }
            else
                for (int i = 0; i < 15; i++)
                {
                    temp[i] = 0;
                }
                temp[15] = Convert.ToByte(12 + end_nr);   //Check byte - last column
            return temp;
            
        }

        public byte[] Block31_OperatorSurname(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = WriteCardDetails.OperatorSurname.Length; i < 16; i++)
            {
                WriteCardDetails.OperatorSurname = WriteCardDetails.OperatorSurname + " ";
            }
            temp = ASCIIEncoding.ASCII.GetBytes(WriteCardDetails.OperatorSurname);
            temp[15] = Convert.ToByte(30);
            return temp;
        }

        public byte[] Blank_Block_Temp_1(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                temp[i] = 0;
            }
            temp[15] = Convert.ToByte(18);
            return temp;
        }

        public byte[] Blank_Block_Temp_2(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                temp[i] = 0;
            }
            temp[15] = Convert.ToByte(19);
            return temp;
        }

        public byte[] Blank_Block_Temp_3(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                temp[i] = 0;
            }
            temp[15] = Convert.ToByte(20);
            return temp;
        }

        public byte[] Block32_Dover(CardDetails WriteCardDetails)
        {
            byte[] temp = new byte[16];

            if (WriteCardDetails.Dover_Date.Year > 1000)
                temp[0] = Convert.ToByte(WriteCardDetails.Dover_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[0] = 0;
            temp[1] = Convert.ToByte(WriteCardDetails.Dover_Date.Month.ToString(), 16);
            temp[2] = Convert.ToByte(WriteCardDetails.Dover_Date.Day.ToString(), 16);

            if (WriteCardDetails.Dover_Warn_Date.Year > 1000)
                temp[3] = Convert.ToByte(WriteCardDetails.Dover_Warn_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[3] = 0;
            temp[4] = Convert.ToByte(WriteCardDetails.Dover_Warn_Date.Month.ToString(), 16);
            temp[5] = Convert.ToByte(WriteCardDetails.Dover_Warn_Date.Day.ToString(), 16);

            if (WriteCardDetails.Dover_Exp_Date.Year > 1000)
                temp[6] = Convert.ToByte(WriteCardDetails.Dover_Exp_Date.Year.ToString().Remove(0, 2), 16);
            else
                temp[6] = 0;
            temp[7] = Convert.ToByte(WriteCardDetails.Dover_Exp_Date.Month.ToString(), 16);
            temp[8] = Convert.ToByte(WriteCardDetails.Dover_Exp_Date.Day.ToString(), 16);

            temp[15] = Convert.ToByte(31);
            return temp;
        }

        public int WriteInfoToCard(CardDetails WriteCardDetails, int CardType)
        {
           // byte[] key = { (byte)'S', 0, 2, 1, 3, 7, 255, 07, 128, 105, 255, 255, 255, 255, 255, 255};
           // if (MernokRFID_interface.MiFair_Store_Access_Key(0x0A, key)){

           // }

            byte[] temp = new byte[16];
            temp = Block1(WriteCardDetails);
            if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block1_Addr, temp))
            {
                temp = Block2(WriteCardDetails);
                if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block2_Addr, temp))
                {
                    temp = Block3(WriteCardDetails);
                    if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block3_Addr, temp))
                    {
                        temp = Block4(WriteCardDetails);
                        if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block4_Addr, temp))
                        {
                            temp = Block5(WriteCardDetails);
                            if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block5_Addr, temp))
                            {
                                temp = Block6(WriteCardDetails);
                                if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block6_Addr, temp))
                                {
                                    temp = Block7(WriteCardDetails);
                                    if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block7_Addr, temp))
                                    {
                                        temp = Block8(WriteCardDetails);
                                        if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block8_Addr, temp))
                                        {
                                            temp = Block9(WriteCardDetails);
                                            if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block9_Addr, temp))
                                            {
                                                temp = Block10(WriteCardDetails);
                                                if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block10_Addr, temp))
                                                {
                                                    temp = Block11(WriteCardDetails);
                                                    if(MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block11_Addr, temp))
                                                    {
                                                        temp = Block12(WriteCardDetails);
                                                        if (MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block12_Addr, temp))
                                                        {
                                                            if (CardType == 1 || CardType == 4)
                                                            {
                                                                for (int i = 0; i < 3; i++)
                                                                {
                                                                    temp = Block17_31_VehicleNames(WriteCardDetails, i);
                                                                    if (temp != null) MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_1_Addr + i, temp);
                                                                    else MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_1_Addr + i, Encoding.ASCII.GetBytes(""));
                                                                }

                                                                for (int i = 0; i < 3; i++)
                                                                {
                                                                    temp = Block17_31_VehicleNames(WriteCardDetails, i + 3);
                                                                    if (temp != null) MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_2_Addr + i, temp);
                                                                    else MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_2_Addr + i, Encoding.ASCII.GetBytes(""));
                                                                }
                                                            }

                                                            if (CardType == 4)
                                                            {
                                                                for (int i = 0; i < 3; i++)
                                                                {
                                                                    temp = Block17_31_VehicleNames(WriteCardDetails, i + 9);
                                                                    if (temp != null) MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_3_Addr + i, temp);
                                                                    else MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_3_Addr + i, Encoding.ASCII.GetBytes(""));
                                                                }

                                                                for (int i = 0; i < 3; i++)
                                                                {
                                                                    temp = Block17_31_VehicleNames(WriteCardDetails, i + 12);
                                                                    if (temp != null) MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_4_Addr + i, temp);
                                                                    else MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_4_Addr + i, Encoding.ASCII.GetBytes(""));
                                                                }

                                                                for (int i = 0; i < 3; i++)
                                                                {
                                                                    temp = Block17_31_VehicleNames(WriteCardDetails, i + 15);
                                                                    if (temp != null) MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_5_Addr + i, temp);
                                                                    else MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block17_5_Addr + i, Encoding.ASCII.GetBytes(""));
                                                                }
                                                            }

                                                            temp = Block31_OperatorSurname(WriteCardDetails);
                                                            if (MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block18_Addr, temp))
                                                            {
                                                                temp = Block32_Dover(WriteCardDetails);
                                                                if (MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, cardDetailsW.Block19_Addr, temp))
                                                                {
                                                                    temp = Blank_Block_Temp_1(WriteCardDetails);
                                                                    MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, 60, temp);
                                                                    temp = Blank_Block_Temp_2(WriteCardDetails);
                                                                    MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, 61, temp);
                                                                    temp = Blank_Block_Temp_3(WriteCardDetails);
                                                                    MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, 62, temp);
                                                                    return 100;
                                                                }
                                                                else return 14;
                                                            } else return 13;
                                                        }
                                                        else return 12;
                                                    }
                                                    else
                                                        return 11;
                                                    //MessageBox.Show("Card Successfullly Programed");

                                                }
                                                else
                                                    return 10;
                                                //MessageBox.Show("Card Programeing failed at block 10");
                                            }
                                            else
                                                return 9;
                                            //MessageBox.Show("Card Programeing failed at block 9");
                                        }
                                        else
                                            return 8;
                                        //MessageBox.Show("Card Programeing failed at block 8");
                                    }
                                    else
                                        return 7;
                                    //MessageBox.Show("Card Programeing failed at block 7");
                                }
                                else
                                    return 6;
                                // MessageBox.Show("Card Programeing failed at block 6");
                            }
                            else
                                return 5;
                            // MessageBox.Show("Card Programeing failed at block 5");
                        }
                        else
                            return 4;
                        //MessageBox.Show("Card Programeing failed at block 4");
                    }
                    else
                        return 2;
                    //MessageBox.Show("Card Programeing failed at block 2");
                }
                else
                    return 1;
                //MessageBox.Show("Card Programeing failed at block 1");

            
            }
            return 0;
      }
    }
}
