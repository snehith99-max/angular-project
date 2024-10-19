import { Component, HostListener, Input, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SharedService } from '../../services/shared.service';
import { Observable, interval, Subject } from 'rxjs';
import { takeWhile, map, takeUntil, catchError } from 'rxjs/operators';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  @Input() collapsed = false;
  @Input() screenWidth = 0;
  menu: any[] | undefined;
  sidemenu: any[] | string[] | undefined;
  menu_name: any;
  firstMenu: any;
  selectedIndex: number = 0;
  level_one_name: any;
  level_two_name: any;
  level_three_name: any;
  level_four_name: any;
  level_one_link: any;
  level_two_link: any;
  level_three_link: any;
  level_four_link: any;
  showBreadCurmList: boolean = false;
  notification_list: any[] = []; // Initialize as an empty array
  breadcrummaillist: any[] = []; // Initialize as an empty array
  notification_count: number = 0;
  showBadge: boolean = false;
  showMessage: boolean = false;
  windowInterval: any;
  employee_details: any;
  responsedata: any;
  IndiaMartInterval: any;
  hideButton: boolean = false;
  openPanel: boolean = false;
  home_page: any;
  service_flag: any;
  constructor(
    public socketservice: SocketService,
    public router: Router,
    public sharedservice: SharedService,
    private NgxSpinnerService: NgxSpinnerService,
    private route: Router,
    private ToastrService: ToastrService
  ) {
    this.waitForToken().subscribe(() => {
      this.getmenu();
      this.getemployeename();
      this.indiamartLeads();
      this.runBackgroundApiCall();
      this.runBackgroundApiCall1();
      this.notifications();
      this.userhome_page();

    });
  }
  async runBackgroundApiCall() {
    var api2 = 'GmailCampaign/GmailAPIInboxMailLoad';
    try {
      const result = await this.socketservice.get(api2).toPromise();
      // console.log(result);
    } catch (error) {
      //console.error(error);
    }
  }
  async runBackgroundApiCall1() {
    var api2 = 'OutlookCampaign/ReadEmailsOutlookmail';
    try {
      const result = await this.socketservice.get(api2).toPromise();
      // console.log(result);
    } catch (error) {
      //console.error(error);
    }
  }
  ngOnInit(): void {
    this.hideButton = window.location.host === 'lawyer.storyboardsystems.com';
    this.sharedservice.setMenuToCall(this.showBreadCurm.bind(this));
    this.showBreadCurm_local();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.windowInterval) {
      clearInterval(this.windowInterval);
    }
    if (this.IndiaMartInterval) {
      clearInterval(this.IndiaMartInterval);
    }
  }

  waitForToken(): Observable<boolean> {
    return interval(2000) // interval every 2 seconds  
      .pipe(
        takeUntil(this.destroy$), // Cleanup when the component is destroyed
        map(() => {
          const token = localStorage.getItem('token');
          return token !== null && token !== '';
        }),
        takeWhile((tokenAvailable) => !tokenAvailable, true),
        catchError((error) => {
          console.error('Error while polling for token:', error);
          return [];
        })
      );
  }

  getHeaderClass(): string {
    let styleClass = '';
    if (this.collapsed && this.screenWidth > 768) {
      styleClass = 'head-trimmed';
    } else if (this.collapsed && this.screenWidth <= 768 && this.screenWidth > 0) {
      styleClass = 'head-md-screen';
    }
    return styleClass;
  }

  onClickNotification() {
    this.showMessage = !this.showMessage;
  }

  getmenu() {
    this.NgxSpinnerService.show();
    const user_gid = localStorage.getItem('user_gid');
    const param = { user_gid };
    const url = 'User/topmenu';
    this.socketservice.getparams(url, param).subscribe((result: any) => {
      this.menu = result.menu_list;
      const menuState = localStorage.getItem('menuState');
      if (menuState) {
        const { selectedIndex, selectedData } = JSON.parse(menuState);
        this.selectedIndex = selectedIndex;
        this.sharedservice.setData(selectedData);
      } else {
        this.firstMenu = result.menu_list[0];
        this.sharedservice.setData(this.firstMenu);
      }
      this.NgxSpinnerService.hide();
    }, error => {
      console.error('Error fetching menu:', error);
      this.NgxSpinnerService.hide();
    });
  }

  logout() {
    localStorage.clear();
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
    sessionStorage.removeItem('CRM_TOMAILID');
    this.router.navigate(['auth/login']);
  }

  social() {
    this.router.navigate(['crm/CrmSocailMediaDashboard']);
  }

  service() {
    this.router.navigate(['crm/CrmSmmCampaignsettings']);
  }

  getsidemenu(data: any, index: number) {
    this.sharedservice.setData(data);
    this.sharedservice.functionToCall();
    const menuState = {
      selectedIndex: index,
      selectedData: data
    };
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
    sessionStorage.removeItem('CRM_TOMAILID');
    localStorage.setItem('menuState', JSON.stringify(menuState));
    if (data.sref) {
      this.router.navigate([data.sref]);
    }
  }

  redirect_menu(data: any) {
    if (data) {
      this.router.navigate([data]);
    }
  }

  redirect_menu_header(data: any, name: any) {
    if (data) {
      this.sharedservice.setmenuHeadPosition(name);
      this.sharedservice.sethead_index(true);
      this.sharedservice.setsecond_head_index(false);
      this.sharedservice.functionHeadToMenu();
      this.router.navigate([data]);
    }
  }

  selectHead(index: number) {
    this.selectedIndex = index;
  }

  showBreadCurm() {
    this.showBreadCurmList = true;
    this.sharedservice.getMenuOne().subscribe((data) => {
      this.level_one_name = data.text;
      this.level_one_link = data.sref;
    });
    this.sharedservice.getMenuTwo().subscribe((data) => {
      this.level_two_name = data.text;
      this.level_two_link = data.sref;
    });
    this.sharedservice.getMenuThree().subscribe((data) => {
      this.level_three_name = data.text;
      this.level_three_link = data.sref;
    });
    this.sharedservice.getMenuFour().subscribe((data) => {
      this.level_four_name = data.text;
      this.level_four_link = data.sref;
    });

    localStorage.removeItem("datas");
    const menuBreadCrum = [
      {
        "level_one_name": this.level_one_name,
        "level_one_link": this.level_one_link,
        "level_two_name": this.level_two_name,
        "level_two_link": this.level_two_link,
        "level_three_name": this.level_three_name,
        "level_three_link": this.level_three_link,
        "level_four_name": this.level_four_name,
        "level_four_link": this.level_four_link
      },
    ];
    localStorage.setItem("datas", JSON.stringify(menuBreadCrum));
  }

  showBreadCurm_local() {
    this.showBreadCurmList = true;
    const menuLocalData = JSON.parse(localStorage.getItem("datas") || '[]');
    if (menuLocalData.length) {
      const localData = menuLocalData[0];
      this.level_one_name = localData.level_one_name;
      this.level_one_link = localData.level_one_link;
      this.level_two_name = localData.level_two_name;
      this.level_two_link = localData.level_two_link;
      this.level_three_name = localData.level_three_name;
      this.level_three_link = localData.level_three_link;
      this.level_four_name = localData.level_four_name;
      this.level_four_link = localData.level_four_link;
    }
  }

  showNotifications(event: Event) {
    // Implementation here
  }

  routepage() {
    this.router.navigate(['system/MstUserProfile']);
  }

  customer360redirect(param1: string, param2: string) {
    this.showMessage = !this.showMessage;
    const secretKey = 'storyboarderp';
    const lspage1 = "LeadBankdistributor";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    const deencryptedParam = AES.decrypt(leadbank_gid, secretKey).toString(enc.Utf8);
    if (!deencryptedParam) {
      this.router.navigate(['/crm/CrmSmmWhatsapp']);

      let param = {
        sref: 'crm/CrmSmmWhatsapp'
      }
      var url = 'Whatsapp/Getbreadcrumbmail'
      this.socketservice.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.breadcrummaillist = this.responsedata.breadcrummaillist;
        this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
        this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
        this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
        this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
        this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
        this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
      });

    } else {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, lead2campaign_gid, lspage]);
      let param = {
        sref: 'crm/CrmTrn360view'
      }
      var url = 'Whatsapp/Getbreadcrumbmail'
      this.socketservice.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.breadcrummaillist = this.responsedata.breadcrummaillist;
        this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
        this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
        this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
        this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
        this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
        this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
      });

    }
  }

  customer360redirect1(param1: string, param2: string) {
    this.showMessage = !this.showMessage;
    const secretKey = 'storyboarderp';
    const lspage1 = "LeadBankdistributor";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    const deencryptedParam = AES.decrypt(leadbank_gid, secretKey).toString(enc.Utf8);
    if (!deencryptedParam) {
      this.router.navigate(['/crm/CrmSmmEmailmanagement']);

      let param = {
        sref: 'crm/CrmSmmEmailmanagement'
      }
      var url = 'Whatsapp/Getbreadcrumbmail'
      this.socketservice.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.breadcrummaillist = this.responsedata.breadcrummaillist;
        this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
        this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
        this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
        this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
        this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
        this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
      });

    } else {
      this.router.navigate(['/crm/CrmTrn360view', leadbank_gid, lead2campaign_gid, lspage]);
      let param = {
        sref: 'crm/CrmTrn360view'
      }
      var url = 'Whatsapp/Getbreadcrumbmail'
      this.socketservice.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.breadcrummaillist = this.responsedata.breadcrummaillist;
        this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
        this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
        this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
        this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
        this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
        this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
      });

    }
  }


  customer360redirect2(contact_id: any) {

    const param1 = contact_id;
    const key = 'storyboard';
    const inbox_gid = AES.encrypt(param1, key).toString();
    this.showMessage = !this.showMessage;
    this.router.navigate(['/crm/CrmSmmGmailInboxSummary', inbox_gid])

    let param = {
      sref: 'crm/CrmSmmGmailInboxSummary'
    }
    var url = 'Whatsapp/Getbreadcrumbmail'
    this.socketservice.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.breadcrummaillist = this.responsedata.breadcrummaillist;
      this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
      this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
      this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
      this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
      this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
      this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
    });

  }


  customer360redirect3(contact_id: any) {
    this.showMessage = !this.showMessage;
    const param1 = contact_id;
    const key = 'storyboard';
    const inbox_gid = AES.encrypt(param1, key).toString();
    this.showMessage = !this.showMessage;
    this.router.navigate(['/crm/CrmSmmOutlookInboxSummary', inbox_gid]);

    let param = {
      sref: 'crm/CrmSmmOutlookInboxSummary'
    }
    var url = 'Whatsapp/Getbreadcrumbmail'
    this.socketservice.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.breadcrummaillist = this.responsedata.breadcrummaillist;
      this.level_one_name = this.responsedata.breadcrummaillist[0].l1_menu
      this.level_two_name = this.responsedata.breadcrummaillist[0].l2_menu
      this.level_three_name = this.responsedata.breadcrummaillist[0].l3_menu
      this.level_one_link = this.responsedata.breadcrummaillist[0].l1_sref
      this.level_two_link = this.responsedata.breadcrummaillist[0].l2_sref
      this.level_three_link = this.responsedata.breadcrummaillist[0].l3_sref
    });

  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!event.target || !(event.target as HTMLElement).closest('#notification') && !(event.target as HTMLElement).closest('.sampel')) {
      this.showMessage = false;
    }
    if (!event.target || !(event.target as HTMLElement).closest('#meetings') && !(event.target as HTMLElement).closest('.sampel')) {
      this.openPanel = false;
    }
  }

  getemployeename() {
    const user_gid = localStorage.getItem('user_gid');
    const param = { user_gid };
    const url = 'ManageEmployee/GetEmployeename';
    this.socketservice.getparams(url, param).subscribe((result: any) => {
      this.employee_details = result.employeename_list[0]?.Name || 'Unknown';
      this.service_flag = result.employeename_list[0]?.service_flag || 'Unknown';
    }, error => {
      console.error('Error fetching employee name:', error);
    });
  }

  redirect_menu2(data: any, name: any) {
    if (data) {
      this.sharedservice.setmenuPosition(name);
      this.sharedservice.sethead_index(false);
      this.sharedservice.setsecond_head_index(true);
      this.router.navigate([data]);
    }
  }

  indiamartLeads() {
    const url = 'IndiaMART/SyncDetails';
    this.socketservice.get(url).subscribe((result: any) => {
      if (result.indiamart_status !== 'N' && result.indiamart_status) {
        this.IndiaMartInterval = window.setInterval(() => {
          const leadsUrl = 'IndiaMART/LoadLeadsFromIndiaMart';
          this.socketservice.get(leadsUrl).subscribe((result: any) => {
            console.log(result.message + "  " + result.code.toString());
            if (result.message === 'STOP' || result.code === 500) {
              clearInterval(this.IndiaMartInterval);
            } else if (result.code === 401) {
              this.ToastrService.warning(result.message);
            }
          });
        }, 390000);
      }
    }, error => {
      console.error('Error syncing IndiaMART leads:', error);
    });
  }

  notifications() {
    this.windowInterval = window.setInterval(() => {
      const url = 'Whatsapp/waNotifications';
      this.socketservice.get(url).subscribe((result: any) => {
        this.notification_list = result.notification_Lists || [];
        this.notification_count = result.notification_count || 0;
        this.showBadge = this.notification_count > 0;
      }, error => {
        console.error('Error fetching notifications:', error);
      });
    }, 30000);
  }

  openMeetingPanel() {
    this.openPanel = !this.openPanel;
  }
  userhome_page() {
    var api = 'CampaignService/Getuserhomepage';
    this.socketservice.get(api).subscribe((result: any) => {
      this.home_page = result.home_page;
    });
  }
}
