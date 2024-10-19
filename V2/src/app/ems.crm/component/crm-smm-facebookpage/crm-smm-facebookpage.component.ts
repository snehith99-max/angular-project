import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { param } from 'jquery';

@Component({
  selector: 'app-crm-smm-facebookpage',
  templateUrl: './crm-smm-facebookpage.component.html',
  styleUrls: ['./crm-smm-facebookpage.component.scss']
})
export class CrmSmmFacebookpageComponent {
  @ViewChild('Inbox') tableRef!: ElementRef;
  searchText = '';

  unique_query_id: any;
  indiamartsummarylist: any;
  responsedata: any;
  chatWindow: string = "Default";
  indiamartview_list: any;
  sender_name: any;
  last_sync_at: any;
  contactsync_till: any;
  nextsync_at: any;
  unique_query_count: any;
  query_type: any;
  sender_email: any;
  sender_mobile: any;
  sender_city: any;
  sender_state: any;
  sender_pincode: any;
  sender_country_iso: any;
  sender_mobile_alt: any;
  query_message: any;
  query_mcat_name: any;
  call_duration: any;
  query_product_name: any;
  sender_address: any;
  receiver_mobile: any;
  sender_company: any;
////////////////////////////
postsummary_List:any;
facebook_page_id: any;
id:any;
  postcomment_list: any;
  facebookpage_summarylist: any;
  post_url: any;
  post_id: any;
  facebookmain_gid: any;
  caption: any;
  views_count: any;
  post_type: any;
  facebookuser_list: any;
  postcreated_time: any;
  comments_count: any;
  isCommentVisible =false;
  deencryptedParam: any;
  page_id: any;
  profile_picture: any;
  user_name: any;
  page_name: any;
  showOptionsDivId: any;

  constructor(private fb: FormBuilder, private route: Router, public service: SocketService, private router: ActivatedRoute, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {

  }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    const facebook_page_id = this.router.snapshot.paramMap.get('facebook_page_id');
    this.facebook_page_id = facebook_page_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.facebook_page_id, secretKey).toString(enc.Utf8);
    this.page_id = deencryptedParam
    this.GetPagedetailssummary();
    this.Postsummarydetails(deencryptedParam);
   
  }

  Postsummarydetails(deencryptedParam: any) {
    this.NgxSpinnerService.show();
    var url = 'Facebook/GetPostsummarydetails' 
    let params = {
      facebook_page_id: deencryptedParam
    }
    this.service.getparams(url,params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.postsummary_List = this.responsedata.postsummary_List;
      this.page_name = this.responsedata.postsummary_List[0].user_name;
      

      setTimeout(() => {
        $('#postsummary_List').DataTable();
      }, 10000);
    });
  }
  toggleOptions(facebook_page_id: any) {
    if (this.showOptionsDivId === facebook_page_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = facebook_page_id;
    }
  }
  downloadImage(post_url: string, post_type: string) {
    if (post_url != null && post_url != "") {
      if (post_type === 'Picture') {
        saveAs(post_url, this.id + '.png');
      }
      else if (post_type === 'Videos') {
        saveAs(post_url, this.id + '.mp4');
      }
      else {
        saveAs(post_url, this.id + '.png');
      }
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }

  }
  onview(facebookmain_gid: any) {
    var url = 'Facebook/GetViewpostcomment'
    let param = {
      facebookmain_gid: facebookmain_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.postcomment_list = result.postcomment_list;
      this.post_url = result.post_url
      this.post_id = result.post_id
      this.facebookmain_gid = result.facebookmain_gid
      this.caption = result.caption
      this.views_count = result.views_count
      this.post_type = result.post_type
      this.postcreated_time = result.postcreated_time

      this.comments_count = result.comments_count
    });
  }
  GetPagedetailssummary() {
    this.NgxSpinnerService.show();
    var url = 'Facebook/GetPagedetailssummary'
    this.service.get(url).subscribe((result: any) => {
      // window.location.reload()
      this.responsedata = result;
      this.profile_picture = this.responsedata.facebookpage_summarylist[0].profile_picture
      this.user_name = this.responsedata.facebookpage_summarylist[0].user_name;
      // this.facebookpage_summarylist = this.responsedata.facebookpage_summarylist;

      setTimeout(() => {
        $('#facebookpage_summarylist').DataTable();
      }, 100);
      this.NgxSpinnerService.hide();

    });
  }
  toggleCommentVisibility() {
    this.isCommentVisible = !this.isCommentVisible;
  }
  onclose() {
   
  }
  postviewrefresh(page_id: any) {
    // Show spinner
    this.NgxSpinnerService.show();

    var url1 = 'Facebook/Getpagepost'
    let param = {
        page_id: page_id 
    };
    this.service.getparams(url1,param).subscribe((result: any) => {
        // Assign the response data
        this.responsedata = result;

        // Hide spinner after 10 seconds
        setTimeout(() => {
            this.NgxSpinnerService.hide();
            
            // Reload the page after hiding the spinner
            window.location.reload();
        }, 5000); // 10 seconds delay
    });
}
}