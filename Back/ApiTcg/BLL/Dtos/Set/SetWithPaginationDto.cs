namespace BLL.Dtos.Set;

public class SetWithPaginationDto
{

    public IEnumerable<SetDto> sets { get; set; } = new List<SetDto> (); 
    
    public int totalSets { get; set; }
}
