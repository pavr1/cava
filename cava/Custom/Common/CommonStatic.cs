using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace cava.Custom.Common
{
    public class CommonObjects
    {
        public enum Actions
        {
            Bar,
            Kitchen,
            Experience,
            Reservation,
            ReservationRetrieve,
            Login
        }

        public const string _CAVA_ADMIN_EMAIL_KEY = "CavaAdminEmail";
        public const string _CONFIG_ADMIN_EMAIL_KEY = "adminEmail";
        public const string _MAIL_HOST_KEY = "mailhost";
        public const string _MAIL_PORT_KEY = "mailport";
        public const string _NOTIFICATIONS_USER_KEY = "NotificationsUser";
        public const string _NOTIFICATIONS_PASSWORD_KEY = "notificationspassword";

        public const string _ERROR = "Error";

        public const string _RESERVATION_BODY = "<html><head></head><body>{0} HA REALIZADO UNA RESERVA DE {1} PERSONA(S) PARA EL {2}. <br /> INFORMACIÓN DE USUARIO: <br /> - CORREO ELECTRÓNICO: {3} <br /> - TELÉFONO: {4} </body></html>";
        public const string _RESERVATION_EMAIL_BODY = "<html><head></head><body>USTED HA REALIZADO UNA RESERVA EN WWW.CAVARESTOBAR.COM DE {0} PERSONA(S) PARA EL {1}. </body></html>";
        public const string _ERROR_EMAIL_BODY = "<html><head></head><body>HEMOS ENCONTRADO UN ERROR GUID: {1}</body></html>";

        public const string _NEW_RESERVATION_SUBJECT = "NUEVA RESERVA CREADA";
        public const string _NEW_RESERVATION_SUBJECT2 = "RESERVACIONES CAVA RESTOBAR";
        public const string _NEW_ERROR_SUBJECT2 = "CAVA RESTOBAR. ERROR REGISTRADO. GUID: {0}";

        public const string _DATE_FORMAT_1 = "dd/MM/yyyy";
        public const string _A_LAS = "A LAS";

    }

    public class CommonStatic
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackFrame callStack = new StackFrame(1, true);

            return callStack.GetFileName();
        }

        public static int GetCurrentLine()
        {
            StackFrame callStack = new StackFrame(1, true);

            return callStack.GetFileLineNumber();
        }
    }
}