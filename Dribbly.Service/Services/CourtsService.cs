﻿using Dribbly.Authentication.Services;
using Dribbly.Core.Enums.Permissions;
using Dribbly.Core.Utilities;
using Dribbly.Model;
using Dribbly.Model.Courts;
using Dribbly.Model.Games;
using Dribbly.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dribbly.Service.Services
{
    public class CourtsService : BaseEntityService<CourtModel>, ICourtsService
    {
        IAuthContext _context;
        ISecurityUtility _securityUtility;
        IFileService _fileService;
        ICourtsRepository _courtsRepo;
        IAccountRepository _accountRepo;

        public CourtsService(IAuthContext context,
            ISecurityUtility securityUtility,
            IFileService fileService,
            ICourtsRepository courtsRepo,
            IAccountRepository accountRepo) : base(context.Courts)
        {
            _context = context;
            _securityUtility = securityUtility;
            _fileService = fileService;
            _courtsRepo = courtsRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IEnumerable<CourtModel>> GetAllAsync()
        {
            var allCourts = All();

            foreach (var court in allCourts)
            {
                await PopulateOwner(court);
            }

            return allCourts;
        }

        public async Task<CourtModel> GetCourtAsync(long id)
        {
            CourtModel court = GetById(id);
            await PopulateOwner(court);
            return court;
        }

        private async Task PopulateOwner(CourtModel court)
        {
            court.Owner = await _accountRepo.GetAccountBasicInfo(court.OwnerId);
        }

        public async Task<IEnumerable<CourtModel>> FindCourtsAsync(CourtSearchInputModel input)
        {
            return await _courtsRepo.FindCourtsAsync(input);
        }

        public long Register(CourtModel court)
        {
            court.OwnerId = _securityUtility.GetUserId();
            Add(court);
            _context.SaveChanges();
            return court.Id;
        }

        public IEnumerable<PhotoModel> AddPhotos(long courtId)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            List<PhotoModel> photos = new List<PhotoModel>();
            for (int i = 0; i < files.Count; i++)
            {
                photos.Add(AddCourtPhoto(courtId, files[i]));
                _context.SaveChanges();
            }

            return photos;
        }

        public async Task DeletePhotoAsync(long courtId, long photoId)
        {
            CourtPhotoModel courtPhoto = await _context.CourtPhotos.Include(p2 => p2.Photo)
                .SingleOrDefaultAsync(p => p.CourtId == courtId && p.PhotoId == photoId);
            if (courtPhoto == null)
            {
                throw new InvalidOperationException("Photo not found.");
            }
            else
            {
                if (courtPhoto.Photo.UploadedById == _securityUtility.GetUserId())
                {
                    courtPhoto.Photo.DateDeleted = DateTime.UtcNow;
                    _context.CourtPhotos.AddOrUpdate(courtPhoto);
                    _context.SaveChanges();
                }
                else
                {
                    throw new UnauthorizedAccessException("You do not have access to delete this photo.");
                }
            }
        }

        public void UpdateCourtPhoto(long courtId)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            string uploadPath = _fileService.Upload(files[0], "court/");
            CourtModel court = GetById(courtId);
            court.PrimaryPhotoUrl = uploadPath;
            PhotoModel photo = new PhotoModel
            {
                Url = uploadPath,
                UploadedById = _securityUtility.GetUserId(),
                DateAdded = DateTime.Now
            };
            _context.Photos.Add(photo);
            _context.CourtPhotos.Add(new CourtPhotoModel
            {
                CourtId = courtId,
                PhotoId = photo.Id
            });
            UpdateCourt(court);
        }

        public void UpdateCourt(CourtModel court)
        {
            if(court.OwnerId == _securityUtility.GetUserId() || AuthenticationService.HasPermission(CourtPermission.UpdateNotOwned))
            {
                Update(court);
                _context.SaveChanges();
            }
            else
            {
                throw new UnauthorizedAccessException("Authorization failed when attempting to update court details.");
            }
        }

        public IEnumerable<PhotoModel> GetCourtPhotos(long courtId)
        {
            //CourtModel court = _context.Courts.Include(c => c.Photos.Select(p => p.Photo).Where(p1=>p1.DateDeleted == null))
            //    .FirstOrDefault(_court => _court.Id == courtId);
            //return court.Photos.Select(p => p.Photo).OrderByDescending(x => x.DateAdded);
            return _context.CourtPhotos.Include(p1=>p1.Photo)
                .Where(p => p.CourtId == courtId && p.Photo.DateDeleted == null)
                .Select(p => p.Photo).OrderByDescending(x => x.DateAdded);
        }

        private PhotoModel AddCourtPhoto(long courtId, HttpPostedFile file)
        {
            string uploadPath = _fileService.Upload(file, "court/");
            PhotoModel photo = new PhotoModel
            {
                Url = uploadPath,
                UploadedById = _securityUtility.GetUserId(),
                DateAdded = DateTime.Now
            };
            _context.Photos.Add(photo);
            _context.CourtPhotos.Add(new CourtPhotoModel
            {
                CourtId = courtId,
                PhotoId = photo.Id
            });

            return photo;
        }

        public IEnumerable<GameModel> GetCourtGames(long courtId)
        {
            var games = _context.Games.Where(g => g.CourtId == courtId).ToList();
            return games;
        }
    }
}