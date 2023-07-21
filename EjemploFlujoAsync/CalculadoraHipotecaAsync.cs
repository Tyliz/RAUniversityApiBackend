namespace EjemploFlujoAsync
{
	public static class CalculadoraHipotecaAsync
	{
		public static async Task<int> ObtenerAnnosVidaLaboral()
		{
			Console.WriteLine("\nObteniendo años de vida laboral...");
			await Task.Delay(5000);
			return new Random().Next(1, 35);
		}

		public static async Task<bool> EsContratoIndefinido()
		{
			Console.WriteLine("\nVerificando si el tipo de contrato es indefinido");
			await Task.Delay(5000);
			return new Random().Next(1, 10) % 2 == 0;
		}

		public static async Task<int> ObtenerSueldoNeto()
		{
			Console.WriteLine("\nObteniendo suedo neto de un empleado...");
			await Task.Delay(5000);
			return new Random().Next(800, 6000);
		}


		public static async Task<int> ObtenerGastosMensauales()
		{
			Console.WriteLine("\nObteniendo gastos mensauales...");
			await Task.Delay(5000);
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
