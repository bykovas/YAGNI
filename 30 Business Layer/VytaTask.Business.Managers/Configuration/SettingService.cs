﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using VytaTask.Business.Contracts.Infrastructure;
//using VytaTask.Business.Contracts.Managers;
//using VytaTask.Business.Contracts.Repositories;
//using VytaTask.Business.Domain.Models.Configuration;
//using VytaTask.CrossCutting.Infrastructure;
//using VytaTask.Dal.Caching;

//namespace VytaTask.Business.Managers.Configuration
//{
//    public partial class SettingService : ISettingService
//    {
//        #region Constants

//        /// <summary>
//        /// Key for caching
//        /// </summary>
//        private const string SETTINGS_ALL_KEY = "Mr.setting.all";
//        /// <summary>
//        /// Key pattern to clear cache
//        /// </summary>
//        private const string SETTINGS_PATTERN_KEY = "Mr.setting.";

//        #endregion

//        #region Fields

//        private readonly IGenericRepository<Setting, int> _settingRepository;
//        private readonly IEventPublisher _eventPublisher;
//        private readonly ICacheManager _cacheManager;

//        #endregion

//        #region Ctor

//        /// <summary>
//        /// Ctor
//        /// </summary>
//        /// <param name="cacheManager">Cache manager</param>
//        /// <param name="eventPublisher">Event publisher</param>
//        /// <param name="settingRepository">Setting repository</param>
//        public SettingService(ICacheManager cacheManager, IEventPublisher eventPublisher,
//            IGenericRepository<Setting, int> settingRepository)
//        {
//            this._cacheManager = cacheManager;
//            this._eventPublisher = eventPublisher;
//            this._settingRepository = settingRepository;
//        }

//        #endregion

//        #region Nested classes

//        [Serializable]
//        public class SettingForCaching
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//            public string Value { get; set; }
//            public int RegionId { get; set; }
//        }

//        #endregion

//        #region Utilities

//        /// <summary>
//        /// Gets all settings
//        /// </summary>
//        /// <returns>Settings</returns>
//        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
//        {
//            //cache
//            string key = string.Format(SETTINGS_ALL_KEY);
//            return _cacheManager.Get(key, () =>
//            {
//                //we use no tracking here for performance optimization
//                //anyway records are loaded only for read-only operations
//                var query = from s in _settingRepository.TableNoTracking
//                            orderby s.Name, s.RegionId
//                            select s;
//                var settings = query.ToList();
//                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
//                foreach (var s in settings)
//                {
//                    var resourceName = s.Name.ToLowerInvariant();
//                    var settingForCaching = new SettingForCaching
//                            {
//                                Id = s.Id,
//                                Name = s.Name,
//                                Value = s.Value,
//                                RegionId = s.RegionId
//                            };
//                    if (!dictionary.ContainsKey(resourceName))
//                    {
//                        //first setting
//                        dictionary.Add(resourceName, new List<SettingForCaching>
//                        {
//                            settingForCaching
//                        });
//                    }
//                    else
//                    {
//                        //already added
//                        //most probably it's the setting with the same name but for some certain Region (regionId > 0)
//                        dictionary[resourceName].Add(settingForCaching);
//                    }
//                }
//                return dictionary;
//            });
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        /// Adds a setting
//        /// </summary>
//        /// <param name="setting">Setting</param>
//        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
//        public virtual void InsertSetting(Setting setting, bool clearCache = true)
//        {
//            if (setting == null)
//                throw new ArgumentNullException(nameof(setting));

//            _settingRepository.Insert(setting);

//            //cache
//            if (clearCache)
//                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

//            //event notification
//            _eventPublisher.EntityInserted<Setting, int>(setting);
//        }

//        /// <summary>
//        /// Updates a setting
//        /// </summary>
//        /// <param name="setting">Setting</param>
//        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
//        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
//        {
//            if (setting == null)
//                throw new ArgumentNullException(nameof(setting));

//            _settingRepository.Update(setting);

