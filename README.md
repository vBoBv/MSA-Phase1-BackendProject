# MSA-2020 - Database and API
App url: https://aspdotnet-ponhvath.azurewebsites.net

## Query Editor Screenshots
![Address Table](backendImgs/1.png)
![Student Table](backendImgs/2.png)

## Swagger UI Screenshot
![Swagger UI](backendImgs/3.png)

## Microsoft Learn Modules
![Module 1](backendImgs/4.png)
![Module 2](backendImgs/5.png)
![Module 3](backendImgs/6.png)

## Project Structure
### 1) Models
These models are classes which represent the entities for this assignment.
* Student
* Address

Inside Models, I have also created a folder called `Dtos` which stands for `Data Transfer Objects`. This folder includes all the data transfer objects which is used to transfer the data between the Client and the Server. In case we need to refactor our domain model, it reduces the chance of our application from breaking. In convention, API should not be receiving domain objects, this is because a hacker can inject any additional data into some property that should not be updated. So, in order to prevent this from happening, different `DTO` structure can be used to hide any sensitive data depending on the API actions.

### 2) DbContext
I have one dbContext which is `StudentContext` acts as a bridge to communicate data between my domain/entity classes with SQL database that is hosted on Azure. Inside StudentContext, DbSet is being used to query, read or write information about Student and Address.

* StudentContext
```tsx
public DbSet<Student> Student { get; set; }
public DbSet<Address> Address { get; set; }
```
### 3) Repository Pattern
[Conceptually, a Repository encapsulates the set of objects persisted in a data store and the operations performed over them, providing a more object-oriented view of the persistence layer.](https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7)
A repository in practice acts as a collection of objects in memory. This means that it should include methods that are used to query collection such as add(), remove(), get() and more. In this assignment, I created two interfaces `IStudentRepository` and `IAddressRepository` containing the collection methods. After that, I implemented `StudentRepository` and `AddressRepository` which utilises the interfaces above. These two repository classes use the `StudentContext` to make query to the database.
Address Interface:
```tsx
public interface IAddressRepository
{
  ICollection<Address> GetAddresses();
  ICollection<Address> GetAddressesOfStudent(int studentId);
  Address GetAddress(int addressId);
  bool CreateAddress(Address address);
  bool UpdateAddress(Address address);
  bool DeleteAddress(Address address);
  bool AddressExists(int id);
  bool Save();
}
```
Address Repository:
```tsx
public class AddressRepository : IAddressRepository
  {
  private readonly StudentContext _db;

  public AddressRepository(StudentContext db)
  {
      _db = db;
  }

  public bool CreateAddress(Address address)
  {
      _db.Address.Add(address);
      return Save();
  }

  public bool DeleteAddress(Address address)
  {
      _db.Address.Remove(address);
      return Save();
  }

  public Address GetAddress(int addressId)
  {
      return _db.Address.Include(a => a.Student).FirstOrDefault(a => a.addressId == addressId); ;
  }

  public ICollection<Address> GetAddresses()
  {
      return _db.Address.Include(a => a.Student).OrderBy(a => a.addressId).ToList();
  }

  public bool Save()
  {
      return _db.SaveChanges() >= 0 ? true : false;
  }

  public bool AddressExists(int id)
  {
      return _db.Address.Any(a => a.addressId == id);
  }

  public bool UpdateAddress(Address address)
  {
      _db.Address.Update(address);
      return Save();
  }

  public ICollection<Address> GetAddressesOfStudent(int studentId)
  {
      return _db.Address.Include(a => a.Student).Where(a => a.studentId == studentId).ToList();
  }
}
 ```

### 4) Controllers
The controllers is used to create API, which takes two arguments, a repository interface and an automapper instance. The repository minimises duplicated query logic. For instance, instead of `var allAddresses = _db.Address.Include(a => a.Student).FirstOrDefault(a => a.addressId == addressId);`, it can be shortened to `var allAddresses = _addressRepo.GetAddresses()` in the controller. Apart from that, AutoMapper is being used to map objects belonging to dissimilar types. In particular, I can map `AddressCreateDto` to `Address` as shown below: 
```tsx
public IActionResult CreateAddress([FromBody] AddressCreateDto addressDto)
{
  if (addressDto == null)
  {
      return BadRequest(ModelState);
  }

  var addressObj = _mapper.Map<Address>(addressDto);
  if (!_addressRepo.CreateAddress(addressObj))
  {
      ModelState.AddModelError("", $"Something went wrong when saving the record {addressObj.addressId}");
      return StatusCode(500, ModelState);
  }

  return CreatedAtAction("GetAddress", new { addressId = addressObj.addressId }, addressDto);
}
```


## Student Model & Address Model Relationship
For models, I have set a `one-to-many` relationship between `Student` table and `Address` table. In this case, Address table contains the attribute `studentId` which is a foreign key. Moreover, in the Address entity, I have also included the navigation property called `Student`. The main reason that the one-to-many relationship was chosen between Student and Address was because it was explicitly stated as a requirement for this assignment. There have been some debates that Address can belong to many students as well (in this case, a `many-to-many` relationship between Student and Address). This many-to-many relationship can be resolved by creating a middle table, but I chose to stay with the one-to-many relationship as this is a requirement stated and simpler to implement at this stage.

Student Model:
```tsx
public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int studentId { get; set; }
    [Required, MaxLength(100)]
    public string firstName { get; set; }
    public string middleName { get; set; }
    [Required]
    public string lastName { get; set; }
    public string emailAddress { get; set; }
    public string phoneNumber { get; set; }
    public DateTime timeCreated { get; set; }
}
```
Address Model:
```tsx
public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int addressId { get; set; }
    [Required]
    public int studentId { get; set; }
    [Required]
    public int streetNumber { get; set; }
    [Required]
    public string street { get; set; }
    [Required]
    public string suburb { get; set; }
    [Required]
    public string city { get; set; }
    public int postCode { get; set; }
    [Required]
    public string country { get; set; }
    public DateTime timeCreated { get; set; }

    [ForeignKey("studentId")]
    public Student Student { get; set; }
}
```
## References
https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
