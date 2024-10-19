import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { Component, Output, EventEmitter, OnInit, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';



import { NgxSpinnerService } from 'ngx-spinner';
import { SharedService } from '../../services/shared.service';
import { INavbarData, fadeInOut } from '../../model/layout_model';

interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}

@Component({
  selector: 'layout-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
  animations: [
    fadeInOut,
    trigger('rotate', [
      transition(':enter', [
        animate('1000ms',
          keyframes([
            style({ transform: 'rotate(0deg)', offset: '0' }),
            style({ transform: 'rotate(2turn)', offset: '1' })
          ])
        )
      ])
    ])
  ]
})
export class SidenavComponent implements OnInit {

  @Output() onToggleSideNav: EventEmitter<SideNavToggle> = new EventEmitter();
  collapsed = false;
  screenWidth = 0;
  // navData = navbarData;
  multiple: boolean = false;
  received: any;
  submenu: any;
  module_name: any;
  secondMenu !: INavbarData;
  current_domain: any;
  menulogo: any;
  sidenav: any;
  hideImage: any;
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.screenWidth = window.innerWidth;
    if (this.screenWidth <= 768) {
      this.collapsed = false;
      this.onToggleSideNav.emit({ collapsed: this.collapsed, screenWidth: this.screenWidth });
    }
  }

  constructor(
    public socketservice: SocketService,
    public router: Router,
    public sharedservice: SharedService,
    private NgxSpinnerService: NgxSpinnerService
  ) {

  }

  ngOnInit(): void {
    this.current_domain = window.location.hostname;
    if (this.current_domain == 'crm.bobateacompany.co.uk') {
      this.menulogo = './assets/media/logos/Boba_Tea_Company.png';
      this.sidenav = './assets/media/logos/Bobatesidenav.png';
    }
    else if (this.current_domain == 'manojbhavan.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/manoj_bhavan_menulogo.png';
      this.sidenav = './assets/media/logos/manoj_bhavan_sidenav_logo.png';
    }
    else if (this.current_domain == 'kot.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/sidenavlogosangeetha.png';
      this.sidenav = './assets/media/logos/sangeetha_mainlogo_2.png';
    }
    else if (this.current_domain == 'capwing.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/capwing_logo.png';
      this.sidenav = './assets/media/logos/capwing_side.png';
    }

    else if (this.current_domain == 'lawyer.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/saha_mainlogo.png';
      this.sidenav = './assets/media/logos/saha_toplogo.png';
    }

    else if (this.current_domain == 'medialink.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/MediaLink_Logo.png';
      this.sidenav = './assets/media/logos/MediaLink_Logo1.png';
    }
    else if (this.current_domain == 'noqu.storyboardsystems.com') {
      this.menulogo = './assets/media/logos/NoQu.png';
      this.sidenav = './assets/media/logos/NoQu.png';
    }
    else if (this.current_domain == 'techone.storyboardsystems.com') {
      this.menulogo = "./assets/media/logos/techonesidenav.png";
      this.sidenav = "./assets/media/logos/Techones.png";
    }
    else if (this.current_domain == 'ionicpharma.storyboardsystems.com') {
      this.menulogo = "./assets/media/logos/Iconicsidenav.png";
      this.sidenav = "./assets/media/logos/Iconicextend.png";
    }
    else if (this.current_domain == 'narpavi.storyboardsystems.com') {
      this.menulogo = "./assets/media/logos/narpaviSidenav.png";
      this.sidenav = "./assets/media/logos/narpavisidenavlogoss.png";
    }
    else if (this.current_domain == 'narpavi.storyboardsystems.com') {
      this.menulogo = "./assets/media/logos/narpaviSidenav.png";
      this.sidenav = "./assets/media/logos/narpavisidenavlogoss.png";
    }
    else if (this.current_domain == 'aarkay.storyboardsystems.com') {
      this.menulogo = "./assets/media/logos/sidenavkay.png";
      this.sidenav = "./assets/media/logos/sidenavkay.png";
    }
    else if (this.current_domain == 'handpicked.storyboardsystems.com') {

      this.menulogo = "./assets/media/logos/handpickedlogos.png";
      this.sidenav = "./assets/media/logos/handpickedlogos.png";
    }
    else {
      this.menulogo = "./assets/media/logos/vcx.png";
      this.sidenav = "./assets/media/logos/storyboardsystem_menu.png";
    }
    this.NgxSpinnerService.show();
    this.screenWidth = window.innerWidth;
    this.sharedservice.getData().subscribe((data) => {
      this.received = data;
      if (this.received != null) {
        this.submenu = this.received.submenu;
        this.module_name = this.received.text;
        //console.log('sidenav',this.module_name)
      }
      else {
        this.submenu = [];
      }
      //this.submenu = this.received.submenu;
    });

    this.sharedservice.setFunctionToCall(this.submenucall.bind(this));
    this.sharedservice.setSideNavCloseCall(this.toggleCollapse.bind(this));
    this.sharedservice.selectMenuOneTwo(this.secondMenuIdentifier.bind(this));
    this.sharedservice.selectMenuOne(this.secondMenuIdentifier.bind(this));
    setTimeout(() => { this.NgxSpinnerService.hide() }, 3000);

  }

  toggleCollapse(): void {
    if (this.collapsed === false) {
      this.collapsed = !this.collapsed;
      this.onToggleSideNav.emit({ collapsed: this.collapsed, screenWidth: this.screenWidth });
    }
  }
  // getmenu() {
  //   this.NgxSpinnerService.show();
  //   let user_gid = localStorage.getItem('user_gid');
  //   let param = {
  //     user_gid: user_gid
  //   }
  //   var url = 'User/topmenu';
  //   this.socketservice.getparams(url, param).subscribe((result: any) => {
  //     this.menu = result.menu_list;
  //     this.firstMenu = result.menu_list[1];
  //     //console.log('iuheiuhdwehd',this.firstMenu)

  //   });
  //   this.NgxSpinnerService.hide();
  // }
  closeSidenav(): void {
    this.collapsed = false;
    this.onToggleSideNav.emit({ collapsed: this.collapsed, screenWidth: this.screenWidth });
  }

  handleClick(item: INavbarData): void {
   
    sessionStorage.removeItem('CRM_LEADBANK_GID_ENQUIRY');
    sessionStorage.removeItem('CRM_APPOINTMENT_GID');
    sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
    sessionStorage.removeItem('CRM_TOMAILID');
    this.shrinkItems(item);
    item.expanded = !item.expanded;
    this.secondMenu = item;
  }

  getActiveClass(data: INavbarData): string {
    return this.router.url.includes(data.sref) ? 'active' : '';
  }

  shrinkItems(item: INavbarData): void {
    if (!this.multiple) {
      for (let modelItem of this.submenu) {
        if (item !== modelItem && modelItem.expanded) {
          modelItem.expanded = false;
        }
      }
    }
  }

  submenucall() {
    this.toggleCollapse();
    this.shrinkItems(this.submenu);
  }

  redirectToPage(data: string) {
    this.router.navigate([data]);
  }

  secondMenuIdentifier() {
    this.sharedservice.setMenuTwo(this.secondMenu);
    this.sharedservice.setMenuOne(this.received);
  }

  firstMenuIdentifier() {
    this.sharedservice.setMenuOne(this.received);
  }
}
