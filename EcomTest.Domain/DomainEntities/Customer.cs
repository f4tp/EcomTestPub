using EcomTest.Domain.DomainEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EcomTest.Domain.DomainEntities
{
    [Table("Customer")]
    public class Customer : Entity
    {
        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }

        //email address might be better in its own entity depending on whether you want the customer to be able to be able to change their email address / add new ones (which adds a new layer of complexity, especially for systems like Identity Server)
        [Required]
        [EmailAddress]
        public string Email { get; private set; }


        //Nav props
        private List<Order> _ordersPlaced;

        public IEnumerable<Order> OrdersPlaced { get { return _ordersPlaced; } }

        //Address would be required but this would be handled in another entity - one customer might have multiple addresses (e.g. postal), multiple customers might also have the same address (multiple people living there with different accounts), caters for these situations

        private Customer() { } // Private constructor for EF

        public Customer(string firstName, string lastName, string email)
        {
            //add validation logic to check props as expected, e.g. email address is in a valid format
            _ordersPlaced = new List<Order>();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public void AddOrder(Order order)
        {
            // Add validation logic
            _ordersPlaced.Add(order);
        }

        public void RemoveOrder(Order order)
        {
            // Add validation logic
            _ordersPlaced.Remove(order);
        }
    }
}
