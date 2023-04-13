using System;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    public class IDChecker
    {
        public async void CheckId()
        {
            // -100 nemá ID
            // -1000 nedá se spojit s API

            ClientService clientService = new ClientService();
            try
            {
                Program.IdOfThisStation = Convert.ToInt32(await clientService.GetId());
                if (Program.IdOfThisStation == -100)
                {
                    Console.WriteLine("Stanice neexistuje v databázi.");
                    await clientService.Create();
                    Console.WriteLine("Stanice byla vytvořena v databázi.");
                    Program.IdOfThisStation = Convert.ToInt32(await clientService.GetId());
                }
            }
            catch
            {
                Program.IdOfThisStation = -1000;
                Console.WriteLine("Nejde komuniovat s API za úelem zjištění ID");
            }
        } 
    }
}
