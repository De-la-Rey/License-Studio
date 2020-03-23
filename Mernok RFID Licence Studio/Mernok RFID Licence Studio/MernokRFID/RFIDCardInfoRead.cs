using System;
using System.Text;
using System.Windows;
using RFID;

namespace Mernok_RFID_Licence_Studio
{



    public class RFIDCardInfoRead
    {
        
        public ulong UID = 0;
        public bool formatted = false;

        public string UIDtoString(ulong Value)
        {

            byte[] ByteCard = BitConverter.GetBytes(Value);
            string StringCard = BitConverter.ToString(ByteCard);
            return StringCard;
        }

        #region enums
        public enum AccessLevel_enum
        {
            Operator = 1,
            Electrician = 2,
            Foreman = 3,
            Shift_Boss = 4,
            Trainee_Operator = 5,
            Maintenance = 6,
            Field_Technician = 7,
            Temporary_Operator = 8,
            Training_Officer = 9,                     //- Only this person can issue licenses!!
            Engineer = 10,                  //- Password required
            Mernok_Technician = 11,                    //- Password required
            Mernok_Field_Technician = 12,              //- Password required
            Mernok_Engineer = 13                       //- Password required
        };

        public enum VehicleAccessType
        {
            Vehicle_Type = 0,
            Vehicle_Serial = 1,
            Vehicle_Name = 2,
            Vehicle_Group = 3,
        }

    #endregion

    #region InitialInfo

    public CardDetails cardDetails = new CardDetails();

        #endregion

        #region Block1 functions

        void AccessLevel_read(byte licencebyte)
        {
            cardDetails.AccessLevel = (uint)licencebyte;
        }

        void Warn_Date_read(byte year_b, byte month_b, byte day_b)
        {
            Int32 Year = Convert.ToInt32("20" + year_b.ToString("X"));
            Int32 Month = Convert.ToInt32(month_b.ToString("X"));
            Int32 day = Convert.ToInt32(day_b.ToString("X"));
            if(Month!=0)
            {
                cardDetails.Warning_Date = new DateTime(Year, Month, day);
                formatted = true;
            }
            else
            {
                //MessageBox.Show("Card Not Formatted");
                formatted = false;
            }
                
            
        }

        void Exp_Date_read(byte year_b, byte month_b, byte day_b)
        {
            Int32 Year = Convert.ToInt32("20" + year_b.ToString("X"));
            Int32 Month = Convert.ToInt32(month_b.ToString("X"));
            Int32 day = Convert.ToInt32(day_b.ToString("X"));
            cardDetails.Expiry_Date = new DateTime(Year, Month, day);
        }

        void Client_Group_read(byte Client_group_H, byte Client_group_L)
        {
            byte[] localbyte = new byte[2];
            localbyte[0] = Client_group_H;
            localbyte[1] = Client_group_L;
            Array.Reverse(localbyte);
            uint localint = BitConverter.ToUInt16(localbyte, 0);
            cardDetails.Client_Group = localint;
            //if (Enum.GetValues(typeof(ClientGroupCode)).Length >= localint)
            //    cardDetails.Client_Group = Enum.GetNames(typeof(ClientGroupCode))[localint];
            //else
            //    cardDetails.Client_Group = "None";
        }

        void Client_Site_read(byte Client_group_H, byte Client_group_L)
        {
            byte[] localbyte = new byte[2];
            localbyte[0] = Client_group_H;
            localbyte[1] = Client_group_L;
            Array.Reverse(localbyte);
            uint localint = BitConverter.ToUInt16(localbyte, 0);

            cardDetails.Client_Site = localint;
            //if (Enum.GetValues(typeof(AGACode)).LongLength >= localint)
            //    cardDetails.Client_Site = Enum.GetNames(typeof(AGACode))[localint];
            //else
            //    cardDetails.Client_Site = "None";
        }

        void OperationalArea_Code(byte OperationalbyteH, byte OperationalbyteL)
        {
            //if ((int)Operationalbyte <= ClientGroupCodeS.Length) cardDetails.OpperationalArea = ClientGroupCodeS[(int)Operationalbyte];
            //else cardDetails.OpperationalArea = "still undefined";
            byte[] localbyte = new byte[2];
            localbyte[0] = OperationalbyteH;
            localbyte[1] = OperationalbyteL;
            Array.Reverse(localbyte);
            UInt16 localint = BitConverter.ToUInt16(localbyte, 0);

            cardDetails.OperationalArea = localint;
        }

        void ByPassByte_read(byte BypassByte)
        {
            cardDetails.ByPassBits = (uint)BypassByte;
        }

        void OptionsByte_read(byte OptionsByte)
        {
            cardDetails.Options = (int)OptionsByte;
        }


