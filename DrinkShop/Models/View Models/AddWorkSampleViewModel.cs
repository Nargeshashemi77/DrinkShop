namespace DrinkShop.Models.View_Models
{
    public class AddWorkSampleViewModel
    {
        public AddWorkSampleViewModel()
        {
            groups = new List<Group>();
        }
        public WorkSamples Sample { get; set; }
        public List<Group> groups { get; set; }
        public IFormFile SampleImage { get; set; }
    }
}
