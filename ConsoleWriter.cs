using System;

namespace Angular_Utility {
    static class ConsoleWriter {

        //------------| MESSAGE_LINE |------------------------------------------------------------------------------------------
        public static void messageLine(string message_, ConsoleColor messageColor_) {
            ConsoleColor defColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor_;
            Console.WriteLine(message_);
            Console.ForegroundColor = defColor;
        }

        //------------| WARN_MESSAGE_LINE |-------------------------------------------------------------------------------------
        public static void warnMessageLine(string warnString_) {
            messageLine(warnString_, ConsoleColor.Red);
        }

        //------------| OK_MESSAGE_LINE |-------------------------------------------------------------------------------------
        public static void okMessageLine(string okString_) {
            messageLine(okString_, ConsoleColor.Green);
        }

        //------------| ACCENT_MESSAGE_LINE |----------------------------------------------------------------------------------
        public static void accentMessageLine(string accentMessage_) {
            messageLine(accentMessage_, ConsoleColor.Yellow);
        }

        //------------| MESSAGE_SPAN |-------------------------------------------------------------------------------------
        public static void messageSpan(string message_, ConsoleColor messageColor_, bool endLine_ = false) {
            ConsoleColor defColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor_;
            Console.Write(message_ + ((endLine_) ? "\n" : ""));
            Console.ForegroundColor = defColor;
        }

        //------------| WARN_MESSAGE_SPAN |-------------------------------------------------------------------------------------
        public static void warnMessageSpan(string warnString_, bool endLine_ = false) {
            messageSpan(warnString_, ConsoleColor.Red, endLine_);
        }

        //------------| OK_MESSAGE_SPAN |-------------------------------------------------------------------------------------
        public static void okMessageSpan(string okString_, bool endLine_ = false) {
            messageSpan(okString_, ConsoleColor.Green, endLine_);
        }

        //------------| ACCENT_MESSAGE_SPAN |----------------------------------------------------------------------------------
        public static void accentMessageSpan(string accentMessage_, bool endLine_ = false) {
            messageSpan(accentMessage_, ConsoleColor.Yellow, endLine_);
        }
    }
}
