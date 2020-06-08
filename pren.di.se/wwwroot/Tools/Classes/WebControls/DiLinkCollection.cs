using System;

using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace PrenDiSe.Tools.Classes.WebControls
{
    public class DiLinkCollection : PropertyLinkCollection
    {
        public DiLinkCollection(PageData page, String PropertyName)
        {
            PropertyLinkCollection linkCollection = page.Property[PropertyName] as PropertyLinkCollection;

            if (linkCollection != null)
            {
                Links = linkCollection.Links;
            }
        }

        public PageDataCollection SelectedPages()
        {
            PageDataCollection pageDataCollection = new PageDataCollection();

            foreach (LinkItem item in Links)
            {
                //get guid for link
                Guid guid = PermanentLinkUtility.GetGuid(item.Href);
                //get pagereference for guid
                PageReference pageReference = PermanentLinkUtility.FindPageReference(guid);

                //internal page
                if (!PageReference.IsNullOrEmpty(pageReference))
                {
                    PageData selectedPage = DataFactory.Instance.GetPage(pageReference);

                    if (selectedPage.QueryDistinctAccess(AccessLevel.Read))
                    {
                        PageData clone = selectedPage.CreateWritableClone();

                        AddPropertyHelper(clone, "PageName", new PropertyString(item.Text));

                        // Add frame name - if it has been defined
                        PropertyFrame frameProp = new PropertyFrame { Name = "PageTargetFrame" };
                        if (string.IsNullOrEmpty(item.Target) == false)
                        {
                            Frame targetFrame = Frame.Load(item.Target);
                            if (targetFrame != null)
                            {
                                frameProp.FrameName = targetFrame.Name;
                                // We need the id, or the property will
                                // tell us that it is null, even if the
                                // FrameName has been set
                                frameProp.Number = targetFrame.ID;
                            }
                        }
                        // Add frame to page
                        AddPropertyHelper(clone, "PageTargetFrame", frameProp);

                        // It needs to have published status, or it may be filtered away            
                        AddPropertyHelper(clone, "PageWorkStatus", new PropertyNumber((int)VersionStatus.Published));

                        // It should look like something we just fetched
                        clone.IsModified = false;
                        pageDataCollection.Add(clone);
                    }
                }
                //External reference or a file
                else
                {
                    // Construct a memory only pagedata from scratch
                    PageData page = new PageData();

                    // In the CMS 5 Published Filter the start publish date time is compared to when the request was initiated
                    // so we need to add a date that we're sure has passed, as we're creating the pagedata object after the
                    // request has started.                    
                    AddPropertyHelper(page, "PageStartPublish", new PropertyDate(DateTime.Now.AddDays(-1)));

                    // Page needs to have published status, or it may be filtered away   
                    AddPropertyHelper(page, "PageWorkStatus", new PropertyNumber((int)VersionStatus.Published));

                    // Give Everyone role read access to avoid the FilterAccess filter from 
                    AccessControlEntry accessEntry = new AccessControlEntry(
                        EveryoneRole.RoleName,
                        AccessLevel.Read,
                        SecurityEntityType.Role);

                    AddPropertyHelper(page, "PageShortCutType", new PropertyNumber((int)PageShortcutType.External));
                    page.ACL.Add(accessEntry);

                    AddPropertyHelper(page, "PageName", new PropertyString(item.Text));

                    //if guid not empty, it'a file
                    if (guid != Guid.Empty)
                    {
                        String vitualPath = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(item.Href).VirtualPath;
                        AddPropertyHelper(page, "PageLinkURL", new PropertyString(vitualPath));
                    }
                    else
                    {
                        AddPropertyHelper(page, "PageLinkURL", new PropertyString(item.Href));
                    }

                    // Add a PageLink so we can use <EPiServer:Property> in a PageList
                    AddPropertyHelper(page, "PageLink", new PropertyPageReference(-1));

                    // Add frame name - if it has been defined
                    PropertyFrame frameProp = new PropertyFrame { Name = "PageTargetFrame" };
                    if (string.IsNullOrEmpty(item.Target) == false)
                    {
                        Frame targetFrame = Frame.Load(item.Target);
                        if (targetFrame != null)
                        {
                            frameProp.FrameName = targetFrame.Name;
                            // We need the id, or the property will
                            // tell us that it is null, even if the
                            // FrameName has been set
                            frameProp.Number = targetFrame.ID;
                        }
                    }
                    // Add frame to page
                    AddPropertyHelper(page, "PageTargetFrame", frameProp);

                    // It should look like something we just fetched
                    page.IsModified = false;
                    //add page
                    pageDataCollection.Add(page);
                }
            }

            return pageDataCollection;
        }

        public PageDataCollection SelectedPages(bool filterAccess)
        {
            PageDataCollection pageDataCollection = new PageDataCollection();

            foreach (LinkItem item in Links)
            {
                //get guid for link
                Guid guid = PermanentLinkUtility.GetGuid(item.Href);
                //get pagereference for guid
                PageReference pageReference = PermanentLinkUtility.FindPageReference(guid);

                //internal page
                if (!PageReference.IsNullOrEmpty(pageReference))
                {
                    PageData selectedPage = DataFactory.Instance.GetPage(pageReference);

                    if (filterAccess && !selectedPage.QueryDistinctAccess(AccessLevel.Read))
                        continue;

                    PageData clone = selectedPage.CreateWritableClone();

                    AddPropertyHelper(clone, "PageName", new PropertyString(item.Text));

                    // Add frame name - if it has been defined
                    PropertyFrame frameProp = new PropertyFrame { Name = "PageTargetFrame" };
                    if (string.IsNullOrEmpty(item.Target) == false)
                    {
                        Frame targetFrame = Frame.Load(item.Target);
                        if (targetFrame != null)
                        {
                            frameProp.FrameName = targetFrame.Name;
                            // We need the id, or the property will
                            // tell us that it is null, even if the
                            // FrameName has been set
                            frameProp.Number = targetFrame.ID;
                        }
                    }
                    // Add frame to page
                    AddPropertyHelper(clone, "PageTargetFrame", frameProp);

                    // It needs to have published status, or it may be filtered away            
                    AddPropertyHelper(clone, "PageWorkStatus", new PropertyNumber((int)VersionStatus.Published));

                    // It should look like something we just fetched
                    clone.IsModified = false;
                    pageDataCollection.Add(clone);

                }
                //External reference or a file
                else
                {
                    // Construct a memory only pagedata from scratch
                    PageData page = new PageData();

                    // In the CMS 5 Published Filter the start publish date time is compared to when the request was initiated
                    // so we need to add a date that we're sure has passed, as we're creating the pagedata object after the
                    // request has started.                    
                    AddPropertyHelper(page, "PageStartPublish", new PropertyDate(DateTime.Now.AddDays(-1)));

                    // Page needs to have published status, or it may be filtered away   
                    AddPropertyHelper(page, "PageWorkStatus", new PropertyNumber((int)VersionStatus.Published));

                    // Give Everyone role read access to avoid the FilterAccess filter from 
                    AccessControlEntry accessEntry = new AccessControlEntry(
                        EveryoneRole.RoleName,
                        AccessLevel.Read,
                        SecurityEntityType.Role);

                    AddPropertyHelper(page, "PageShortCutType", new PropertyNumber((int)PageShortcutType.External));
                    page.ACL.Add(accessEntry);

                    AddPropertyHelper(page, "PageName", new PropertyString(item.Text));

                    //if guid not empty, it'a file
                    if (guid != Guid.Empty)
                    {
                        String vitualPath = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(item.Href).VirtualPath;
                        AddPropertyHelper(page, "PageLinkURL", new PropertyString(vitualPath));
                    }
                    else
                    {
                        AddPropertyHelper(page, "PageLinkURL", new PropertyString(item.Href));
                    }

                    // Add a PageLink so we can use <EPiServer:Property> in a PageList
                    AddPropertyHelper(page, "PageLink", new PropertyPageReference(-1));

                    // Add frame name - if it has been defined
                    PropertyFrame frameProp = new PropertyFrame { Name = "PageTargetFrame" };
                    if (string.IsNullOrEmpty(item.Target) == false)
                    {
                        Frame targetFrame = Frame.Load(item.Target);
                        if (targetFrame != null)
                        {
                            frameProp.FrameName = targetFrame.Name;
                            // We need the id, or the property will
                            // tell us that it is null, even if the
                            // FrameName has been set
                            frameProp.Number = targetFrame.ID;
                        }
                    }
                    // Add frame to page
                    AddPropertyHelper(page, "PageTargetFrame", frameProp);

                    // It should look like something we just fetched
                    page.IsModified = false;
                    //add page
                    pageDataCollection.Add(page);
                }
            }

            return pageDataCollection;
        }

        private static void AddPropertyHelper(PageData pageData, string name, PropertyData property)
        {
            if (pageData.Property[name] != null)
            {
                pageData.Property[name] = property;
            }
            else
            {
                pageData.Property.Add(name, property);
            }
            pageData.Property[name].IsModified = false;
        }
    }
}