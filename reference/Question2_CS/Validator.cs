using System;
using System.IO;
using System.Text;

namespace Question2
{
    class Validator
    {
        public bool CheckIdPsw(string id, string psw)
        {
            bool bRet = false;
            String encPsw = CardUtility.passwordEncryption_SHA256(psw);

            string[] lines = System.IO.File.ReadAllLines(@"..\\..\\..\\CLIENT\\INSPECTOR.TXT");
            foreach (string line in lines)
            {
                String fileId = line.Substring(0, 8);
                String filePsw = line.Substring(9);

                if (id.Equals(fileId) && encPsw.Equals(filePsw))
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }

        // cardInfo : [카드ID(8)][버스번호(7)][승차/하차 코드(1)][최근 승차시각(14)]
        // sample : CARD_001BUS_001N20171019143610
        public void InspectCard(string startTime, string id, string busID, string cardInfo)
        {
            string strValidateCode;

            // cardInfo parsing 
            //string cardID = cardInfo.Substring(0, 8); 
            string cardBusID = cardInfo.Substring(8, 7);
            string code = cardInfo.Substring(15, 1);
            string rideTime = cardInfo.Substring(16);

            // Get Inspect Time
            string strInspectTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Validation
            // 1. Bus ID Match
            if (busID.Equals(cardBusID))
            {
                // 2. Check Aboard Code
                if (code.Equals("N"))
                {
                    // 3. Time Difference
                    if (CardUtility.HourDiff(strInspectTime, rideTime) < 3)
                    {
                        strValidateCode = "R1";
                    }
                    else
                    {
                        strValidateCode = "R4";
                    }
                }
                else
                {
                    strValidateCode = "R3";
                }
            }
            else
            {
                strValidateCode = "R2";
            }

            // Create Folder
            string destFolder = "..\\..\\..\\" + id;
            System.IO.Directory.CreateDirectory(destFolder);

            // File Writing
            string strFilename = destFolder + "\\" + id + "_" + startTime + ".TXT";
            string strWrite = id + "#" + busID + "#" + cardInfo + "#" + strValidateCode + "#" + strInspectTime + "\n";
            FileStream fs = new System.IO.FileStream(strFilename, FileMode.Append);
            fs.Write(Encoding.UTF8.GetBytes(strWrite), 0, strWrite.Length);
            fs.Close();
        }
    }
}
