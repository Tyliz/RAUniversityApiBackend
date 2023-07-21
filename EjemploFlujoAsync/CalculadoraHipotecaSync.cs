using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploFlujoAsync
{
	public static class CalculadoraHipotecaSync
	{
		public static int ObtenerAnnosVidaLaboral()
		{
			Console.WriteLine("\nObteniendo años de vida laboral...");
			Task.Delay(5000).Wait();
			return new Random().Next(1, 35);
		}

		public static bool EsContratoIndefinido()
		{
			Console.WriteLine("\nVerificando si el tipo de contrato es indefinido");
			Task.Delay(5000).Wait();
			return new Random().Next(1, 10) % 2 == 0;
		}

		public static int ObtenerSueldoNeto()
		{
			Console.WriteLine("\nObteniendo suedo neto de un empleado...");
			Task.Delay(5000).Wait();
			return new Random().Next(800, 6000);
		}


		public static int ObtenerGastosMensauales()
		{
			Console.WriteLine("\nObteniendo gastos mensauales...");
			Task.Delay(5000).Wait();
			return new Random().Next(200, 1000);
		}

		public static bool AnalizarInformacionParaObtenerHipoteca(
			int annosVidaLaboral,
			bool esContratoIndefinido,
			int sueldoNeto,
			int GastosMensuales,
			int cantidadSolicitada,
			int annosPagar
		)
		{
			Console.WriteLine("\nAnalizando Información para conceder hipoteca...");

			if (annosVidaLaboral < 2)
			{
				return false;
			}

			//Obtener la cuota mensual a pagar
			var cuota = (cantidadSolicitada / annosPagar) / 12;

			if (cuota >= sueldoNeto || cuota > sueldoNeto / 2)
				return false;

			var porcentajeGastosSobreSueldo = (GastosMensuales * 100) / sueldoNeto;

			if (porcentajeGastosSobreSueldo > 30)
				return false;

			if (cuota + GastosMensuales >= sueldoNeto)
				return false;

			if (!esContratoIndefinido && (cuota + GastosMensuales) > sueldoNeto / 3)
				return false;

			return true;
		}
	}
}
