import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { fadeInOut, INavbarData } from '../../model/layout_model';
import { SharedService } from '../../services/shared.service';


@Component({
  selector: 'layout-sublevel-menu',
  template: `
    <ul *ngIf="collapsed && data.sub1menu && data.sub1menu.length > 0"
    [@submenu]="expanded
      ? {value: 'visible', 
          params: {transitionParams: '400ms cubic-bezier(0.86, 0, 0.07, 1)', height: '*'}}
      : {value: 'hidden', 
          params: {transitionParams: '400ms cubic-bezier(0.86, 0, 0.07, 1)', height: '0'}}"
      class="sublevel-nav"
    >
      <li *ngFor="let item of data.sub1menu; let i = index;" class="sublevel-nav-item">
          <a class="sublevel-nav-link"
          (click)="handleClick(item); redirectToPage(item.sref);"
            *ngIf="item.sub2menu && item.sub2menu.length > 0"
            [ngClass]="getActiveClass(item)"
          >
            <i class="sublevel-link-icon fa fa-circle"></i>
            <span class="sublevel-link-text" @fadeInOut 
                *ngIf="collapsed">{{item.text}}</span>
            <i *ngIf="item.sub2menu && collapsed" class="menu-collapse-icon"
              [ngClass]="!item.expanded ? 'fa fa-angle-right' : 'fa fa-angle-down'"
            ></i>
          </a>
          <a class="sublevel-nav-link"
            *ngIf="!item.sub2menu || (item.sub2menu && item.sub2menu.length === 0)"
            (click)="levelThreeMenu(i);redirectToPage(item);"
            routerLinkActive="active-sublevel"
            [routerLinkActiveOptions]="{exact: true}"
          >
            <i class="sublevel-link-icon fa fa-circle"></i>
            <span class="sublevel-link-text" @fadeInOut 
               *ngIf="collapsed">{{item.text}}</span>
          </a>
          <div *ngIf="item.sub2menu && item.sub2menu.length > 0">
           
          <ul *ngIf="collapsed && item.sub2menu && item.sub2menu.length > 0"
          [@submenu]="item.expanded
            ? {value: 'visible', 
                params: {transitionParams: '400ms cubic-bezier(0.86, 0, 0.07, 1)', height: '*'}}
            : {value: 'hidden', 
                params: {transitionParams: '400ms cubic-bezier(0.86, 0, 0.07, 1)', height: '0'}}"
            class="sublevel2-nav"
          >
            <li *ngFor="let item2 of item.sub2menu" class="sublevel-nav-item">
                
                <a class="sublevel-nav-link"
                  *ngIf="!item2.items || (item2.items && item2.items.length === 0)"
                  (click)="redirectToPage(item2)"
                  routerLinkActive="active-sublevel"
                  [routerLinkActiveOptions]="{exact: true}"
                >
                  <i class="sublevel-link-icon fa fa-circle"></i>
                  <span class="sublevel-link-text" @fadeInOut 
                     *ngIf="collapsed">{{item2.text}}</span>
                </a>
                
            </li>
          </ul>
          </div>
      </li>
    </ul>
  `,
  styleUrls: ['./sidenav.component.scss'],
  animations: [
    fadeInOut,
    trigger('submenu', [
      state('hidden', style({
        height: '0',
        overflow: 'hidden'
      })),
      state('visible', style({
        height: '*'
      })),
      transition('visible <=> hidden', [style({overflow: 'hidden'}), 
        animate('{{transitionParams}}')]),
      transition('void => *', animate(0))
    ])
  ]
})
export class SublevelMenuComponent implements OnInit {

  @Input() data: INavbarData = {
    sref: '',
    text: '',
    sub1menu: []
  }
  @Input() collapsed = false;
  @Input() animating: boolean | undefined;
  @Input() expanded: boolean | undefined;
  @Input() multiple: boolean = false;
  index!: number;

  constructor(
    public router: Router,
    public sharedservice:SharedService
  ) {}

  ngOnInit(): void {
    this.sharedservice.setFunction1ToCall(this.submenu1call.bind(this));
  }

  handleClick(item: any): void {
    if (!this.multiple) {
      if (this.data.sub1menu && this.data.sub1menu.length > 0) {
        for(let modelItem of this.data.sub1menu) {
          if (item !==modelItem && modelItem.expanded) {
            modelItem.expanded = false;
          }
        }
      }
    }
    item.expanded = !item.expanded;
  }

  getActiveClass(item: any): string {
    return item.expanded && this.router.url.includes(item.sref) 
      ? 'active-sublevel' 
      : '';
  }

  submenu1call(){
    this.handleClick(this.data.sub1menu);
  }

  redirectToPage(data:any){
    this.sharedservice.setMenuThree(this.data.sub1menu[this.index]);
    this.sharedservice.setMenuFour(data);
    this.sharedservice.menuOneTwo();
    this.sharedservice.menufunctionToCall();
    this.router.navigate([data.sref]);
    this.sharedservice.sideNavCloseCall();
  }
  
  levelThreeMenu(index: number){
     this.index = index;
   }

}
