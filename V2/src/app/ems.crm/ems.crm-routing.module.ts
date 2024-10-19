import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CrmTrnSalesordersummaryComponent } from './component/crm-trn-salesordersummary/crm-trn-salesordersummary.component';
import { CrmTrnSalesorderaddComponent } from './component/crm-trn-salesorderadd/crm-trn-salesorderadd.component';
import { CrmMstSourcesummaryComponent } from './component/crm-mst-sourcesummary/crm-mst-sourcesummary.component';
import { CrmMstProductgroupsummaryComponent } from './component/crm-mst-productgroupsummary/crm-mst-productgroupsummary.component';
import { CrmMstCategoryindustrySummaryComponent } from './component/crm-mst-categoryindustry-summary/crm-mst-categoryindustry-summary.component';
import { CrmMstRegionsummaryComponent } from './component/crm-mst-regionsummary/crm-mst-regionsummary.component';
import { CrmSmmFacebookaccountComponent } from './component/crm-smm-facebookaccount/crm-smm-facebookaccount.component';
import { CrmSmmFacebookpageComponent } from './component/crm-smm-facebookpage/crm-smm-facebookpage.component';
import { CrmSmmTelegramaccountComponent } from './component/crm-smm-telegramaccount/crm-smm-telegramaccount.component';
import { CrmSmmLinkedaccountComponent } from './component/crm-smm-linkedaccount/crm-smm-linkedaccount.component';
import { CrmMstProductsummaryComponent } from './component/crm-mst-productsummary/crm-mst-productsummary.component';
import { CrmMstProductAddComponent } from './component/crm-mst-product-add/crm-mst-product-add.component';
import { CrmMstProductEditComponent } from './component/crm-mst-product-edit/crm-mst-product-edit.component';
import { CrmMstProductViewComponent } from './component/crm-mst-product-view/crm-mst-product-view.component';
import { CrmTrnLeadbanksummaryComponent } from './component/crm-trn-leadbanksummary/crm-trn-leadbanksummary.component';
import { CrmTrnLeadbankaddComponent } from './component/crm-trn-leadbankadd/crm-trn-leadbankadd.component';
import { CrmTrnLeadbankviewComponent } from './component/crm-trn-leadbankview/crm-trn-leadbankview.component';
import { CrmTrnLeadbankeditComponent } from './component/crm-trn-leadbankedit/crm-trn-leadbankedit.component';
import { CrmTrnTelemycampaignComponent } from './component/crm-trn-telemycampaign/crm-trn-telemycampaign.component';
import { CrmTrnMyvisitComponent } from './component/crm-trn-myvisit/crm-trn-myvisit.component';
import { CrmCampaignMailmanagementComponent } from './component/crm-campaign-mailmanagement/crm-campaign-mailmanagement.component';
import { CrmTrnMarketingmanagersummaryComponent } from './component/crm-trn-marketingmanagersummary/crm-trn-marketingmanagersummary.component';
import { CrmTrnCampaignmanagerComponent } from './component/crm-trn-campaignmanager/crm-trn-campaignmanager.component';
import { CrmTrnLeadbankbranchComponent } from './component/crm-trn-leadbankbranch/crm-trn-leadbankbranch.component';
import { CrmTrnLeadmasteraddComponent } from './component/crm-trn-leadmasteradd/crm-trn-leadmasteradd.component';
import { CrmTrnLeadmastersummaryComponent } from './component/crm-trn-leadmastersummary/crm-trn-leadmastersummary.component';
import { CrmTrnLeadbankbrancheditComponent } from './component/crm-trn-leadbankbranchedit/crm-trn-leadbankbranchedit.component';
import { CrmTrnAssignvisitsummaryComponent } from './component/crm-trn-assignvisitsummary/crm-trn-assignvisitsummary.component';
import { CrmSmmInstagramComponent } from './component/crm-smm-instagram/crm-smm-instagram.component';
import { CrmMstCampaignsummaryComponent } from './component/crm-mst-campaignsummary/crm-mst-campaignsummary.component';
import { CrmTrnLeadbankcontactComponent } from './component/crm-trn-leadbankcontact/crm-trn-leadbankcontact.component';
import { CrmTrnLeadbankcontactEditComponent } from './component/crm-trn-leadbankcontact-edit/crm-trn-leadbankcontact-edit.component';
import { CrmTrnCampaignleadallocationComponent } from './component/crm-trn-campaignleadallocation/crm-trn-campaignleadallocation.component';
import { CrmTrnMycampaignComponent } from './component/crm-trn-mycampaign/crm-trn-mycampaign.component';
import { CrmTrnAddtocustomerComponent } from './component/crm-trn-addtocustomer/crm-trn-addtocustomer.component';
import { CrmDashboardComponent } from './component/crm-dashboard/crm-dashboard.component';
import { CrmSmmTelegramsummaryComponent } from './component/crm-smm-telegramsummary/crm-smm-telegramsummary.component';
import { CrmCampaignMailmanagementsummaryComponent } from './component/crm-campaign-mailmanagementsummary/crm-campaign-mailmanagementsummary.component';
import { CrmTrnUpcomingmeetingsComponent } from './component/crm-trn-upcomingmeetings/crm-trn-upcomingmeetings.component';
import { CrmTrnNewtaskComponent } from './component/crm-trn-newtask/crm-trn-newtask.component';
import { CrmTrnProspectsComponent } from './component/crm-trn-prospects/crm-trn-prospects.component';
import { CrmTrnPotentialsComponent } from './component/crm-trn-potentials/crm-trn-potentials.component';
import { CrmTrnCompletedComponent } from './component/crm-trn-completed/crm-trn-completed.component';
import { CrmTrnDropleadsComponent } from './component/crm-trn-dropleads/crm-trn-dropleads.component';
import { CrmTrnAllleadsComponent } from './component/crm-trn-allleads/crm-trn-allleads.component';
import { CrmMstCallresponseComponent } from './component/crm-mst-callresponse/crm-mst-callresponse.component';
import { CrmSmmWatsappComponent } from './component/crm-smm-watsapp/crm-smm-watsapp.component';
import { CrmSmmComposemailComponent } from './component/crm-smm-composemail/crm-smm-composemail.component';
import { CrmSmmShopifycustomerComponent } from './component/crm-smm-shopifycustomer/crm-smm-shopifycustomer.component';
import { CrmSmmWhatsappmessagetemplateComponent } from './component/crm-smm-whatsappmessagetemplate/crm-smm-whatsappmessagetemplate.component';
import { CrmTrnCorporateregisterleadComponent } from './component/crm-trn-corporateregisterlead/crm-trn-corporateregisterlead.component';
import { CrmTrnRetailerregisterleadComponent } from './component/crm-trn-retailerregisterlead/crm-trn-retailerregisterlead.component';
import { CrmTrnCorporateleadbankComponent } from './component/crm-trn-corporateleadbank/crm-trn-corporateleadbank.component';
import { CrmTrnRetailerleadbankComponent } from './component/crm-trn-retailerleadbank/crm-trn-retailerleadbank.component';
import { CrmSmmSendmailComponent } from './component/crm-smm-sendmail/crm-smm-sendmail.component';
import { CrmSmmEmailmanagementComponent } from './component/crm-smm-emailmanagement/crm-smm-emailmanagement.component';
import { CrmSocailMediaDashboardComponent } from './component/crm-socail-media-dashboard/crm-socail-media-dashboard.component';
import { CrmSmmShopifycustomerassignedComponent } from './component/crm-smm-shopifycustomerassigned/crm-smm-shopifycustomerassigned.component';
import { CrmSmmShopifycustomerunassignedComponent } from './component/crm-smm-shopifycustomerunassigned/crm-smm-shopifycustomerunassigned.component';
import { CrmSmmMailcampaigntemplateComponent } from './component/crm-smm-mailcampaigntemplate/crm-smm-mailcampaigntemplate.component';
import { CrmSmmMailcampaignsummaryComponent } from './component/crm-smm-mailcampaignsummary/crm-smm-mailcampaignsummary.component';
import { CrmSmmMailcampaignsendComponent } from './component/crm-smm-mailcampaignsend/crm-smm-mailcampaignsend.component';
import { CrmSmmMailcampaigntemplateviewComponent } from './component/crm-smm-mailcampaigntemplateview/crm-smm-mailcampaigntemplateview.component';
import { CrmSmmMailcampaignsendstatusComponent } from './component/crm-smm-mailcampaignsendstatus/crm-smm-mailcampaignsendstatus.component';
import { CrmSmmWhatsappcampaignComponent } from './component/crm-smm-whatsappcampaign/crm-smm-whatsappcampaign.component';
import { CrmMstWabulkmessageComponent } from './component/crm-mst-wabulkmessage/crm-mst-wabulkmessage.component';
import { CrmSmmWhatsapplogComponent } from './component/crm-smm-whatsapplog/crm-smm-whatsapplog.component';
import { CrmTrnRetaileraddComponent } from './component/crm-trn-retaileradd/crm-trn-retaileradd.component';
import { CrmTrnRetailerviewComponent } from './component/crm-trn-retailerview/crm-trn-retailerview.component';
import { CrmTrnRetailereditComponent } from './component/crm-trn-retaileredit/crm-trn-retaileredit.component';
import { CrmTrnOverallviewComponent } from './component/crm-trn-overallview/crm-trn-overallview.component';
import { CrmTrnUpcomingmarketingComponent } from './component/crm-trn-upcomingmarketing/crm-trn-upcomingmarketing.component';
import { CrmTrnNewmarketingComponent } from './component/crm-trn-newmarketing/crm-trn-newmarketing.component';
import { CrmTrnProspectmarketingComponent } from './component/crm-trn-prospectmarketing/crm-trn-prospectmarketing.component';
import { CrmTrnPotentialmarketingComponent } from './component/crm-trn-potentialmarketing/crm-trn-potentialmarketing.component';
import { CrmTrnCustomermarketingComponent } from './component/crm-trn-customermarketing/crm-trn-customermarketing.component';
import { CrmTrnDropmarketingComponent } from './component/crm-trn-dropmarketing/crm-trn-dropmarketing.component';
import { CrmTrnLapsedleadmarketingComponent } from './component/crm-trn-lapsedleadmarketing/crm-trn-lapsedleadmarketing.component';
import { CrmTrnLongestleadmarketingComponent } from './component/crm-trn-longestleadmarketing/crm-trn-longestleadmarketing.component';
import { CrmSmmSmscampaignComponent } from './component/crm-smm-smscampaign/crm-smm-smscampaign.component';
import { CrmSmmSmscampaignsendComponent } from './component/crm-smm-smscampaignsend/crm-smm-smscampaignsend.component';
import { CrmSmmCampaignsettingsComponent } from './component/crm-smm-campaignsettings/crm-smm-campaignsettings.component';
import { CrmSmmIndiamartComponent } from './component/crm-smm-indiamart/crm-smm-indiamart.component';
import { CrmSmmTradeindiaComponent } from './component/crm-smm-tradeindia/crm-smm-tradeindia.component';
import { CrmSmmWebsiteComponent } from './component/crm-smm-website/crm-smm-website.component';
import { CrmSmmFlipkartComponent } from './component/crm-smm-flipkart/crm-smm-flipkart.component';
import { CrmSmmAmazonComponent } from './component/crm-smm-amazon/crm-smm-amazon.component';
import { CrmSmmJustdialComponent } from './component/crm-smm-justdial/crm-smm-justdial.component';
import { CrmSmmMeeshoComponent } from './component/crm-smm-meesho/crm-smm-meesho.component';
import { CrmSmmFacebookpostviewComponent } from './component/crm-smm-facebookpostview/crm-smm-facebookpostview.component';
import { CrmTrnMtdComponent } from './component/crm-trn-mtd/crm-trn-mtd.component';
import { CrmTrnYtdComponent } from './component/crm-trn-ytd/crm-trn-ytd.component';
import { CrmMstCampaignexpandComponent } from './component/crm-mst-campaignexpand/crm-mst-campaignexpand.component';
import { CrmTrnTraiseorderComponent } from './component/crm-trn-traiseorder/crm-trn-traiseorder.component';
import { CrmTrnTrasieinvoiceComponent } from './component/crm-trn-trasieinvoice/crm-trn-trasieinvoice.component';
import { CrmTrnTcustomerraiseenquiryComponent } from './component/crm-trn-tcustomerraiseenquiry/crm-trn-tcustomerraiseenquiry.component';
import { CrmSmmShopifyproductaddComponent } from './component/crm-smm-shopifyproductadd/crm-smm-shopifyproductadd.component';
import { CrmSmmShopifyproducteditComponent } from './component/crm-smm-shopifyproductedit/crm-smm-shopifyproductedit.component';
import { CrmSmmWebsitechatsComponent } from './component/crm-smm-websitechats/crm-smm-websitechats.component';
import { CrmTrnCustomerProductPriceComponent } from './component/crm-trn-customer-product-price/crm-trn-customer-product-price.component';
import { CrmRptActivitylogreportComponent } from './component/crm-rpt-activitylogreport/crm-rpt-activitylogreport.component';
import { CrmRptEnquiryrptComponent } from './component/crm-rpt-enquiryrpt/crm-rpt-enquiryrpt.component';
import { CrmRptProductconsumptionreportComponent } from './component/crm-rpt-productconsumptionreport/crm-rpt-productconsumptionreport.component';
import { CrmRptProductgroupreportComponent } from './component/crm-rpt-productgroupreport/crm-rpt-productgroupreport.component';
import { CrmRptProductpreviouspricereportComponent } from './component/crm-rpt-productpreviouspricereport/crm-rpt-productpreviouspricereport.component';
import { CrmSmmWebsitechatanalyticsComponent } from './component/crm-smm-websitechatanalytics/crm-smm-websitechatanalytics.component';
//import { CrmTrnTraisequtationComponent } from './component/crm-trn-traisequtation/crm-trn-traisequtation.component';
import { CrmTrnQuoteaddComponent } from './component/crm-trn-quoteadd/crm-trn-quoteadd.component';
import { CrmSmmWacustomizebulkmessageComponent } from './component/crm-smm-wacustomizebulkmessage/crm-smm-wacustomizebulkmessage.component';
import { CrmMstProductunitsummaryComponent } from './component/crm-mst-productunitsummary/crm-mst-productunitsummary.component';
import { CrmSmmWhatsappcampaigncreationComponent } from './component/crm-smm-whatsappcampaigncreation/crm-smm-whatsappcampaigncreation.component';
import { CrmSmmClicktocallComponent } from './component/crm-smm-clicktocall/crm-smm-clicktocall.component';
import { CrmSmmClicktocallagentsComponent } from './component/crm-smm-clicktocallagents/crm-smm-clicktocallagents.component';
import { CrmSmmClicktocalllogComponent } from './component/crm-smm-clicktocalllog/crm-smm-clicktocalllog.component';
import { CrmTrnImportfromleadComponent } from './component/crm-trn-importfromlead/crm-trn-importfromlead.component';
import { CrmTrnTelecallerTeamComponent } from './component/crm-trn-telecaller-team/crm-trn-telecaller-team.component';
import { CrmTrnTelecampaignmanagersummaryComponent } from './component/crm-trn-telecampaignmanagersummary/crm-trn-telecampaignmanagersummary.component';
import { CrmTrnTelecampaignmanagersummaryNewComponent } from './component/crm-trn-telecampaignmanagersummary-new/crm-trn-telecampaignmanagersummary-new.component';
import { CrmTrnTelecampaignmanagersummaryPendingcallsComponent } from './component/crm-trn-telecampaignmanagersummary-pendingcalls/crm-trn-telecampaignmanagersummary-pendingcalls.component';
import { CrmTrnTelecampaignmanagersummaryFollowupComponent } from './component/crm-trn-telecampaignmanagersummary-followup/crm-trn-telecampaignmanagersummary-followup.component';
import { CrmTrnTelecampaignmanagersummaryProspectComponent } from './component/crm-trn-telecampaignmanagersummary-prospect/crm-trn-telecampaignmanagersummary-prospect.component';
import { CrmTrnTelecampaignmanagersummaryDropComponent } from './component/crm-trn-telecampaignmanagersummary-drop/crm-trn-telecampaignmanagersummary-drop.component';
import { CrmTrnTelecampaignmanagersummaryScheduledComponent } from './component/crm-trn-telecampaignmanagersummary-scheduled/crm-trn-telecampaignmanagersummary-scheduled.component';
import { CrmTrnTelecampaignmanagersummaryLapsedleadsComponent } from './component/crm-trn-telecampaignmanagersummary-lapsedleads/crm-trn-telecampaignmanagersummary-lapsedleads.component';
import { CrmTrnTelecampaignmanagersummaryLongestleadsComponent } from './component/crm-trn-telecampaignmanagersummary-longestleads/crm-trn-telecampaignmanagersummary-longestleads.component';
import { CrmTrnTelacallerteamLeadComponent } from './component/crm-trn-telacallerteam-lead/crm-trn-telacallerteam-lead.component';
import { CrmSmmGmailcampaignsendComponent } from './component/crm-smm-gmailcampaignsend/crm-smm-gmailcampaignsend.component';
import { CrmSmmGmailcampaignsendstatusComponent } from './component/crm-smm-gmailcampaignsendstatus/crm-smm-gmailcampaignsendstatus.component';
import { CrmSmmGmailcampaignsummaryComponent } from './component/crm-smm-gmailcampaignsummary/crm-smm-gmailcampaignsummary.component';
import { CrmSmmGmailcampaigntemplateComponent } from './component/crm-smm-gmailcampaigntemplate/crm-smm-gmailcampaigntemplate.component';
import { CrmSmmGmailinboxComponent } from './component/crm-smm-gmailinbox/crm-smm-gmailinbox.component';
import { CrmSmmGmailviewComponent } from './component/crm-smm-gmailview/crm-smm-gmailview.component';
import { CrmTrnTelemycampaignPendingComponent } from './component/crm-trn-telemycampaign-pending/crm-trn-telemycampaign-pending.component';
import { CrmTrnTelemycampaignNewComponent } from './component/crm-trn-telemycampaign-new/crm-trn-telemycampaign-new.component';
import { CrmTrnTelemycampaignProspectComponent } from './component/crm-trn-telemycampaign-prospect/crm-trn-telemycampaign-prospect.component';
import { CrmTrnTelemycampaignFollowupComponent } from './component/crm-trn-telemycampaign-followup/crm-trn-telemycampaign-followup.component';
import { CrmTrnTelemycampaignDropComponent } from './component/crm-trn-telemycampaign-drop/crm-trn-telemycampaign-drop.component';
import { CrmTrnTelemycampaignAllComponent } from './component/crm-trn-telemycampaign-all/crm-trn-telemycampaign-all.component';
import { CrmTrnTelemycampaignInboundComponent } from './component/crm-trn-telemycampaign-inbound/crm-trn-telemycampaign-inbound.component';
import { CrmSmmSmslogComponent } from './component/crm-smm-smslog/crm-smm-smslog.component';
import { CrmSmmSmscontactlogComponent } from './component/crm-smm-smscontactlog/crm-smm-smscontactlog.component';
import { CrmTrnUpcomingtelecallerComponent } from './component/crm-trn-upcomingtelecaller/crm-trn-upcomingtelecaller.component';
import { CrmTrnAssignvisitsummaryupcomingComponent } from './component/crm-trn-assignvisitsummaryupcoming/crm-trn-assignvisitsummaryupcoming.component';
import { CrmTrnAssignvisitsummaryexpiredComponent } from './component/crm-trn-assignvisitsummaryexpired/crm-trn-assignvisitsummaryexpired.component';
import { CrmTrnAssignvisitsummaryassignedComponent } from './component/crm-trn-assignvisitsummaryassigned/crm-trn-assignvisitsummaryassigned.component';
import { CrmTrnMyvisitupcomingComponent } from './component/crm-trn-myvisitupcoming/crm-trn-myvisitupcoming.component';
import { CrmTrnMyvisitexpiredComponent } from './component/crm-trn-myvisitexpired/crm-trn-myvisitexpired.component';
import { CrmTrnTfilemanagementComponent } from './component/crm-trn-tfilemanagement/crm-trn-tfilemanagement.component';
import { CrmTrnTfolderviewComponent } from './component/crm-trn-tfolderview/crm-trn-tfolderview.component';
import { CrmTrnTcontactmanagementComponent } from './component/crm-trn-tcontactmanagement/crm-trn-tcontactmanagement.component';
import { CrmTrnContactCorporateViewComponent } from './component/crm-trn-contact-corporate-view/crm-trn-contact-corporate-view.component';
import { CrmTrnContactIndividualViewComponent } from './component/crm-trn-contact-individual-view/crm-trn-contact-individual-view.component';
import { CrmTrnContactManagementAddComponent } from './component/crm-trn-contact-management-add/crm-trn-contact-management-add.component';
import { CrmTrnContactManagementCorporateEditComponent } from './component/crm-trn-contact-management-corporate-edit/crm-trn-contact-management-corporate-edit.component';
import { CrmTrnContactManagementEditComponent } from './component/crm-trn-contact-management-edit/crm-trn-contact-management-edit.component';
import { CrmTrnContactManagementIndividualEditComponent } from './component/crm-trn-contact-management-individual-edit/crm-trn-contact-management-individual-edit.component';
import { CrmTrnContactManagementViewComponent } from './component/crm-trn-contact-management-view/crm-trn-contact-management-view.component';
import { CrmTrnContactCorporateaddComponent } from './component/crm-trn-contact-corporateadd/crm-trn-contact-corporateadd.component';
import { CrmTrnCorporateContactSummaryComponent } from './component/crm-trn-corporate-contact-summary/crm-trn-corporate-contact-summary.component';
import { CrmTrnIndividualContactSummaryComponent } from './component/crm-trn-individual-contact-summary/crm-trn-individual-contact-summary.component';
import { CrmTrnTcontactCountComponent } from './component/crm-trn-tcontact-count/crm-trn-tcontact-count.component';
import { CrmMstCustomertypeComponent } from './component/crm-mst-customertype/crm-mst-customertype.component';
import { CrmTrnTteamleadsviewComponent } from './component/crm-trn-tteamleadsview/crm-trn-tteamleadsview.component';
import { CrmTrnTeleteamviewComponent } from './component/crm-trn-teleteamview/crm-trn-teleteamview.component';
import { CrmSmmInstagrampageComponent } from './component/crm-smm-instagrampage/crm-smm-instagrampage.component';
import { CrmTrnMycallsaddleadsComponent } from './component/crm-trn-mycallsaddleads/crm-trn-mycallsaddleads.component';
import { CrmTrnQuotationadd360NewComponent } from './component/crm-trn-quotationadd360-new/crm-trn-quotationadd360-new.component';
import { CrmTrnTwhatsappanalyticsComponent } from './component/crm-trn-twhatsappanalytics/crm-trn-twhatsappanalytics.component';
import { CrmMstTbusinessverticalComponent } from './component/crm-mst-tbusinessvertical/crm-mst-tbusinessvertical.component';
import { CrmTrnMyappointmentsummaryComponent } from './component/crm-trn-myappointmentsummary/crm-trn-myappointmentsummary.component';
import { CrmTrnMyappointmenttodayComponent } from './component/crm-trn-myappointmenttoday/crm-trn-myappointmenttoday.component';
import { CrmTrnMyappointmentupcomingComponent } from './component/crm-trn-myappointmentupcoming/crm-trn-myappointmentupcoming.component';
import { CrmTrnMyappointmentexpiredComponent } from './component/crm-trn-myappointmentexpired/crm-trn-myappointmentexpired.component';
import { CrmTrnAppointmentmanagementComponent } from './component/crm-trn-appointmentmanagement/crm-trn-appointmentmanagement.component';
import { CrmTrnMyappointmentnewComponent } from './component/crm-trn-myappointmentnew/crm-trn-myappointmentnew.component';
import { CrmTrnMyappointmentprospectComponent } from './component/crm-trn-myappointmentprospect/crm-trn-myappointmentprospect.component';
import { CrmTrnMyappointmentpotentialComponent } from './component/crm-trn-myappointmentpotential/crm-trn-myappointmentpotential.component';
import { CrmTrnMyappointmentclosedComponent } from './component/crm-trn-myappointmentclosed/crm-trn-myappointmentclosed.component';
import { CrmTrnMyappointmentdropComponent } from './component/crm-trn-myappointmentdrop/crm-trn-myappointmentdrop.component';
import { CrmTrnContactIndividualeditComponent } from './component/crm-trn-contact-individualedit/crm-trn-contact-individualedit.component';
import { CrmTrnContactCorporateeditComponent } from './component/crm-trn-contact-corporateedit/crm-trn-contact-corporateedit.component';
import { CrmTrnAdvocacymanagementComponent } from './component/crm-trn-advocacymanagement/crm-trn-advocacymanagement.component';
import { CrmSmmOutlookcampaignsummaryComponent } from './component/crm-smm-outlookcampaignsummary/crm-smm-outlookcampaignsummary.component';
import { CrmSmmOutlookcampaigntemplateComponent } from './component/crm-smm-outlookcampaigntemplate/crm-smm-outlookcampaigntemplate.component';
import { CrmSmmOutlookinboxComponent } from './component/crm-smm-outlookinbox/crm-smm-outlookinbox.component';
import { CrmSmmMailsinboxComponent } from './component/crm-smm-mailsinbox/crm-smm-mailsinbox.component';
import { CrmSmmMailscomposeComponent } from './component/crm-smm-mailscompose/crm-smm-mailscompose.component';
import { CrmSmmMailsentComponent } from './component/crm-smm-mailsent/crm-smm-mailsent.component';
import { CrmMstSalutationsComponent } from './component/crm-mst-salutations/crm-mst-salutations.component';
import { CrmMstDesignationComponent } from './component/crm-mst-designation/crm-mst-designation.component';
import { CrmMstConstitutionComponent } from './component/crm-mst-constitution/crm-mst-constitution.component';
import { CrmTrnShopifycontactusComponent } from './component/crm-trn-shopifycontactus/crm-trn-shopifycontactus.component';
import { CrmSmmGmailinboxsummaryComponent } from './component/crm-smm-gmailinboxsummary/crm-smm-gmailinboxsummary.component';
import { CrmSmmOutlookcampaignsendComponent } from './component/crm-smm-outlookcampaignsend/crm-smm-outlookcampaignsend.component';
import { CrmSmmOutlookcampaignsentsummaryComponent } from './component/crm-smm-outlookcampaignsentsummary/crm-smm-outlookcampaignsentsummary.component';
import { CrmSmmLinkedinpostComponent } from './component/crm-smm-linkedinpost/crm-smm-linkedinpost.component';
import { CrmSmmGmailtrashsummaryComponent } from './component/crm-smm-gmailtrashsummary/crm-smm-gmailtrashsummary.component';
import { CrmSmmGmailfoldersummaryComponent } from './component/crm-smm-gmailfoldersummary/crm-smm-gmailfoldersummary.component';
import { CrmSmmGmailtagcustomerComponent } from './component/crm-smm-gmailtagcustomer/crm-smm-gmailtagcustomer.component';
import { CrmSmmGmailuntagcustomerComponent } from './component/crm-smm-gmailuntagcustomer/crm-smm-gmailuntagcustomer.component';
import { CrmTrnWhatsappcustomerreportComponent } from './component/crm-trn-whatsappcustomerreport/crm-trn-whatsappcustomerreport.component';
import { CrmTrnCreateopportunityComponent } from './component/crm-trn-createopportunity/crm-trn-createopportunity.component';
import { CrmTrnIndiamartenquiryComponent } from './component/crm-trn-indiamartenquiry/crm-trn-indiamartenquiry.component';
import { CrmSmmOutlookmailcomposeComponent } from './component/crm-smm-outlookmailcompose/crm-smm-outlookmailcompose.component';
import { CrmSmmOutlooksentitemsComponent } from './component/crm-smm-outlooksentitems/crm-smm-outlooksentitems.component';
import { CrmSmmOutlookinboxsummaryComponent } from './component/crm-smm-outlookinboxsummary/crm-smm-outlookinboxsummary.component';
import { CrmSmmOutlookfoldersummaryComponent } from './component/crm-smm-outlookfoldersummary/crm-smm-outlookfoldersummary.component';
import { CrmSmmOutlooktrashsummaryComponent } from './component/crm-smm-outlooktrashsummary/crm-smm-outlooktrashsummary.component';
import { CrmMstLeadtypeComponent } from './component/crm-mst-leadtype/crm-mst-leadtype.component';
import { CrmSmmGmaildirectinboxsummaryComponent } from './component/crm-smm-gmaildirectinboxsummary/crm-smm-gmaildirectinboxsummary.component';

