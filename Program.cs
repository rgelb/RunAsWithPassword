using Microsoft.CommandLineHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RunAsX {
    class Program {

        private static readonly Arguments appArgs = new Arguments();

        static void Main(string[] args) {
            if (!Parser.ParseArgumentsWithUsage(args, appArgs))
                Environment.Exit(-1);

            try {
                AskForPassword();
                RunAsImpersonator.Run(appArgs.File, appArgs.Domain, appArgs.Login, appArgs.Password);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private static void AskForPassword() {
            // Ask for password if there is a passed in Domain Login, but no password
            if (string.IsNullOrEmpty(appArgs.DomainLogin) || !string.IsNullOrEmpty(appArgs.Password)) return;

            string pass = "";
            Console.Write("Enter your password: ");
            ConsoleKeyInfo key;

            do {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0) {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            appArgs.Password = pass;
            Console.WriteLine();
        }
    }


    [CommandLineArguments(CaseSensitive = false)]
    public class Arguments {
        [Argument(ArgumentType.AtMostOnce, HelpText = @"Domain Login, e.g CompanyDomain\username", DefaultValue = "", LongName = "DomainLogin", ShortName = "dl")]
        public string DomainLogin;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Password", DefaultValue = "", LongName = "Password", ShortName = "p")]
        public string Password;
        [Argument(ArgumentType.Required, HelpText = "File to execute", LongName = "File to execute", ShortName = "f")]
        public string File;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Arguments", DefaultValue = "", LongName = "Arguments", ShortName = "a")]
        public string Args;


        // helper properties
        public string Domain {
            get {
                if (string.IsNullOrEmpty(DomainLogin)) return "";

                var split = DomainLogin.Split(@"\".ToCharArray());
                if (split.Length == 2)
                    return split[0];

                return "";
            }
        }

        public string Login {
            get {
                if (string.IsNullOrEmpty(DomainLogin)) return "";

                var split = DomainLogin.Split(@"\".ToCharArray());
                if (split.Length == 2)
                    return split[1];

                return "";
            }
        }
    }
}
