﻿using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureLayer.Data
{
    public class MyDemoDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public MyDemoDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Theater> Theater { get; set; }
        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<SeatBooking> SeatBooking { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Genre
            modelBuilder
            .Entity<Movie>()
            .Property(d => d.Genre)
            .HasConversion(new EnumToStringConverter<MovieGenre>());

            // BookingStatus
            modelBuilder
            .Entity<Booking>()
            .Property(d => d.BookingStatus)
            .HasConversion(new EnumToStringConverter<BookingStatus>());

            #region Application User

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(m => m.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            #endregion

            #region Movie

            modelBuilder.Entity<Movie>()
            .HasMany(m => m.Bookings)
            .WithOne(b => b.Movie)
            .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.ShowTime)
                .WithOne(st => st.Movie)
                .HasForeignKey(st => st.MovieId);

            #endregion

            #region Theater

            modelBuilder.Entity<Theater>()
            .HasMany(m => m.Seats)
            .WithOne(b => b.Theater)
            .HasForeignKey(b => b.TheaterId);

            modelBuilder.Entity<Theater>()
           .HasMany(m => m.ShowTime)
           .WithOne(b => b.Theater)
           .HasForeignKey(b => b.TheaterId);

            #endregion

            #region Booking

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Movie)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ShowTime)
                .WithMany(st => st.Bookings)
                .HasForeignKey(b => b.ShowTimeId);

            modelBuilder.Entity<Booking>()
                .HasMany(m => m.SeatBooking)
                .WithOne(b => b.Booking)
                .HasForeignKey(b => b.BookingId);

            #endregion

            #region Reviews

            modelBuilder.Entity<Reviews>()
               .HasOne(b => b.User)
               .WithMany(u => u.Reviews)
               .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Reviews>()
                .HasOne(b => b.Movie)
                .WithMany(u => u.Reviews)
                .HasForeignKey(b => b.MovieId);

            #endregion

            #region SeatBooking

            modelBuilder.Entity<SeatBooking>()
               .HasOne(b => b.Booking)
               .WithMany(u => u.SeatBooking)
               .HasForeignKey(b => b.BookingId);

            modelBuilder.Entity<SeatBooking>()
                .HasOne(b => b.Seats)
                .WithMany(u => u.SeatBooking)
                .HasForeignKey(b => b.SeatId);

            #endregion

            #region Seats

            modelBuilder.Entity<Seats>()
               .HasOne(b => b.Theater)
               .WithMany(u => u.Seats)
               .HasForeignKey(b => b.TheaterId);

            modelBuilder.Entity<Seats>()
                .HasOne(b => b.ShowTime)
                .WithMany(u => u.Seats)
                .HasForeignKey(b => b.ShowTimeId);

            modelBuilder.Entity<Seats>()
                .HasMany(m => m.SeatBooking)
                .WithOne(b => b.Seats)
                .HasForeignKey(b => b.SeatId);

            #endregion

            #region ShowTime

            modelBuilder.Entity<ShowTime>()
               .HasOne(b => b.Movie)
               .WithMany(u => u.ShowTime)
               .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<ShowTime>()
                .HasOne(b => b.Theater)
                .WithMany(u => u.ShowTime)
                .HasForeignKey(b => b.TheaterId);


            modelBuilder.Entity<ShowTime>()
                .HasMany(m => m.Bookings)
                .WithOne(r => r.ShowTime)
                .HasForeignKey(r => r.ShowTimeId);

            modelBuilder.Entity<ShowTime>()
         .HasMany(s => s.Seats)
         .WithOne(seat => seat.ShowTime)
         .HasForeignKey(seat => seat.ShowTimeId)
         .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
