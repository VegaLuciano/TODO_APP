using API_TODO.Models.Custom;

namespace API_TODO.Service
{
	public interface IAutorizacionService
	{
		Task<AutorizacionResponse> ReturnToken(AutorizacionRequest autorizacion);
	}
}
