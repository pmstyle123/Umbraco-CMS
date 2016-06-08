using System;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Models;
using umbraco;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Cache;
using Umbraco.Core.Services.Changes;

namespace Umbraco.Web.Cache
{
    /// <summary>
    /// Extension methods for <see cref="DistributedCache"/>.
    /// </summary>
    internal static class DistributedCacheExtensions
    {
        #region PublicAccessCache

        public static void RefreshPublicAccess(this DistributedCache dc)
        {
            dc.RefreshAll(PublicAccessCacheRefresher.UniqueId);
        }

        #endregion

        #region ApplicationTreeCache

        public static void RefreshAllApplicationTreeCache(this DistributedCache dc)
        {
            dc.RefreshAll(ApplicationTreeCacheRefresher.UniqueId);
        }

        #endregion

        #region ApplicationCache

        public static void RefreshAllApplicationCache(this DistributedCache dc)
        {
            dc.RefreshAll(ApplicationCacheRefresher.UniqueId);
        }

        #endregion

        #region UserTypeCache

        public static void RemoveUserTypeCache(this DistributedCache dc, int userTypeId)
        {
            dc.Remove(UserTypeCacheRefresher.UniqueId, userTypeId);
        }

        public static void RefreshUserTypeCache(this DistributedCache dc, int userTypeId)
        {
            dc.Refresh(UserTypeCacheRefresher.UniqueId, userTypeId);
        }

        public static void RefreshAllUserTypeCache(this DistributedCache dc)
        {
            dc.RefreshAll(UserTypeCacheRefresher.UniqueId);
        }

        #endregion

        #region UserCache

        public static void RemoveUserCache(this DistributedCache dc, int userId)
        {
            dc.Remove(UserCacheRefresher.UniqueId, userId);
        }

        public static void RefreshUserCache(this DistributedCache dc, int userId)
        {
            dc.Refresh(UserCacheRefresher.UniqueId, userId);
        }

        public static void RefreshAllUserCache(this DistributedCache dc)
        {
            dc.RefreshAll(UserCacheRefresher.UniqueId);
        }

        #endregion

        #region UserPermissionsCache

        public static void RemoveUserPermissionsCache(this DistributedCache dc, int userId)
        {
            dc.Remove(UserPermissionsCacheRefresher.UniqueId, userId);
        }

        public static void RefreshUserPermissionsCache(this DistributedCache dc, int userId)
        {
            dc.Refresh(UserPermissionsCacheRefresher.UniqueId, userId);
        }

        public static void RefreshAllUserPermissionsCache(this DistributedCache dc)
        {
            dc.RefreshAll(UserPermissionsCacheRefresher.UniqueId);
        }

        #endregion

        #region TemplateCache

        public static void RefreshTemplateCache(this DistributedCache dc, int templateId)
        {
            dc.Refresh(TemplateCacheRefresher.UniqueId, templateId);
        }

        public static void RemoveTemplateCache(this DistributedCache dc, int templateId)
        {
            dc.Remove(TemplateCacheRefresher.UniqueId, templateId);
        }

        #endregion

        #region DictionaryCache

        public static void RefreshDictionaryCache(this DistributedCache dc, int dictionaryItemId)
        {
            dc.Refresh(DictionaryCacheRefresher.UniqueId, dictionaryItemId);
        }

        public static void RemoveDictionaryCache(this DistributedCache dc, int dictionaryItemId)
        {
            dc.Remove(DictionaryCacheRefresher.UniqueId, dictionaryItemId);
        }

        #endregion

        #region DataTypeCache

        public static void RefreshDataTypeCache(this DistributedCache dc, IDataTypeDefinition dataType)
        {
            if (dataType == null) return;
            var payloads = new[] { new DataTypeCacheRefresher.JsonPayload(dataType.Id, dataType.Key, false) };
            dc.RefreshByPayload(DataTypeCacheRefresher.UniqueId, payloads);
        }

        public static void RemoveDataTypeCache(this DistributedCache dc, IDataTypeDefinition dataType)
        {
            if (dataType == null) return;
            var payloads = new[] { new DataTypeCacheRefresher.JsonPayload(dataType.Id, dataType.Key, true) };
            dc.RefreshByPayload(DataTypeCacheRefresher.UniqueId, payloads);
        }

        #endregion

        #region ContentCache

