using Microsoft.Identity.Client;
using PacificToursApp.Shared;
using System.Net.Http.Headers;

namespace PacificToursApp.Server.Data
{
    // The DataContext class is a DbContext instance used to interact with the database.
    public class DataContext : DbContext
    {
        // The DataContext constructor takes a DbContextOptions<DataContext> instance and passes it to the base DbContext class.
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // The OnModelCreating method is overridden to configure the model that was discovered by convention from the entity types exposed in DbSet properties on your derived context.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding the Tour table with data.
            modelBuilder.Entity<Tour>().HasData(
            new Tour
            {
                TourId = 1,
                TourName = "Real Britain",
                TourDescription = "Discover the heart of England, Wales, and Scotland",
                TourImageUrl = "https://www.thewowstyle.com/wp-content/uploads/2015/02/1653100london.jpg",
                TourLength = 6,
                TourPrice = 1200.00m,
                TourSpacesAvailable = 30
            },
            new Tour
            {
                TourId = 2,
                TourName = "Britain and Ireland Explorer",
                TourDescription = "Venture across England, Wales, Scotland, and Ireland",
                TourImageUrl = "https://static.standard.co.uk/s3fs-public/thumbnails/image/2017/07/10/10/shutterstock-521968378.jpg?crop=8:5,smart&quality=75&auto=webp&width=1024",
                TourLength = 16,
                TourPrice = 2000.00m,
                TourSpacesAvailable = 40
            },
            new Tour
            {
                TourId = 3,
                TourName = "Best of Britain",
                TourDescription = "Visit the tourist attractions throughout England, Wales, and Scotland",
                TourImageUrl = "https://www.english-heritage.org.uk/siteassets/home/visit/places-to-visit/stonehenge/history/stonehenge-aerial-1440x612.jpg?w=1440&h=612&mode=crop&scale=both&quality=100&anchor=NoFocus&WebsiteVersion=20231208103628",
                TourLength = 12,
                TourPrice = 2900.00m,
                TourSpacesAvailable = 30
            }
            );

