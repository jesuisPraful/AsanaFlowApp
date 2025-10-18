using AsanaFlowDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsanaFlowDataAccessLayer
{
    public class AsanaFlowRepository
    {
        private readonly AsanaFlowDbContext _context;
        public AsanaFlowRepository()
        {
            _context = new AsanaFlowDbContext();
        }

        #region User
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<User>?> GetUserAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            try
            {
                var users = await _context.Users.Where(u => u.Name == userName).ToListAsync();
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            try
            {
                var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0.", nameof(userId));

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdatePasswordAsync(string email,string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if(newPassword.Length < 8) throw new ArgumentException(nameof(newPassword));
            try
            {
                var users = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
                if (users == null)
                {
                    return -1;
                }
                users.PasswordHash = newPassword;
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public async Task<int> UpdateUserNameAsync(string newUserName, string userEmail)
        {

            if (string.IsNullOrWhiteSpace(newUserName))
                throw new ArgumentNullException(nameof(newUserName));

            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentNullException(nameof(userEmail));



            try
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    return -1;
                }
                user.Name = newUserName;
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return -99;
            }
        }

        public async Task<int> DeleteUserAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {

                return -99;
            }
        }
        #endregion

        #region YogaPose
        public async Task<List<YogaPose>> GetAllYogaPosesAsync()
        {
            try
            {
                return await _context.YogaPoses.ToListAsync();
            }
            catch (Exception)
            {
                return new List<YogaPose>();
            }
        }

        public async Task<YogaPose?> GetYogaPoseByIdAsync(int poseId)
        {
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));

            try
            {
                var pose = await _context.YogaPoses.FirstOrDefaultAsync(p => p.PoseId == poseId);
                return pose;
            }
            catch (Exception)
            {
                return new YogaPose();
            }
        }

        public async Task<YogaPose?> GetYogaPoseByNameAsync(string poseName)
        {
            if (poseName == null) throw new ArgumentOutOfRangeException(nameof(poseName));

            try
            {
                var pose = await _context.YogaPoses.FirstOrDefaultAsync(p => p.PoseName == poseName);
                if (pose == null)
                {
                    return pose;
                }
                return pose;
            }
            catch (Exception)
            {
                return new YogaPose();
            }
        }

        public async Task<List<YogaPose>> GetYogaPosesByBenefitAsync(string benefit)
        {
            if (benefit == null) throw new ArgumentOutOfRangeException(nameof(benefit));

            try
            {
                var poses = await _context.YogaPoses.Where(p => p.Benefits == benefit).ToListAsync();
                return poses;
            }
            catch (Exception)
            {
                return new List<YogaPose>();
            }
        }

        public async Task<List<YogaPose>> GetYogaPosesByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentOutOfRangeException(nameof(categoryId));

            try
            {
                var poses = await _context.YogaPoses.Where(p => p.CategoryId == categoryId).ToListAsync();
                return poses;
            }
            catch (Exception)
            {
                return new List<YogaPose>();
            }
        }

        public async Task<List<YogaPose>> GetYogaPosesByCategoryNameAsync(string categoryName)
        {
            if (categoryName == null) throw new ArgumentOutOfRangeException(nameof(categoryName));

            try
            {
                var id = await _context.YogaCategories.Where(c => c.CategoryName == categoryName).Select(c => c.CategoryId).FirstOrDefaultAsync();
                var poses = await _context.YogaPoses.Where(p => p.CategoryId == id).ToListAsync();
                return poses;
            }
            catch (Exception)
            {
                return new List<YogaPose>();
            }
        }

        public async Task<bool> AddYogaPose(YogaPose yogaPose)
        {
            if (yogaPose == null) throw new ArgumentNullException(nameof(yogaPose));
            try
            {
                await _context.YogaPoses.AddAsync(yogaPose);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateYogaPosesNameByAsync(int poseId, string poseName)
        {
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));
            if (poseName == null) throw new ArgumentOutOfRangeException(nameof(poseName));

            try
            {
                var yogaPose = await _context.YogaPoses.FindAsync(poseId);
                if (yogaPose == null)
                {
                    return false;
                }
                yogaPose.PoseName = poseName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateYogaPosesByBenefitAsync(int poseId, string benefit)
        {
            if (benefit == null) throw new ArgumentOutOfRangeException(nameof(benefit));
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));

            try
            {
                var pose = await _context.YogaPoses.FirstOrDefaultAsync(p => p.PoseId == poseId);
                if (pose == null)
                {
                    return false;
                }

                pose.Benefits = benefit;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateYogaPoseInstructionsAsync(int poseId, string instructions)
        {
            if (instructions == null) throw new ArgumentOutOfRangeException(nameof(instructions));
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));

            try
            {
                var pose = await _context.YogaPoses.FirstOrDefaultAsync(p => p.PoseId == poseId);
                if (pose == null)
                {
                    return false;
                }
                pose.Instructions = instructions;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateYogaPosePrecautionsAsync(int poseId, string precautions)
        {
            if (precautions == null) throw new ArgumentOutOfRangeException(nameof(precautions));
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));

            try
            {
                var pose = await _context.YogaPoses.FirstOrDefaultAsync(p => p.PoseId == poseId);
                if (pose == null)
                {
                    return false;
                }
                pose.Instructions = precautions;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> DeleteYogaPoseAsync(int poseId)
        {
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));
            try
            {
                var pose = await _context.YogaPoses.FindAsync(poseId);
                if (pose == null) return -1;

                _context.YogaPoses.Remove(pose);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception)
            {
                return -99;
            }
        }



        #endregion

        #region YogaCategories
        public async Task<List<YogaCategory>> GetAllYogaCategoriesAsync()
        {
            try
            {
                var yogaCategories = await _context.YogaCategories.ToListAsync();

                if (yogaCategories == null || yogaCategories.Count == 0)
                {
                    return new List<YogaCategory>();
                }

                return yogaCategories;


            }
            catch (Exception)
            {
                return new List<YogaCategory>();
            }
        }

        public async Task<YogaCategory> GetCategoryByNameAsync(string yogaCategoryName)
        {
            try
            {
                var category = await _context.YogaCategories.Where(c => c.CategoryName == yogaCategoryName).FirstOrDefaultAsync();
                if (category == null)
                {
                    return new YogaCategory();
                }
                return category;
            }
            catch (Exception)
            {
                return new YogaCategory();
            }
        }

        public async Task<bool> AddYogaCategoryAsync(YogaCategory yogaCategory)
        {
            if (yogaCategory == null) throw new ArgumentNullException(nameof(yogaCategory));
            try
            {
                await _context.YogaCategories.AddAsync(yogaCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> UpdateYogaCategoryDescriptionById(int categoryId, string description)
        {
            if (categoryId == null) throw new ArgumentOutOfRangeException(nameof(categoryId));
            if (description == null) throw new ArgumentOutOfRangeException(nameof(description));
            try
            {
                var category = await _context.YogaCategories.FindAsync(categoryId);
                if (category == null)
                {
                    return false;
                }
                category.Description = description;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteYogaCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentOutOfRangeException(nameof(categoryId));

            try
            {
                var category = await _context.YogaCategories.FindAsync(categoryId);
                if (category == null)
                {
                    return false;
                }
                await _context.YogaCategories.FindAsync(categoryId);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region UserProgress
        public async Task<List<UserProgress>> GetAllUserProgressAsync()
        {
            try
            {
                return await _context.UserProgresses.ToListAsync();
            }
            catch (Exception)
            {
                return new List<UserProgress>();
            }
        }

        public async Task<UserProgress> GetUserProgressByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            try
            {
                var userProgress = await _context.UserProgresses.Where(UP => UP.UserId == userId).FirstOrDefaultAsync();
                if (userProgress == null)
                {
                    return new UserProgress();
                }
                return userProgress;
            }
            catch (Exception)
            {
                return new UserProgress();
            }
        }

        public async Task<UserProgress> GetUserProgressByProgressIdAsync(int progressId)
        {
            if (progressId <= 0) throw new ArgumentOutOfRangeException(nameof(progressId));
            try
            {
                var userProgress = await _context.UserProgresses.FindAsync(progressId);
                if (userProgress == null)
                {
                    return new UserProgress();
                }
                return userProgress;
            }
            catch (Exception)
            {
                return new UserProgress();
            }
        }
        public async Task<bool> AddUserProgressAsync(UserProgress userProgress)
        {
            if (userProgress == null) throw new ArgumentNullException(nameof(userProgress));

            try
            {
                await _context.UserProgresses.AddAsync(userProgress);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserProgressAsync(int progressId, int userId, int poseId, int proficiencyLevel)
        {
            if (progressId <= 0) throw new ArgumentOutOfRangeException(nameof(progressId));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (proficiencyLevel < 0 || proficiencyLevel > 10) throw new ArgumentOutOfRangeException(nameof(proficiencyLevel));
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));

            try
            {
                var progress = await _context.UserProgresses.Where(UP => UP.ProgressId == progressId && UP.UserId == userId && UP.PoseId == poseId).FirstOrDefaultAsync();
                if (progress == null)
                {
                    return false;
                }
                progress.ProficiencyLevel = proficiencyLevel;
                progress.LastPracticed = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserProgressAsync(int progressId)
        {
            if (progressId <= 0) throw new ArgumentOutOfRangeException(nameof(progressId));

            try
            {
                var progress = await _context.UserProgresses.FindAsync(progressId);
                if (progress == null) return false;

                _context.UserProgresses.Remove(progress);
                await _context.SaveChangesAsync();
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Session
        public async Task<List<Session>> GetAllSessionsAsync()
        {
            try
            {
                return await _context.Sessions.ToListAsync();
            }
            catch (Exception)
            {
                return new List<Session>();
            }
        }

        public async Task<Session> GetSessionBySessionIdAsync(int sessionId)
        {
            if (sessionId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionId));
            try
            {
                var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
                if (session == null)
                {
                    return new Session();
                }
                return session;
            }
            catch (Exception)
            {
                return new Session();
            }
        }

        public async Task<List<Session>> GetSessionsByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            try
            {
                var sessions = await _context.Sessions.Where(s => s.UserId == userId).ToListAsync();
                if (sessions == null || sessions.Count == 0)
                {
                    return new List<Session>();
                }
                return sessions;
            }
            catch (Exception)
            {
                return new List<Session>();
            }
        }

        public async Task<List<Session>> GetSessionsByUserIdAsync(int sessionId, int userId)
        {
            if (sessionId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionId));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            try
            {
                var sessions = await _context.Sessions.Where(s => s.SessionId == sessionId && s.UserId == userId).ToListAsync();
                if (sessions == null || sessions.Count == 0)
                {
                    return new List<Session>();
                }
                return sessions;
            }
            catch (Exception)
            {
                return new List<Session>();
            }
        }


        public async Task<bool> AddSessionAsync(Session session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            try
            {
                await _context.Sessions.AddAsync(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSessionAsync(int sessionId)
        {
            if (sessionId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionId));

            try
            {
                var session = await _context.Sessions.FindAsync(sessionId);
                if (session == null) return false;

                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion

        #region SessionPose
        public async Task<List<SessionPose>> GetAllSessionPosesAsync()
        {
            try
            {
                return await _context.SessionPoses.ToListAsync();
            }
            catch (Exception)
            {
                return new List<SessionPose>();
            }
        }

        //public async Task<SessionPose> GetSessionPoseBySessionId(int sessionPoseId)
        //{
        //    if (sessionPoseId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionPoseId));
        //    try
        //    {
        //        var sessionPose = await _context.SessionPoses.Where(sp => sp.SessionPoseId == sessionPoseId).FirstOrDefaultAsync();
        //        if(sessionPose == null)
        //        {
        //            return new SessionPose();
        //        }
        //        return sessionPose;
        //    }
        //    catch (Exception)
        //    {
        //        return new SessionPose();
        //    }
        //}

        public async Task<List<SessionPose>> GetSessionPosesBySessionIdAsync(int sessionId)
        {
            if (sessionId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionId));
            try
            {
                var sessionPoses = await _context.SessionPoses.Where(sp => sp.SessionId == sessionId).ToListAsync();
                if (sessionPoses == null || sessionPoses.Count == 0)
                {
                    return new List<SessionPose>();
                }
                return sessionPoses;
            }
            catch (Exception)
            {
                return new List<SessionPose>();
            }
        }
        public async Task<List<SessionPose>> GetSessionPosesByPoseIdAsync(int poseId)
        {
            if (poseId <= 0) throw new ArgumentOutOfRangeException(nameof(poseId));
            try
            {
                var sessionPoses = await _context.SessionPoses.Where(sp => sp.PoseId == poseId).ToListAsync();
                if (sessionPoses == null || sessionPoses.Count == 0)
                {
                    return new List<SessionPose>();
                }
                return sessionPoses;
            }
            catch (Exception)
            {
                return new List<SessionPose>();
            }
        }

        public async Task<SessionPose> GetSessionPoseByIdAsync(int sessionPoseId)
        {
            if (sessionPoseId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionPoseId));
            try
            {
                var sessionPose = await _context.SessionPoses.FindAsync(sessionPoseId);
                if (sessionPose == null)
                {
                    return new SessionPose();
                }
                return sessionPose;
            }
            catch (Exception)
            {
                return new SessionPose();
            }
        }

        public async Task<bool> AddSessionPoseAsync(SessionPose sessionPose)
        {
            if (sessionPose == null) throw new ArgumentNullException(nameof(sessionPose));

            try
            {
                await _context.SessionPoses.AddAsync(sessionPose);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSessionPoseAsync(int sessionPoseId)
        {
            if (sessionPoseId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionPoseId));

            try
            {
                var sessionPose = await _context.SessionPoses.FindAsync(sessionPoseId);
                if (sessionPose == null) return false;

                _context.SessionPoses.Remove(sessionPose);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSessionPosesBySessionIdAsync(int sessionId)
        {
            if (sessionId <= 0) throw new ArgumentOutOfRangeException(nameof(sessionId));

            try
            {
                var sessionPoses = await _context.SessionPoses.Where(sp => sp.SessionId == sessionId).ToListAsync();
                if (sessionPoses == null || sessionPoses.Count == 0) return false;

                _context.SessionPoses.RemoveRange(sessionPoses);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}