        public static void RefreshAllContentCache(this DistributedCache dc)
        {
            var payloads = new[] { new ContentCacheRefresher.JsonPayload(0, TreeChangeTypes.RefreshAll) };

            // note: refresh all content cache does refresh content types too
            dc.RefreshByPayload(ContentCacheRefresher.UniqueId, payloads);
        }

        public static void RefreshContentCache(this DistributedCache dc, TreeChange<IContent>[] changes)
        {
            if (changes.Length == 0) return;

            var payloads = changes
                .Select(x => new ContentCacheRefresher.JsonPayload(x.Item.Id, x.ChangeTypes));

            dc.RefreshByPayload(ContentCacheRefresher.UniqueId, payloads);
        }

        #endregion

        #region MemberCache

        public static void RefreshMemberCache(this DistributedCache dc, params IMember[] members)
        {
            dc.Refresh(MemberCacheRefresher.UniqueId, x => x.Id, members);
        }

        public static void RemoveMemberCache(this DistributedCache dc, params IMember[] members)
        {
            dc.Remove(MemberCacheRefresher.UniqueId, x => x.Id, members);
        }

        [Obsolete("Use the RefreshMemberCache with strongly typed IMember objects instead")]
        public static void RefreshMemberCache(this DistributedCache dc, int memberId)
        {
            dc.Refresh(MemberCacheRefresher.UniqueId, memberId);
        }

        [Obsolete("Use the RemoveMemberCache with strongly typed IMember objects instead")]
        public static void RemoveMemberCache(this DistributedCache dc, int memberId)
        {
            dc.Remove(MemberCacheRefresher.UniqueId, memberId);
        }

        #endregion

        #region MemberGroupCache

        public static void RefreshMemberGroupCache(this DistributedCache dc, int memberGroupId)
        {
            dc.Refresh(MemberGroupCacheRefresher.UniqueId, memberGroupId);
        }

        public static void RemoveMemberGroupCache(this DistributedCache dc, int memberGroupId)
        {
            dc.Remove(MemberGroupCacheRefresher.UniqueId, memberGroupId);
        }

        #endregion

        #region MediaCache

        public static void RefreshAllMediaCache(this DistributedCache dc)
        {
            var payloads = new[] { new MediaCacheRefresher.JsonPayload(0, TreeChangeTypes.RefreshAll) };

            // note: refresh all media cache does refresh content types too
            dc.RefreshByPayload(MediaCacheRefresher.UniqueId, payloads);
        }

        public static void RefreshMediaCache(this DistributedCache dc, TreeChange<IMedia>[] changes)
        {
            if (changes.Length == 0) return;

            var payloads = changes
                .Select(x => new MediaCacheRefresher.JsonPayload(x.Item.Id, x.ChangeTypes));

            dc.RefreshByPayload(MediaCacheRefresher.UniqueId, payloads);
        }

        #endregion

        #region Facade

        public static void RefreshAllFacade(this DistributedCache dc)
        {
            // note: refresh all content & media caches does refresh content types too
            dc.RefreshAllContentCache();
            dc.RefreshAllMediaCache();
            dc.RefreshAllDomainCache();
        }

        #endregion

        #region MacroCache

        // fixme!
        /*
        public static void ClearAllMacroCacheOnCurrentServer(this DistributedCache dc)
        {
            var macroRefresher = CacheRefreshersResolver.Current.GetById(MacroCacheRefresher.UniqueId);
            macroRefresher.RefreshAll();
        }
        */

        public static void RefreshMacroCache(this DistributedCache dc, IMacro macro)
        {
            if (macro == null) return;
            dc.RefreshByJson(MacroCacheRefresher.UniqueId, MacroCacheRefresher.Serialize(macro));
        }

        public static void RemoveMacroCache(this DistributedCache dc, IMacro macro)
        {
            if (macro == null) return;
            dc.RefreshByJson(MacroCacheRefresher.UniqueId, MacroCacheRefresher.Serialize(macro));
        }

        public static void RefreshMacroCache(this DistributedCache dc, global::umbraco.cms.businesslogic.macro.Macro macro)
        {
            if (macro == null) return;
            dc.RefreshByJson(MacroCacheRefresher.UniqueId, MacroCacheRefresher.Serialize(macro));
        }