//            //cache
//            if (clearCache)
//                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

//            //event notification
//            _eventPublisher.EntityUpdated<Setting, int>(setting);
//        }

//        /// <summary>
//        /// Deletes a setting
//        /// </summary>
//        /// <param name="setting">Setting</param>
//        public virtual void DeleteSetting(Setting setting)
//        {
//            if (setting == null)
//                throw new ArgumentNullException(nameof(setting));

//            _settingRepository.Delete(setting);

//            //cache
//            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

//            //event notification
//            _eventPublisher.EntityDeleted<Setting, int>(setting);
//        }

//        /// <summary>
//        /// Deletes settings
//        /// </summary>
//        /// <param name="settings">Settings</param>
//        public virtual void DeleteSettings(IList<Setting> settings)
//        {
//            if (settings == null)
//                throw new ArgumentNullException(nameof(settings));

//            _settingRepository.Delete(settings);

//            //cache
//            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

//            //event notification
//            foreach (var setting in settings)
//            {
//                _eventPublisher.EntityDeleted<Setting, int>(setting);
//            }
//        }

//        /// <summary>
//        /// Gets a setting by identifier
//        /// </summary>
//        /// <param name="settingId">Setting identifier</param>
//        /// <returns>Setting</returns>
//        public virtual ISetting GetSettingById(int settingId)
//        {
//            if (settingId == 0)
//                return null;

//            return _settingRepository.GetById(settingId);
//        }

//        /// <summary>
//        /// Get setting by key
//        /// </summary>
//        /// <param name="key">Key</param>
//        /// <param name="regionId">Region identifier</param>
//        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all regions) value should be loaded if a value specific for a certain is not found</param>
//        /// <returns>Setting</returns>
//        public virtual ISetting GetSetting(string key, int regionId = 0, bool loadSharedValueIfNotFound = false)
//        {
//            if (string.IsNullOrEmpty(key))
//                return null;

//            var settings = GetAllSettingsCached();
//            key = key.Trim().ToLowerInvariant();
//            if (settings.ContainsKey(key))
//            {
//                var settingsByKey = settings[key];
//                var setting = settingsByKey.FirstOrDefault(x => x.RegionId == regionId);

//                //load shared value?
//                if (setting == null && regionId > 0 && loadSharedValueIfNotFound)
//                    setting = settingsByKey.FirstOrDefault(x => x.RegionId == 0);

//                if (setting != null)
//                    return GetSettingById(setting.Id);
//            }

//            return null;
//        }

//        /// <summary>
//        /// Get setting value by key
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="key">Key</param>
//        /// <param name="defaultValue">Default value</param>
//        /// <param name="regionId">Region identifier</param>
//        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all regions) value should be loaded if a value specific for a certain is not found</param>
//        /// <returns>Setting value</returns>
//        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T), 
//            int regionId = 0, bool loadSharedValueIfNotFound = false)
//        {
//            if (String.IsNullOrEmpty(key))
//                return defaultValue;

//            var settings = GetAllSettingsCached();
//            key = key.Trim().ToLowerInvariant();
//            if (settings.ContainsKey(key))
//            {
//                var settingsByKey = settings[key];
//                var setting = settingsByKey.FirstOrDefault(x => x.RegionId == regionId);

//                //load shared value?
//                if (setting == null && regionId > 0 && loadSharedValueIfNotFound)
//                    setting = settingsByKey.FirstOrDefault(x => x.RegionId == 0);

//                if (setting != null)
//                    return CommonHelper.To<T>(setting.Value);
//            }

//            return defaultValue;
//        }

//        /// <summary>
//        /// Set setting value
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="key">Key</param>
//        /// <param name="value">Value</param>
//        /// <param name="regionId">Region identifier</param>
//        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
//        public virtual void SetSetting<T>(string key, T value, int regionId = 0, bool clearCache = true)
//        {
//            if (key == null)
//                throw new ArgumentNullException(nameof(key));
//            key = key.Trim().ToLowerInvariant();
//            string valueStr = CommonHelper.GetMrCustomTypeConverter(typeof(T)).ConvertToInvariantString(value);

