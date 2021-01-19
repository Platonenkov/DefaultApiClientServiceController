using DefaultApiClientServiceController.Entity;

namespace Domain
{
    public class Student :BaseEntity<int>
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
