using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Authentication
{
    public class UtilsCls
    {
        //this type config is static in code,
        //1 = confirmation mail ,
        //2 = otp verify
        //3 = invoice mail
        //4 = CUSTOMER SUPPORT mail

        public static string GetMailSubjectByLang(string lang,int type)
        {
            
            if (type == 1)
            //mean confirmation mail
            {
                if (lang == "ar")
                    return "مرحباً بك في هوريزون";
                else if (lang == "en")
                    return "Welcome to Expand-Horizon !";
                else if (lang == "de")
                    return "Willkommen bei Expand-Horizon";
                else return "";
            }
            else if (type == 2)
            {
                //mean otp verify
                if (lang == "ar")
                    return "تأكيد البريد الإلكتروني-هوريزون";
                else if (lang == "en")
                    return "Expand-Horizon - Verify Your Email";
                else if (lang == "de")
                    return "Expand-Horizon - Bestätigen Sie Ihre E-Mail";
                else return "";
            }
            else if (type == 3)
            {
                //mean invoice
                if (lang == "ar")
                    return "   فاتوره - هوريزون";
                else if (lang == "en")
                    return "Expand-Horizon - Packages' Invoice";
                else if (lang == "de")
                    return "Expand-Horizon - Factuur van pakketten";
                else return "";
            }
            else if (type == 4)
            {
                //mean invoice
                if (lang == "ar")
                    return "   خدمة عملاء - هوريزون";
                else if (lang == "en")
                    return "Expand-Horizon - Customer Support";
                else if (lang == "de")
                    return "Expand-Horizon - Klantenondersteuning";
                else return "";
            }
            else if (type == 5)
            {
                //mean checkout notify to customer care
                if (lang == "ar")
                    return "   اخطار الدفع - هوريزون";
                else if (lang == "en")
                    return "Expand-Horizon - Checkout Notify";
                else if (lang == "de")
                    return "Expand-Horizon - Checkout-Benachrichtigung";
                else return "";
            }
            else return "";
            
        }
    }
}