//            var allSettings = GetAllSettingsCached();
//            var settingForCaching = allSettings.ContainsKey(key) ? 
//                allSettings[key].FirstOrDefault(x => x.RegionId == regionId) : null;
//            if (settingForCaching != null)
//            {
//                //update
//                var setting = GetSettingById(settingForCaching.Id);
//                setting.Value = valueStr;
//                UpdateSetting(setting, clearCache);
//            }
//            else
//            {
//                //insert
//                var setting = new Setting
//                {
//                    Name = key,
//                    Value = valueStr,
//                    RegionId = regionId
//                };
//                InsertSetting(setting, clearCache);
//            }
//        }

//        /// <summary>
//        /// Gets all settings
//        /// </summary>
//        /// <returns>Settings</returns>
//        public virtual IList<ISetting> GetAllSettings()
//        {
//            var query = from s in _settingRepository.Table
//                        orderby s.Name, s.RegionId
//                        select s;
//            var settings = query.ToList();
//            return settings;
//        }

//        /// <summary>
//        /// Determines whether a setting exists
//        /// </summary>
//        /// <typeparam name="T">Entity type</typeparam>
//        /// <typeparam name="TPropType">Property type</typeparam>
//        /// <param name="settings">Entity</param>
//        /// <param name="keySelector">Key selector</param>
//        /// <param name="regionId">Region identifier</param>
//        /// <returns>true -setting exists; false - does not exist</returns>
//        public virtual bool SettingExists<T, TPropType>(T settings, 
//            Expression<Func<T, TPropType>> keySelector, int regionId = 0) 
//            where T : ISettings, new()
//        {
//            string key = settings.GetSettingKey(keySelector);

//            var setting = GetSettingByKey<string>(key, regionId: regionId);
//            return setting != null;
//        }

//        /// <summary>
//        /// Load settings
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="regionId">Region identifier for which settigns should be loaded</param>
//        public virtual T LoadSetting<T>(int regionId = 0) where T : ISettings, new()
//        {
//            var settings = Activator.CreateInstance<T>();

//            foreach (var prop in typeof(T).GetProperties())
//            {
//                // get properties we can read and write to
//                if (!prop.CanRead || !prop.CanWrite)
//                    continue;

//                var key = typeof(T).Name + "." + prop.Name;
//                //load by region
//                var setting = GetSettingByKey<string>(key, regionId: regionId, loadSharedValueIfNotFound: true);
//                if (setting == null)
//                    continue;

//                if (!CommonHelper.GetMrCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
//                    continue;

//                if (!CommonHelper.GetMrCustomTypeConverter(prop.PropertyType).IsValid(setting))
//                    continue;

//                object value = CommonHelper.GetMrCustomTypeConverter(prop.PropertyType).ConvertFromInvariantString(setting);

//                //set property
//                prop.SetValue(settings, value, null);
//            }

//            return settings;
//        }

//        /// <summary>
//        /// Save settings object
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <param name="regionId">Region identifier</param>
//        /// <param name="settings">Setting instance</param>
//        public virtual void SaveSetting<T>(T settings, int regionId = 0) where T : ISettings, new()
//        {
//            /* We do not clear cache after each setting update.
//             * This behavior can increase performance because cached settings will not be cleared 
//             * and loaded from database after each update */
//            foreach (var prop in typeof(T).GetProperties())
//            {
//                // get properties we can read and write to
//                if (!prop.CanRead || !prop.CanWrite)
//                    continue;

//                if (!CommonHelper.GetMrCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
//                    continue;

//                string key = typeof(T).Name + "." + prop.Name;
//                //Duck typing is not supported in C#. That's why we're using dynamic type
//                dynamic value = prop.GetValue(settings, null);
//                if (value != null)
//                    SetSetting(key, value, regionId, false);
//                else
//                    SetSetting(key, "", regionId, false);
//            }

//            //and now clear cache
//            ClearCache();
//        }