        public bool Block1Info()
        {
            byte[] output;
            if (UID!=0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(Mifare_key.A, 0, cardDetails.Block1_Addr, out output))
                {
                    if (output[2] == 0)
                        return false;
                    AccessLevel_read(output[0]);                           //sets the licencetype string
                    Warn_Date_read(output[1], output[2], output[3]);        //sets the warning date
                    if (!formatted)
                        return false;
                    Exp_Date_read(output[4], output[5], output[6]);         //sets the expiry date
                    Client_Group_read(output[7], output[8]);
                    Client_Site_read(output[9], output[10]);
                    OperationalArea_Code(output[11], output[12]);
                    ByPassByte_read(output[13]);
                    OptionsByte_read(output[14]);
                    return true;
                }
                else
                    return false;
            }
            else
                return false; 
        }

        #endregion

        #region Block2 and 3 functions : Vehicle type
        void Vehicle_Type_names(byte[] Block2, byte[] Block3)
        {

            for (int i = 0; i < Block2.Length-1; i++)
            {
                int localint = (int)Block2[i];
                //if (Enum.GetValues(typeof(Vehicle_Type)).Length >= localint)
                    cardDetails.VehicleLicenceType[i]  = (uint)localint;
                //else if(localint == 255)
                //{
                //    cardDetails.VehicleLicenceType[i] = (uint)localint;
                //}
                //else
                //    cardDetails.VehicleLicenceType[i] = 0;
            }
            for (int i = 0; i < Block3.Length-2; i++)
            {
                int localint = (int)Block3[i];
                //if (Enum.GetValues(typeof(Vehicle_Type)).Length >= localint)
                    cardDetails.VehicleLicenceType[Block3.Length + i] = (uint)localint;
                //else
                //    cardDetails.VehicleLicenceType[Block3.Length + i] = 0;
            }

        }