            // Seeding the Hotel table with data.
            modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                HotelId = 1,
                HotelName = "Hilton Hotel, London",
                HotelDescription = "Based in the heart of London, the Hilton hotel provides guests with an end-to-end luxury experience",
                HotelImageUrl = "https://tse4.mm.bing.net/th?id=OIP.tQSg-BLABZoeC2UTH5M9ygHaE7&pid=Api&P=0&h=180",
                SingleSuitePrice = 375.00m,
                DoubleSuitePrice = 775.00m,
                FamilySuitePrice = 950.00m,
            },
            new Hotel
            {
                HotelId = 2,
                HotelName = "Marriott Hotel, London",
                HotelDescription = "The Marriott hotel in London is high-end, with a range of premium suites available",
                HotelImageUrl = "https://tse4.mm.bing.net/th?id=OIP.zDlK-pAGg6Hsy1hwhmI6XQHaFB&pid=Api&P=0&h=180",
                SingleSuitePrice = 300.00m,
                DoubleSuitePrice = 500.00m,
                FamilySuitePrice = 900.00m,
            },
            new Hotel
            {
                HotelId = 3,
                HotelName = "Travelodge, Brighton Seafront",
                HotelDescription = "With unmatched views, the Travelodge at Brighton Seafront is a great option for travellers trying to budget without sacrificing quality",
                HotelImageUrl = "data:image/jpeg;base64,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",
                SingleSuitePrice = 80.00m,
                DoubleSuitePrice = 120.00m,
                FamilySuitePrice = 150.00m,
            },
            new Hotel
            {
                HotelId = 4,
                HotelName = "Kings Hotel, Brighton",
                HotelDescription = "A luxury hotel fit for a king!",
                HotelImageUrl = "https://tse2.mm.bing.net/th?id=OIP.wYTmPCEJZSwpSrsQL3XxLQHaE7&pid=Api&P=0&h=180",
                SingleSuitePrice = 180.00m,
                DoubleSuitePrice = 400.00m,
                FamilySuitePrice = 520.00m,
            },
            new Hotel
            {
                HotelId = 5,
                HotelName = "Leonardo Hotel, Brighton",
                HotelDescription = "Often described by travellers as a home away from home, the Leonardo Hotel in Brighton boasts a warm and welcoming setting",
                HotelImageUrl = "https://tse1.mm.bing.net/th?id=OIP.wtcKyFMWqbzGzeKuKfhjCwHaE8&pid=Api&P=0&h=180",
                SingleSuitePrice = 180.00m,
                DoubleSuitePrice = 400.00m,
                FamilySuitePrice = 520.00m,
                
            },
            new Hotel
            {
                HotelId = 6,
                HotelName = "Nevis Bank Inn, Fort William",
                HotelDescription = "Only a short walk from Fort William, Nevis Bank Inn is perfect for anyone who wants an authentic experience in the Scottish countryside",
                HotelImageUrl = "https://tse4.mm.bing.net/th?id=OIP.hsCg2iq55dahz5HxIV2OGwHaE6&pid=Api&P=0&h=180",
                SingleSuitePrice = 90.00m,
                DoubleSuitePrice = 100.00m,
                FamilySuitePrice = 155.00m,
            }
        );
            // Configuring the Tour entity:
            modelBuilder.Entity<Tour>()
            // Setting TourId as the primary key for the Tour entity.
            .HasKey(t => t.TourId);

            // Configuring the Hotel entity:
            modelBuilder.Entity<Hotel>()
            // Setting HotelId as the primary key for the Hotel entity.
            .HasKey(h => h.HotelId);

            // Configuring the User entity:
            modelBuilder.Entity<User>()
            // Setting UserId as the primary key for the User entity.
            .HasKey(u => u.UserId);

            // Configuring the HotelBookings entity:
            modelBuilder.Entity<HotelBookings>()
            // Setting BookingId as the primary key for the HotelBookings entity.
            .HasKey(hb => hb.BookingId);

            modelBuilder.Entity<HotelBookings>()
                // Defining a one-to-many relationship between User and HotelBookings entities.
                .HasOne(hb => hb.user)
                .WithMany()
                .HasForeignKey(hb => hb.UserId);

            modelBuilder.Entity<HotelBookings>()
                // Defining a one-to-many relationship between Hotel and HotelBookings entities.
                .HasOne(hb => hb.hotel)
                .WithMany()
                .HasForeignKey(hb => hb.HotelId);


            // Configuring the TourBookings entity:
            modelBuilder.Entity<TourBookings>()
            // Setting BookingId as the primary key for the TourBookings entity.
            .HasKey(tb => tb.BookingId);

            modelBuilder.Entity<TourBookings>()
                // Defining a one-to-many relationship between User and TourBookings entities.
                .HasOne(tb => tb.user)
                .WithMany()
                .HasForeignKey(tb => tb.UserId);

            modelBuilder.Entity<TourBookings>()
                // Defining a one-to-many relationship between Tour and TourBookings entities.
                .HasOne(tb => tb.tour)
                .WithMany()
                .HasForeignKey(tb => tb.TourId);

            // Configuring the PackageBookings entity:
            modelBuilder.Entity<PackageBookings>()
            // Setting BookingId as the primary key for the PackageBookings entity.
            .HasKey(pb => pb.BookingId);

            modelBuilder.Entity<PackageBookings>()
                // Defining a one-to-many relationship between User and PackageBookings entities.
                .HasOne(pb => pb.user)
                .WithMany()
                .HasForeignKey(pb => pb.UserId);

            modelBuilder.Entity<PackageBookings>()
                // Defining a one-to-many relationship between Tour and PackageBookings entities.
                .HasOne(pb => pb.tour)
                .WithMany()
                .HasForeignKey(pb => pb.TourId);

            modelBuilder.Entity<PackageBookings>()
                // Defining a one-to-many relationship between Hotel and PackageBookings entities.
                .HasOne(pb => pb.hotel)
                .WithMany()
                .HasForeignKey(pb => pb.HotelId);
        }
        // The Tours property is a DbSet representing the Tours table in the database.
        public DbSet<Tour> Tours { get; set; }
        // The Hotels property is a DbSet representing the Hotels table in the database.
        public DbSet<Hotel> Hotels { get; set; }
        // The Users property is a DbSet representing the Users table in the database.
        public DbSet<User> Users { get; set; }
        // The HotelBookings property is a DbSet representing the HotelBookings table in the database.
        public DbSet<HotelBookings> HotelBookings { get; set; }
        // The TourBookings property is a DbSet representing the TourBookings table in the database.
        public DbSet<TourBookings> TourBookings { get; set; }
        // The PackageBookings property is a DbSet representing the PackageBookings table in the databa
        public DbSet<PackageBookings> PackageBookings { get; set; }
    }
}