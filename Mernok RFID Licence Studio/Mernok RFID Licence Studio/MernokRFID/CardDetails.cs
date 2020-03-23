using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mernok_RFID_Licence_Studio
{
    public class CardDetails
    {
        public ulong cardUID;
        public int CommanderRFIDCardMemoryBlock = 36; //was sector 35 (176)

        //New public adresses
        public int Block1_Addr = 36;    // Access level, Warning date, expiry date, client group, client site, operational area, bypass bits, options
        public int Block2_Addr = 37;    // License asset type
        public int Block3_Addr = 38;    // License asset type

        public int Block4_Addr = 40;    // Licensed SN
        public int Block5_Addr = 41;    
        public int Block6_Addr = 42;    // Operator name

        public int Block7_Addr = 44;    // Issuer SN
        public int Block8_Addr = 45;    // Engineer name
        public int Block9_Addr = 46;    // Hot flagged

        public int Block10_Addr = 48;   // Unit details
        public int Block11_Addr = 49;   // Mernok product code
        public int Block12_Addr = 50;   // Employee ID

        public int Block17_1_Addr = 52; // Vehicle name start

        public int Block17_2_Addr = 56; // Vehicle name (contd.)

        public int Block17_3_Addr = 64; // Vehicle name (contd.)

        public int Block17_4_Addr = 68; // Vehicle name (contd.)

        public int Block17_5_Addr = 72; // Vehicle name (contd.)

        public int Block18_Addr = 76; // Operator surname
        public int Block19_Addr = 77; // Dover test dates

        public int S9_trailer = 39;     // Sector 9 trailer (keys)
        public int S10_trailer = 43;     // Sector 10 trailer (keys) 
        public int S11_trailer = 47;     // Sector 11 trailer (keys)
        public int S12_trailer = 51;     // Sector 12 trailer (keys) 
        public int S13_trailer = 55;     // Sector 13 trailer (keys)
        public int S14_trailer = 59;     // Sector 14 trailer (keys) 
        public int S16_trailer = 67;     // Sector 16 trailer (keys) 
        public int S17_trailer = 71;     // Sector 17 trailer (keys) 
        public int S18_trailer = 75;     // Sector 18 trailer (keys) 
        public int S19_trailer = 79;     // Sector 19 trailer (keys)

        //Sector35 Block1                                   //NOTE: A lot of these addresses are outdated, comments kept for data structuring only
        public uint AccessLevel;                              //adress B0[0]
        public DateTime Warning_Date;                            //adress B0[1..3]
        public DateTime Expiry_Date;                             //adress B0[4..6]
        public uint Client_Group;                            //adress B0[7..8]
        public uint Client_Site;                             //adress B0[9..10]
        public UInt16 OperationalArea;                          //adress B0[11..12]
        public uint ByPassBits;                                //adress B0[13]
        public int Options;                                   //adress B0[14]

        //Sector35 Block[2..3]
        public uint[] VehicleLicenceType = new uint[30];   //adress B1..B2

        //sector35 Block[4..5]
        public UInt16[] VID = new UInt16[15];              //adress B3..B4

        //sector35 Block6
        public string OperatorName;                        //adress B5

        public string OperatorSurname = "";

        public string OperatorID = "";

        //sector35 Block7
        public ulong IssuerUID;                            //adress B6[0..3]
        public DateTime Training_Date;                       //adress B6[4..6]
        public DateTime Issue_Date;                          //adress B6[7..9]
        public ulong EngineerUID;                         //adress B6[10..14]

        //sector35 Block8
        public string EngineerName;         //adress B7

        //sector35 Block9
        public DateTime HotFlagedDate;                       //adress B8[0..2]
        public UInt16 HotFlagedVID;                        //adress B8[3..4]
        public bool Hotflaged_status;                      //adress B8[5]

        //sector35 Block10
        public byte[] VehicleGroup = new byte[15];         //adress B9
        public string[] VehicleGroupstr = new string[15];

        //sector36 Block11
        public uint ProductCode;                            //adress BA[0]



        //sector36 Block[17..31]
        public string[] VehicleNames = new string[15];     //adress [C0..CE]


        public DateTime Training_Warn_Date;
        public DateTime Training_Exp_Date;
        public DateTime Medical_Date;
        public DateTime Medical_Warn_Date;
        public DateTime Medical_Exp_Date;
        public DateTime Dover_Date;
        public DateTime Dover_Warn_Date;
        public DateTime Dover_Exp_Date;

    }

    public class CardDetailsFile
    {
        public CardDetails FCardDetails;
        public UInt16 version;
        public DateTime dateCreated;
        public string createdBy;            //Name of file creator
    }
}
