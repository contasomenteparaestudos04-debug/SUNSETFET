namespace Sunset.Web.Helpers;

// Escolhe a foto da quadra pela categoria. Beach Tennis usa fotos de areia
// e Vôlei usa fotos de ginásio. O Id serve só para alternar entre as duas
// fotos de cada tipo, assim as quadras não ficam todas iguais na lista.
public static class FotoQuadra
{
    public static string Caminho(string categoria, int id)
    {
        var numero = id % 2 == 0 ? 1 : 2;
        var tipo = categoria == Sunset.Modelos.Categorias.BeachTennis ? "areia" : "ginasio";
        return $"~/img/quadras/{tipo}-{numero}.jpg";
    }
}
