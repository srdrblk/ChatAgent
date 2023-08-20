using Dtos;
using Entities;

namespace Business.Queues
{
    public class SupportQueue 
    {
        private Queue<Support> Queue { get; set; } = new Queue<Support>();
        public SupportQueue() { }

        public void AddSupport(Support support)
        {
            Queue.Enqueue(support);
        } 
    }
}