//        /// <summary>
//        /// Save settings object
//        /// </summary>
//        /// <typeparam name="T">Entity type</typeparam>
//        /// <typeparam name="TPropType">Property type</typeparam>
//        /// <param name="settings">Settings</param>
//        /// <param name="keySelector">Key selector</param>
//        /// <param name="regionId">Region ID</param>
//        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
//        public virtual void SaveSetting<T, TPropType>(T settings,
//            Expression<Func<T, TPropType>> keySelector,
//            int regionId = 0, bool clearCache = true) where T : ISettings, new()
//        {
//            var member = keySelector.Body as MemberExpression;
//            if (member == null)
//            {
//                throw new ArgumentException(string.Format(
//                    "Expression '{0}' refers to a method, not a property.",
//                    keySelector));
//            }

//            var propInfo = member.Member as PropertyInfo;
//            if (propInfo == null)
//            {
//                throw new ArgumentException(string.Format(
//                       "Expression '{0}' refers to a field, not a property.",
//                       keySelector));
//            }

//            string key = settings.GetSettingKey(keySelector);
//            //Duck typing is not supported in C#. That's why we're using dynamic type
//            dynamic value = propInfo.GetValue(settings, null);
//            if (value != null)
//                SetSetting(key, value, regionId, clearCache);
//            else
//                SetSetting(key, "", regionId, clearCache);
//        }

//        /// <summary>
//        /// Save settings object (per region). If the setting is not overridden per region then it'll be delete
//        /// </summary>
//        /// <typeparam name="T">Entity type</typeparam>
//        /// <typeparam name="TPropType">Property type</typeparam>
//        /// <param name="settings">Settings</param>
//        /// <param name="keySelector">Key selector</param>
//        /// <param name="overrideForRegion">A value indicating whether to setting is overridden in some region</param>
//        /// <param name="regionId">Region ID</param>
//        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
//        public virtual void SaveSettingOverridablePerRegion<T, TPropType>(T settings,
//            Expression<Func<T, TPropType>> keySelector,
//            bool overrideForRegion, int regionId = 0, bool clearCache = true) where T : ISettings, new()
//        {
//            if (overrideForRegion || regionId == 0)
//                SaveSetting(settings, keySelector, regionId, clearCache);
//            else if (regionId > 0)
//                DeleteSetting(settings, keySelector, regionId);
//        }

//        /// <summary>
//        /// Delete all settings
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        public virtual void DeleteSetting<T>() where T : ISettings, new()
//        {
//            var settingsToDelete = new List<Setting>();
//            var allSettings = GetAllSettings();
//            foreach (var prop in typeof(T).GetProperties())
//            {
//                string key = typeof(T).Name + "." + prop.Name;
//                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
//            }

//            DeleteSettings(settingsToDelete);
//        }

//        /// <summary>
//        /// Delete settings object
//        /// </summary>
//        /// <typeparam name="T">Entity type</typeparam>
//        /// <typeparam name="TPropType">Property type</typeparam>
//        /// <param name="settings">Settings</param>
//        /// <param name="keySelector">Key selector</param>
//        /// <param name="regionId">Region ID</param>
//        public virtual void DeleteSetting<T, TPropType>(T settings,
//            Expression<Func<T, TPropType>> keySelector, int regionId = 0) where T : ISettings, new()
//        {
//            string key = settings.GetSettingKey(keySelector);
//            key = key.Trim().ToLowerInvariant();

//            var allSettings = GetAllSettingsCached();
//            var settingForCaching = allSettings.ContainsKey(key) ?
//                allSettings[key].FirstOrDefault(x => x.RegionId == regionId) : null;
//            if (settingForCaching != null)
//            {
//                //update
//                var setting = GetSettingById(settingForCaching.Id);
//                DeleteSetting(setting);
//            }
//        }

//        /// <summary>
//        /// Clear cache
//        /// </summary>
//        public virtual void ClearCache()
//        {
//            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
//        }

//        #endregion
//    }
//}