const routes: Routes = [

  { path: 'CrmTrnSalesOrderSummary', component: CrmTrnSalesordersummaryComponent },
  { path: 'CrmTrnSalesOrderAdd', component: CrmTrnSalesorderaddComponent },
  { path: 'CrmMstSourcesummary', component: CrmMstSourcesummaryComponent },
  { path: 'CrmMstProductgroupsummary', component: CrmMstProductgroupsummaryComponent },
  { path: 'CrmMstCategoryindustrySummary', component: CrmMstCategoryindustrySummaryComponent },
  { path: 'CrmMstRegionsummary', component: CrmMstRegionsummaryComponent },
  { path: 'CrmSmmFacebookaccount', component: CrmSmmFacebookaccountComponent },
  { path: 'CrmSmmFacebookPage/:facebook_page_id', component: CrmSmmFacebookpageComponent },
  { path: 'CrmSmmTelegramaccount', component: CrmSmmTelegramaccountComponent },
  { path: 'CrmSmmLinkedaccount', component: CrmSmmLinkedaccountComponent },
  { path: 'CrmMstProductsummary', component: CrmMstProductsummaryComponent },
  { path: 'CrmMstProductAdd', component: CrmMstProductAddComponent },
  { path: 'CrmMstProductEdit/:product_gid', component: CrmMstProductEditComponent },
  { path: 'CrmMstProductView/:product_gid', component: CrmMstProductViewComponent },
  { path: 'CrmTrnLeadbanksummary', component: CrmTrnLeadbanksummaryComponent },
  { path: 'CrmTrnLeadbankadd/:lspage', component: CrmTrnLeadbankaddComponent },
  { path: 'CrmTrnLeadbankview/:leadbank_gid/:lspage', component: CrmTrnLeadbankviewComponent },
  { path: 'CrmTrnLeadbankedit/:leadbank_gid/:lspage', component: CrmTrnLeadbankeditComponent },
  { path: 'CrmTrnTelemycampaign', component: CrmTrnTelemycampaignComponent },
  { path: 'CrmTrnMyvisit', component: CrmTrnMyvisitComponent },
  { path: 'CrmCampaignMailmanagement', component: CrmCampaignMailmanagementComponent },
  { path: 'CrmTrnMarketingManagerSummary', component: CrmTrnMarketingmanagersummaryComponent },
  { path: 'CrmTrnCampaignmanager/:campaign_gid/:assign_to/:stage', component: CrmTrnCampaignmanagerComponent },
  { path: 'CrmTrnLeadMasterSummary', component: CrmTrnLeadmastersummaryComponent },
  { path: 'CrmTrnLeadmasteradd/:lspage', component: CrmTrnLeadmasteraddComponent },
  { path: 'CrmTrnLeadBankbranch/:leadbank_gid', component: CrmTrnLeadbankbranchComponent },
  { path: 'CrmTrnLeadbankbranchedit/:leadbank_gid/:leadbankcontact_gid', component: CrmTrnLeadbankbrancheditComponent },
  { path: 'CrmTrnAssignvisitsummary', component: CrmTrnAssignvisitsummaryComponent },
  { path: 'CrmSmmInstagram', component: CrmSmmInstagramComponent },
  { path: 'CrmMstCampaignsummary', component: CrmMstCampaignsummaryComponent },
  { path: 'CrmTrnLeadbankcontact/:leadbank_gid', component: CrmTrnLeadbankcontactComponent },
  { path: 'CrmTrnLeadbankcontactEdit/:leadbank_gid/:leadbankcontact_gid', component: CrmTrnLeadbankcontactEditComponent },
  { path: 'CrmTrnCampaignleadallocation/:campaign_gid/:employee_gid', component: CrmTrnCampaignleadallocationComponent },
  { path: 'CrmTrnMycampaign', component: CrmTrnMycampaignComponent },
  { path: 'CrmTrnAddtocustomer/:leadbank_gid', component: CrmTrnAddtocustomerComponent },
  { path: 'CrmDashboard', component: CrmDashboardComponent },
  { path: 'CrmSmmTelegramsummary', component: CrmSmmTelegramsummaryComponent },
  { path: 'CrmCampaignMailmanagementsummary', component: CrmCampaignMailmanagementsummaryComponent },
  { path: 'CrmTrnUpcomingMeetings', component: CrmTrnUpcomingmeetingsComponent },
  { path: 'CrmTrnNewtask', component: CrmTrnNewtaskComponent },
  { path: 'CrmTrnProspects', component: CrmTrnProspectsComponent },
  { path: 'CrmTrnPotentials', component: CrmTrnPotentialsComponent },
  { path: 'CrmTrnCompleted', component: CrmTrnCompletedComponent },
  { path: 'CrmTrnDropleads', component: CrmTrnDropleadsComponent },
  { path: 'CrmTrnAllleads', component: CrmTrnAllleadsComponent },
  { path: 'CrmMstCallresponse', component: CrmMstCallresponseComponent },
  { path: 'CrmSmmWhatsapp', component: CrmSmmWatsappComponent },
  { path: 'CrmSmmComposemail', component: CrmSmmComposemailComponent },
  { path: 'CrmSmmShopifycustomer', component: CrmSmmShopifycustomerComponent },
  { path: 'CrmSmmWhatsappmessagetemplate', component: CrmSmmWhatsappmessagetemplateComponent },
  { path: 'CrmTrnCorporateRegisterLead', component: CrmTrnCorporateregisterleadComponent },
  { path: 'CrmTrnRetailerRegisterLead', component: CrmTrnRetailerregisterleadComponent },
  { path: 'CrmTrnCorporateLeadBank', component: CrmTrnCorporateleadbankComponent },
  { path: 'CrmTrnRetailerLeadBank', component: CrmTrnRetailerleadbankComponent },
  { path: 'CrmSmmSendmail/:mailmanagement_gid/:leadbank_gid', component: CrmSmmSendmailComponent },
  { path: 'CrmSmmEmailmanagement', component: CrmSmmEmailmanagementComponent },
  { path: 'CrmSocailMediaDashboard', component: CrmSocailMediaDashboardComponent },
  { path: 'CrmSmmShopifycustomerassigned', component: CrmSmmShopifycustomerassignedComponent },
  { path: 'CrmSmmShopifycustomerunassigned', component: CrmSmmShopifycustomerunassignedComponent },
  { path: 'CrmSmmMailcampaigntemplate', component: CrmSmmMailcampaigntemplateComponent },
  { path: 'CrmSmmMailcampaignsummary', component: CrmSmmMailcampaignsummaryComponent },
  { path: 'CrmSmmMailcampaignsend/:temp_mail_gid', component: CrmSmmMailcampaignsendComponent },
  { path: 'CrmSmmMailcampaigntemplateview/:temp_mail_gid', component: CrmSmmMailcampaigntemplateviewComponent },
  { path: 'CrmSmmMailcampaignsendstatus/:temp_mail_gid', component: CrmSmmMailcampaignsendstatusComponent },
  { path: 'CrmSmmWhatsappcampaign', component: CrmSmmWhatsappcampaignComponent },
  { path: 'CrmSmmWhatsapplog/:project_id', component: CrmSmmWhatsapplogComponent },
  { path: 'CrmTrnRetaileradd/:lspage', component: CrmTrnRetaileraddComponent },
  { path: 'CrmTrnRetailerview/:leadbank_gid/:lspage', component: CrmTrnRetailerviewComponent },
  { path: 'CrmTrnRetaileredit/:leadbank_gid/:lspage', component: CrmTrnRetailereditComponent },
  { path: 'CrmTrn360view/:leadbank_gid/:appointment_gid/:lspage', component: CrmTrnOverallviewComponent },
  { path: 'CrmTrnUpcomingmarketing', component: CrmTrnUpcomingmarketingComponent },
  { path: 'CrmTrnNewmarketing', component: CrmTrnNewmarketingComponent },
  { path: 'CrmTrnProspectmarketing', component: CrmTrnProspectmarketingComponent },
  { path: 'CrmTrnPotentialmarketing', component: CrmTrnPotentialmarketingComponent },
  { path: 'CrmTrnCustomermarketing', component: CrmTrnCustomermarketingComponent },
  { path: 'CrmTrnLapsedleadmarketing', component: CrmTrnLapsedleadmarketingComponent },
  { path: 'CrmTrnLongestleadmarketing', component: CrmTrnLongestleadmarketingComponent },
  { path: 'CrmTrnDropmarketing', component: CrmTrnDropmarketingComponent },
  { path: 'CrmSmmSmscampaign', component: CrmSmmSmscampaignComponent },
  { path: 'CrmSmmSmscampaignsend/:template_id', component: CrmSmmSmscampaignsendComponent },
  { path: 'CrmSmmCampaignsettings', component: CrmSmmCampaignsettingsComponent },
  { path: 'CrmSmmJustdial', component: CrmSmmJustdialComponent },
  { path: 'CrmSmmIndiamart', component: CrmSmmIndiamartComponent },
  { path: 'CrmSmmTradeindia', component: CrmSmmTradeindiaComponent },
  { path: 'CrmSmmFlipkart', component: CrmSmmFlipkartComponent },
  { path: 'CrmSmmAmazon', component: CrmSmmAmazonComponent },
  { path: 'CrmSmmWebsite', component: CrmSmmWebsiteComponent },
  { path: 'CrmSmmMeesho', component: CrmSmmMeeshoComponent },
  { path: 'CrmSmmFacebookpostview/:facebookmain_gid', component: CrmSmmFacebookpostviewComponent },
  { path: 'CrmCrmTrnMtd', component: CrmTrnMtdComponent },
  { path: 'CrmTrnYtd', component: CrmTrnYtdComponent },
  { path: 'CrmMstCampaignexpand/:campaign_gid', component: CrmMstCampaignexpandComponent },
  { path: 'CrmTrnTraiseorder/:leadbank_gid/:leadbankcontact_gid/:lead2campaign_gid/:lspage', component: CrmTrnTraiseorderComponent },
  { path: 'CrmTrnTrasieinvoice', component: CrmTrnTrasieinvoiceComponent },
  { path: 'CrmTrnTraiseenquiry/:leadbank_gid/:leadbankcontact_gid/:lead2campaign_gid/:lspage', component: CrmTrnTcustomerraiseenquiryComponent },
  //{ path: 'CrmTrnTraisequtation/:leadbank_gid/:leadbankcontact_gid/:lead2campaign_gid/:lspage', component: CrmTrnTraisequtationComponent },
  { path: 'CrmSmmShopifyProductAdd', component: CrmSmmShopifyproductaddComponent },
  { path: 'CrmSmmShopifyProductEdit/:id', component: CrmSmmShopifyproducteditComponent },
  { path: 'CrmSmmWebsiteChat', component: CrmSmmWebsitechatsComponent },
  { path: 'CrmTrnCustomerProductPrice/:customer_gid/:leadbank_gid/:lead2campaign_gid/:leadbankcontact_gid/:lspage', component: CrmTrnCustomerProductPriceComponent },
  { path: 'CrmRptActivitylogreport', component: CrmRptActivitylogreportComponent},
  { path: 'CrmRptEnquiryrpt', component: CrmRptEnquiryrptComponent},
  { path: 'CrmRptProductconsumptionreport' ,component: CrmRptProductconsumptionreportComponent},
  { path: 'CrmRptProductgroupreport' ,component: CrmRptProductgroupreportComponent},
  { path: 'CrmRptProductpreviouspricereport' ,component: CrmRptProductpreviouspricereportComponent},
  { path: 'CrmMstWabulkmessage/:project_id/:version_id', component: CrmMstWabulkmessageComponent },
  { path: 'CrmSmmWebsitechatanalytics' ,component: CrmSmmWebsitechatanalyticsComponent},
  { path: 'CrmTrnQuoteadd/:leadbank_gid/:leadbankcontact_gid/:lead2campaign_gid/:lspage' ,component: CrmTrnQuoteaddComponent},
  { path: 'CrmSmmWacustomizebulkmessage' ,component: CrmSmmWacustomizebulkmessageComponent},
  { path: 'CrmMstProductunitsummary' ,component: CrmMstProductunitsummaryComponent},
  { path: 'CrmSmmWhatsappcampaigncreation' ,component: CrmSmmWhatsappcampaigncreationComponent},
  { path: 'CrmSmmClicktocall' ,component: CrmSmmClicktocallComponent},
  { path: 'CrmSmmClicktocallagents', component: CrmSmmClicktocallagentsComponent },
  { path: 'CrmSmmClicktocalllog', component: CrmSmmClicktocalllogComponent },
  { path: 'CrmTrnImportfromlead', component: CrmTrnImportfromleadComponent },
  { path: 'CrmTrnTelecallerTeam', component: CrmTrnTelecallerTeamComponent },
  { path: 'CrmTrnTelecampaignmanagersummary', component: CrmTrnTelecampaignmanagersummaryComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryNew', component: CrmTrnTelecampaignmanagersummaryNewComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryPendingcalls', component: CrmTrnTelecampaignmanagersummaryPendingcallsComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryFollowup', component: CrmTrnTelecampaignmanagersummaryFollowupComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryProspect', component: CrmTrnTelecampaignmanagersummaryProspectComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryDrop', component: CrmTrnTelecampaignmanagersummaryDropComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryScheduled', component: CrmTrnTelecampaignmanagersummaryScheduledComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryLapsedleads', component: CrmTrnTelecampaignmanagersummaryLapsedleadsComponent},
  { path: 'CrmTrnTelecampaignmanagersummaryLongestleads', component: CrmTrnTelecampaignmanagersummaryLongestleadsComponent},
  { path: 'CrmTrnTelacallerteamLeadComponent/:campaign_gid/:employee_gid', component: CrmTrnTelacallerteamLeadComponent },
  { path: 'CrmSmmGmailcampaignsend/:template_gid', component: CrmSmmGmailcampaignsendComponent },
  { path: 'CrmSmmGmailcampaignsendstatus/:template_gid', component: CrmSmmGmailcampaignsendstatusComponent},
  { path: 'CrmSmmGmailcampaignsummary', component: CrmSmmGmailcampaignsummaryComponent },
  { path: 'CrmSmmGmailcampaigntemplate', component:CrmSmmGmailcampaigntemplateComponent },
  { path: 'CrmSmmGmailinbox', component: CrmSmmGmailinboxComponent },
  { path: 'CrmSmmGmailview/:gmail_gid/:leadbank_gid', component: CrmSmmGmailviewComponent },
  { path: 'CrmTrnTelemycampaignNew', component: CrmTrnTelemycampaignNewComponent },
  { path: 'CrmTrnTelemycampaignPending', component: CrmTrnTelemycampaignPendingComponent },
  { path: 'CrmTrnTelemycampaignFollowup', component: CrmTrnTelemycampaignFollowupComponent },
  { path: 'CrmTrnTelemycampaignProspect', component: CrmTrnTelemycampaignProspectComponent },
  { path: 'CrmTrnTelemycampaignDrop', component: CrmTrnTelemycampaignDropComponent },
  { path: 'CrmTrnTelemycampaignAll', component: CrmTrnTelemycampaignAllComponent },
  { path: 'CrmTrnTelemycampaignInbound', component: CrmTrnTelemycampaignInboundComponent },
  { path: 'CrmSmmSmslog/:template_id', component: CrmSmmSmslogComponent },
  { path: 'CrmSmmSmscontactlog', component: CrmSmmSmscontactlogComponent },
  { path: 'CrmTrnUpcomingtelecaller', component: CrmTrnUpcomingtelecallerComponent },
  { path: 'CrmTrnAssignvisitsummaryupcoming', component: CrmTrnAssignvisitsummaryupcomingComponent },
  { path: 'CrmTrnAssignvisitsummaryexpired', component: CrmTrnAssignvisitsummaryexpiredComponent },
  { path: 'CrmTrnAssignvisitsummaryassigned', component: CrmTrnAssignvisitsummaryassignedComponent },
  { path: 'CrmTrnMyvisitexpired', component: CrmTrnMyvisitexpiredComponent},
  { path: 'CrmTrnMyvisitupcoming', component: CrmTrnMyvisitupcomingComponent},
  { path: 'CrmTrnTfilemanagement', component: CrmTrnTfilemanagementComponent},
  { path: 'CrmTrnTfolderview/:folder', component: CrmTrnTfolderviewComponent},
  { path: 'CrmTrnTcontactmanagement', component: CrmTrnTcontactmanagementComponent},
  { path: 'CrmTrnTcontactCount', component: CrmTrnTcontactCountComponent},
  { path: 'CrmTrnContactCorporateView/:leadbank_gid', component: CrmTrnContactCorporateViewComponent },
  { path: 'CrmTrnContactIndividualView/:leadbank_gid', component: CrmTrnContactIndividualViewComponent },
  { path: 'CrmTrnContactManagementAdd', component: CrmTrnContactManagementAddComponent },
  { path: 'CrmTrnContactManagementCorporateEdit', component: CrmTrnContactManagementCorporateEditComponent },
  { path: 'CrmTrnContactManagementEdit', component: CrmTrnContactManagementEditComponent },
  { path: 'CrmTrnContactManagementIndividualEdit', component: CrmTrnContactManagementIndividualEditComponent },
  { path: 'CrmTrnContactManagementView', component: CrmTrnContactManagementViewComponent},
  { path: 'CrmTrnContactCorporateadd', component: CrmTrnContactCorporateaddComponent},
  { path: 'CrmTrnCorporateContactSummary', component: CrmTrnCorporateContactSummaryComponent},
  { path: 'CrmTrnIndividualContactSummary', component: CrmTrnIndividualContactSummaryComponent},
  { path: 'CrmMstCustomertypeComponent', component: CrmMstCustomertypeComponent},
  { path: 'CrmTrnTteamleadsview/:encryptedParam', component: CrmTrnTteamleadsviewComponent},
  { path: 'CrmTrnTeleteamview/:encryptedParam', component: CrmTrnTeleteamviewComponent},
  { path: 'CrmSmmInstagrampage/:account_id', component: CrmSmmInstagrampageComponent },
  { path: 'CrmTrnMycallsaddleads/:lspage', component: CrmTrnMycallsaddleadsComponent },
  { path: 'CrmTrnQuoteAdd360New/:leadbank_gid/:leadbankcontact_gid/:lead2campaign_gid/:lspage', component: CrmTrnQuotationadd360NewComponent},
  { path: 'CrmTrnTwhatsappanalytics', component: CrmTrnTwhatsappanalyticsComponent },
  { path: 'CrmMstTbusinessvertical', component: CrmMstTbusinessverticalComponent },
  { path: 'CrmTrnMyappointmentsummary', component: CrmTrnMyappointmentsummaryComponent },
  { path: 'CrmTrnMyappointmenttoday', component: CrmTrnMyappointmenttodayComponent },
  { path: 'CrmTrnMyappointmentupcoming', component: CrmTrnMyappointmentupcomingComponent },
  { path: 'CrmTrnMyappointmentexpired', component: CrmTrnMyappointmentexpiredComponent },
  { path: 'CrmTrnAppointmentmanagement', component: CrmTrnAppointmentmanagementComponent },
  { path: 'CrmTrnMyappointmentnew', component: CrmTrnMyappointmentnewComponent },
  { path: 'CrmTrnMyappointmentprospect', component: CrmTrnMyappointmentprospectComponent },
  { path: 'CrmTrnMyappointmentpotential', component: CrmTrnMyappointmentpotentialComponent },
  { path: 'CrmTrnMyappointmentclosed', component: CrmTrnMyappointmentclosedComponent },
  { path: 'CrmTrnMyappointmentdrop', component: CrmTrnMyappointmentdropComponent },
  { path: 'CrmTrnContactIndividualedit/:leadbank_gid', component: CrmTrnContactIndividualeditComponent },
  { path: 'CrmTrnContactCorporateedit/:leadbank_gid', component: CrmTrnContactCorporateeditComponent },
  { path: 'CrmTrnAdvocacymanagement', component: CrmTrnAdvocacymanagementComponent },
  { path: 'CrmSmmOutlookcampaignsummary', component: CrmSmmOutlookcampaignsummaryComponent },
  { path: 'CrmSmmOutlookcampaigntemplate', component: CrmSmmOutlookcampaigntemplateComponent },
  { path: 'CrmSmmOutlookinbox', component: CrmSmmOutlookinboxComponent },
  { path: 'CrmSmmMailsinbox', component: CrmSmmMailsinboxComponent },
  { path: 'CrmSmmMailscompose', component: CrmSmmMailscomposeComponent },
  { path: 'CrmSmmMailsent', component: CrmSmmMailsentComponent },
  { path: 'CrmMstSalutations', component: CrmMstSalutationsComponent },
  { path: 'CrmMstDesignation', component: CrmMstDesignationComponent },
  { path: 'CrmMstConstitution', component: CrmMstConstitutionComponent },
  { path: 'CrmTrnShopifycontactus', component: CrmTrnShopifycontactusComponent },
  { path: 'CrmSmmGmailInboxSummary', component: CrmSmmGmailinboxsummaryComponent },
  { path: 'CrmSmmOutlookcampaignsend/:template_gid/:template_name', component: CrmSmmOutlookcampaignsendComponent },
  { path: 'CrmSmmOutlookcampaignsentsummary/:template_gid/:template_name', component: CrmSmmOutlookcampaignsentsummaryComponent },
  { path: 'CrmSmmLinkedinpost/:account_id', component: CrmSmmLinkedinpostComponent },
  { path: 'CrmSmmGmailTrashSummary', component: CrmSmmGmailtrashsummaryComponent },
  { path: 'CrmSmmGmailFolderSummary', component: CrmSmmGmailfoldersummaryComponent },
  { path: 'CrmSmmGmailTagCustomer/:inbox_id/:lspage', component: CrmSmmGmailtagcustomerComponent },
  { path: 'CrmSmmGmailUntagCustomer/:inbox_id/:lspage', component: CrmSmmGmailuntagcustomerComponent },
  { path: 'CrmTrnWhatsappcustomerreport', component: CrmTrnWhatsappcustomerreportComponent },
  { path: 'CrmTrnCreateopportunity', component: CrmTrnCreateopportunityComponent },
  { path: 'CrmTrnIndiamartenquiry', component: CrmTrnIndiamartenquiryComponent },
  { path: 'CrmSmmOutlookmailcompose', component: CrmSmmOutlookmailcomposeComponent },
  { path: 'CrmSmmOutlooksentitems', component: CrmSmmOutlooksentitemsComponent },
  { path: 'CrmSmmOutlookInboxSummary', component: CrmSmmOutlookinboxsummaryComponent },
  { path: 'CrmSmmOutlookFolderSummary', component: CrmSmmOutlookfoldersummaryComponent },
  { path: 'CrmSmmOutlookTrashSummary', component: CrmSmmOutlooktrashsummaryComponent},
  { path: 'CrmTrnFileManagement', component: CrmTrnTfilemanagementComponent},
  { path: 'CrmTrnFolderview/:docupload_gid1/:docupload_name2', component: CrmTrnTfolderviewComponent},
  { path: 'CrmSmmGmailInboxSummary/:inbox_gid', component: CrmSmmGmailinboxsummaryComponent },
  { path: 'CrmSmmOutlookInboxSummary/:inbox_gid', component: CrmSmmOutlookinboxsummaryComponent },
  { path: 'CrmTrnLeadbankcontact/:leadbank_gid', component: CrmTrnLeadbankcontactComponent },
  { path: 'CrmMstLeadtype', component: CrmMstLeadtypeComponent},
  { path: 'CrmSmmGmailDirectInboxSummary', component: CrmSmmGmaildirectinboxsummaryComponent},


];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsCrmRoutingModule { }
