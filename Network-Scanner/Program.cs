using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Linq;

namespace Escaner_de_red
{
    class Program
    {

        static void Main(string[] args)
        {
            StreamWriter writer = new StreamWriter("datos.txt");

            Ping pingSender = new Ping();

            string hi, hf; //Host inicio y Host final
            string ip;

            Console.Write("IP de inicio: ");    hi = Console.ReadLine();
            Console.Write("IP de fin: ");       hf = Console.ReadLine();


            //Separamos los octetos
            string[] ip_inicio  = hi.Split('.');
            string[] ip_fin     = hf.Split('.');


            // Para transformar el arreglo de string a int
            int[] inicio = ip_inicio.Select(x => int.Parse(x)).ToArray();
            int[] final = ip_fin.Select(x => int.Parse(x)).ToArray();

            // Indica si debe recorrer todo el rango
            bool rango_completo = false;

            int timeout = 10; //tiempo de espera

            try
            {
                if (inicio[0] <= 255 && inicio[1] <= 255 && inicio[2] <= 255 && inicio[3] <= 255)
                {
                    if (final[0] <= 255 && final[1] <= 255 && final[2] <= 255 && final[3] <= 255)
                    {
                        if (inicio[0] <= final[0] && inicio[1] <= final[1] && inicio[2] <= final[2] && inicio[3] <= final[3])
                        {

                            if (inicio[0] != final[0] || inicio[1] != final[1] || inicio[2] != final[2]) rango_completo = true;

                            for (int a = inicio[0]; a <= final[0]; a++)
                            {
                                for (int b = inicio[1]; b <= final[1]; b++)
                                {
                                    for (int c = inicio[2]; c <= final[2]; c++)
                                    {

                                        /* Si completamos todos los rangos anteriores pasamos la variable a false y el 
                                           ultimo for recorre hasta el ultimo octeto de la ip final*/
                                        if (a == final[0] && b == final[1] && c == final[2]) rango_completo = false;

                                        for (int d = inicio[3]; d <= ((rango_completo) ? 255 : final[3]); d++)
                                        {

                                            ip = a + "." + b + "." + c + "." + d;

                                            PingReply reply = pingSender.Send(ip, timeout);

                                            if (reply.Status == IPStatus.Success)
                                            {
                                                Console.WriteLine("IP: {0}  Bytes: {1} Time: {2}ms TTL:{3}", reply.Address, reply.Buffer.Length, reply.RoundtripTime, reply.Options.Ttl);
                                                writer.WriteLine(reply.Address);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else Console.WriteLine("El rango final no puede ser superior al inicial.");
                    }
                    else Console.WriteLine("Un octeto soporta como máximo 255, reingrese la ip de \"final\".");
                }
                else Console.WriteLine("Un octeto soporta como máximo 255, reingrese la ip de \"inicio\".");
            }
            catch (Exception)
            {
                Console.WriteLine("ha ocurrido un error, por favor revisar los datos ingresados e intentar nuevamente.");
            }


            Console.WriteLine("Finalizado");
            writer.Close();
            Console.ReadKey();
        }
    }
}
