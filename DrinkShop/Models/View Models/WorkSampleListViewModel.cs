namespace DrinkShop.Models.View_Models
{
    public class WorkSampleListViewModel
    {
        public WorkSampleListViewModel()
        {
            groups = new List<Group>();
            Samples = new List<WorkSamples>();
        }
        public List<WorkSamples> Samples { get; set; }
        public List<Group> groups { get; set; }
         
    }
}
