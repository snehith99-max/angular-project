import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-crm-smm-instagrampage',
  templateUrl: './crm-smm-instagrampage.component.html',
})
export class CrmSmmInstagrampageComponent {
  @ViewChild('Inbox') tableRef!: ElementRef;
  responsedata: any;
  instapostsummary_List: any;
  account_id: any;
  id: any;
  postcomment_list: any;
  post_url: any;
  post_id: any;
  caption: any;
  post_type: any;
  comments_count: any;
  isCommentVisible = false;
  deencryptedParam: any;
  username: any;
  instacomment_list:any;
  viewinsights_list:any;
  like_count:any;
  media_count: any;
  follows_count: any;
  followers_count: any;
  
  showOptionsDivId: any;
  constructor(private fb: FormBuilder, private route: Router, public service: SocketService, private router: ActivatedRoute, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {

  }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    const account_id = this.router.snapshot.paramMap.get('account_id');
    this.account_id = account_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.account_id, secretKey).toString(enc.Utf8);
    this.account_id = deencryptedParam
    // this.GetPagedetailssummary();
    this.Postsummarydetails(deencryptedParam);
    this.Getcommentsdetails(deencryptedParam);
    this.Getinsights(deencryptedParam);
    
  }
  toggleOptions(post_id: any) {
    if (this.showOptionsDivId === post_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = post_id;
    }
  }

  Postsummarydetails(deencryptedParam: any) {
    this.NgxSpinnerService.show();
    var url = 'Instagram/Getinstapostsummary'
    let params = {
      account_id: deencryptedParam
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.instapostsummary_List = this.responsedata.instapostsummary_List;
      this.username = this.responsedata.instapostsummary_List[0].username;


      setTimeout(() => {
        $('#instapostsummary_List').DataTable();
      }, 10000);
    });
  }
  Getcommentsdetails(deencryptedParam:any){
    var url = 'Instagram/Getcomments'
    let params={
      account_id: deencryptedParam
    }
    this.service.getparams(url,params).subscribe((result: any) => {
    });
  }
  downloadImage(post_url: string, post_type: string) {
    if (post_url != null && post_url != "") {
      if (post_type === 'IMAGE' || post_type === 'CAROUSEL_ALBUM') {
        saveAs(post_url, this.id + '.png');
      }
      else if (post_type === 'VIDEO') {
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
  onview(post_id: any) {
    var url = 'Instagram/Getinstacomments'
    let param = {
      post_id: post_id
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.instacomment_list = result.instacomment_list;
      this.caption = result.caption
        this.post_type = result.post_type
        this.post_url = result.post_url
        this.like_count = result.like_count
        this.comments_count = result.comments_count
        this.username = result.username
    });
  }
 
  viewinsight(post_id:any,){
    var url = 'Instagram/Getinsights'
      let param = {
        post_id: post_id
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        this.viewinsights_list = result.instainsights_list;
        this.post_type = result.post_type
        this.post_url = result.post_url
        this.media_count = result.media_count
        this.follows_count = result.follows_count
        this.followers_count = result.followers_count
      });
  }
  toggleCommentVisibility() {
    this.isCommentVisible = !this.isCommentVisible;
  }
  onclose() {

  }
  postviewrefresh(account_id: any) {
    // Show spinner
    this.NgxSpinnerService.show();

    var url1 = 'Instagram/Getinstapost'
    let param = {
      account_id: account_id
    };
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.responsedata = result;
      setTimeout(() => {
        this.NgxSpinnerService.hide();
        window.location.reload();
      }, 5000);
    });
  }
  Getinsights(deencryptedParam:any){
    this.NgxSpinnerService.show();

    var url1 = 'Instagram/Getviewinsights'
    let params={
      account_id: deencryptedParam
    }
    this.service.getparams(url1,params).subscribe((result: any) => {
      this.responsedata = result;
      this.NgxSpinnerService.hide();

    });
  }

}