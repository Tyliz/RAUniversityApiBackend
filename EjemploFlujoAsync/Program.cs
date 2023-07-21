using EjemploFlujoAsync;
using System.Diagnostics;
// See https://aka.ms/new-console-template for more information

Stopwatch sw = new Stopwatch();
sw.Start();

Console.WriteLine("\n*************************************************");
Console.WriteLine("\nBienvenido a la calculadora de hipotecas sincrona");

var annosVidaLaboral = CalculadoraHipotecaSync.ObtenerAnnosVidaLaboral();
Console.WriteLine($"\nAños de vida laboral obtenidos: {annosVidaLaboral}");

var esContratoIndefinido = CalculadoraHipotecaSync.EsContratoIndefinido();
Console.WriteLine($"\nEs contrato indefinido: {esContratoIndefinido}");

var sueldoNeto = CalculadoraHipotecaSync.ObtenerSueldoNeto();
Console.WriteLine($"\nSueldo neto: {sueldoNeto} €");

var gastosMensauales = CalculadoraHipotecaSync.ObtenerGastosMensauales();
Console.WriteLine($"\nGastos mensuales: {gastosMensauales} €");

var hipotecaConcedida = CalculadoraHipotecaSync.AnalizarInformacionParaObtenerHipoteca(annosVidaLaboral, esContratoIndefinido, sueldoNeto, gastosMensauales, cantidadSolicitada: 50000, annosPagar: 30);
var resultado = hipotecaConcedida ? "aprobada" : "denegada";
Console.WriteLine($"\nAnalisis finalizado, su solicitud de hipoteca ha sido : {resultado}");

sw.Stop();

Console.WriteLine($"\nLa operación a durado {sw.Elapsed}");

// Reiniciamos el contador para la asyncrono
sw.Restart();

Console.WriteLine("\n*************************************************");
Console.WriteLine("\nBienvenido a la calculadora de hipotecas sincrona");

Task<int> annosVidaLaboralTask = CalculadoraHipotecaAsync.ObtenerAnnosVidaLaboral();
Task<bool> esContratoIndefinidoTask = CalculadoraHipotecaAsync.EsContratoIndefinido();
Task<int> sueldoNetoTask = CalculadoraHipotecaAsync.ObtenerSueldoNeto();
Task<int> gastosMensaualesTask = CalculadoraHipotecaAsync.ObtenerGastosMensauales();

var tasks = new List<Task>
{
	annosVidaLaboralTask,
	esContratoIndefinidoTask,
	sueldoNetoTask,
	gastosMensaualesTask,
};

while (tasks.Any())
{
	Task tareafinalizada = await Task.WhenAny(tasks);

	if (tareafinalizada == annosVidaLaboralTask)
	{
		Console.WriteLine($"\nAños de vida laboral obtenidos: {annosVidaLaboralTask.Result}");
	}
	else if (tareafinalizada == esContratoIndefinidoTask)
	{
		Console.WriteLine($"\nEs contrato indefinido: {esContratoIndefinidoTask.Result}");
	}
	else if (tareafinalizada == sueldoNetoTask)
	{
		Console.WriteLine($"\nSueldo neto: {sueldoNetoTask.Result} €");
	}
	else if (tareafinalizada == gastosMensaualesTask)
	{
		Console.WriteLine($"\nGastos mensuales: {gastosMensaualesTask.Result} €");
	}
	tasks.Remove(tareafinalizada);
}

bool hipotecaConcedidaAsync = CalculadoraHipotecaAsync.AnalizarInformacionParaObtenerHipoteca(annosVidaLaboralTask.Result, esContratoIndefinidoTask.Result, sueldoNetoTask.Result, gastosMensaualesTask.Result, cantidadSolicitada: 50000, annosPagar: 30);


var resultadoAsync = hipotecaConcedidaAsync ? "aprobada" : "denegada";
Console.WriteLine($"\nAnalisis finalizado, su solicitud de hipoteca ha sido : {resultadoAsync}");


sw.Stop();

Console.WriteLine($"\nLa operación asincrona a durado {sw.Elapsed}");
