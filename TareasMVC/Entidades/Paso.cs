namespace TareasMVC.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public bool Realizado { get; set; }
        public int Orden { get; set; }
        // Relaciones
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; }
    }
}
