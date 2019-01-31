using ProERP.Core.Data;
using ProERP.Data.Models;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.User
{
    public class UserService
    {

        private readonly IRepository<Data.Models.User> _userRepository;
        private readonly IRepository<Data.Models.UserRole> _userRoleRepository;
        private readonly IRepository<Data.Models.UserAssignment> _userAssignments;
        private readonly IRepository<Data.Models.UserProfile> _userProfileRepository;
        private readonly IRepository<Data.Models.Role> _userRole;
        private readonly IRepository<Data.Models.MaintenanceUserAssignment> _MUARepository;
        private readonly IRepository<Data.Models.PLMMVersion> _plmmVersionRepository;
        private readonly IRepository<Data.Models.VersionNote> _versionRepository;

        //  private readonly IRepository<Data.Models.Plant> _
        public UserService(IRepository<Data.Models.User> userRepository, IRepository<Data.Models.UserAssignment> userAssignments,
            IRepository<Data.Models.MaintenanceUserAssignment> muaRepository, IRepository<Data.Models.UserRole> userRoleRepository, 
            IRepository<Data.Models.Role> userRole,IRepository<Data.Models.UserProfile> userProfileRepository,
            IRepository<Data.Models.VersionNote> versionRepository,
            IRepository<Data.Models.PLMMVersion> plmmVersionRepository)
        {
            this._userRepository = userRepository;
            this._userAssignments = userAssignments;
            this._MUARepository = muaRepository;
            this._userRoleRepository = userRoleRepository;
            this._userRole = userRole;
            this._userProfileRepository = userProfileRepository;
            this._plmmVersionRepository = plmmVersionRepository;
            this._versionRepository = versionRepository;
        }
        public List<Data.Models.User> GetAll()
        {
            int[] roleIds = { };
            //return this._userRepository.Table.ToList();
            return this._userRoleRepository.Table.Where(w => w.RoleId > 3).Select(s => s.User).ToList();
        }
        public List<Data.Models.User> GetAllUserForRemarks()
        {
            int[] roleIds = { };
            return this._userRoleRepository.Table.Select(s => s.User).ToList();
        }
        public List<ListUserViewModel> GetAllUsers(string UserName, string Email)
        {
            string[] adminRoles = new string[] { "SysAdmin", "Admin" };
            var allData = from a in this._userRoleRepository.Table
                          join b in this._userRepository.Table on a.UserId equals b.Id
                          join c in this._userProfileRepository.Table on b.UserProfile_Id equals c.Id into j1
                          from d in j1.DefaultIfEmpty()
                          where !adminRoles.Contains(a.Role.Name)
                          select new ListUserViewModel
                          {
                              Id = b.Id,
                              UserProfile_Id = b.UserProfile_Id,
                              FirstName = (d == null ? "" : d.FirstName),
                              LastName = (d == null ? "" : d.LastName),
                              UserName = b.UserName,
                              Email = b.Email,
                              IsActive = (d == null ? true : d.IsActive),
                              IsBlocked = (d == null ? false : d.IsBlocked)
                          };
            //var allData =  this._userRepository.Table;

            if (!string.IsNullOrEmpty(UserName))
                allData = allData.Where(w => w.UserName.ToLower().Contains(UserName.Trim().ToLower()));

            if (!string.IsNullOrEmpty(Email))
                allData = allData.Where(w => w.Email.ToLower().Contains(Email.Trim().ToLower()));

            return allData.ToList();
        }
        public void UpdateUserData(int Id, bool IsActive, bool IsBlocked)
        {
            var existingUserProfile = this._userProfileRepository.Table.FirstOrDefault(w => w.Id == Id);
            if (existingUserProfile != null)
            {
                existingUserProfile.IsActive = IsActive;
                existingUserProfile.IsBlocked = IsBlocked;
                this._userProfileRepository.Update(existingUserProfile);
            }
        }
        //public int? GetUserProfileId(int userId)
        //{
        //    return this._userRepository.Table.Where(w => w.Id == userId).Select(s => s.UserProfile_Id).FirstOrDefault();

        //}
        public int Update(string fname, string lname, string email, int userId)
        {
            var profileId = this._userRepository.Table.Where(w => w.Id == userId).Select(s => s.UserProfile_Id).FirstOrDefault();
            Data.Models.UserProfile oldProfile = _userProfileRepository.Table.Where(w => w.Id == profileId).FirstOrDefault();
            Data.Models.User oldEmail = _userRepository.Table.Where(w => w.Id == userId).FirstOrDefault();

            if (_userRepository.Table.Any(a => a.Email == email && a.Id != userId))
                return 101;

            oldEmail.Email = email;
            _userRepository.Update(oldEmail);

            if (oldProfile != null)
            {

                oldProfile.FirstName = fname;
                oldProfile.LastName = lname;
                _userProfileRepository.Update(oldProfile);
            }
            else
            {
                var newUserProfile = new Data.Models.UserProfile { FirstName = fname, LastName = lname, IsActive = true, IsBlocked = false };
                _userProfileRepository.Insert(newUserProfile);
                var userExisting = _userRepository.Table.FirstOrDefault(w => w.Id == userId);
                userExisting.UserProfile_Id = newUserProfile.Id;
                _userRepository.Update(userExisting);
            }
            return 0;
        }
        public void GetUsersFnameLname(int userId, out string firstName, out string lastName)
        {
            firstName = "";
            lastName = "";
            var allData = from s in this._userRepository.Table
                          join e in this._userProfileRepository.Table on s.UserProfile_Id equals e.Id into j1
                          from d in j1.DefaultIfEmpty()
                          where s.Id == userId
                          select new { d.FirstName, d.LastName };
            var result = allData.FirstOrDefault();

            if (result != null)
            {
                firstName = result.FirstName;
                lastName = result.LastName;
            }
        }
        
        public void GetFnameLname(string uname, out string firstName, out string lastName)
        {
            firstName = "";
            lastName = "";
            var allData = from s in this._userRepository.Table
                          join e in this._userProfileRepository.Table on s.UserProfile_Id equals e.Id into j1
                          from d in j1.DefaultIfEmpty()
                          where s.UserName == uname
                          select new { d.FirstName, d.LastName };
            var result = allData.FirstOrDefault();

            if (result != null)
            {
                firstName = result.FirstName;
                lastName = result.LastName;
            }
        }

        public List<Data.Models.Role> GetAllUserRole(string name)
        {
            var allData = this._userRole.Table;

            allData = allData.Where(w => w.Name != "SysAdmin" && w.Name != "Admin");

            return allData.ToList();
        }

        public string GetUserRoleById(int userId)
        {
            var allData = this._userRoleRepository.Table.FirstOrDefault(W => W.UserId == userId);
            return allData.Role.Name;
        }

        public void Add(Data.Models.UserProfile userprofile)
        {
            _userProfileRepository.Insert(userprofile);
            //return userprofile.Id;
        }

        public List<int> GetAllByUserAssignments(int PreventiveMaintenanceId)
        {
            return this._userAssignments.Table.Where(w => w.PreventiveMaintenanceId == PreventiveMaintenanceId).Select(s => s.UserId).ToList();
        }

        public string[] GetWhatsNewNotes()
        {
            return this._versionRepository.Table.Where(w => w.PLMMVersion.ReleaseDate == this._versionRepository.Table.Max(m => m.PLMMVersion.ReleaseDate)).Select(s => s.Notes).ToArray();
        }

        public string GetVersion()
        {
            return this._versionRepository.Table.Where(w=>w.PLMMVersion.ReleaseDate == this._versionRepository.Table.Max(m => m.PLMMVersion.ReleaseDate)).Select(s => s.PLMMVersion.ReleaseVersion).FirstOrDefault();
        }

        public void IsVersionUpdated(int UserId, bool VersionStatus)
        {
            var oldProfile = this._userRepository.Table.Where(w => w.Id == UserId).Select(s => s.UserProfile).FirstOrDefault();
            if (oldProfile != null)
            {
                oldProfile.IsVersionUpdated = VersionStatus;
                _userProfileRepository.Update(oldProfile);
            }
        }

        public bool GetVersionStatus(int userId)
        {
            var profileId = this._userRepository.Table.Where(w => w.Id == userId).Select(s => s.UserProfile_Id).FirstOrDefault();
            var Status= _userProfileRepository.Table.Where(w => w.Id == profileId).Select(s=>s.IsVersionUpdated).FirstOrDefault();
            return Status;
        }
        public List<Data.Models.VersionNote> GetAllNotes()
        {
            var allData = this._versionRepository.Table;
            allData.Select(s => new
            {
                s.Id,
                s.Notes,
                s.VersionId,
                s.PLMMVersion.ReleaseDate,
                s.PLMMVersion.ReleaseVersion
            }).GroupBy(g=> new { g.ReleaseDate,g.ReleaseVersion });
            return allData.ToList();
        }
        public List<Data.Models.VersionNote> GetNotes(int VersionId)
        {
            var allData = this._versionRepository.Table.Where(w=>w.VersionId == VersionId);
            allData.Select(s => new
            {
                s.Id,
                s.Notes,
                s.VersionId,
            });
            return allData.ToList();
        }
        public List<Data.Models.PLMMVersion> GetAllVersion()
        {
            var allData = this._plmmVersionRepository.Table;
            allData.Select(s => new
            {
                s.Id,
                s.ReleaseDate,
                s.ReleaseVersion
            }).GroupBy(g => new { g.ReleaseDate, g.ReleaseVersion });
            return allData.ToList();
        }
        public List<Data.Models.VersionNote> getVersionId(int VersionId)
        {
            return this._versionRepository.Table.Where(w=>w.VersionId == VersionId).ToList();
        }
        public void SaveNotesData(VersionGridModel NotesData,int[] DeletedNotesIds)
        {
            if (DeletedNotesIds != null && DeletedNotesIds.Length > 0)
            {
                var deleteNotesItems = this._versionRepository.Table.Where(w => DeletedNotesIds.Contains(w.Id));
                this._versionRepository.Delete(deleteNotesItems);
            }
            
            if (NotesData.Id == 0)
            {
                //Insert
                var plmmVersion = new PLMMVersion
                {
                    ReleaseDate = DateTime.UtcNow,
                    ReleaseVersion = NotesData.ReleaseVersion,
                    VersionNotes = new List<VersionNote>()
                };
                var notes = NotesData.Notes.ToArray();
                for (int j = 0; j < notes.Length; j++)
                {
                    VersionNotesModel item = notes[j];
                    plmmVersion.VersionNotes.Add(new Data.Models.VersionNote
                    {
                        Id = 0,
                        VersionId = plmmVersion.Id,
                        Notes = item.Notes
                    });
                }
                this._plmmVersionRepository.Insert(plmmVersion);

                List<UserProfile> profile = this._userProfileRepository.Table.Where(w => w.IsActive == true).ToList();
                foreach(UserProfile p in profile)
                {
                    p.IsVersionUpdated = true;
                }
                _userProfileRepository.Update(profile);
            }
            else
            {
                //update
                var notes = NotesData.Notes.ToArray();
                for(int k = 0; k < notes.Length; k++)
                {
                    VersionNotesModel item = notes[k];
                    Data.Models.VersionNote objNote = this._versionRepository.Table.FirstOrDefault(w => w.Id == item.Id);
                    if(item.Id > 0)
                    {
                        if(objNote.Notes != item.Notes)
                        {
                            objNote.Notes = item.Notes;
                            this._versionRepository.Update(objNote);
                        }
                    }
                    else
                    { //add new note in existing
                        var newNote = new VersionNote
                        {
                            VersionId = NotesData.Id,
                            Notes = item.Notes
                        };
                        this._versionRepository.Insert(newNote);
                    }
                }
            }   
        }

        public void DeleteVersion(int Id)
        {
            var existingNotes = _versionRepository.Table.Where(w => w.VersionId == Id);
            var existingVersion = _plmmVersionRepository.Table.Where(w => w.Id == Id);
            this._versionRepository.Delete(existingNotes);
            this._plmmVersionRepository.Delete(existingVersion);
        }
    }
}
