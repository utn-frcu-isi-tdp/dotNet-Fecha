using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTN.FRCU.ISI.Taller
{
    class Program
    {
        static void Main(string[] args)
        {

            Fecha fecha = new Fecha(1, 1, 2499);

            Console.Write(fecha.DiaSemana);

            Console.ReadKey();

        }
    }
}