        public static void RemoveMacroCache(this DistributedCache dc, global::umbraco.cms.businesslogic.macro.Macro macro)
        {
            if (macro == null) return;
            dc.RefreshByJson(MacroCacheRefresher.UniqueId, MacroCacheRefresher.Serialize(macro));
        }

        #endregion

        #region Content/Media/Member type cache

        public static void RefreshContentTypeCache(this DistributedCache dc, ContentTypeChange<IContentType>[] changes)
        {
            if (changes.Length == 0) return;

            var payloads = changes
                .Select(x => new ContentTypeCacheRefresher.JsonPayload(typeof (IContentType).Name, x.Item.Id, x.ChangeTypes));

            dc.RefreshByPayload(ContentTypeCacheRefresher.UniqueId, payloads);
        }

        public static void RefreshContentTypeCache(this DistributedCache dc, ContentTypeChange<IMediaType>[] changes)
        {
            if (changes.Length == 0) return;

            var payloads = changes
                .Select(x => new ContentTypeCacheRefresher.JsonPayload(typeof(IMediaType).Name, x.Item.Id, x.ChangeTypes));

            dc.RefreshByPayload(ContentTypeCacheRefresher.UniqueId, payloads);
        }

        public static void RefreshContentTypeCache(this DistributedCache dc, ContentTypeChange<IMemberType>[] changes)
        {
            if (changes.Length == 0) return;

            var payloads = changes
                .Select(x => new ContentTypeCacheRefresher.JsonPayload(typeof(IMemberType).Name, x.Item.Id, x.ChangeTypes));

            dc.RefreshByPayload(ContentTypeCacheRefresher.UniqueId, payloads);
        }

        #endregion

        #region Domain Cache

        public static void RefreshDomainCache(this DistributedCache dc, IDomain domain)
        {
            if (domain == null) return;
            var payloads = new[] { new DomainCacheRefresher.JsonPayload(domain.Id, DomainCacheRefresher.ChangeTypes.Refresh) };
            dc.RefreshByPayload(DomainCacheRefresher.UniqueId, payloads);
        }

        public static void RemoveDomainCache(this DistributedCache dc, IDomain domain)
        {
            if (domain == null) return;
            var payloads = new[] { new DomainCacheRefresher.JsonPayload(domain.Id, DomainCacheRefresher.ChangeTypes.Remove) };
            dc.RefreshByPayload(DomainCacheRefresher.UniqueId, payloads);
        }

        public static void RefreshAllDomainCache(this DistributedCache dc)
        {
            var payloads = new[] { new DomainCacheRefresher.JsonPayload(0, DomainCacheRefresher.ChangeTypes.RefreshAll) };
            dc.RefreshByPayload(DomainCacheRefresher.UniqueId, payloads);
        }

        // fixme?
        /*
        public static void ClearDomainCacheOnCurrentServer(this DistributedCache dc)
        {
            var domainRefresher = CacheRefreshersResolver.Current.GetById(DomainCacheRefresher.UniqueId);
            domainRefresher.RefreshAll();
        }
        */

        #endregion

        #region Language Cache

        public static void RefreshLanguageCache(this DistributedCache dc, ILanguage language)
        {
            if (language == null) return;
            dc.Refresh(LanguageCacheRefresher.UniqueId, language.Id);
        }

        public static void RemoveLanguageCache(this DistributedCache dc, ILanguage language)
        {
            if (language == null) return;
            dc.Remove(LanguageCacheRefresher.UniqueId, language.Id);
        }

        public static void RefreshLanguageCache(this DistributedCache dc, global::umbraco.cms.businesslogic.language.Language language)
        {
            if (language == null) return;
            dc.Refresh(LanguageCacheRefresher.UniqueId, language.id);
        }

        public static void RemoveLanguageCache(this DistributedCache dc, global::umbraco.cms.businesslogic.language.Language language)
        {
            if (language == null) return;
            dc.Remove(LanguageCacheRefresher.UniqueId, language.id);
        }

        #endregion

        #region Xslt Cache

        // fixme?
        public static void ClearXsltCacheOnCurrentServer(this DistributedCache dc, CacheHelper cacheHelper)
        {
            if (UmbracoConfig.For.UmbracoSettings().Content.UmbracoLibraryCacheDuration <= 0) return;
            cacheHelper.RuntimeCache.ClearCacheObjectTypes("MS.Internal.Xml.XPath.XPathSelectionIterator");
        }

        #endregion
    }
}