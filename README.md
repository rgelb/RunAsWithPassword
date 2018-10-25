# Runas With Password Specified
When running the _runas.exe_ command, you cannot specify the password on the command line.  This small application fixes that.

# Usage Examples

Example of starting Microsoft SQL Server Management Studio  
_RunAsX.exe /dl:MyDomain\MyUser /p:Password1 /f:"C:\Program Files\ManagementStudio\Ssms.exe"_

# Documentation

Command line swithes:  
/dl (or /DomainLogin) - domain login, e.g CompanyDomain\username  
/p (or /Password) - password  
/f:{file to execute) - file to execute  
/a:{arguments to pass to file} - not working at the moment  