        public bool Block2_3Info()
        {
            byte[] output1, output2;
            if (UID != 0)
            {
                if ((MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block2_Addr, out output1))&& (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block3_Addr, out output2)))
                {
                    Vehicle_Type_names(output1, output2);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Block4 and 5 functions : VIDs

        void VIDs_read(byte[] block4, byte[] block5)
        {
            byte[] localbyte = new byte[2];
            for (int i = 0; i < 7; i++)
            {
                localbyte[0] = block4[i*2];
                localbyte[1] = block4[i*2+1];
                Array.Reverse(localbyte);
                UInt16 localint = BitConverter.ToUInt16(localbyte, 0);
                cardDetails.VID[i] = localint;
            }
            localbyte[0] = block4[15];
            localbyte[1] = block5[0];
            UInt16 localint1 = BitConverter.ToUInt16(localbyte, 0);
            cardDetails.VID[7] = localint1;
            for (int i = 1; i < 8; i++)
            {
                localbyte[0] = block5[i * 2];
                localbyte[1] = block5[i * 2 + 1];
                Array.Reverse(localbyte);
                UInt16 localint = BitConverter.ToUInt16(localbyte, 0);
                cardDetails.VID[i+7] = localint;
            }

            //VID
        }

        public bool Block4_5Info()
        {
            byte[] output1, output2;
            if (UID != 0)
            {
                if ((MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block4_Addr, out output1)) && (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block5_Addr, out output2)))
                {
                    VIDs_read(output1, output2);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion 

        #region Block 6 functions : Operator Name

        void OperatorName_read(byte[] block6)
        {
            cardDetails.OperatorName = Encoding.ASCII.GetString(block6);
            cardDetails.OperatorName = cardDetails.OperatorName.Replace("\0", "");
        }

        public bool Block6Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block6_Addr, out output1))
                {
                    OperatorName_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Block7 functions : Issuer UID, Traning Date, Issue Date, EngineerUID

        void IssuerUID_read(byte[] block7)
        {
            cardDetails.IssuerUID = 0;
            byte[] localbyte = new byte[4];
            localbyte[0] = block7[0];
            localbyte[1] = block7[1];
            localbyte[2] = block7[2];
            localbyte[3] = block7[3];
            //Array.Reverse(localbyte);
            cardDetails.IssuerUID = BitConverter.ToUInt32(localbyte, 0);

        }

        void Training_Date_read(byte[] block7)
        {

            //Training_Date = 0;

            Int32 Year = Convert.ToInt32("20" + block7[4].ToString("X"));
            Int32 Month = Convert.ToInt32(block7[5].ToString("X"));
            Int32 day = Convert.ToInt32(block7[6].ToString("X"));
            if (block7[5] != 0)
            {
                cardDetails.Training_Date = new DateTime(Year, Month, day);
            }

        }

        void Issue_Date_read(byte[] block7)
        {

            Int32 Year = Convert.ToInt32("20" + block7[7].ToString("X"));
            Int32 Month = Convert.ToInt32(block7[8].ToString("X"));
            Int32 day = Convert.ToInt32(block7[9].ToString("X"));
            if (block7[8] != 0)
            {
                cardDetails.Issue_Date = new DateTime(Year, Month, day);
            }
            
        }

        void EngineerUID_read(byte[] block7)
        {
            cardDetails.EngineerUID = 0;
            byte[] localbyte = new byte[4];
            localbyte[0] = block7[10];
            localbyte[1] = block7[11];
            localbyte[2] = block7[12];
            localbyte[3] = block7[13];
            //Array.Reverse(localbyte);
            cardDetails.EngineerUID = BitConverter.ToUInt32(localbyte, 0);
        }


        public bool Block7Info() //reads EngineerUID and other important info for desision making
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block7_Addr, out output1))
                {
                    IssuerUID_read(output1);
                    Training_Date_read(output1);
                    Issue_Date_read(output1);
                    EngineerUID_read(output1);

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Block8 functions : Engineer Name

        void EngineerName_read(byte[] block8)
        {
            cardDetails.EngineerName = Encoding.ASCII.GetString(block8);
            cardDetails.EngineerName = cardDetails.EngineerName.Replace("\0", "");
        }

        public bool Block8Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block8_Addr, out output1))
                {
                    EngineerName_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Block9 functions : Hot Flags

        void HotFlagedDate_read(byte[] block9)
        {

            //Training_Date = 0;

            Int32 Year = Convert.ToInt32("20" + block9[0].ToString("X"));
            Int32 Month = Convert.ToInt32(block9[1].ToString("X"));
            Int32 day = Convert.ToInt32(block9[2].ToString("X"));
            cardDetails.HotFlagedDate = new DateTime(Year, Month, day);

        }

        void HotFlagVID_read(byte[] block9)
        {
//            cardDetails.IssuerUID = 0;
            byte[] localbyte = new byte[2];
            localbyte[0] = block9[3];
            localbyte[1] = block9[4];
            Array.Reverse(localbyte);
            cardDetails.HotFlagedVID = BitConverter.ToUInt16(localbyte, 0);

        }

        void HotFlagger(byte HotFlagbyte)
        {
            cardDetails.Hotflaged_status = Convert.ToBoolean(HotFlagbyte);
        }
        void TrainingWarningDate_read(byte[] block9)
        {
            if (block9[9] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block9[8].ToString("X"));
                Int32 Month = Convert.ToInt32(block9[9].ToString("X"));
                Int32 day = Convert.ToInt32(block9[10].ToString("X"));
                cardDetails.Training_Warn_Date = new DateTime(Year, Month, day);
            }
            

        }
        void TrainingExpiryDate_read(byte[] block9)
        {
            if(block9[12] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block9[11].ToString("X"));
                Int32 Month = Convert.ToInt32(block9[12].ToString("X"));
                Int32 day = Convert.ToInt32(block9[13].ToString("X"));
                cardDetails.Training_Exp_Date = new DateTime(Year, Month, day);
            }

        }

        public bool Block9Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block9_Addr, out output1))
                {
                    HotFlagedDate_read(output1);
                    HotFlagVID_read(output1);
                    HotFlagger(output1[5]);
                    TrainingWarningDate_read(output1);
                    TrainingExpiryDate_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Block10 functions : Vehicle Groups

        void VehicleGroup_read(byte[] block10)
        {
            for (int i = 0; i < 15; i++)
            {
                cardDetails.VehicleGroup[i] = block10[i];
            }
             
        }

        public bool Block10Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block10_Addr, out output1))
                {
                    VehicleGroup_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        public void ReadProductCode(byte[] Block11)
        {
            cardDetails.ProductCode = Block11[0];
        }
        void MedicalDate_read(byte[] block11)
        {
            if (block11[2] != 0) { 
                Int32 Year = Convert.ToInt32("20" + block11[1].ToString("X"));
                Int32 Month = Convert.ToInt32(block11[2].ToString("X"));
                Int32 day = Convert.ToInt32(block11[3].ToString("X"));
                cardDetails.Medical_Date = new DateTime(Year, Month, day);
            }

        }
        void MedicalWarnDate_read(byte[] block11)
        {
            if (block11[5] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block11[4].ToString("X"));
                Int32 Month = Convert.ToInt32(block11[5].ToString("X"));
                Int32 day = Convert.ToInt32(block11[6].ToString("X"));
                cardDetails.Medical_Warn_Date = new DateTime(Year, Month, day);
            }

        }
        void MedicalExpDate_read(byte[] block11)
        {
            if (block11[8] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block11[7].ToString("X"));
                Int32 Month = Convert.ToInt32(block11[8].ToString("X"));
                Int32 day = Convert.ToInt32(block11[9].ToString("X"));
                cardDetails.Medical_Exp_Date = new DateTime(Year, Month, day);
            }

        }

        public bool Block11_5Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block11_Addr, out output1))
                {
                    ReadProductCode(output1);
                    MedicalDate_read(output1);
                    MedicalWarnDate_read(output1);
                    MedicalExpDate_read(output1);
                    return true;

                }
                else
                    return false;
            }
            else
                return false;
        }

        #region Block11 functions : Vehicle Names

        void VehicleName_read(byte[] block11, int i)
        {
            cardDetails.VehicleNames[i] = Encoding.ASCII.GetString(block11);
            cardDetails.VehicleNames[i] = cardDetails.VehicleNames[i].Replace("\0", "");
        }

        public bool Block11Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                for (int i = 0; i < 3; i++) //block1
                {
                    if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block17_1_Addr + i, out output1))
                    {
                        VehicleName_read(output1, i);
                        //return true;
                    }
                    else
                        return false;
                }

                for (int i = 0; i < 3; i++)//block2
                {
                    if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block17_2_Addr + i, out output1))
                    {
                        VehicleName_read(output1, i+3);
                        //return true;
                    }
                    else
                        return false;
                }

                for (int i = 0; i < 3; i++)//block3
                {
                    if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block17_3_Addr + i, out output1))
                    {
                        VehicleName_read(output1, i+6);
                        //return true;
                    }
                    else
                        return false;
                }

                for (int i = 0; i < 3; i++)//block4
                {
                    if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block17_4_Addr + i, out output1))
                    {
                        VehicleName_read(output1, i+9);
                        //return true;
                    }
                    else
                        return false;
                }

                for (int i = 0; i < 3; i++)//block5
                {
                    if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block17_5_Addr + i, out output1))
                    {
                        VehicleName_read(output1, i+12);
                        //return true;
                    }
                    else
                        return false;
                }

                return true;
                
            }
            else
                return false;
        }

        #endregion

        #region Block 12 functions : Operator ID
        void OperatorID_read(byte[] block12)
        {
            cardDetails.OperatorID = Encoding.ASCII.GetString(block12);
            cardDetails.OperatorID = cardDetails.OperatorID.Replace("\0", "");
        }
        public bool Block12Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block12_Addr, out output1))
                {
                    OperatorID_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        #region Block 31 functions : Operator Surname
        void OperatorSurname_read(byte[] block31)
        {
            cardDetails.OperatorSurname = Encoding.ASCII.GetString(block31);
            cardDetails.OperatorSurname = cardDetails.OperatorSurname.Replace("\0", "");
        }
        public bool Block31Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block18_Addr, out output1))
                {
                    OperatorSurname_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        public bool ReadInfo(ulong _UID)
        {
            UID = (ulong)_UID;
            cardDetails.cardUID = (uint)UID;
            if (Block1Info() && Block2_3Info() && Block4_5Info() && Block6Info() && Block7Info() && Block8Info() && Block9Info() &&
            Block10Info() && Block11_5Info() && Block12Info() && Block11Info() && Block31Info() && Block32Info())
            {
                return true;
            }
            else return false;
        }

        public bool ReadIssuer(ulong _UID)
        {
            UID = (ulong)_UID;
            if (Block1Info() && Block7Info() && Block8Info())
            {
                return true;
            }
            else return false;
        }
        #region Block 32 functions
        void DoverDate_read(byte[] block19)
        {
            if (block19[1] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block19[0].ToString("X"));
                Int32 Month = Convert.ToInt32(block19[1].ToString("X"));
                Int32 day = Convert.ToInt32(block19[2].ToString("X"));
                cardDetails.Dover_Date = new DateTime(Year, Month, day);
            }

        }
        void DoverWarningDate_read(byte[] block19)
        {
            if (block19[4] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block19[3].ToString("X"));
                Int32 Month = Convert.ToInt32(block19[4].ToString("X"));
                Int32 day = Convert.ToInt32(block19[5].ToString("X"));
                cardDetails.Dover_Warn_Date = new DateTime(Year, Month, day);
            }

        }
        void DoverExpiryDate_read(byte[] block19)
        {
            if (block19[7] != 0)
            {
                Int32 Year = Convert.ToInt32("20" + block19[6].ToString("X"));
                Int32 Month = Convert.ToInt32(block19[7].ToString("X"));
                Int32 day = Convert.ToInt32(block19[8].ToString("X"));
                cardDetails.Dover_Exp_Date = new DateTime(Year, Month, day);
            }

        }

        public bool Block32Info()
        {
            byte[] output1;
            if (UID != 0)
            {
                if (MernokRFID_interface.Mifair_Read_Block(0, 0, cardDetails.Block19_Addr, out output1))
                {
                    DoverDate_read(output1);
                    DoverWarningDate_read(output1);
                    DoverExpiryDate_read(output1);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

#endregion

    }
}
